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
            ViewBag.PatientID = new SelectList(context.Patients.Select(p => new
            {
                PatientID = p.PatientID,
                FullName = p.FirstName + " " + p.LastName
            }), "PatientID", "FullName", patientId);

            ViewBag.Medications = new SelectList(context.Medications.Select(m => new
            {
                MedicationID = m.MedicationID,
                MedicationName = m.MedicationName
            }), "MedicationID", "MedicationName");

            return View(new Treatment());
        }

        [HttpPost][ValidateAntiForgeryToken]
        public ActionResult Create(Treatment treatments)
        {
            if (ModelState.IsValid)
            {
                context.Treatment.Add(treatments);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(context.Patients.Select(p => new
            {
                PatientID = p.PatientID,
                FullName = p.FirstName + " " + p.LastName
            }), "PatientID", "FullName", treatments.PatientID);

            ViewBag.Medications = new SelectList(context.Medications.Select(m => new
            {
                MedicationID = m.MedicationID,
                MedicationName = m.MedicationName
            }), "MedicationID", "MedicationName", treatments.MedicationID);

            ViewBag.Dosages = new SelectList(context.Dosages, "DosageID", "DosageAmount", treatments.DosageID);

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

