using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

        // GET: Treatment
        public ActionResult Index()
        {
            var treatments = context.Treatment
                         .Include(t => t.Medication)
                         .Include(t => t.Dosages) 
                         .ToList();

            return View(treatments);
        }

        // GET: Treatment/Create
        public ActionResult Create()
        {
            ViewBag.Patients = new SelectList(context.Patients, "PatientID", "FullName");
            ViewBag.Medications = new SelectList(context.Medications, "MedicationID", "MedicationName");
            ViewBag.Dosages = new SelectList(context.Dosages, "DosageID", "Dosage");

            return View();
        }

        // POST: Treatment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                context.Treatment.Add(treatment);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Patients = new SelectList(context.Patients, "PatientID", "FullName", treatment.PatientID);
            ViewBag.Medications = new SelectList(context.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            ViewBag.Dosages = new SelectList(context.Dosages, "DosageID", "Dosage", treatment.DosageID);

            return View(treatment);
        }

        // GET: Treatment/Edit
        public ActionResult Edit(int id)
        {
            var treatment = context.Treatment.Find(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }

            ViewBag.Patients = new SelectList(context.Patients, "PatientID", "FirstName", "LastName", treatment.PatientID);
            ViewBag.Medications = new SelectList(context.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            ViewBag.Dosages = new SelectList(context.Dosages, "DosageID", "Dosage", treatment.DosageID);

            return View(treatment);
        }

        // POST: Treatment/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                context.Entry(treatment).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Patients = new SelectList(context.Patients, "PatientID", "FullName", treatment.PatientID);
            ViewBag.Medications = new SelectList(context.Medications, "MedicationID", "MedicationName", treatment.MedicationID);
            ViewBag.Dosages = new SelectList(context.Dosages, "DosageID", "Dosage", treatment.DosageID);

            return View(treatment);
        }

        // GET: Treatment/Delete
        public ActionResult Delete(int id)
        {
            var treatment = context.Treatment.Find(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }

            return View(treatment);
        }

        // POST: Treatment/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var treatment = context.Treatment.Find(id);
            context.Treatment.Remove(treatment);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}


//ViewBag.Patients = new SelectList(
//    context.Patients.Select(p => new
//    {
//        PatientID = p.PatientID,
//        FullName = p.FirstName + " " + p.LastName
//    }),
//    "PatientID",
//    "FullName",
//    treatment.PatientID);