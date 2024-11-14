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
            return View(new Medications());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMedication(Medications medication)
        {
            if (ModelState.IsValid) 
            {
                using (var context = new PatientRecordDataEntities())
                {
                    context.Medications.Add(medication);
                    context.SaveChanges();
                }

                TempData["message"] = "Lääke Lisätty.";
                return RedirectToAction("MedicationsView");
            }
            return View(medication);
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
        public ActionResult UpdateMedication(Medications medication)
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
                        TempData["message"] = "Tiedot päivitetty.";
                        return RedirectToAction("UpdateMedication");
                    }
                    else
                    {
                        ModelState.AddModelError("", ""); /*lisää joku ilmotus juttu*/
                    }                       
                }
            }
            return View(medication);            
        }

        public ActionResult MedicationDetails(int id)
        {
            using (var context = new PatientRecordDataEntities())
            {
                var medication = context.Medications.SingleOrDefault(x => x.MedicationID == id);
                if (medication == null)
                {
                    TempData["error"] = "Ei näytettäviä tietoja.";
                    return RedirectToAction("MedicationView");
                }
                return View(medication);
            }
        }

        [HttpGet]
        public ActionResult DeleteMedication(int id)
        { 
            using (var context = new PatientRecordDataEntities())
            {
                var medication = context.Medications.SingleOrDefault(x => x.MedicationID == id);
                if (medication != null)
                {
                    context.Medications.Remove(medication);
                    context.SaveChanges();
                    TempData["message"] = "Lääke poistettu.";
                }
                else
                {
                    TempData["error"] = "Ei osuvaa lääkettä.";
                }
            }
            return RedirectToAction("MedicationsView");
        }
    }
}
