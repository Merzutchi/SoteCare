using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    namespace SoteCare.Controllers
    {
        public class MedicationListController : Controller
        {
            private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

            // GET: MedicationList
            public ActionResult Index()
            {
                var medicationLists = context.MedicationLists.ToList();
                return View(medicationLists);
            }

            public ActionResult Create()
            {
                return View();
            }

            // GET: MedicationList/Details/5
            public ActionResult Details(int id)
            {
                var medicationList = context.MedicationLists.Find(id);
                if (medicationList == null)
                {
                    return HttpNotFound();
                }
                return View(medicationList);
            }

            // POST: MedicationList/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(MedicationLists medicationList)
            {
                if (ModelState.IsValid)
                {
                    context.MedicationLists.Add(medicationList);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(medicationList);
            }

            // GET: MedicationList/Edit/5
            public ActionResult Edit(int id)
            {
                var medicationList = context.MedicationLists.Find(id);
                if (medicationList == null)
                {
                    return HttpNotFound();
                }
                return View(medicationList);
            }

            // POST: MedicationList/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(MedicationLists medicationList)
            {
                if (ModelState.IsValid)
                {
                    context.Entry(medicationList).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(medicationList);
            }

            // GET: MedicationList/Delete/5
            public ActionResult Delete(int id)
            {
                var medicationList = context.MedicationLists.Find(id);
                if (medicationList == null)
                {
                    return HttpNotFound();
                }
                return View(medicationList);
            }

            // POST: MedicationList/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                var medicationList = context.MedicationLists.Find(id);
                if (medicationList != null)
                {
                    context.MedicationLists.Remove(medicationList);
                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
    }
}

