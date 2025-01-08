using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoteCare.Models;
using SoteCare.ViewModels;

namespace SoteCare.Controllers
{
    public class ObservationsController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Observations
        public ActionResult Index()
        {
            var observations = db.Observations
                .OrderByDescending(o => o.CreatedDate)  // Sorts by most recent first
                .Select(o => new ObservationViewModel
                {
                    ObservationID = o.ObservationID,
                    ObservationText = o.ObservationText,
                    CreatedByName = db.Users
                        .Where(u => u.UserID == o.CreatedBy)
                        .Select(u =>
                            u.Role == "Doctor"
                                ? db.Doctors
                                    .Where(d => d.UserID == u.UserID)
                                    .Select(d => d.FirstName + " " + d.LastName)
                                    .FirstOrDefault()
                                : u.Role == "Nurse"
                                    ? db.Nurses
                                        .Where(n => n.UserID == u.UserID)
                                        .Select(n => n.FirstName + " " + n.LastName)
                                        .FirstOrDefault()
                                    : "Unknown")
                        .FirstOrDefault(),

                    CreatedDate = o.CreatedDate ?? DateTime.Now,
                    IsCompleted = o.IsCompleted ?? false
                })
                .ToList();

            return View(observations);
        }

        // GET: Observations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Observations observations = db.Observations.Find(id);
            if (observations == null)
            {
                return HttpNotFound();
            }
            return View(observations);
        }

        // GET: Observations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Observations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Observations observation)
        {
            if (ModelState.IsValid)
            {
                // Get the logged-in user ID from the session
                int userId = (int)Session["UserID"];

                observation.CreatedBy = userId;
                observation.CreatedDate = DateTime.Now;
                observation.IsCompleted = false; // Default status for observation
                observation.AssignedTo = userId; // Ensures AssignedTo is set to a valid UserID

                db.Observations.Add(observation);
                db.SaveChanges();

                return RedirectToAction("Index", "Observations");
            }

            return View(observation);
        }

        // GET: Observations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Observations observations = db.Observations.Find(id);
            if (observations == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedTo = new SelectList(db.Users, "UserID", "Username", observations.AssignedTo);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "Username", observations.CreatedBy);
            return View(observations);
        }

        // POST: Observations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ObservationID,CreatedBy,AssignedTo,ObservationText,IsCompleted,CreatedDate")] Observations observations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(observations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedTo = new SelectList(db.Users, "UserID", "Username", observations.AssignedTo);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "Username", observations.CreatedBy);
            return View(observations);
        }

        // GET: Observations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Observations observation = db.Observations.Find(id);
            if (observation == null)
            {
                return HttpNotFound();
            }
            return View(observation);
        }

        // POST: Observations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Observations observation = db.Observations.Find(id);
            db.Observations.Remove(observation);
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
