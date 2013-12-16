using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class AttendanceController : Controller
    {
        //
        // GET: /Attendance/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewByStaff()
        {
            return View();
        }

        public ActionResult CreateAttendance()
        {
            return View();
        }

    }
}
