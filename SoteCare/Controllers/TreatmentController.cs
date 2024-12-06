using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

        // GET: Treatment
        public ActionResult Index()
        {
            var treatments = context.Treatment
                         .Include(t => t.Patients)
                         .Include (t => t.Medication)
                         .Include(t => t.Dosages)
                         .ToList();

            return View(treatments);
        }

        // GET: Treatment/Create
        public ActionResult Create(int? patientId)
        {
            var dosages = context.Dosages.ToList(); 
            foreach (var dosage in dosages)
            {
                System.Diagnostics.Debug.WriteLine($"DosageID: {dosage.DosageID}, DosageAmount: {dosage.DosageAmount}");
            }

            ViewBag.PatientID = new SelectList(context.Patients.Select(p => new
            {
                p.PatientID,
                FullName = p.FirstName + " " + p.LastName
            }), "PatientID", "FullName", patientId);

            ViewBag.MedicationID = new SelectList(context.Medications.Select(m => new
            {
                m.MedicationID,
                m.MedicationName
            }), "MedicationID", "MedicationName");

            ViewBag.DosageID = new SelectList(context.Dosages.Select(d => new
            {
                d.DosageID,
                d.DosageAmount
            }), "DosageID", "DosageAmount");

            return View(new Treatment());
        }

        [HttpPost][ValidateAntiForgeryToken]
        public ActionResult Create(Treatment treatments)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Treatment.Add(treatments);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                        }
                    }

                    ModelState.AddModelError("", "There was a problem saving the treatment. Please check the input.");
                }
            }

            ViewBag.PatientID = new SelectList(context.Patients.Select(p => new
            {
                p.PatientID,
                FullName = p.FirstName + " " + p.LastName
            }), "PatientID", "FullName", treatments.PatientID);

            ViewBag.MedicationID = new SelectList(context.Medications.Select(m => new
            {
                m.MedicationID,
                m.MedicationName
            }), "MedicationID", "MedicationName", treatments.MedicationID);

            ViewBag.DosageID = new SelectList(context.Dosages.Select(d => new
            {
                d.DosageID,
                d.DosageAmount
            }), "DosageID", "DosageAmount", treatments.DosageID);

            return View(treatments);
        }
    }
}
//ei
//toimi
//tää
//paska
//joku
//muu
//saa
//hoitaa
//kiitos

