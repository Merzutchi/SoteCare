﻿using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class VitalFunctionsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: VitalFunctions
        public ActionResult Index()
        {

            var vitalFunctions = db.VitalFunctions.Include(v => v.Patients);
            return View(vitalFunctions.ToList());
        }

        public ActionResult AddVitalFunction(int patientId)
        {
            var vitalFunction = new VitalFunctions
            {
                PatientID = patientId,
                DateTime = DateTime.Now // Default to the current time
            };
            return View(vitalFunction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddVitalFunction([Bind(Include = "PatientID,DateTime,HeartRate,SystolicBloodPressure,DiastolicBloodPressure,RespiratoryRate,Temperature,OxygenSaturation")] VitalFunctions vitalFunction)
        {
            if (ModelState.IsValid)
            {
                db.VitalFunctions.Add(vitalFunction);
                db.SaveChanges();

                var updatedData = db.VitalFunctions
                    .Where(v => v.PatientID == vitalFunction.PatientID)
                    .OrderBy(v => v.DateTime)
                    .Select(v => new
                    {
                        v.DateTime,
                        v.HeartRate,
                        v.SystolicBloodPressure,
                        v.DiastolicBloodPressure
                    })
                    .ToList();

                return Json(new { success = true, data = updatedData });
            }

            return Json(new { success = false, message = "Failed to add vital function." });
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

        // GET: VitalFunctions/Create
        public ActionResult Create()
        {

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName");
            return View();
        }

        // POST: VitalFunctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VitalFunctionID,PatientID,DateTime,HeartRate,SystolicBloodPressure,DiastolicBloodPressure,RespiratoryRate,Temperature,OxygenSaturation")] VitalFunctions vitalFunctions)
        {
            if (ModelState.IsValid)
            {
                db.VitalFunctions.Add(vitalFunctions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", vitalFunctions.PatientID);
            return View(vitalFunctions);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

