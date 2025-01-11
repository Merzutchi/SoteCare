using SoteCare.Attributes;
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
    [AuthorizeUser]
    public class DiagnosesController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Diagnoses
        public ActionResult Index()
        {
            var diagnoses = db.Diagnoses.Include(d => d.Patients)
                                        .Include(d => d.Doctors)
                                        .ToList();
            return View(diagnoses);
        }

        // GET: Diagnoses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diagnoses diagnosis = db.Diagnoses.Find(id);
            if (diagnosis == null)
            {
                return HttpNotFound();
            }
            return View(diagnosis);
        }

        // GET: Diagnoses/Create
        public ActionResult Create(int? patientID)
        {
            if (patientID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Hae potilas
            var patient = db.Patients.Find(patientID);
            if (patient == null)
            {
                return HttpNotFound("Potilasta ei löydy.");
            }

            ViewBag.DoctorID = new SelectList(
                db.Doctors.Select(d => new { d.DoctorID, FullName = d.FirstName + " " + d.LastName }),
                "DoctorID",
                "FullName"
            );
            var diagnosis = new Diagnoses { PatientID = patientID.Value };
            ViewBag.PatientName = patient.FirstName + " " + patient.LastName;

            return View(diagnosis);
        }

        // POST: Diagnoses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiagnosisName, DiagnosisDate, Notes, PatientID, DoctorID")] Diagnoses diagnosis)
        {
            if (ModelState.IsValid)
            {
                db.Diagnoses.Add(diagnosis);
                db.SaveChanges();

                // Palauttaa käyttäjän diagnoosien listaukseen
                return RedirectToAction("Diagnoses", "Patients", new { id = diagnosis.PatientID });
            }
            ViewBag.DoctorID = new SelectList(
                db.Doctors.Select(d => new { d.DoctorID, FullName = d.FirstName + " " + d.LastName }),
                "DoctorID",
                "FullName",
                diagnosis.DoctorID
            );

            return View(diagnosis);
        }

        // GET: Diagnoses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diagnoses diagnosis = db.Diagnoses.Find(id);
            if (diagnosis == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", diagnosis.PatientID);
            return View(diagnosis);
        }

        // POST: Diagnoses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DiagnosisID,PatientID,DiagnosisName,DiagnosisDate,Notes")] Diagnoses diagnosis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diagnosis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Diagnoses", "Patients", new { id = diagnosis.PatientID });
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", diagnosis.PatientID);
            return View(diagnosis);
        }

        // GET: Diagnoses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diagnoses diagnosis = db.Diagnoses.Find(id);
            if (diagnosis == null)
            {
                return HttpNotFound();
            }
            return View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Diagnoses diagnosis = db.Diagnoses.Find(id);
            if (diagnosis != null)
            {
                int patientId = (int)diagnosis.PatientID; // Get the PatientID before deleting
                db.Diagnoses.Remove(diagnosis);
                db.SaveChanges();

                // Redirect to the patient's diagnoses page
                return RedirectToAction("Diagnoses", "Patients", new { id = patientId });
            }
            // If the diagnose is not found, redirect to the general index as a fallback
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