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
                return HttpNotFound("Patient not found.");
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
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", treatment.PatientID);
            return View(treatment);
        }

        // POST: Treatments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TreatmentID,PatientID,MedicationID,StartDate,EndDate,TreatmentType,Notes")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(treatment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            Treatment treatment = db.Treatment.Find(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }
            return View(treatment);
        }

        // POST: Treatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Treatment treatment = db.Treatment.Find(id);
            if (treatment != null)
            {
                db.Treatment.Remove(treatment);
                db.SaveChanges();
            }
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
