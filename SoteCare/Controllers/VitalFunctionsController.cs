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
                OxygenSaturations = vitalFunctions.Select(v => v.OxygenSaturation ?? 0).ToList()
            };

            ViewBag.PatientID = id;
            ViewBag.NoRecords = !vitalFunctions.Any();
            return View(viewModel);
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddVitalFunction(VitalFunctions vitalFunction)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Ensures the temperature is correctly parsed and handled
        //        if (Request.Form["Temperature"] != null)
        //        {
        //            if (decimal.TryParse(Request.Form["Temperature"], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal temperature))
        //            {
        //                vitalFunction.Temperature = temperature;
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("Temperature", "Invalid temperature format.");
        //                return View(vitalFunction);
        //            }
        //        }

        //        db.VitalFunctions.Add(vitalFunction);
        //        db.SaveChanges();

        //        return RedirectToAction("VitalFunctions", "Patients", new { id = vitalFunction.PatientID });
        //    }

        //    return View(vitalFunction);
        //}

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
                DateTime = DateTime.Now // Sets the DateTime to the current time
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
                
                // Uses helper to parse Temperature field
                if (Request.Form["Temperature"] != null)
                {
                    var rawTemperature = Request.Form["Temperature"].Replace(',', '.'); // Normalizes commas to dots
                    if (decimal.TryParse(rawTemperature, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal temperature))
                    {
                        vitalFunction.Temperature = temperature;
                    }
                    else
                    {
                        ModelState.AddModelError("Temperature", "Invalid temperature format. Please enter a valid number.");
                    }
                }

                db.VitalFunctions.Add(vitalFunction);
                db.SaveChanges();
                return RedirectToAction("VitalFunctions", "Patients", new { id = vitalFunction.PatientID });
            }

            // Handles invalid ModelState and reload the form with patient details
            var patientDetails = db.Patients.Find(vitalFunction.PatientID);
            ViewBag.PatientName = $"{patientDetails.FirstName} {patientDetails.LastName}";
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

        private decimal? ParseDecimal(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var normalizedInput = input.Replace(',', '.'); 
                if (decimal.TryParse(normalizedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                {
                    return result;
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveVitalFunction(VitalFunctions model)
        {
            if (ModelState.IsValid)
            {
                // Save the valid model (Temperature will be parsed correctly)
                return RedirectToAction("Success");
            }

            // If invalid, re-render the form
            return View(model);
        }
    }
}

