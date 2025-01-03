﻿using SoteCare.Models;
using SoteCare.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace SoteCare.Controllers
{
    public class PatientsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Patients
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            var patientMedications = db.PatientMedications
                .Where(m => m.PatientID == id)
                .Include(m => m.Medications)
                .Include(m => m.Dosages)
                .ToList();

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.PatientID = patient.PatientID;
            ViewBag.PatientMedications = patientMedications;

            return View(patient);
        }

        // GET: PatientHistory
        public ActionResult PatientHistory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            // Set ViewBag.PatientID to be used in the view for the "Back to Patient Details" button
            ViewBag.PatientID = patient.PatientID;

            // Fetch diagnoses, treatments, and medications for the patient
            var diagnoses = db.Diagnoses.Where(d => d.PatientID == id).ToList();
            var treatments = db.Treatment.Where(t => t.PatientID == id).ToList();
            var medications = db.PatientMedications
                .Where(m => m.PatientID == id)
                .Select(m => new
                {
                    m.Medications.MedicationName,
                    m.StartDate
                })
                .ToList();

            var viewModel = new PHViewModel
            {
                PatientName = $"{patient.FirstName} {patient.LastName}",
                Diagnoses = diagnoses.Select(d => new DiagnosisViewModel
                {
                    DiagnosisName = d.DiagnosisName,
                    DiagnosisDate = d.DiagnosisDate,
                    Notes = d.Notes
                }).ToList(),
                Treatments = treatments.Select(t => new TreatmentViewModel
                {
                    TreatmentType = t.TreatmentType,
                    TreatmentDetails = string.Join(", ", t.TreatmentDetails.Select(td => td.Medications.MedicationName)),
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                }).ToList(),
                Medications = medications.Select(m => new MedicationViewModel
                {
                    MedicationName = m.MedicationName,
                    StartDate = m.StartDate ?? DateTime.MinValue
                }).ToList()
            };

            ViewBag.ActiveTab = "PatientHistory"; // Set the active tab
            return View(viewModel);
        }

        // GET: Diagnoses
        public ActionResult Diagnoses(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var diagnoses = db.Diagnoses
                .Include(d => d.Doctors) 
                .Where(d => d.PatientID == id)
                .OrderByDescending(d => d.DiagnosisDate)
                .ToList();

            if (!diagnoses.Any())
            {
                return HttpNotFound("No diagnoses found for this patient.");
            }

            ViewBag.PatientID = id; 
            ViewBag.PatientName = db.Patients.Find(id)?.FirstName + " " + db.Patients.Find(id)?.LastName;

            return View(diagnoses);
        }

        // GET: PatientMedications for a specific Patient
        public ActionResult PatientMedications(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Fetch patient details
            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            // Fetch medications for the specific patient
            var patientMedications = db.PatientMedications
                .Where(pm => pm.PatientID == id)
                .Include(pm => pm.Medications)
                .Include(pm => pm.Dosages)
                .Include(pm => pm.Doctors) // Include related doctor information
                .ToList();

            // Pass patient details to the view
            ViewBag.PatientID = id;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";

            // Return the medications view
            return View(patientMedications);
        }

        // GET: VitalFunctions
        public ActionResult VitalFunctions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vitalFunctions = db.VitalFunctions
                .Where(v => v.PatientID == id)
                .OrderBy(v => v.DateTime)
                .ToList();

            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            var viewModel = new VFunctionChart
            {
                PatientID = id.Value, 
                PatientName = $"{patient.FirstName} {patient.LastName}",
                Dates = vitalFunctions.Select(v => v.DateTime.ToString("dd-MM-yyyy HH:mm")).ToList(),
                HeartRates = vitalFunctions.Select(v => v.HeartRate ?? 0).ToList(),
                SystolicBP = vitalFunctions.Select(v => v.SystolicBloodPressure ?? 0).ToList(),
                DiastolicBP = vitalFunctions.Select(v => v.DiastolicBloodPressure ?? 0).ToList(),
                RespiratoryRates = vitalFunctions.Select(v => v.RespiratoryRate ?? 0).ToList(),
                Temperatures = vitalFunctions.Select(v => v.Temperature ?? 0).ToList(),
                OxygenSaturations = vitalFunctions.Select(v => v.OxygenSaturation ?? 0).ToList()
            };

            ViewBag.PatientID = id;
            ViewBag.NoRecords = !vitalFunctions.Any();
            return View(viewModel);
        }

        // GET: Patients/Treatments/5
        public ActionResult Treatments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var treatments = db.Treatment
                .Where(t => t.PatientID == id)
                .Include(t => t.Medications)
                .ToList();

            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.PatientID = id;

            return View(treatments);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID,FirstName,LastName,DateOfBirth,Gender,Address,PhoneNumber,Email,EmergencyContactName,EmergencyContactPhone")] Patients patients)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patients);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patients patients = db.Patients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientID,FirstName,LastName,DateOfBirth,Gender,Address,PhoneNumber,Email,EmergencyContactName,EmergencyContactPhone")] Patients patients)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patients).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patients);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patients patients = db.Patients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patients patients = db.Patients.Find(id);
            db.Patients.Remove(patients);
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


