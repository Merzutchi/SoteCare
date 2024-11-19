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
            try
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medications", "MedicationsView"));
            }
        }

        public ActionResult CreateMedication()
        {
            ViewBag.Patients = new SelectList(context.Patients.Select(p => new { p.PatientID, FullName = p.FirstName + "" + p.LastName }).ToList(), "PatientID", "FullName");
            return PartialView("CreateMedication");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateMedication(Medications medication, List<Dosages> dosages)
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

                    if (dosages != null && dosages.Any())
                    {
                        foreach (var dosage in dosages)
                        {
                            dosage.MedicationID = medication.MedicationID;
                            context.Dosages.Add(dosage);
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        return Json(new { success = false, message = "Dosage information is missing." });
                    }

                    return Json(new { success = true, medication });
                }
                catch 
                {
                    return Json(new { success = false, message = "An error occurred while saving the medication." });
                }
            }

            return Json(new { success = false, message = "Invalid model data." });
        }

        // GET: UpdateMedication
        public ActionResult UpdateMedication(int id)
        {
            var medication = context.Medications.Include(m => m.Dosages)
                                                .FirstOrDefault(m => m.MedicationID == id);

            if (medication == null)
            {
                return HttpNotFound();
            }

            ViewBag.Patients = new SelectList(context.Patients.Select(p => new { p.PatientID, FullName = p.FirstName + " " + p.LastName }).ToList(), "PatientID", "FullName");

            return View(medication);
        }

        // POST: UpdateMedication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMedication(Medications medication, List<Dosages> dosages)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingMedication = context.Medications.Include(m => m.Dosages)
                                                                .FirstOrDefault(m => m.MedicationID == medication.MedicationID);
                    if (existingMedication == null)
                    {
                        return HttpNotFound();
                    }

                    existingMedication.MedicationName = medication.MedicationName;
                    existingMedication.PatientID = medication.PatientID;
                    existingMedication.RefillStatus = medication.RefillStatus;

                    // Removes existing dosages, adds new ones
                    context.Dosages.RemoveRange(existingMedication.Dosages);
                    if (dosages != null)
                    {
                        foreach (var dosage in dosages)
                        {
                            dosage.MedicationID = medication.MedicationID;
                            context.Dosages.Add(dosage);
                        }
                    }

                    context.SaveChanges();

                    return RedirectToAction("MedicationsView"); //back to Meds list
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while updating the medication.");
                }
            }

            return View(medication);
        }
        // GET: DeleteMedication
        public ActionResult DeleteMedication(int id)
        {
            var medication = context.Medications
                                    .Include(m => m.Dosages)
                                    .FirstOrDefault(m => m.MedicationID == id);

            if (medication == null)
            {
                return HttpNotFound();
            }

            return View(medication);
        }

        // POST: DeleteMedication
        [HttpPost, ActionName("DeleteMedication")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteMedication(int id)
        {
            try
            {
                var medication = context.Medications.Include(m => m.Dosages)
                                                    .FirstOrDefault(m => m.MedicationID == id);

                if (medication == null)
                {
                    return HttpNotFound();
                }

                context.Dosages.RemoveRange(medication.Dosages);
                context.Medications.Remove(medication);
                context.SaveChanges();

                return RedirectToAction("MedicationsView");
            }
            catch
            {
                var medication = context.Medications.Include(m => m.Dosages).Include(m => m.Treatment)
                                            .FirstOrDefault(m => m.MedicationID == id);
                return View(medication);
            }
        }
    }
}
