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

    public ActionResult AddDosagePartial(int medicationId)
    {
        var medication = context.Medications.Find(medicationId);
        if (medication == null)
        {
            return HttpNotFound();
        }

        ViewBag.MedicationName = medication.MedicationName;
        var dosage = new Dosages { MedicationID = medicationId };
        return PartialView("_AddDosagePartial", dosage);  
    }

    public ActionResult AddDosage(int medicationId)
    {
        var medication = context.Medications.Find(medicationId);
        if (medication == null)
        {
            return HttpNotFound();
        }

        ViewBag.MedicationName = medication.MedicationName;
        var dosage = new Dosages { MedicationID = medicationId };
        return PartialView("_AddDosagePartial", dosage);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddDosage(Dosages dosage)
    {
        if (ModelState.IsValid)
        {
            context.Dosages.Add(dosage);
            context.SaveChanges();
            return Json(new { success = true, message = "Dosage added successfully." });
        }

        return Json(new { success = false, message = "Failed to add dosage. Please check the details." });
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
            return RedirectToAction("Index"); // Redirect to Index after successful creation
        }
        return View(medication); // Return the same view if the model is invalid
    }

    // GET: Delete Confirmation
    //public ActionResult DeletePartial(int id)
    //{
    //    var medication = context.Medications.Find(id);
    //    if (medication == null)
    //    {
    //        return HttpNotFound(); // Medication not found, return 404 error
    //    }
    //    return PartialView("DeletePartial", medication); // Return the correct partial view for deletion
    //}

    // POST: Confirm Deletion
    //[HttpPost, ActionName("DeleteConfirmed")]
    //[ValidateAntiForgeryToken]
    //public ActionResult DeleteConfirmed(int id)
    //{
    //    var medication = context.Medications.Find(id);
    //    if (medication != null)
    //    {
    //        context.Medications.Remove(medication);
    //        context.SaveChanges();
    //    }
    //    return RedirectToAction("Index");
    //}
}

//Tällä hetkellä ei tarvetta olla delete optionia lääkehommille, joten se on kommentoitu pois.