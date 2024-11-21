using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;


namespace SoteCare.Controllers
{
    public class MedicationsController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

        public ActionResult MedicationsView(string searchTerm)
        {
            return View(context);
        }

        
    }
}
