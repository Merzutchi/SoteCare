using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;


namespace SoteCare.Controllers
{
    public class MedicationsController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

       public ActionResult Index()
        {
            var medications = context.Medications.ToList();
            return View(medications);

        }

        public ActionResult Create()
        {
            return View();
        }

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

        public ActionResult DeletePartial(int id)
        {
            var medication = context.Medications.Find(id);
            if (medication == null)
            {
                return HttpNotFound();
            }
            return PartialView("DeleteConfirmation", medication);  
        }

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var medication = context.Medications.Find(id);
        //    if (medication == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(medication);
        //}

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
}



