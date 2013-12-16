using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportByClass()
        {
            return View();
        }

        public ActionResult ReportByStudents()
        {
            return View();
        }

        public ActionResult MonthlyReortOfStudent()
        {
            return View();
        }

        public ActionResult AssessmentCentre()
        {
            return View();
        }
    }
}
