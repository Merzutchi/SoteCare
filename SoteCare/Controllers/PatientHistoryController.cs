using SoteCare.Attributes;
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
    public class PatientHistoryController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: PatientHistory
        public ActionResult Index()
        {
            var patientHistory = db.PatientHistory.Include(p => p.Patients);
            return View(patientHistory.ToList());
        }

        // GET: PatientHistory/Details/5
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

            // Fetch related data
            var diagnoses = db.Diagnoses.Where(d => d.PatientID == id)
                .Select(d => new DiagnosisViewModel
                {
                    DiagnosisName = d.DiagnosisName,
                    DiagnosisDate = d.DiagnosisDate,
                    Notes = d.Notes
                }).ToList();

            var treatments = db.Treatment.Where(t => t.PatientID == id)
                .Select(t => new TreatmentViewModel
                {
                    TreatmentType = t.TreatmentType,
                    TreatmentDetails = t.Notes,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                }).ToList();

            var medications = db.PatientMedications
                .Where(m => m.PatientID == id)
                .Select(m => new MedicationViewModel
                {
                    MedicationName = m.Medications.MedicationName,
                    StartDate = m.Medications.Dosages.FirstOrDefault().StartDate
                }).ToList();

            // Prepare the ViewModel
            var viewModel = new PHViewModel
            {
                PatientName = $"{patient.FirstName} {patient.LastName}",
                Diagnoses = diagnoses,
                Treatments = treatments,
                Medications = medications
            };

            return View(viewModel);
        }

        // GET: PatientHistory/Create
        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName");
            return View();
        }

        // POST: PatientHistory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HistoryID,PatientID,ConditionName,TreatmentDetails,SurgeryDate,Notes")] PatientHistory patientHistory)
        {
            if (ModelState.IsValid)
            {
                db.PatientHistory.Add(patientHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", patientHistory.PatientID);
            return View(patientHistory);
        }

        // GET: PatientHistory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientHistory patientHistory = db.PatientHistory.Find(id);
            if (patientHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", patientHistory.PatientID);
            return View(patientHistory);
        }

        // POST: PatientHistory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HistoryID,PatientID,ConditionName,TreatmentDetails,SurgeryDate,Notes")] PatientHistory patientHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", patientHistory.PatientID);
            return View(patientHistory);
        }

        // GET: PatientHistory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientHistory patientHistory = db.PatientHistory.Find(id);
            if (patientHistory == null)
            {
                return HttpNotFound();
            }
            return View(patientHistory);
        }

        // POST: PatientHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientHistory patientHistory = db.PatientHistory.Find(id);
            db.PatientHistory.Remove(patientHistory);
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

