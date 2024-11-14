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

        public ActionResult AddMedication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMedication(Medications medications)
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

        [HttpGet]
        public ActionResult GetMedications()
        {
            using (var context = new PatientRecordDataEntities())
            {
                List<Medications> data = context.Medications.ToList();
                return View(data);
            }
        }

        public ActionResult UpdateMedication(int MedicationID)
        {
            using (var context = new PatientRecordDataEntities())
            {
                var data = context.Medications.Where(x => x.MedicationID == MedicationID).SingleOrDefault();

                if (data == null)
                {
                    return HttpNotFound();
                }

                return View(data);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMedication(int MedicationID, Medications medication)
        {
            if (ModelState.IsValid)
            {
                using (var context = new PatientRecordDataEntities())
                {
                    
                    var existingMedication = context.Medications.SingleOrDefault(x => x.MedicationID == medication.MedicationID);

                    if (existingMedication != null)
                    {
                       
                        existingMedication.MedicationName = medication.MedicationName;
                        existingMedication.MedicationStatus = medication.MedicationStatus;
                        existingMedication.Dosage = medication.Dosage;
                        existingMedication.StartDate = medication.StartDate;
                        existingMedication.EndDate = medication.EndDate;
                        existingMedication.Frequency = medication.Frequency;
                        existingMedication.RouteOfAdministration = medication.RouteOfAdministration;
                        existingMedication.Instructions = medication.Instructions;
                        existingMedication.RefillStatus = medication.RefillStatus;
                        existingMedication.Allergies = medication.Allergies;
                        existingMedication.Comments = medication.Comments;

                        context.SaveChanges();

                        return RedirectToAction("UpdateMedication");
                    }
                    else
                    {
                        return View(medication);
                    }                       
                }
            }
            return View(medication);
        }

    }
}
