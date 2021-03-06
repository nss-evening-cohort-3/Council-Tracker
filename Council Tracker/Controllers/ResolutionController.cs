﻿using Council_Tracker.DAL;
using Council_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Council_Tracker.Controllers
{
    public class ResolutionController : ApiController
    {
        CTrackerRepository repo = new CTrackerRepository();
        // GET api/<controller>
        public List<Resolution> Get()
        {
            return repo.GetResolutions();
        }

        public Resolution Get(int id)
        {
            return repo.GetSingleRes(id);
        }

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}