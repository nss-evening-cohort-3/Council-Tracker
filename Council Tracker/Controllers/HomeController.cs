﻿using Council_Tracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Council_Tracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CouncilMemberData data = new CouncilMemberData();
            var ViceMayor = data.seedViceMayor();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}