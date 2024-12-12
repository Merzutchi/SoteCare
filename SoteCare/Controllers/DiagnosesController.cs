using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

public class DiagnosesController : Controller
{
    private PatientRecordDataEntities db = new PatientRecordDataEntities();

    // GET: Diagnoses
    public ActionResult Index()
    {
        var diagnoses = db.Diagnoses.Include(d => d.Patients);
        return View(diagnoses.ToList());
    }

    // GET: Diagnoses/Details/5
    public ActionResult Details(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Diagnoses diagnosis = db.Diagnoses.Find(id);
        if (diagnosis == null)
        {
            return HttpNotFound();
        }
        return View(diagnosis);
    }

    // GET: Diagnoses/Create
    public ActionResult Create()
    {
        ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName");
        return View();
    }

    // POST: Diagnoses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "DiagnosisID,PatientID,DiagnosisName,DiagnosisDate,Notes")] Diagnoses diagnosis)
    {
        if (ModelState.IsValid)
        {
            db.Diagnoses.Add(diagnosis);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", diagnosis.PatientID);
        return View(diagnosis);
    }

    // GET: Diagnoses/Edit/5
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Diagnoses diagnosis = db.Diagnoses.Find(id);
        if (diagnosis == null)
        {
            return HttpNotFound();
        }
        ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", diagnosis.PatientID);
        return View(diagnosis);
    }

    // POST: Diagnoses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "DiagnosisID,PatientID,DiagnosisName,DiagnosisDate,Notes")] Diagnoses diagnosis)
    {
        if (ModelState.IsValid)
        {
            db.Entry(diagnosis).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "FirstName", diagnosis.PatientID);
        return View(diagnosis);
    }

    // GET: Diagnoses/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Diagnoses diagnosis = db.Diagnoses.Find(id);
        if (diagnosis == null)
        {
            return HttpNotFound();
        }
        return View(diagnosis);
    }

    // POST: Diagnoses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Diagnoses diagnosis = db.Diagnoses.Find(id);
        db.Diagnoses.Remove(diagnosis);
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


