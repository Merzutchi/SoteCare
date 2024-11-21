using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class MedicationListController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

        // GET: MedicationList
        public ActionResult ListIndex()
        {
            var medicationList = context.MedicationLists.ToList();
            return View(medicationList);
        }

        // GET: Add Medication to existing list
        public ActionResult AddMedication(int listId)
        {
            return View(listId);
        }

        // POST: Add Medication to an existing list
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMedication(int listId, int medicationId)
        {          
            return RedirectToAction("ListIndex");
        }
    }
}