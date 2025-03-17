using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoteCare.Attributes;
using SoteCare.Models;

namespace SoteCare.Controllers
{
    [AuthorizeUser]
    public class TreatmentsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Treatments
        public ActionResult Index()
        {
            var treatments = db.Treatment
                .Include(t => t.Medications) 
                .Include(t => t.Patients)   
                .ToList();

            return View(treatments);
        }

        // GET: Treatments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treatment treatment = db.Treatment.Find(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }
            return View(treatment);
        }

        // GET: Treatments/Create
        public ActionResult Create(int? patientId)
        {
            if (patientId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patient = db.Patients.Find(patientId);
            if (patient == null)
            {
                return HttpNotFound("Potilasta ei löytynyt.");
            }

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName");
            return View(new Treatment { PatientID = patient.PatientID, StartDate = DateTime.Now });
        }

        // POST: Treatments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TreatmentID,PatientID,MedicationID,StartDate,EndDate,TreatmentType,Notes")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                db.Treatment.Add(treatment);
                db.SaveChanges();
                return RedirectToAction("Treatments", "Patients", new { id = treatment.PatientID });
            }

            var patient = db.Patients.Find(treatment.PatientID);
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            return View(treatment);
        }

        // GET: Treatments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Treatment treatment = db.Treatment.Find(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }

            ViewBag.PatientID = treatment.PatientID; // Pass the PatientID to the view
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            ViewBag.PatientIDSelect = new SelectList(db.Patients, "PatientID", "FirstName", treatment.PatientID);
            return View(treatment);
        }

        // POST: Treatments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TreatmentID,PatientID,MedicationID,StartDate,EndDate,TreatmentType,Notes")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(treatment).State = EntityState.Modified;
                db.SaveChanges();

                // Redirect to the specific patient's treatment page
                return RedirectToAction("Treatments", "Patients", new { id = treatment.PatientID });
            }

            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", treatment.PatientID);
            return View(treatment);
        }

        // GET: Treatments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Treatment treatment = db.Treatment
                .Include(t => t.Patients) // Ensure Patients are included
                .FirstOrDefault(t => t.TreatmentID == id);

            if (treatment == null)
            {
                return HttpNotFound();
            }

            return View(treatment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Treatment treatment = db.Treatment.Find(id);
            if (treatment != null)
            {
                int patientId = treatment.PatientID; // Get the PatientID before deleting
                db.Treatment.Remove(treatment);
                db.SaveChanges();

                // Redirect to the patient's treatments page
                return RedirectToAction("Treatments", "Patients", new { id = patientId });
            }

            // If the treatment is not found, redirect to the general index as a fallback
            return RedirectToAction("Index");
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
