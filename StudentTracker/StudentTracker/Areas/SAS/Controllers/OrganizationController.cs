using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Areas.SAS.Controllers
{
    public class OrganizationController : Controller
    {
        //
        // GET: /SAS/Organization/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CheckUser(string Username)
        {
            //var user = null;
            //if (user != null)
            //{
            //    return Json("User with this name already exist", JsonRequestBehavior.AllowGet);
            //}
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
