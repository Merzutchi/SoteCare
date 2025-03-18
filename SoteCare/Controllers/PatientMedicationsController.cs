using SoteCare.Attributes;
using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    [AuthorizeUser]
    public class PatientMedicationsController : Controller
    {
        private readonly PatientRecordDataEntities db = new PatientRecordDataEntities();

        public ActionResult Index()
        {
            var patientMedications = db.PatientMedications
                .Include(p => p.Medications) // Ensures related data like medications are loaded
                .Include(p => p.Dosages) // Includes dosage information as well
                .Include(p => p.Patients) // Includes patient info
                .Include(p => p.Doctors) // Includes doctor info
                .OrderByDescending(m => m.PatientMedicationID)
                .ToList();

            return View(patientMedications); // Passes the data to the view
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
                return HttpNotFound("Lääkitys potilaalle ei löytynyt.");
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
                return HttpNotFound("Potilasta ei löytynyt.");
            }

            ViewBag.PatientID = id;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.MedicationID = new SelectList(db.Medications, "MedicationID", "MedicationName");
            ViewBag.DosageID = new SelectList(db.Dosages, "DosageID", "DosageAmount");
            ViewBag.Doctors = new SelectList(
                db.Doctors.Select(d => new
                {
                    d.DoctorID,
                    DoctorName = d.FirstName + " " + d.LastName
                }),
                "DoctorID",
                "DoctorName"
            );

            // Lääkkeen tyyppi (Säännöllinen / Tarvittaessa)
            ViewBag.MedicationType = new SelectList(new List<string> { "Säännöllinen", "Tarvittaessa" });

            return View(new PatientMedications
            {
                PatientID = id.Value,
                MedicationType = "Säännöllinen" // Oletuksena säännöllinen, mutta käyttäjä voi muuttaa
            });
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
                    ModelState.AddModelError("", "Virhe tallennettaessa lääkettä.");
                    Debug.WriteLine($"Virhe: {ex.Message}");
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
                return HttpNotFound("Lääkitys ei saatavilla.");
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
                    ModelState.AddModelError("", "Tapahtui virhe päivittäessä lääkkeen.");
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
                return HttpNotFound("Lääke ei löydetty.");
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
                    DoctorName = d.FirstName + " " + d.LastName
                }),
                "DoctorID",
                "DoctorName", model?.DoctorID
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