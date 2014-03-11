using EmailHandler;
using StudentTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class HelpController : BaseController
    {
        //
        // GET: /Help/
        StudentRepository repository = new StudentRepository();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string name, string email, string message)
        {
            Email objEmail = new Email();
            objEmail.mailTo = email;
            objEmail.mailBody = message;
            objEmail.mailSubject = "Help";
            objEmail.mailFrom = this.repository.GetUserEmail(_userStatistics.UserId);
            Utilities.SendMailThruGmail(objEmail);
            TempData["IsEmailSent"] = "Mail successfully sent.";
            return View("Index");
        }
    }
}
