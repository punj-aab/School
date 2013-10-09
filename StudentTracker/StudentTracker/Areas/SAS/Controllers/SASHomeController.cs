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
    }
}
