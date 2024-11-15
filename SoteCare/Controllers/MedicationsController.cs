using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SoteCare.Controllers
{
    public class MedicationsController : Controller
    {
        // GET: Medications
        public async Task<ActionResult> MedicationsView(string searchTerm)
        {
            using (var context = new PatientRecordDataEntities())
            {
                var medications = await context.Medications
             .Where(m => string.IsNullOrEmpty(searchTerm) || m.MedicationName.Contains(searchTerm))
             .ToListAsync();
                return View(medications); 
            }
        }

        //public ActionResult PatientList()
        //{
        //    using (var context = new PatientRecordDataEntities())
        //    { 
        //        var patients = context.Patients.ToList();
        //        return View(patients);
        //    }
        //}        

        [HttpGet]
        public ActionResult AddMedications()
        {
            using (var context = new PatientRecordDataEntities())
            {
                var patients = context.Patients.Select(p => new
                {
                    p.PatientID,
                    FullName = p.FirstName + " " + p.LastName
                }).ToList();

                ViewBag.Patients = new SelectList(patients, "PatientID", "FullName");

                return View(new Medications());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMedications(Medications medication)
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
                var medication = context.Medications
                    .Where(m => m.MedicationID == MedicationID)
                    .SingleOrDefault();

                if (medication == null)
                {
                    TempData["error"] = "Medication not found.";
                    return RedirectToAction("MedicationsView");
                }
              
                var patients = context.Patients
                    .Select(p => new { p.PatientID, FullName = p.FirstName + " " + p.LastName })
                    .ToList();

                ViewBag.Patients = new SelectList(patients, "PatientID", "FullName", medication.PatientID);

                return View(medication);
            }
        }

        // POST: Update Medication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMedication(int MedicationID, int PatientID, Medications medication)
        {
            if (ModelState.IsValid)
            {
                using (var context = new PatientRecordDataEntities())
                {
                    var existingMedication = context.Medications
                        .SingleOrDefault(x => x.MedicationID == MedicationID);

                    if (existingMedication != null)
                    {
                        
                        existingMedication.MedicationName = medication.MedicationName;
                        existingMedication.Dosage = medication.Dosage;
                        existingMedication.Frequency = medication.Frequency;
                        existingMedication.Instructions = medication.Instructions;
                        existingMedication.PatientID = PatientID;

                        context.SaveChanges(); 

                        TempData["message"] = "Medication updated successfully.";
                        return RedirectToAction("MedicationsView");
                    }
                    else
                    {
                        TempData["error"] = "Medication not found.";
                        return View(medication);
                    }
                }
            }
            return View(medication);
        }

        public ActionResult MedicationDetails(int id)
        {
            using (var context = new PatientRecordDataEntities())
            {
                
                var medication = context.Medications
                                        .Include(m => m.Patients) 
                                        .SingleOrDefault(x => x.MedicationID == id);

                if (medication == null)
                {
                    TempData["error"] = "Ei näytettäviä tietoja.";
                    return RedirectToAction("MedicationsView");
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
                if (medication == null)
                {
                    TempData["error"] = "Medication not found.";
                    return RedirectToAction("MedicationsView");
                }                
                return View(medication);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMedicationConfirm(int MedicationID)
        {
            using (var context = new PatientRecordDataEntities())
            {
                var medication = context.Medications.SingleOrDefault(x => x.MedicationID == MedicationID);
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
