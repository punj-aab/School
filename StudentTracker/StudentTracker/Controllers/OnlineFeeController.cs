﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class OnlineFeeController : Controller
    {
        //
        // GET: /Payment/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {
            return View();
        }
    }
}
