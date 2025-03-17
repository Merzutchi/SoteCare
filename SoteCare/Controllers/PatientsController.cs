﻿using SoteCare.Attributes;
using SoteCare.Models;
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
    [AuthorizeUser]
    public class PatientsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Patients
        public ActionResult Index()
        {
            var patients = db.Patients.Include(p => p.PatientRooms).ToList();
            return View(patients);
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
                .Include(m => m.Doctors) 
                .ToList();

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.PatientID = patient.PatientID;
            ViewBag.PatientMedications = patientMedications;

            return View(patient);
        }

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
            ViewBag.PatientID = patient.PatientID;

            // Fetches diagnoses, treatments, and medications for the patient
            var diagnoses = db.Diagnoses
                .Where(d => d.PatientID == id)
                .OrderByDescending(d => d.DiagnosisDate)  // Uusimmat ensin
                .ToList();
            var treatments = db.Treatment
                .Where(t => t.PatientID == id)
                .OrderByDescending(t => t.StartDate)  // Uusimmat ensin
                .ToList();
            var medications = db.PatientMedications
                .Where(m => m.PatientID == id)
                .OrderByDescending(m => m.StartDate)  // Uusimmat ensin
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

            ViewBag.ActiveTab = "PatientHistory"; // Sets the active tab
            return View(viewModel);
        }

        public ActionResult Diagnoses(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Fetches patient by ID
            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            // Fetches the patient's diagnoses, related doctors, ordered by DiagnosisDate
            var diagnoses = db.Diagnoses
                .Where(d => d.PatientID == id)
                .OrderByDescending(d => d.DiagnosisDate)
                .Include(d => d.Doctors)
                .ToList();

            ViewBag.PatientID = id;
            ViewBag.PatientName = patient.FirstName + " " + patient.LastName;

            // Passes diagnoses or a message if none are found
            if (diagnoses.Any())
            {
                return View(diagnoses);
            }
            else
            {
                ViewBag.Message = "Potilaalla ei ole diagnooseja.";
                return View();  //returns an empty view with a message if no diagnoses exist
            }
        }

        // GET: PatientMedications for a specific Patient
        public ActionResult PatientMedications(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Haetaan potilaan tiedot
            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            // Haetaan potilaan lääkitykset ja niiden liittyvät tiedot
            var patientMedications = db.PatientMedications
                .Where(pm => pm.PatientID == id)
                .Include(pm => pm.Medications)
                .Include(pm => pm.Dosages)
                .Include(pm => pm.Doctors)
                .ToList();

            // Muunnetaan tiedot näkymämalliksi
            var medicationViewModels = patientMedications.Select(pm => new PatientMedicationViewModel
            {
                PatientMedicationID = pm.PatientMedicationID,
                MedicationName = pm.Medications?.MedicationName ?? "Tuntematon lääke",
                DosageAmount = pm.Dosages?.DosageAmount ?? "Annosta ei saatavilla",
                SingleDose = pm.SingleDose ?? "Ei määritelty",
                DailyFrequency = pm.DailyFrequency.HasValue ? pm.DailyFrequency.Value : (int?)null,
                AdministrationTimes = !string.IsNullOrEmpty(pm.AdministrationTimes) ? pm.AdministrationTimes : "Ei määritelty",
                StartDate = pm.StartDate,
                EndDate = pm.EndDate,
                DoctorName = pm.Doctors != null ? $"{pm.Doctors.FirstName} {pm.Doctors.LastName}" : "Lääkäriä ei määritelty",
                RouteOfAdministration = pm.Dosages?.RouteOfAdministration ?? "Tietoa ei saatavilla",
                MedicationType = pm.MedicationType ?? "Ei määritelty"
            }).ToList();

            ViewBag.PatientID = id;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";

            return View(medicationViewModels);
        }

        // GET: Patients/VitalFunctions
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
                OxygenSaturations = vitalFunctions.Select(v => v.OxygenSaturation ?? 0).ToList(),
                BloodSugars = vitalFunctions.Select(v => v.BloodSugar ?? 0).ToList() // Uusi rivi verisokerille
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

        public ActionResult Contact(int? id)
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
            ViewBag.ActiveTab = "Contact";

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAllergiesRisk(int patientId, string allergies, string riskInfo)
        {
            // Haetaan potilasta tietokannasta
            var patient = db.Patients.Find(patientId);
            if (patient == null)
            {
                return HttpNotFound();
            }

            // Tallentaa allergiat ja riskitiedot
            patient.Allergies = allergies;
            patient.RiskInfo = riskInfo;
            db.SaveChanges();

            // Palaa takaisin potilaan "Details" -näkymään
            return RedirectToAction("Details", new { id = patientId });
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            ViewBag.RoomID = new SelectList(db.PatientRooms, "RoomID", "RoomNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID,FirstName,LastName,DateOfBirth,Gender,Address,PhoneNumber,Email,EmergencyContactName,EmergencyContactPhone,RoomID")] Patients patients)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Jos tallennus epäonnistuu, täytetään RoomID dropdown
            ViewBag.RoomID = new SelectList(db.PatientRooms, "RoomID", "RoomNumber", patients.RoomID);
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
            ViewBag.RoomID = new SelectList(db.PatientRooms, "RoomID", "RoomNumber", patients.RoomID);
            return View(patients);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientID,FirstName,LastName,DateOfBirth,Gender,Address,PhoneNumber,Email,EmergencyContactName,EmergencyContactPhone,RoomID")] Patients patients)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patients).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomID = new SelectList(db.PatientRooms, "RoomID", "RoomNumber", patients.RoomID);
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
            var patients = db.Patients
            .Include("VitalFunctions")
            .Include(p => p.PatientNurseAssignment)
            .Include(p => p.Diagnoses)
            .Include(p => p.PatientMedications)
            .Include(p => p.PatientHistory)
            .Include(p => p.Treatment)
            .FirstOrDefault(p => p.PatientID == id);

            if (patients == null)
            {
                return HttpNotFound();
            }

            // Removes related Vitalfunctions first
            db.VitalFunctions.RemoveRange(patients.VitalFunctions);

            // Removes related PatientNurseAssignment records first
            db.PatientNurseAssignment.RemoveRange(patients.PatientNurseAssignment);

            // Removes related Diagnoses first
            db.Diagnoses.RemoveRange(patients.Diagnoses);

            // Removes related PatientMedications first
            db.PatientMedications.RemoveRange(patients.PatientMedications);

            // Remove srelated PatientHistory first
            db.PatientHistory.RemoveRange(patients.PatientHistory);

            // Removes related Treatments first
            db.Treatment.RemoveRange(patients.Treatment);

            // Removes the Patient record
            db.Patients.Remove(patients);

            // Saves changes
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult AssignToNurse(int patientId)
        {
            var patient = db.Patients.Find(patientId);
            if (patient == null)
            {
                return HttpNotFound();
            }
            int doctorId = (int)Session["UserID"];
            var doctor = db.Doctors.SingleOrDefault(d => d.UserID == doctorId);
            if (doctor == null)
            {
                return HttpNotFound(); 
            }

            ViewBag.Nurses = db.Nurses.Select(n => new SelectListItem
            {
                Value = n.NurseID.ToString(),
                Text = n.FirstName + " " + n.LastName
            }).ToList();
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignToNurse(int patientId, int nurseId)
        {
            var patient = db.Patients.Find(patientId);
            if (patient == null)
            {
                return HttpNotFound();  
            }
            int doctorId = (int)Session["UserID"];
            var doctor = db.Doctors.SingleOrDefault(d => d.UserID == doctorId);
            if (doctor == null)
            {
                return HttpNotFound();  
            }
            var assignment = new PatientNurseAssignment
            {
                PatientID = patient.PatientID,
                NurseID = nurseId,
                DoctorID = doctor.DoctorID,  
                AssignmentDate = DateTime.Now
            };

            db.PatientNurseAssignment.Add(assignment);
            db.SaveChanges();

            return RedirectToAction("AssignedPatients");
        }

        public ActionResult AssignedPatients()
        {
            int nurseId = (int)Session["UserID"];
            var assignedPatients = db.PatientNurseAssignment
                .Where(a => a.NurseID == nurseId) 
                .Select(a => new AssignedPatientViewModel
                {
                    PatientName = a.Patients.FirstName + " " + a.Patients.LastName,
                    AssignmentDate = a.AssignmentDate,
                    DoctorName = a.Patients.Doctors != null
                                ? a.Patients.Doctors.FirstName + " " + a.Patients.Doctors.LastName
                                : "Ei määrättyä lääkäriä." 
                })
                .ToList();

            return View(assignedPatients);
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


