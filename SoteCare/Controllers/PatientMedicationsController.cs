using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoteCare.Models;

namespace SoteCare.Controllers
{
    public class PatientMedicationsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: PatientMedications
        public ActionResult Index()
        {
            var patientMedications = db.PatientMedications
                .Include(p => p.Medications)
                .Include(p => p.Dosages)  
                .Include(p => p.Patients)
                .OrderByDescending(m => m.PatientMedicationID)
                .ToList();

            return View(patientMedications);
        }

        // GET: PatientMedications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedications patientMedications = db.PatientMedications.Find(id);
            if (patientMedications == null)
            {
                return HttpNotFound();
            }
            return View(patientMedications);
        }

        // GET: PatientMedications/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            ViewBag.PatientID = id; 
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName");
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "DosageAmount");

            return View();
        }

        // POST: PatientMedications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientMedicationID,PatientID,MedicationID,StartDate,EndDate,MedicationListID,DoseStrength, DosageID")] PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                db.PatientMedications.Add(patientMedications);
                db.SaveChanges();
                return RedirectToAction("PatientMedications", "Patients", new { id = patientMedications.PatientID });
            }

            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "DosageAmount", patientMedications.DosageID);
            return View(patientMedications);
        }

        // GET: PatientMedications/AddMedication/1
        public ActionResult AddMedication(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            ViewBag.PatientID = id;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName");
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "DosageAmount");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMedication([Bind(Include = "PatientMedicationID,PatientID,MedicationID,StartDate,EndDate,MedicationListID,DoseStrength, DosageID")] PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                db.PatientMedications.Add(patientMedications);
                db.SaveChanges();
                return RedirectToAction("PatientMedications", "Patients", new { id = patientMedications.PatientID });
            }

            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "DosageAmount", patientMedications.DosageID);
            return View(patientMedications);
        }

        public ActionResult AddDosage(int medicationId, int patientId)
        {
            // Ensure that medicationId and patientId are nullable when creating the Dosages model
            var medication = db.Medications.Find(medicationId);
            var patient = db.Patients.Find(patientId);

            if (medication == null || patient == null)
            {
                return HttpNotFound();
            }

            ViewBag.PatientID = (int?)patientId; // Ensures the ViewBag value is nullable
            ViewBag.MedicationID = (int?)medicationId; 
            ViewBag.MedicationName = medication.MedicationName;

            // If Dosage model requires nullable fields, pass nullable types
            return View(new Dosages
            {
                MedicationID = medicationId,
                PatientID = patientId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDosage([Bind(Include = "DosageID, MedicationID, PatientID, Dosage, Frequency, StartDate, EndDate, RouteOfAdministration, Instructions, DosageAmount")] Dosages dosage)
        {
            if (ModelState.IsValid)
            {
                db.Dosages.Add(dosage);
                db.SaveChanges();
                return RedirectToAction("PatientMedications", "Patients", new { id = dosage.PatientID });
            }

            ViewBag.PatientID = dosage.PatientID;
            ViewBag.MedicationID = dosage.MedicationID;
            return View(dosage);
        }

        // GET: PatientMedications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedications patientMedications = db.PatientMedications.Find(id);
            if (patientMedications == null)
            {
                return HttpNotFound();
            }

            ViewBag.MedicationListID = new SelectList(db.MedicationLists, "MedicationListID", "MedicationName", patientMedications.MedicationListID);
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", patientMedications.PatientID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "Dosage", patientMedications.DosageID);

            return View(patientMedications);
        }

        // POST: PatientMedications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientMedicationID, PatientID, MedicationID, StartDate, EndDate, MedicationListID, DoseStrength, DosageID")] PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientMedications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PatientMedications", "Patients", new { id = patientMedications.PatientID });
            }

            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "DosageAmount", patientMedications.DosageID);

            return View(patientMedications);
        }

        // GET: PatientMedications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedications patientMedications = db.PatientMedications.Find(id);
            if (patientMedications == null)
            {
                return HttpNotFound();
            }
            return View(patientMedications);
        }

        // POST: PatientMedications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientMedications patientMedications = db.PatientMedications.Find(id);
            if (patientMedications != null)
            {
                db.PatientMedications.Remove(patientMedications);
                db.SaveChanges();
            }

            return RedirectToAction("PatientMedications", "Patients", new { id = patientMedications.PatientID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
