using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class TimeTableController : Controller
    {
        //
        // GET: /TimeTable/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditTimeTable()
        {
            return View();
        }

    }
}
