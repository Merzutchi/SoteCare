using SoteCare.Models;
using SoteCare.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace SoteCare.Controllers
{
    public class ObservationController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Observation/Index
        public ActionResult Index()
        {
            int userId = (int)(Session["UserID"] ?? 0);  
            string userRole = Session["Role"] as string;

            if (userRole == "Doctor")
            {
                ViewBag.Observations = db.Observations
                    .Where(o => o.CreatedBy == userId)  
                    .OrderByDescending(o => o.CreatedDate)
                    .Take(5)
                    .ToList();
            }
            else if (userRole == "Nurse")
            {
                ViewBag.Observations = db.Observations
                    .Where(o => o.AssignedTo == userId && o.IsCompleted == false)  
                    .OrderByDescending(o => o.CreatedDate)
                    .Take(5)
                    .ToList();
            }

            return View();  
        }

        // GET: Observation/Create
        public ActionResult Create()
        {
            // Passes a list of active nurses for selection
            ViewBag.Nurses = db.Users
                .Where(u => u.Role == "Nurse" && u.IsActive)
                .Select(u => new { u.UserID, FullName = u.Nurses.FirstOrDefault().FirstName + " " + u.Nurses.FirstOrDefault().LastName })
                .ToList();

            return View();
        }

        // POST: Observation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Observations observation, int AssignedTo)
        {
            if (ModelState.IsValid)
            {
                observation.CreatedBy = (int)Session["UserID"];
                observation.CreatedDate = DateTime.Now;
                observation.IsCompleted = false;
                observation.AssignedTo = AssignedTo;

                db.Observations.Add(observation);  // Adds the observation
                db.SaveChanges();  
                return RedirectToAction("Index", "Dashboard");  
            }

            ViewBag.Nurses = db.Users
                .Where(u => u.Role == "Nurse" && u.IsActive)
                .Select(u => new { u.UserID, FullName = u.Nurses.FirstOrDefault().FirstName + " " + u.Nurses.FirstOrDefault().LastName })
                .ToList();

            return View(observation);  
        }

        // GET: Observation/Edit/5
        public ActionResult Edit(int id)
        {
            var observation = db.Observations.Find(id);
            if (observation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Nurses = db.Users
                .Where(u => u.Role == "Nurse" && u.IsActive)
                .Select(u => new { u.UserID, FullName = u.Nurses.FirstOrDefault().FirstName + " " + u.Nurses.FirstOrDefault().LastName })
                .ToList();

            return View(observation);
        }

        // POST: Observation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Observations observation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(observation).State = EntityState.Modified;  
                db.SaveChanges();  
                return RedirectToAction("Index", "Dashboard");  
            }
            ViewBag.Nurses = db.Users
                .Where(u => u.Role == "Nurse" && u.IsActive)
                .Select(u => new { u.UserID, FullName = u.Nurses.FirstOrDefault().FirstName + " " + u.Nurses.FirstOrDefault().LastName })
                .ToList();

            return View(observation);  
        }

        // GET: Observation/Delete/5
        public ActionResult Delete(int id)
        {
            var observation = db.Observations.Find(id);
            if (observation == null)
            {
                return HttpNotFound();
            }

            return View(observation);  
        }

        // POST: Observation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var observation = db.Observations.Find(id);
            db.Observations.Remove(observation);  
            db.SaveChanges();  
            return RedirectToAction("Index", "Dashboard");  
        }
    }
}