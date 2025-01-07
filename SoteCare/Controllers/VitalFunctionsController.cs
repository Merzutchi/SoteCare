using SoteCare.Attributes;
using SoteCare.Models;
using SoteCare.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    [AuthorizeUser]
    public class VitalFunctionsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: VitalFunctions
        public ActionResult Index()
        {

            var vitalFunctions = db.VitalFunctions.Include(v => v.Patients);
            return View(vitalFunctions.ToList());
        }
      
        // GET: VitalFunctions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VitalFunctions vitalFunctions = db.VitalFunctions.Find(id);
            if (vitalFunctions == null)
            {
                return HttpNotFound();
            }
            return View(vitalFunctions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVitalFunction(VitalFunctions vitalFunction)
        {
            if (ModelState.IsValid)
            {
                // Ensure the temperature is correctly parsed and handled
                if (Request.Form["Temperature"] != null)
                {
                    if (decimal.TryParse(Request.Form["Temperature"], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal temperature))
                    {
                        vitalFunction.Temperature = temperature;
                    }
                    else
                    {
                        ModelState.AddModelError("Temperature", "Invalid temperature format.");
                        return View(vitalFunction);
                    }
                }

                db.VitalFunctions.Add(vitalFunction);
                db.SaveChanges();

                return RedirectToAction("VitalFunctions", "Patients", new { id = vitalFunction.PatientID });
            }

            return View(vitalFunction);
        }

        // GET: VitalFunctions/Create
        public ActionResult Create(int? id)
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

            var vitalFunction = new VitalFunctions
            {
                PatientID = id.Value,
                DateTime = DateTime.Now // Set the DateTime to the current time
            };

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            return View(vitalFunction); 
        }

        // POST: VitalFunctions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID,DateTime,HeartRate,SystolicBloodPressure,DiastolicBloodPressure,RespiratoryRate,Temperature,OxygenSaturation")] VitalFunctions vitalFunction)
        {
            if (ModelState.IsValid)
            {
                db.VitalFunctions.Add(vitalFunction);
                db.SaveChanges();
                return RedirectToAction("VitalFunctions", "Patients", new { id = vitalFunction.PatientID });
            }

            var patient = db.Patients.Find(vitalFunction.PatientID);
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            return View(vitalFunction);
        }

        // GET: VitalFunctions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VitalFunctions vitalFunctions = db.VitalFunctions.Find(id);
            if (vitalFunctions == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", vitalFunctions.PatientID);
            return View(vitalFunctions);
        }

        // POST: VitalFunctions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VitalFunctionID,PatientID,DateTime,HeartRate,SystolicBloodPressure,DiastolicBloodPressure,RespiratoryRate,Temperature,OxygenSaturation")] VitalFunctions vitalFunctions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vitalFunctions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", vitalFunctions.PatientID);
            return View(vitalFunctions);
        }

        // GET: VitalFunctions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VitalFunctions vitalFunctions = db.VitalFunctions.Find(id);
            if (vitalFunctions == null)
            {
                return HttpNotFound();
            }
            return View(vitalFunctions);
        }

        // POST: VitalFunctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VitalFunctions vitalFunctions = db.VitalFunctions.Find(id);
            db.VitalFunctions.Remove(vitalFunctions);
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

