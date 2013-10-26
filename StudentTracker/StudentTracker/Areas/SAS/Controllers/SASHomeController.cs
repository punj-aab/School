using EmailHandler;
using StudentTracker.Core.Entities;
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
            ViewBag.Token = id;
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
            RegistrationToken Token = repository.GetRegistrationCode(objToken.Token);
            if (Token != null)
            {
                return RedirectToAction("RegisterUserStep3", new { Token.Token });
            }
            ViewBag.Error = true;
            return View();
        }

        public ActionResult RegisterUserStep3(string token)
        {
            User objUser = new User();
            objUser.RegistrationToken = token;
            return View(objUser);
        }

        [HttpPost]
        public ActionResult RegisterUserStep3(User objUser)
        {
            RegistrationToken Token = repository.GetRegistrationCode(objUser.RegistrationToken);
            long userId = WebSecurity.RegisterNewUser(objUser.Username, "none", "none", false, objUser.FirstName, objUser.LastName, Token.OrganizationId, Token.Token, objUser.DateOfBirth, objUser.Title);
            if (userId != -1)
            {
                Roles.AddUserToRole(objUser.Username, "Student");
                return RedirectToAction("RegisterUserStep4", new { userId });
            }
            return View(objUser);
        }

        public ActionResult RegisterUserStep4(long userId)
        {
            User objUser = new User();
            objUser.UserId = userId;
            return View(objUser);
        }

        [HttpPost]
        public ActionResult RegisterUserStep4(User objUser)
        {
            DBConnectionString.User User = DBConnectionString.User.FirstOrDefault("select * from Users where UserId=@0", objUser.UserId);
            User.Email = objUser.Email;
            User.MobileNumber = objUser.MobileNumber;
            User.HomeTelephoneNumber = objUser.HomeTelephoneNumber;
            if (User.Update() > 0)
            {
                return RedirectToAction("RegisterUserStep5", new { userId = User.UserId });
            }
            return View(objUser);
        }

        public ActionResult RegisterUserStep5(long userId)
        {
            User objUser = new User();
            objUser.UserId = userId;
            objUser.SecurityQuestionList = new SelectList(repository.SecurityQuestions(), "Id", "Question");
            return View(objUser);
        }
        [HttpPost]
        public ActionResult RegisterUserStep5(User objUser)
        {
            DBConnectionString.User User = DBConnectionString.User.FirstOrDefault("select * from Users where UserId=@0", objUser.UserId);

            // User.Password = objUser.Password;
            WebSecurity.ResetPassword(User.Username, objUser.Password);
            User = DBConnectionString.User.FirstOrDefault("select * from Users where UserId=@0", objUser.UserId);

            // WebSecurity.ChangePassword(User.Username, User.Password, objUser.Password);
            User.SecurityQuestionId = objUser.SecurityQuestionId == null ? 1 : objUser.SecurityQuestionId.Value;
            User.SecurityAnswer = objUser.SecurityAnswer;
            if (User.Update() > 0)
            {
                return RedirectToAction("RegisterUserStep6", new { userId = User.UserId });
            }
            return View(objUser);
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
