﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrdersWeb.Controllers
{
    public class HubController : Controller
    {
        // GET: Hub
        public ActionResult Index()
        {
            return View();
        }
    }
}