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
    public class NursesController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Nurses
        public ActionResult Index()
        {
            return View(db.Nurses.ToList());
        }

        // GET: Nurses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nurses nurses = db.Nurses.Find(id);
            if (nurses == null)
            {
                return HttpNotFound();
            }
            return View(nurses);
        }

        // GET: Nurses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nurses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NurseID,FirstName,LastName,PhoneNumber,Email")] Nurses nurses)
        {
            if (ModelState.IsValid)
            {
                db.Nurses.Add(nurses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nurses);
        }

        // GET: Nurses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nurses nurses = db.Nurses.Find(id);
            if (nurses == null)
            {
                return HttpNotFound();
            }
            return View(nurses);
        }

        // POST: Nurses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NurseID,FirstName,LastName,PhoneNumber,Email")] Nurses nurses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nurses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nurses);
        }

        // GET: Nurses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nurses nurses = db.Nurses.Find(id);
            if (nurses == null)
            {
                return HttpNotFound();
            }
            return View(nurses);
        }

        // POST: Nurses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)
        {
            var nurses = db.Nurses
            .Include("PatientNurseAssignment")
            .FirstOrDefault(m => m.NurseID == id);

            if (nurses == null)
            {
                return HttpNotFound();
            }

            // Remove related PatientNurseAssignment first
            db.PatientNurseAssignment.RemoveRange(nurses.PatientNurseAssignment);

            // Remove the Nurse record
            db.Nurses.Remove(nurses);

            // Save changes
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Nurses nurses = db.Nurses.Find(id);
        //    db.Nurses.Remove(nurses);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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

