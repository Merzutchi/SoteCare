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
        public class MedicationListsController : Controller
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

            // POST: MedicationLists/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create([Bind(Include = "MedicationName, MedicationType, Description")] MedicationLists medicationList)
            {
                if (ModelState.IsValid)
                {
                    context.MedicationLists.Add(medicationList);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(medicationList);
            }

            // GET: MedicationLists/Edit/5
            public ActionResult Edit(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                MedicationLists medicationList = context.MedicationLists.Find(id);
                if (medicationList == null)
                {
                    return HttpNotFound();
                }
                return View(medicationList);
            }

            // POST: MedicationLists/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit([Bind(Include = "MedicationListID, MedicationName, MedicationType, Description")] MedicationLists medicationList)
            {
                if (ModelState.IsValid)
                {
                    context.Entry(medicationList).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(medicationList);
            }

            // GET: MedicationLists/Delete/5
            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                MedicationLists medicationList = context.MedicationLists.Find(id);
                if (medicationList == null)
                {
                    return HttpNotFound();
                }
                return View(medicationList);
            }

            // POST: MedicationLists/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                MedicationLists medicationList = context.MedicationLists.Find(id);
                context.MedicationLists.Remove(medicationList);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }


