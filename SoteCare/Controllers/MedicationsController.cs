using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class MedicationsController : Controller
{
    private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

    // GET: Index for Medications
    public ActionResult Index()
    {
        var medications = context.Medications.ToList();
        return View(medications);
    }

    public ActionResult CreatePartial()
    {
        return PartialView("CreateMedication");
    }

    // POST: Create Medication
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Medications medication)
    {
        if (ModelState.IsValid)
        {
            context.Medications.Add(medication);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(medication);
    }

    // GET: Delete Confirmation
    public ActionResult DeletePartial(int id)
    {
        var medication = context.Medications.Find(id);
        if (medication == null)
        {
            return HttpNotFound();
        }
        return PartialView("DeleteConfirmation", medication);
    }

    // POST: Confirm Deletion
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        var medication = context.Medications.Find(id);
        if (medication != null)
        {
            context.Medications.Remove(medication);
            context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}

