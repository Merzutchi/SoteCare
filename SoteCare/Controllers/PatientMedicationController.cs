using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class PatientMedicationController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

        // GET: PatientMedication
        public ActionResult Index(int? patientId)
        {
            if (!patientId.HasValue)
            {
                return RedirectToAction("SelectPatient");
            }

            var medications = context.PatientMedications
                .Where(pm => pm.PatientID == patientId.Value)
                .Include(pm => pm.Medication)   
                .Include(pm => pm.Dosage)    
                .Include(pm => pm.Patient)        
                .ToList();

            ViewBag.PatientID = patientId.Value;
            return View(medications);
        }

        public ActionResult SelectPatient()
        {
            var patients = context.Patients.ToList();
            return View(patients);
        }

        [HttpGet]
        public ActionResult Create(int? patientId)
        {
            if (!patientId.HasValue)
            {
                return HttpNotFound("Patient not specified.");
            }

            var patientMedication = new PatientMedication { PatientID = patientId.Value };

            ViewBag.PatientID = patientId.Value;
            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName");
            ViewBag.DosageID = new SelectList(context.Dosages, "DosageID", "DosageAmount");
            ViewBag.Description = new SelectList(context.Medications, "MedicationID", "Description");
            ViewBag.DoseInterval = new SelectList(new List<string> { "12 hours", "24 hours", "Every 4 hours" });

            return View(patientMedication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientMedication patientMedication, int? patientId)
        {
            if (!patientId.HasValue)
            {
                return HttpNotFound("Patient not specified.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    patientMedication.PatientID = patientId.Value;
                    context.PatientMedications.Add(patientMedication);

                    if (!string.IsNullOrEmpty(patientMedication.Dosage.Frequency))
                    {
                        var dosage = context.Dosages.FirstOrDefault(d => d.DosageID == patientMedication.DosageID);
                        if (dosage != null)
                        {
                            dosage.Frequency = patientMedication.Dosage.Frequency;
                            context.Entry(dosage).State = EntityState.Modified;
                        }
                        else
                        {
                            ModelState.AddModelError("DosageNotFound", "Selected dosage could not be found.");
                        }
                    }

                    context.SaveChanges();
                    return RedirectToAction("Index", new { patientId = patientId });
                }
                catch (DbEntityValidationException dbEx)
                {
                    string message = string.Empty;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            message += string.Format("Property: {0} Error: {1}\n", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    Console.WriteLine(message);
                    ModelState.AddModelError("", "Data validation failed. Please check the form for errors.");
                    ViewBag.ValidationErrors = message;

                    ViewBag.PatientID = patientId.Value;
                    ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName", patientMedication.MedicationID);
                    ViewBag.DosageID = new SelectList(context.Dosages, "DosageID", "DosageAmount", patientMedication.DosageID);
                    ViewBag.MedicationDescription = new SelectList(context.Medications, "MedicationID", "Description", patientMedication.MedicationID);
                    ViewBag.DoseInterval = new SelectList(new List<string> { "12 hours", "24 hours", "Every 4 hours" }, patientMedication.Dosage.Frequency);

                    return View(patientMedication);
                }
            }
            ViewBag.PatientID = patientId.Value;
            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName", patientMedication.MedicationID);
            ViewBag.DosageID = new SelectList(context.Dosages, "DosageID", "DosageAmount", patientMedication.DosageID);
            ViewBag.MedicationDescription = new SelectList(context.Medications, "MedicationID", "Description", patientMedication.MedicationID);
            ViewBag.DoseInterval = new SelectList(new List<string> { "12 hours", "24 hours", "Every 4 hours" }, patientMedication.Dosage.Frequency);

            return View(patientMedication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? patientId, int medicationId)
        {
            var patientMedication = context.PatientMedications
                .FirstOrDefault(pm => pm.PatientID == patientId && pm.MedicationID == medicationId);

            if (patientMedication != null)
            {
                context.PatientMedications.Remove(patientMedication);
                context.SaveChanges();
            }

            return RedirectToAction("Index", new { patientId = patientId });
        }
    }

}

