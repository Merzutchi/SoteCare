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
    public class PatientMedicationsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: PatientMedications
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var patientList = db.Patients
                                .Select(p => new
                                {
                                    PatientID = p.PatientID,
                                    FullName = p.FirstName + " " + p.LastName 
                                })
                                .ToList();

            ViewBag.PatientID = new SelectList(patientList, "PatientID", "FullName");

            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName");
            ViewBag.MedicationListID = new SelectList(db.MedicationLists, "MedicationListID", "MedicationName");

            return View();
        }

        // POST: PatientMedications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID,MedicationID,StartDate,EndDate,DoseStrength,MedicationListID")] PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                db.PatientMedications.Add(patientMedications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", patientMedications.PatientID);
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.MedicationListID = new SelectList(db.MedicationLists, "MedicationListID", "MedicationName", patientMedications.MedicationListID);

            return View(patientMedications);
        }

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

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "Name", patientMedications.PatientID);
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.MedicationListID = new SelectList(db.MedicationLists, "MedicationListID", "MedicationName", patientMedications.MedicationListID);

            return View(patientMedications);
        }

        // POST: PatientMedications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientMedicationID,PatientID,MedicationID,StartDate,EndDate,DoseStrength,MedicationListID")] PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientMedications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "Name", patientMedications.PatientID);
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", patientMedications.MedicationID);
            ViewBag.MedicationListID = new SelectList(db.MedicationLists, "MedicationListID", "MedicationName", patientMedications.MedicationListID);

            return View(patientMedications);
        }
    }
}
