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
    public class PatientHistoryController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: PatientHistory
        public ActionResult Index()
        {
            var patientHistory = db.PatientHistory.Include(p => p.Patients);
            return View(patientHistory.ToList());
        }

        // GET: PatientHistory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientHistory patientHistory = db.PatientHistory.Find(id);
            if (patientHistory == null)
            {
                return HttpNotFound();
            }
            return View(patientHistory);
        }

        // GET: PatientHistory/Create
        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName");
            return View();
        }

        // POST: PatientHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HistoryID,PatientID,ConditionName,TreatmentDetails,SurgeryDate,Notes")] PatientHistory patientHistory)
        {
            if (ModelState.IsValid)
            {
                db.PatientHistory.Add(patientHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", patientHistory.PatientID);
            return View(patientHistory);
        }

        // GET: PatientHistory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientHistory patientHistory = db.PatientHistory.Find(id);
            if (patientHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", patientHistory.PatientID);
            return View(patientHistory);
        }

        // POST: PatientHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HistoryID,PatientID,ConditionName,TreatmentDetails,SurgeryDate,Notes")] PatientHistory patientHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", patientHistory.PatientID);
            return View(patientHistory);
        }

        // GET: PatientHistory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientHistory patientHistory = db.PatientHistory.Find(id);
            if (patientHistory == null)
            {
                return HttpNotFound();
            }
            return View(patientHistory);
        }

        // POST: PatientHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientHistory patientHistory = db.PatientHistory.Find(id);
            db.PatientHistory.Remove(patientHistory);
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
