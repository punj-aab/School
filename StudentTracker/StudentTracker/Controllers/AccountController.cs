using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StudentTracker.Core.Utilities;
using StudentTracker.ViewModels;
using StudentTracker.Core.DAL;
using StudentTracker.Repository;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;

namespace StudentTracker.Controllers
{
    public class AccountController : BaseController
    {

        [AllowAnonymous]
        public ActionResult Confirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Verify(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Msg = "Not Good!!!";
                return View();
            }
            else
            {
                var user = Membership.GetUser(Convert.ToInt64(id));
                if (!user.IsApproved)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                    FormsAuthentication.SetAuthCookie(user.UserName, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    FormsAuthentication.SignOut();
                    ViewBag.Msg = "Account Already Approved";
                    return RedirectToAction("Login");
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterUser(string id)
        {
            StudentContext db = new StudentContext();
            var tokenObject = db.RegistrationTokens.Where(t => t.Token == id).FirstOrDefault();
            StudentTracker.Core.Entities.User objUser = new Core.Entities.User();
            objUser.OrganizationId = tokenObject.OrganizationId;
            objUser.MasterId = tokenObject.CreatedBy;
            objUser.RegistrationToken = id;
            return View(objUser);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(StudentTracker.Core.Entities.User objUser)
        {
            StudentContext db = new StudentContext();
            var tokenObject = db.RegistrationTokens.Where(t => t.Token == objUser.RegistrationToken).FirstOrDefault();
            objUser.OrganizationId = tokenObject.OrganizationId;
            objUser.MasterId = tokenObject.CreatedBy;
            CodeFirstMembershipProvider membership = new CodeFirstMembershipProvider();
            var token = membership.CreateAccount(objUser);
            Roles.AddUserToRole(objUser.Username, Enum.GetName(typeof(UserRoles), tokenObject.RoleId));
            EmailHandler.Utilities.SendConfirmationEmail(objUser.Username);
            return RedirectToAction("Confirmation", "Account");

        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            WebSecurity.Register("Admin", "123456", "Admin@demo.com", true, "Admin", "Demo");
            Roles.CreateRole("OrganizationAdmin");
            Roles.CreateRole("SiteAdmin");
            Roles.AddUserToRole("Admin", "SiteAdmin");
            Roles.CreateRole("Student");
            Roles.CreateRole("Parent");
            Roles.CreateRole("Teacher");
            Roles.CreateRole("OtherStaff");
            if (Request.IsAuthenticated)
            {
                Response.Redirect("/home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                Session["UserId"] = Convert.ToInt32(WebSecurity.GetUser(model.UserName).ProviderUserKey);
                //PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
                //Session["UserId"] = db.SingleOrDefault<long>("select UserId from Users where Username = @0", model.UserName);
                returnUrl = string.IsNullOrEmpty(returnUrl) ? "/home/index" : returnUrl;
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpGet]
        // [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var membership = new CodeFirstMembershipProvider();
                try
                {
                    MembershipCreateStatus status;
                    membership.CreateUser(model.Username, model.Password, model.Email, string.Empty, string.Empty, false, null, out status);
                    if (status == MembershipCreateStatus.Success)
                        //  EmailManager.SendConfirmationEmail(model.Username);
                        return RedirectToAction("Confirmation", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgetPasswordModel model)
        {
            return View();
        }

        public ActionResult Test()
        {
            WebSecurity.Register("Admin", "123456", "Admin@demo.com", true, "Admin", "Demo");
            Roles.CreateRole("OrganizationAdmin");
            Roles.CreateRole("SiteAdmin");
            Roles.AddUserToRole("Admin", "SiteAdmin");
            Roles.CreateRole("Student");
            Roles.CreateRole("Parent");
            Roles.CreateRole("Teacher");
            Roles.CreateRole("OtherStaff");
            return RedirectToAction("Login", new { returnUrl = "/Home" });
        }

        [Authorize]
        public ActionResult Profile()
        {
            long userId = _userStatistics.UserId;
            StudentTracker.Core.Entities.Profile objProfile = this.repository.GetUserProfile(userId);

            if (string.IsNullOrEmpty(objProfile.ProfileImageUrl))
            {
                objProfile.ProfileImageUrl = "/img/profile.png";
            }
            return View(objProfile);
        }


        StudentRepository repository = new StudentRepository();
        public ActionResult EditProfile()
        {
            long userId = _userStatistics.UserId;
            StudentTracker.Core.Entities.Profile objProfile = this.repository.GetUserProfile(userId);
            if (objProfile.UserId == 0)
            {
                objProfile.UserId = userId;
            }
            ViewBag.UserId = _userStatistics.UserId;

            if (string.IsNullOrEmpty(objProfile.ProfileImageUrl))
            {
                objProfile.ProfileImageUrl = "/img/profile.png";
            }
            return View(objProfile);
        }

        [HttpPost]
        public ActionResult EditProfile(StudentTracker.Core.Entities.Profile objProfile)
        {
            if (objProfile.ProfileId == 0)
            {
                string destDirectory = Server.MapPath("~/Attachments/ProfileImages");
                destDirectory = Path.Combine(destDirectory, _userStatistics.UserId.ToString());
                string fileUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/Attachments/ProfileImages";
                // destDirectory = Path.Combine(destDirectory, type, importId);
                if (Directory.Exists(destDirectory))
                {
                    var files = Directory.GetFiles(destDirectory);
                    if (files.Count() > 0)
                    {
                        objProfile.ProfileImageUrl = Path.Combine(fileUrl, _userStatistics.UserId.ToString(), files[0]);
                    }
                }
            }

            this.repository.UpdateUserProfile(objProfile);
            return RedirectToAction("profile", "account");
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string SaveProfileImage(IEnumerable<HttpPostedFileBase> file, string importId, string type)
        {
            try
            {
                string targetFilePath = string.Empty;
                foreach (string upload in Request.Files)
                {
                    FileInfo fileinfo = new FileInfo(Request.Files[upload].FileName);
                    string fileUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/Attachments/ProfileImages";
                    StudentContext db = new StudentContext();

                    string destDirectory = Server.MapPath("~/Attachments/ProfileImages");
                    destDirectory = Path.Combine(destDirectory, _userStatistics.UserId.ToString());
                    if (!Directory.Exists(destDirectory))
                    {
                        Directory.CreateDirectory(destDirectory);
                    }

                    //string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
                    string filename = Path.GetFileName(Request.Files[upload].FileName);
                    filename = Regex.Replace(filename, @"\s+", "");

                    //fetch path of the local directory from iCATStaticItemClass.
                    //string targetFolderPath = Path.GetTempPath();//Server.MapPath("~/Administrator/TempFiles/" + iCATGlobal.CurrentTenantInfo.TenantName);  // This is the part Im wondering about. Will this still function the way it should on the webserver after upload?
                    //create target filePath 
                    targetFilePath = Path.Combine(destDirectory, filename);
                    //Check if directory exists.
                    if (Directory.Exists(destDirectory))
                    {
                        //if file with the same name exist in the directory.
                        if (System.IO.File.Exists(targetFilePath))
                        {
                            while (System.IO.File.Exists(targetFilePath))//while file exist in the directory.
                            {
                                try
                                {
                                    //delete file from with target filepath.
                                    System.IO.File.Delete(targetFilePath);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            // save file 
                            Request.Files[upload].SaveAs(targetFilePath);
                        }
                        else
                        {
                            //save file
                            Request.Files[upload].SaveAs(targetFilePath);
                        }
                    }
                    else
                    {
                        //if directory doesn't exist create a new directory.
                        Directory.CreateDirectory(destDirectory);
                        //save file 
                        Request.Files[upload].SaveAs(targetFilePath);
                    }

                    StudentTracker.Core.Entities.Profile objProfile = db.Profiles.Where(u => u.UserId == _userStatistics.UserId).FirstOrDefault();
                    if (objProfile != null)
                    {
                        objProfile.ProfileImageUrl = Path.Combine(fileUrl, _userStatistics.UserId.ToString(), filename);
                    }
                    this.repository.UpdateProfileImage(objProfile);
                }

                return targetFilePath;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                }

                throw;
            }
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }


        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
