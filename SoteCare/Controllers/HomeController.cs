using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{

    

    public class HomeController : Controller
    {

        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact2()
        {
            ViewBag.Message = "Your contact page.";

            return View(db.Patients.ToList());
        }
    }
}