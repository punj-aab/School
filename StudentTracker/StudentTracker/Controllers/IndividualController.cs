﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class IndividualController : Controller
    {
        //
        // GET: /Individual/

        public ActionResult Index()
        {
            return View();
        }

               public ActionResult Parents()
        {
            return View();
        }
    }
}