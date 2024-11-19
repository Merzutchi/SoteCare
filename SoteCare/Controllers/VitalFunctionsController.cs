using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using SoteCare.Models;

namespace SoteCare.Controllers
{
    public class VitalFunctionsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: VitalFunctions
        public ActionResult Index(string sortFunctions)
        {

            ViewBag.CurrentSort = sortFunctions;
            ViewBag.DateTimeSortParm = sortFunctions == "DateTime" ? "datetime_desc" : "DateTime";

            

            var vitalFunctions = db.VitalFunctions.Include(v => v.Patient);

            switch (sortFunctions)
            {
                case "DateTime":
                    vitalFunctions = vitalFunctions.OrderBy(v => v.DateTime);
                    break;

                case "datetime_desc":
                    vitalFunctions = vitalFunctions.OrderByDescending(v => v.DateTime);
                    break;

                default:
                    vitalFunctions = vitalFunctions.OrderByDescending(v => v.DateTime);
                    break;

            }



            return View(vitalFunctions.ToList());
        }



        // GET: VitalFunctions/Details/5
        public ActionResult Details(int? id)
        {

            //var latestDate = db.VitalFunctions.OrderByDescending(v => v.DateTime).FirstOrDefault();
            //ViewBag.LatestDate = latestDate; //EI TOIMI IHA VIEL AINAKAA KU KOITTAA SAADA UUSINTA DETAILII NÄKYY POTILASLISTAA


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VitalFunction vitalFunction = db.VitalFunctions.Find(id);
            if (vitalFunction == null)
            {
                return HttpNotFound();
            }

            

            return View(vitalFunction);
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
        public ActionResult Create([Bind(Include = "VitalFunctionID,PatientID,DateTime,HeartRate,SystolicBloodPressure,DiastolicBloodPressure,RespiratoryRate,Temperature,OxygenSaturation")] VitalFunction vitalFunction)
        {
            if (ModelState.IsValid)
            {
                db.VitalFunctions.Add(vitalFunction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", vitalFunction.PatientID);
            return View(vitalFunction);
        }

        // GET: VitalFunctions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VitalFunction vitalFunction = db.VitalFunctions.Find(id);
            if (vitalFunction == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", vitalFunction.PatientID);
            return View(vitalFunction);
        }

        // POST: VitalFunctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VitalFunctionID,PatientID,DateTime,HeartRate,SystolicBloodPressure,DiastolicBloodPressure,RespiratoryRate,Temperature,OxygenSaturation")] VitalFunction vitalFunction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vitalFunction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", vitalFunction.PatientID);
            return View(vitalFunction);
        }

        // GET: VitalFunctions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VitalFunction vitalFunction = db.VitalFunctions.Find(id);
            if (vitalFunction == null)
            {
                return HttpNotFound();
            }
            return View(vitalFunction);
        }

        // POST: VitalFunctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VitalFunction vitalFunction = db.VitalFunctions.Find(id);
            db.VitalFunctions.Remove(vitalFunction);
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
