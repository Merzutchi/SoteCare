using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;


namespace SoteCare.Controllers
{
    public class MedicationsController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

       public ActionResult Index()
        {
            var medications = context.Medications.ToList();
            return View(medications);

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medications medication)
        {
            if (ModelState.IsValid)
            {
                context.Medications.Add(medication);
                context.SaveChanges();
                return RedirectToAction("Index");
            }            
            return View(medication);
        }
    }
}



