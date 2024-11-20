using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class TreatmentController : Controller
    {
        // GET: Treatment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Treatment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Treatment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Treatment/Create
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

        // GET: Treatment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Treatment/Edit/5
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

        // GET: Treatment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Treatment/Delete/5
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
