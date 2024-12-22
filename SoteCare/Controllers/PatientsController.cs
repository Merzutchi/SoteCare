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
    public class PatientsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Patients
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        public ActionResult Index2()
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
            Patients patients = db.Patients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
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

            var patientMedications = db.PatientMedications
                .Where(m => m.PatientID == id)
                .OrderByDescending(m => m.StartDate)
                .ToList();

            if (!patientMedications.Any())
            {
                return HttpNotFound("No medications found for this patient.");
            }

            ViewBag.PatientName = db.Patients.Find(id)?.FirstName + " " + db.Patients.Find(id)?.LastName;

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

            if (!vitalFunctions.Any())
            {
                return HttpNotFound("No vital function records found for this patient.");
            }

            var viewModel = new VitalFunctionChart
            {
                PatientName = db.Patients.Find(id)?.FirstName + " " + db.Patients.Find(id)?.LastName,
                VitalFunctionDates = vitalFunctions.Select(v => v.DateTime.ToString("yyyy-MM-dd HH:mm")).ToList(),
                HeartRates = vitalFunctions.Select(v => v.HeartRate ?? 0).ToList(),
                SystolicBP = vitalFunctions.Select(v => v.SystolicBloodPressure ?? 0).ToList(),
                DiastolicBP = vitalFunctions.Select(v => v.DiastolicBloodPressure ?? 0).ToList(),
                RespiratoryRates = vitalFunctions.Select(v => v.RespiratoryRate ?? 0).ToList(),
                Temperatures = vitalFunctions.Select(v => v.Temperature ?? 0).ToList(),
                OxygenSaturations = vitalFunctions.Select(v => v.OxygenSaturation ?? 0).ToList()
            };

            return View(viewModel);
        }
    }
}


