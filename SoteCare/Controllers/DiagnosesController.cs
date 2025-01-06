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

            // Pass the PatientID to the view
            ViewBag.PatientID = patientID;
            var diagnosis = new Diagnoses { PatientID = patientID.Value };

            return View(diagnosis);
        }

        // POST: Diagnoses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiagnosisName, DiagnosisDate, Notes, PatientID")] Diagnoses diagnosis)
        {
            if (ModelState.IsValid)
            {
                // Gets the currently logged-in user
                var currentUser = db.Users.Find(Session["UserID"]);

                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account"); // Redirects if the user session is invalid
                }

                // Checks if the user is a doctor (Doctor role)
                if (currentUser.Role == "Doctor")
                {
                    // Finds the doctor based on the UserID
                    var doctor = db.Doctors.SingleOrDefault(d => d.UserID == currentUser.UserID);
                    if (doctor != null)
                    {
                        diagnosis.DoctorID = doctor.DoctorID;
                    }
                    else
                    {
                        diagnosis.DoctorID = null;  // If doctor not found, sets DoctorID to null
                    }
                }
                else
                {
                    diagnosis.DoctorID = null; // If not a doctor, sets DoctorID to null
                }

                // Saves the diagnosis to the database
                db.Diagnoses.Add(diagnosis);
                db.SaveChanges();

                // Redirects to the Diagnoses page for the patient
                return RedirectToAction("Diagnoses", "Patients", new { id = diagnosis.PatientID });
            }

            // If model validation failed, return the diagnosis form
            ViewBag.PatientID = diagnosis.PatientID;
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
            db.Diagnoses.Remove(diagnosis);
            db.SaveChanges();
            return RedirectToAction("Diagnoses", "Patients", new { id = diagnosis.PatientID });
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