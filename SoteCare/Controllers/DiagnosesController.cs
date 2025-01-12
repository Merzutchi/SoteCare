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

            // Fetch the patient
            var patient = db.Patients.Find(patientID);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            // Pass the PatientID and the list of Doctors to the view
            ViewBag.PatientID = patientID;
            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorID", "FullName"); // Populate the Doctor dropdown
            var diagnosis = new Diagnoses { PatientID = patientID.Value };

            return View(diagnosis);
        }

        // POST: Diagnoses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiagnosisName, DiagnosisDate, Notes, PatientID, DoctorID")] Diagnoses diagnosis)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var currentUser = db.Users.Find(Session["UserID"]);

                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account"); // Redirect if the user session is invalid
                }

                // Check if the user is a doctor (Doctor role)
                if (currentUser.Role == "Doctor")
                {
                    // Find the doctor based on the UserID
                    var doctor = db.Doctors.SingleOrDefault(d => d.UserID == currentUser.UserID);
                    if (doctor != null)
                    {
                        // Set the doctor for the diagnosis (either from dropdown or current user)
                        diagnosis.DoctorID = doctor.DoctorID;
                    }
                    else
                    {
                        diagnosis.DoctorID = null;  // If the doctor is not found, set to null
                    }
                }
                else
                {
                    // If not a doctor, the diagnosis will not have a Doctor assigned
                    diagnosis.DoctorID = null;
                }

                // Save the diagnosis to the database
                db.Diagnoses.Add(diagnosis);
                db.SaveChanges();

                // Redirect to the Diagnoses page for the patient
                return RedirectToAction("Diagnoses", "Patients", new { id = diagnosis.PatientID });
            }

            // If validation failed, return the diagnosis form
            ViewBag.PatientID = diagnosis.PatientID;
            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorID", "FullName", diagnosis.DoctorID);
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