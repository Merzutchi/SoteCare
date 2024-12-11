using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace SoteCare.Controllers
{
    public class PatientMedicationsController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

        // GET: PatientMedication
        public ActionResult Index()
        {
            var patientMedications = context.PatientMedications.Include(p => p.MedicationLists).Include(p => p.Medications).Include(p => p.Patients);
            return View(patientMedications.ToList());
        }

        // GET: PatientMedications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedications patientMedications = context.PatientMedications.Find(id);
            if (patientMedications == null)
            {
                return HttpNotFound();
            }
            return View(patientMedications);
        }

        // GET: PatientMedications/Create
        public ActionResult Create()
        {
            ViewBag.MedicationListID = new SelectList(context.MedicationLists, "MedicationListID", "MedicationName"); 
            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName"); 
            ViewBag.PatientID = new SelectList(context.Patients, "PatientID", "FirstName"); 
            return View();
        }

        // POST: PatientMedications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientMedicationID,PatientID,MedicationID,StartDate,EndDate,MedicationListID,DoseStrength")] PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                context.PatientMedications.Add(patientMedications);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MedicationListID = new SelectList(context.MedicationLists, "MedicationListID", "MedicationName", patientMedications.MedicationListID);
            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.PatientID = new SelectList(context.Patients, "PatientID", "FirstName", patientMedications.PatientID);
            return View(patientMedications);
        }

        // GET: PatientMedications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedications patientMedications = context.PatientMedications.Find(id);
            if (patientMedications == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicationListID = new SelectList(context.MedicationLists, "MedicationListID", "MedicationName", patientMedications.MedicationListID);
            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.PatientID = new SelectList(context.Patients, "PatientID", "FirstName", patientMedications.PatientID);
            return View(patientMedications);
        }

        // POST: PatientMedications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientMedicationID,PatientID,MedicationID,StartDate,EndDate,MedicationListID,DoseStrength")] PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                context.Entry(patientMedications).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicationListID = new SelectList(context.MedicationLists, "MedicationListID", "MedicationName", patientMedications.MedicationListID);
            ViewBag.MedicationID = new SelectList(context.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.PatientID = new SelectList(context.Patients, "PatientID", "FirstName", patientMedications.PatientID);
            return View(patientMedications);
        }

        // GET: PatientMedications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedications patientMedications = context.PatientMedications.Find(id);
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
            PatientMedications patientMedications = context.PatientMedications.Find(id);
            context.PatientMedications.Remove(patientMedications);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}


