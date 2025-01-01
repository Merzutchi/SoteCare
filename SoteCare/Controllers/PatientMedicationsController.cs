using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
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

            // Return empty model for the form
            return View(new PatientMedications { PatientID = id.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMedication([Bind(Include = "PatientID,MedicationID,DosageID,StartDate,EndDate")] PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Save the new medication
                    db.PatientMedications.Add(patientMedications);
                    db.SaveChanges();

                    // Redirect to the patient medications list
                    return RedirectToAction("PatientMedications", "Patients", new { id = patientMedications.PatientID });
                }
                catch (Exception ex)
                {
                    // Log exception and return an error
                    Debug.WriteLine($"Error saving medication: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the medication. Please try again.");
                }
            }

            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "DosageAmount", patientMedications.DosageID);

            return View(patientMedications);
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

            // Populate dropdowns for Medication and Dosage
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "DosageAmount", patientMedications.DosageID);

            return View(patientMedications);
        }

        // POST: PatientMedications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientMedicationID, PatientID, MedicationID, DosageID, StartDate, EndDate, MedicationListID, DoseStrength")] PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientMedications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PatientMedications", "Patients", new { id = patientMedications.PatientID });
            }

            // Re-populate dropdowns if ModelState is invalid
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

            // Fetch the specific medication record by ID, including related entities
            var patientMedication = db.PatientMedications
                .Include(pm => pm.Medications) // Include medication details
                .Include(pm => pm.Patients)    // Include patient details
                .FirstOrDefault(pm => pm.PatientMedicationID == id);

            if (patientMedication == null)
            {
                return HttpNotFound("The medication record could not be found.");
            }

            return View(patientMedication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Check if the PatientMedication exists with the given id
            var patientMedication = db.PatientMedications.Find(id);

            if (patientMedication == null)
            {
                // If medication does not exist, return HTTP Not Found
                return HttpNotFound("The medication record could not be found.");
            }

            // Remove the medication from the database
            db.PatientMedications.Remove(patientMedication);
            db.SaveChanges(); // Commit the changes to the database

            // Log the deletion for debugging purposes (optional)
            Debug.WriteLine($"Medication with ID {id} deleted.");

            // Redirect the user to the PatientMedications page for the corresponding patient
            return RedirectToAction("PatientMedications", "Patients", new { id = patientMedication.PatientID });
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
