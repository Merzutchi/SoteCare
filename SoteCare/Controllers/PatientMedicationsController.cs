using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoteCare.Attributes;
using SoteCare.Models;

namespace SoteCare.Controllers
{
    [AuthorizeUser]
    public class PatientMedicationsController : Controller
    {
        private readonly PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: PatientMedications
        public ActionResult Index()
        {
            var patientMedications = db.PatientMedications
                .Include(p => p.Medications)
                .Include(p => p.Dosages)
                .Include(p => p.Patients)
                .Include(p => p.Doctors)
                .OrderByDescending(m => m.PatientMedicationID)
                .ToList();

            return View(patientMedications);
        }

        // GET: PatientMedications/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patientMedications = db.PatientMedications.Find(id.Value);
            if (patientMedications == null)
            {
                return HttpNotFound("Patient medication not found.");
            }

            return View(patientMedications);
        }

        // GET: PatientMedications/Create
        public ActionResult Create(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patient = db.Patients.Find(id.Value);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            ViewBag.PatientID = id;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            PopulateDropdowns();

            return View(new PatientMedications { PatientID = id.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PatientMedications.Add(patientMedications);
                    db.SaveChanges();

                    return RedirectToAction("PatientMedications", "Patients", new { id = patientMedications.PatientID });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error while saving patient medication: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the medication.");
                }
            }

            PopulateDropdowns(patientMedications);
            return View(patientMedications);
        }

        // GET: PatientMedications/AddMedication
        public ActionResult AddMedication(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patient = db.Patients.Find(id.Value);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            ViewBag.PatientID = id;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            PopulateDropdowns();

            return View(new PatientMedications { PatientID = id.Value });
        }

        // POST: PatientMedications/AddMedication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMedication(PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PatientMedications.Add(patientMedications);
                    db.SaveChanges();

                    return RedirectToAction("PatientMedications", "Patients", new { id = patientMedications.PatientID });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error while saving patient medication: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the medication.");
                }
            }

            PopulateDropdowns(patientMedications);
            return View(patientMedications);
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patientMedications = db.PatientMedications.Find(id.Value);
            if (patientMedications == null)
            {
                return HttpNotFound("Patient medication not found.");
            }

            PopulateDropdowns(patientMedications);
            return View(patientMedications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientMedications patientMedications)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(patientMedications).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("PatientMedications", "Patients", new { id = patientMedications.PatientID });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error while updating patient medication: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while updating the medication.");
                }
            }

            PopulateDropdowns(patientMedications);
            return View(patientMedications);
        }

        // GET: PatientMedications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patientMedication = db.PatientMedications
                .Include(pm => pm.Medications)
                .Include(pm => pm.Patients)
                .FirstOrDefault(pm => pm.PatientMedicationID == id.Value);

            if (patientMedication == null)
            {
                return HttpNotFound("Patient medication not found.");
            }

            return View(patientMedication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var patientMedications = db.PatientMedications.Find(id);
            if (patientMedications != null)
            {
                try
                {
                    int patientId = (int)patientMedications.PatientID;
                    db.PatientMedications.Remove(patientMedications);
                    db.SaveChanges();

                    return RedirectToAction("PatientMedications", "Patients", new { id = patientId });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error while deleting patient medication: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while deleting the medication.");
                }
            }

            return RedirectToAction("Index");
        }

        private void PopulateDropdowns(PatientMedications model = null)
        {
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName", model?.MedicationID);
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "DosageAmount", model?.DosageID);
            ViewBag.Doctors = new SelectList(
                db.Doctors.Select(d => new
                {
                    d.DoctorID,
                    DoctorName = d.FirstName + " " + d.LastName // Yhdistetään etunimi ja sukunimi
                }),
                "DoctorID",
                "DoctorName",
                model?.DoctorID
            );
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