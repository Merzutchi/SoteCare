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
    public class DosagesController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Dosages
        public ActionResult Index()
        {
            var dosages = db.Dosages.Include(d => d.Medications).ToList();
            return View(dosages);
        }

        // GET: Dosages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dosages dosage = db.Dosages.Include(d => d.Medications).FirstOrDefault(d => d.DosageID == id);
            if (dosage == null)
            {
                return HttpNotFound();
            }
            return View(dosage);
        }

        // GET: Dosages/Create
        public ActionResult Create()
        {
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName");
            return View();
        }

        // POST: Dosages/Create        //poistin dosage nimikkeen koska sitä ei välttämättä tarvitse kun on jo dosageamount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DosageID,MedicationID,DosageAmount,Frequency,StartDate,EndDate,RouteOfAdministration,Instructions")] Dosages dosage)
        {
            if (ModelState.IsValid)
            {
                db.Dosages.Add(dosage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", dosage.MedicationID);
            return View(dosage);
        }

        // GET: Dosages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dosages dosage = db.Dosages.Find(id);
            if (dosage == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", dosage.MedicationID);
            return View(dosage);
        }

        // POST: Dosages/Edit/5   //sama homma kun create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DosageID,MedicationID,Frequency,StartDate,EndDate,RouteOfAdministration,Instructions,DosageAmount")] Dosages dosage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dosage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", dosage.MedicationID);
            return View(dosage);
        }

        // GET: Dosages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dosages dosage = db.Dosages.Include(d => d.Medications).FirstOrDefault(d => d.DosageID == id);
            if (dosage == null)
            {
                return HttpNotFound();
            }
            return View(dosage);
        }

        // POST: Dosages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dosages dosage = db.Dosages.Find(id);
            db.Dosages.Remove(dosage);
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
