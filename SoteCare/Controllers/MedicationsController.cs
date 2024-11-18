using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace SoteCare.Controllers
{
    public class MedicationsController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

        public ActionResult MedicationsView(string searchTerm)
        {
            var medications = context.Medications
                                     .Include(m => m.Patients)
                                     .Include(m => m.Treatment)
                                     .Include(m => m.Dosages)
                                     .Where(m => string.IsNullOrEmpty(searchTerm) ||
                                                m.MedicationName.Contains(searchTerm) ||
                                                m.Patients.FirstName.Contains(searchTerm) ||
                                                m.Patients.LastName.Contains(searchTerm))
                                     .ToList();

            ViewBag.Patients = new SelectList(context.Patients.Select(p => new
            {
                p.PatientID,
                FullName = p.FirstName + " " + p.LastName
            }).ToList(), "PatientID", "FullName");

            return View(medications);
        }

        public ActionResult CreateMedication()
        {
            ViewBag.Patients = new SelectList(context.Patients.Select(p => new { p.PatientID, FullName = p.FirstName + "" + p.LastName }).ToList(), "PatientID", "FullName");
            return PartialView("CreateMedication");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateMedication(Medications medication, List<Dosage> dosages)
        {
            if (ModelState.IsValid)
            {
                if (medication.PatientID == 0)
                {
                    ModelState.AddModelError("PatientID", "Valitse potilas.");
                    return Json(new { success = false, message = "Patient is required." });
                }

                try
                {
                    context.Medications.Add(medication);
                    context.SaveChanges();

                    if (dosages != null)
                    {
                        foreach (var dosage in dosages)
                        {
                            dosage.MedicationID = medication.MedicationID;
                            context.Dosages.Add(dosage);
                        }
                        context.SaveChanges();
                    }

                    return Json(new { success = true });
                }
                catch
                {
                    return Json(new { success = false, message = "An error occurred while saving the medication." });
                }
            }

            return Json(new { success = false, message = "Invalid model data." });
        }
    }
}
