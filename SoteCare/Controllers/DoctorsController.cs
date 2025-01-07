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
    public class DoctorsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Doctors
        public ActionResult Index()
        {
            return View(db.Doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctors doctors = db.Doctors.Find(id);
            if (doctors == null)
            {
                return HttpNotFound();
            }
            return View(doctors);
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DoctorID,FirstName,LastName,Specialization,PhoneNumber,Email")] Doctors doctors)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctors);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctors);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctors doctors = db.Doctors.Find(id);
            if (doctors == null)
            {
                return HttpNotFound();
            }
            return View(doctors);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DoctorID,FirstName,LastName,Specialization,PhoneNumber,Email")] Doctors doctors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctors).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctors);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctors doctors = db.Doctors.Find(id);
            if (doctors == null)
            {
                return HttpNotFound();
            }
            return View(doctors);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctors doctors = db.Doctors.Find(id);
            db.Doctors.Remove(doctors);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AssignPatientToNurse
        [AuthorizeUser(Role = "Doctor")]
        public ActionResult AssignPatientToNurse()
        {
            // Fetches unassigned patients
            ViewBag.UnassignedPatients = db.Patients
                .Where(p => !db.PatientNurseAssignment.Any(a => a.PatientID == p.PatientID)) // Patients not assigned
                .AsEnumerable() // Converts to memory to allow string formatting
                .Select(p => new SelectListItem
                {
                    Value = p.PatientID.ToString(),
                    Text = $"{p.FirstName} {p.LastName}" // Formatting in memory
                })
                .ToList();

            // Fetches available nurses
            ViewBag.Nurses = db.Nurses
                .AsEnumerable() // Converts to memory to allow string formatting
                .Select(n => new SelectListItem
                {
                    Value = n.NurseID.ToString(),
                    Text = $"{n.FirstName} {n.LastName}" // Formatting in memory
                })
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPatientToNurse(int patientId, int nurseId)
        {
            if (ModelState.IsValid)
            {
                var existingAssignment = db.PatientNurseAssignment.FirstOrDefault(a => a.PatientID == patientId);
                if (existingAssignment != null)
                {
                    existingAssignment.NurseID = nurseId;
                    existingAssignment.AssignmentDate = DateTime.Now;
                }
                else
                {
                    db.PatientNurseAssignment.Add(new PatientNurseAssignment
                    {
                        PatientID = patientId,
                        NurseID = nurseId,
                        AssignmentDate = DateTime.Now
                    });
                }

                db.SaveChanges();
                TempData["SuccessMessage"] = "Patient successfully assigned to nurse!";
                return RedirectToAction("AssignPatientToNurse");
            }

            TempData["ErrorMessage"] = "Failed to assign patient to nurse. Please try again.";
            return RedirectToAction("AssignPatientToNurse");
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

