﻿using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Council_Tracker.Models;
using Council_Tracker.DAL;

namespace Council_Tracker.Controllers
{
    public class UserController : ApiController
    {
        ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        CTrackerRepository repo = new CTrackerRepository();
        // GET api/<controller>/5
        public string Get()
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            return user.Id;
        }

        //[HttpPost]
        //[Route("api/User/Resolution/{resNumber}")]
        //public string PostRes(int resNumber)
        //{
        //    ApplicationUser loggedInUser = userManager.FindById(User.Identity.GetUserId());
        //    if (ModelState.IsValid && User.Identity.IsAuthenticated)
        //    {
        //        // replacing the hash with bill.UserID?? DO I EVEN NEED THIS? 
        //        if (bill.type == "Ordinance")
        //        {
        //            loggedInUser.Ordinances.Add(bill.ordNumber);
        //        }
        //        else if (bill.type == "Resolution")
        //        {
        //            loggedInUser.Resolutions.Add(bill.resNumber);
        //        }
        //        return "Posted. Nice work.";
        //    }
        //    else
        //    {
        //        return "Error! Bummer dude.";
        //    }
        //}
        [HttpPost]
        [Route("api/User/Ordinance/{ordNumber}")]
        public string PostOrd(int ordNumber)
        {
            ApplicationUser loggedInUser = userManager.FindById(User.Identity.GetUserId());
            Ordinance ord = new Ordinance();
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                repo.TrackOrdinance(ordNumber, loggedInUser.Id);
                return "Posted. Nice work.";
            }
            else
            {
                return "Error! Bummer dude.";
            }
        }
    }
}