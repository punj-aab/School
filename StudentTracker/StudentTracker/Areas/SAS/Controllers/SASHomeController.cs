using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Areas.SAS.Controllers
{
    public class SASHomeController : Controller
    {
        //
        // GET: /SAS/SASHome/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

    }
}
