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
    public class DosagesController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Dosages
        public ActionResult Index()
        {
            var dosages = db.Dosages.Include(d => d.Medications).Include(d => d.Patients);
            return View(dosages.ToList());
        }

        // GET: Dosages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dosages dosages = db.Dosages.Find(id);
            if (dosages == null)
            {
                return HttpNotFound();
            }
            return View(dosages);
        }

        // GET: Dosages/Create
        public ActionResult Create()
        {
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName");
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName");
            return View();
        }

        // POST: Dosages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DosageID,MedicationID,Dosage,Frequency,StartDate,EndDate,RouteOfAdministration,Instructions,DosageAmount,PatientID")] Dosages dosages)
        {
            if (ModelState.IsValid)
            {
                db.Dosages.Add(dosages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", dosages.MedicationID);
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", dosages.PatientID);
            return View(dosages);
        }

        // GET: Dosages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dosages dosages = db.Dosages.Find(id);
            if (dosages == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", dosages.MedicationID);
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", dosages.PatientID);
            return View(dosages);
        }

        // POST: Dosages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DosageID,MedicationID,Dosage,Frequency,StartDate,EndDate,RouteOfAdministration,Instructions,DosageAmount,PatientID")] Dosages dosages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dosages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", dosages.MedicationID);
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", dosages.PatientID);
            return View(dosages);
        }

        // GET: Dosages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dosages dosages = db.Dosages.Find(id);
            if (dosages == null)
            {
                return HttpNotFound();
            }
            return View(dosages);
        }

        // POST: Dosages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dosages dosages = db.Dosages.Find(id);
            db.Dosages.Remove(dosages);
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
