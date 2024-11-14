using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class MedicationsController : Controller
    {
        // GET: Medications
        public ActionResult MedicationsView()
        {
            using (var context = new PatientRecordDataEntities()) 
            { 
                var medications = context.Medications.ToList();
                return View(medications);
            }
            
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Medications medications)
        {
            using (var context = new PatientRecordDataEntities()) 
            {
                context.Medications.Add(medications);

                context.SaveChanges();
            }
            string message = "Created the record successfully";

            ViewBag.Message = message;
            return View();



        }





    }
}