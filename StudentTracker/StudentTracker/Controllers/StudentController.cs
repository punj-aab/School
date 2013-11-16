using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateStudent()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}
