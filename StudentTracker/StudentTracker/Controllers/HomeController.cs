using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{

    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("", "", new { area = "sas" });
            else if (User.IsInRole("SiteAdmin"))
                return View();
            else
                return RedirectToAction("", "", new { area = "sas" });
        }

    }
}
