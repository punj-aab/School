using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class ElettersController : Controller
    {
        //
        // GET: /Eletters/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult AddRecipients()
        {
            return PartialView();
        }
    }
}
