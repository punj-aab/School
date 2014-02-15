using EmailHandler;
using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Utilities;
using StudentTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentTracker.Areas.SAS.Controllers
{
    public class SASHomeController : Controller
    {
        //
        // GET: /SAS/SASHome/
        StudentRepository repository = new StudentRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
        //COMMUNICATION
        public ActionResult Communication()
        {
            return View();
        }

        //ACADEMIC
        public ActionResult Academic()
        {
            return View();
        }

        //PAYMENT
        public ActionResult Payment()
        {
            return View();
        }

        //BLOG
        public ActionResult Blog()
        {
            return View();
        }

        //SUPPORT
        public ActionResult Support()
        {
            return View();
        }

        //CONTACT US
        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult RegisterUser(string id = "")
        {
            //ViewBag.Token = id;
            return View();
        }

        public ActionResult RegisterUserStep2(string id = "")
        {
            ViewBag.Error = false;
            RegistrationToken objToken = new RegistrationToken();
            objToken.Token = id;
            return View(objToken);
        }
        [HttpPost]
        public ActionResult RegisterUserStep2(RegistrationToken objToken)
        {
            ViewBag.Error = false;
            RegistrationToken token = repository.GetRegistrationCode(objToken.Token);
            if (token != null && token.InsertedOn.AddDays(7) > DateTime.Now)
            {
                return RedirectToAction("RegisterUserStep3", new { token.Token });
            }
            ViewBag.Error = true;
            return View();
        }

        public ActionResult RegisterUserStep3(string token)
        {
            Profile objProfile = new Profile();
            RegistrationToken objToken = repository.GetRegistrationCode(token);
            StudentContext context = new StudentContext();
            Student student = context.Students.Find(objToken.StudentId);
            if (student != null)
            {
                objProfile.FirstName = student.FullName;
                objProfile.EmailAddress1 = student.Email;
            }

            objProfile.RegistrationToken = token;
            objProfile.TitleList = new SelectList(new[] { new { Id = "Mr.", Value = "Mr." }, new { Id = "Miss.", Value = "Miss." } }, "Id", "Value");
            return View(objProfile);
        }

        [HttpPost]
        public ActionResult RegisterUserStep3(Profile objProfile)
        {
            RegistrationToken Token = repository.GetRegistrationCode(objProfile.RegistrationToken);
            // RegistrationToken objToken = repository.GetRegistrationCode(objProfile.RegistrationToken);
            StudentContext context = new StudentContext();
            Student student = new Student();
            Staff staff = new Staff();
            if (Token.StaffId != null)
            {
                staff = context.Staff.Find(Token.StaffId);
            }

            if (Token.StudentId != null)
            {
                student = context.Students.Find(Token.StudentId);
            }
            //objProfile.RegistrationToken=Toke
            long userId = WebSecurity.RegisterNewUser(objProfile.UserName, "none", "none", false, objProfile.FirstName, objProfile.LastName, Token.OrganizationId, Token.Token);
            if (student != null)
            {
                student.UserId = userId;

            }

            if (staff != null)
            {
                staff.UserId = userId;
            }

            context.SaveChanges();
            DBConnectionString.Profile Profile = new DBConnectionString.Profile();
            if (userId != -1)
            {
                Profile.UserId = userId;
                Profile.Title = objProfile.Title;
                Profile.Address1 = "none";
                Profile.Address2 = "none";
                Profile.InsertedOn = DateTime.Now;
                Profile.EmailAddress1 = "user@dummy.com";
                Profile.HomeTelephoneNumber = DateTime.Now.Ticks.ToString();
                Profile.SecurityQuestionId = 1;
                Profile.SecurityAnswer = "none";
                Profile.DateOfBirth = objProfile.DateOfBirth;
                Profile.ModifiedOn = null;
                Profile.MobileNumber = "none";

                int recAffected = Convert.ToInt32(Profile.Insert());

                string roleName = ((UserRoles)Convert.ToInt16(Token.RoleId)).ToString();
                Roles.AddUserToRole(objProfile.UserName, roleName);

                return RedirectToAction("RegisterUserStep4", new { userId });
            }
            return View("RegisterUserStep3", new { token = Token.Token });
        }

        public ActionResult RegisterUserStep4(long userId)
        {
            Profile objProfile = new Profile();
            objProfile.UserId = userId;
            return View(objProfile);
        }

        [HttpPost]
        public ActionResult RegisterUserStep4(Profile objProfile)
        {
            DBConnectionString.User User = DBConnectionString.User.FirstOrDefault("select * from Users where UserId=@0", objProfile.UserId);
            DBConnectionString.Profile Profile = DBConnectionString.Profile.FirstOrDefault("select * from Profile where UserId=@0", objProfile.UserId);
            User.Email = objProfile.EmailAddress1;
            Profile.MobileNumber = objProfile.MobileNumber;
            Profile.HomeTelephoneNumber = objProfile.HomeTelephoneNumber;
            Profile.EmailAddress1 = objProfile.EmailAddress1;
            Profile.HomeTelephoneNumber = objProfile.HomeTelephoneNumber;

            if (User.Update() > 0 && Profile.Update() > 0)
            {
                return RedirectToAction("RegisterUserStep5", new { userId = User.UserId });
            }
            return View(objProfile);
        }

        public ActionResult RegisterUserStep5(long userId)
        {
            Profile objProfile = new Profile();
            objProfile.UserId = userId;
            objProfile.SecurityQuestionList = new SelectList(repository.SecurityQuestions(), "Id", "Question");
            return View(objProfile);
        }
        [HttpPost]
        public ActionResult RegisterUserStep5(Profile objProfile)
        {
            DBConnectionString.User User = DBConnectionString.User.FirstOrDefault("select * from Users where UserId=@0", objProfile.UserId);
            DBConnectionString.Profile Profile = DBConnectionString.Profile.FirstOrDefault("select * from Profile where UserId=@0", objProfile.UserId);
            // User.Password = objUser.Password;
            WebSecurity.ResetPassword(User.Username, objProfile.Password);
            User = DBConnectionString.User.FirstOrDefault("select * from Users where UserId=@0", objProfile.UserId);

            Profile.SecurityQuestionId = objProfile.SecurityQuestionId;
            Profile.SecurityAnswer = objProfile.SecurityAnswer;
            if (User.Update() > 0 && Profile.Update() > 0)
            {
                return RedirectToAction("RegisterUserStep6", new { userId = User.UserId });
            }
            return View(objProfile);
        }

        public ActionResult RegisterUserStep6(long userId)
        {
            User objUser = new User();
            objUser.UserId = userId;
            return View(objUser);
        }
        [HttpPost]
        public ActionResult RegisterUserStep6(User objUser)
        {
            return RedirectToAction("RegisterUserStep7", new { userId = objUser.UserId });
        }

        public ActionResult RegisterUserStep7(long userId)
        {
            DBConnectionString.User User = DBConnectionString.User.SingleOrDefault(userId);

            User objUser = new User();
            Utilities.SendConfirmationEmail(User.Username);

            objUser.UserId = userId;
            return View(objUser);
        }

        public ActionResult RegisterUserStep8(long id)
        {
            User objUSer = new User();
            objUSer.UserId = id;
            ViewBag.Error = false;
            return View(objUSer);
        }

        [HttpPost]
        public ActionResult RegisterUserStep8(User objUser)
        {
            DBConnectionString.User User = DBConnectionString.User.SingleOrDefault(objUser.UserId);

            if (WebSecurity.Login(User.Username, objUser.Password))
            {
                Session["UserId"] = Convert.ToInt32(WebSecurity.GetUser(User.Username).ProviderUserKey);
                string returnUrl = "/";
                return RedirectToAction(returnUrl);
            }

            ViewBag.Error = true;
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }
    }
}
