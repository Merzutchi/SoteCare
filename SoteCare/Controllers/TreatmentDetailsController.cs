using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class TreatmentDetailsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: TreatmentDetails
        public ActionResult Index()
        {
            var treatmentDetails = db.TreatmentDetails
                .Include(td => td.Treatment)
                .Include(td => td.Medications)
                .Include(td => td.Dosages);
            return View(treatmentDetails.ToList());
        }

        // GET: TreatmentDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TreatmentDetails treatmentDetails = db.TreatmentDetails.Find(id);
            if (treatmentDetails == null)
            {
                return HttpNotFound();
            }
            return View(treatmentDetails);
        }

        // GET: TreatmentDetails/Create
        public ActionResult Create()
        {
            ViewBag.TreatmentID = new SelectList(db.Treatment, "TreatmentID", "TreatmentType");
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName");
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "Dosage");
            return View();
        }

        // POST: TreatmentDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TreatmentDetailID,TreatmentID,MedicationID,DosageID")] TreatmentDetails treatmentDetails)
        {
            if (ModelState.IsValid)
            {
                db.TreatmentDetails.Add(treatmentDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TreatmentID = new SelectList(db.Treatment, "TreatmentID", "TreatmentType", treatmentDetails.TreatmentID);
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatmentDetails.MedicationID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "Dosage", treatmentDetails.DosageID);
            return View(treatmentDetails);
        }

        // GET: TreatmentDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TreatmentDetails treatmentDetails = db.TreatmentDetails
                .Include(td => td.Treatment)
                .Include(td => td.Medications)
                .Include(td => td.Dosages)
                .FirstOrDefault(td => td.TreatmentDetailID == id);

            if (treatmentDetails == null)
            {
                return HttpNotFound();
            }

            ViewBag.TreatmentID = new SelectList(db.Treatment, "TreatmentID", "TreatmentType", treatmentDetails.TreatmentID);
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatmentDetails.MedicationID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "Dosage", treatmentDetails.DosageID);
            return View(treatmentDetails);
        }

        // POST: TreatmentDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TreatmentDetailID,TreatmentID,MedicationID,DosageID")] TreatmentDetails treatmentDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(treatmentDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TreatmentID = new SelectList(db.Treatment, "TreatmentID", "TreatmentType", treatmentDetails.TreatmentID);
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", treatmentDetails.MedicationID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "Dosage", treatmentDetails.DosageID);
            return View(treatmentDetails);
        }

        // GET: TreatmentDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TreatmentDetails treatmentDetails = db.TreatmentDetails.Find(id);
            if (treatmentDetails == null)
            {
                return HttpNotFound();
            }
            return View(treatmentDetails);
        }

        // POST: TreatmentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TreatmentDetails treatmentDetails = db.TreatmentDetails.Find(id);
            db.TreatmentDetails.Remove(treatmentDetails);
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
