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

            var history = db.PatientHistory
                .Where(h => h.PatientID == id)
                .ToList();

            if (!history.Any())
            {
                return HttpNotFound("No treatment history found for this patient.");
            }

            ViewBag.PatientID = id;
            ViewBag.PatientName = db.Patients.Find(id)?.FirstName + " " + db.Patients.Find(id)?.LastName;

            return View(history);
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

        // GET: Medications
        public ActionResult PatientMedications(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            var patientMedications = db.PatientMedications
                .Where(m => m.PatientID == id)
                .Include(m => m.Medications)
                .Include(m => m.Dosages)
                .OrderByDescending(m => m.StartDate)
                .ToList();

            ViewBag.PatientID = id;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.NoRecords = !patientMedications.Any();

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

            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound("Patient not found.");
            }

            var treatments = db.Treatment
                .Include(t => t.Medications) 
                .Where(t => t.PatientID == id)
                .OrderByDescending(t => t.StartDate)
                .ToList();

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.PatientID = id;
            ViewBag.ActiveTab = "Treatments";

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


