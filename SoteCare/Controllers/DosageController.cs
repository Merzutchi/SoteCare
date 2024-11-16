using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class DosageController : Controller
    {
        // GET: Dosage
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dosage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dosage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dosage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Dosage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dosage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Dosage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dosage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
