using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class TreatmentDetailsController : Controller
    {
        // GET: TreatmentDetails
        public ActionResult Index()
        {
            return View();
        }

        // GET: TreatmentDetails/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TreatmentDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TreatmentDetails/Create
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

        // GET: TreatmentDetails/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TreatmentDetails/Edit/5
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

        // GET: TreatmentDetails/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TreatmentDetails/Delete/5
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
