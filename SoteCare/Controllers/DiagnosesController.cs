using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class DiagnosesController : Controller
    {
        // GET: Diagnoses
        public ActionResult Index()
        {
            return View();
        }

        // GET: Diagnoses/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Diagnoses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Diagnoses/Create
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

        // GET: Diagnoses/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Diagnoses/Edit/5
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

        // GET: Diagnoses/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Diagnoses/Delete/5
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
