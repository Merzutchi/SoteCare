using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
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
                .Include(pm => pm.Medications)
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

            var patientMedication = new PatientMedications { PatientID = patientId.Value };

            ViewBag.PatientID = patientId.Value;
            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName");
            ViewBag.DosageID = new SelectList(context.Dosages, "DosageID", "DosageAmount");
            ViewBag.DoseInterval = new SelectList(new List<string> { "Aamu", "Päivä", "Ilta" });
            return View(patientMedication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientMedications patientMedication, int? patientId)
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
                    context.SaveChanges();

                    return RedirectToAction("Index", new { patientId = patientId });
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                        }
                    }

                    ModelState.AddModelError("", "Data validation failed. Please check the form for errors.");
                }
            }
            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName", patientMedication.MedicationID);

            return View(patientMedication);
        }

        [HttpGet]
        public ActionResult Edit(int? patientId, int? medicationId)
        {
            if (!patientId.HasValue || !medicationId.HasValue)
            {
                return HttpNotFound("Patient or Medication not specified.");
            }

            var patientMedication = context.PatientMedications
                .FirstOrDefault(pm => pm.PatientID == patientId.Value && pm.MedicationID == medicationId.Value);

            if (patientMedication == null)
            {
                return HttpNotFound("PatientMedication not found.");
            }

            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName", patientMedication.MedicationID);
            ViewBag.DoseInterval = new SelectList(new List<string> { "Aamu", "Päivä", "Ilta" });
            return View(patientMedication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientMedications patientMedication)
        {
            if (ModelState.IsValid)
            {
                context.Entry(patientMedication).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index", new { patientId = patientMedication.PatientID });
            }

            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName", patientMedication.MedicationID);
            ViewBag.DoseInterval = new SelectList(new List<string> { "Aamu", "Päivä", "Ilta" });
            return View(patientMedication);
        }

        [HttpPost]
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

