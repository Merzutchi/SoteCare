﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class MedicationsController : Controller
    {
        // GET: Medications
        public ActionResult Index()
        {
            return View();
        }
    }
}