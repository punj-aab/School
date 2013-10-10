using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class CalendarController : BaseController
    {
        //
        // GET: /Calendar/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewCalander()
        {
            return View();
        }
    }
}
