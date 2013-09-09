using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class TokenController : Controller
    {
        //
        // GET: /Token/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateToken()
        {
            return View();
        }

        public void DeleteToken()
        {

        }

    }
}
