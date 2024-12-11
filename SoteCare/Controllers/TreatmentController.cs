using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class TreatmentController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Treatment
        public ActionResult Index()
        {
            var treatments = db.Treatment.Include("Patients").Include("Medications");
            return View(treatments.ToList());
        }

        // GET: Treatment/Details/5
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

        // GET: Treatment/Create
        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(
                db.Patients.Select(p => new {
                    PatientID = p.PatientID,
                    FullName = p.FirstName + " " + p.LastName  
                }),
                "PatientID", "FullName"
            );

            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName");
            return View();
        }

        // POST: Treatment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID, MedicationID, StartDate, EndDate, TreatmentType, Notes")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                db.Treatment.Add(treatment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName", treatment.PatientID);
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            return View(treatment);
        }

        // GET: Treatment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the treatment by ID
            Treatment treatment = db.Treatment.Find(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }

            // Concatenate FirstName and LastName for FullName and pass it to the dropdown
            ViewBag.PatientID = new SelectList(
                db.Patients.Select(p => new {
                    PatientID = p.PatientID,
                    FullName = p.FirstName + " " + p.LastName  // Concatenate FirstName and LastName
                }),
                "PatientID", "FullName", treatment.PatientID  // Set the selected PatientID from the current treatment
            );

            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            return View(treatment);
        }

        // POST: Treatment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TreatmentID, PatientID, MedicationID, StartDate, EndDate, TreatmentType, Notes")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(treatment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FullName", treatment.PatientID);
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            return View(treatment);
        }

        // GET: Treatment/Delete/5
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

        // POST: Treatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Treatment treatment = db.Treatment.Find(id);
            db.Treatment.Remove(treatment);
            db.SaveChanges();
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

//edit niiku paris muissa controllereis antaa jotain ihme errorii