using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class DashboardController : Controller
    {
        private readonly PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            // Patient Statistics
            int totalPatients = db.Patients.Count();
            var recentPatients = db.Patients
                .OrderByDescending(p => p.PatientID)
                .Take(5)
                .ToList();

            // Doctor List
            var doctors = db.Doctors.ToList();

            // Active Treatments
            var activeTreatments = db.Treatment
                .Where(t => t.EndDate == null || t.EndDate > DateTime.Now)
                .ToList();

            ViewBag.TotalPatients = totalPatients;
            ViewBag.RecentPatients = recentPatients;
            ViewBag.Doctors = doctors;
            ViewBag.ActiveTreatments = activeTreatments;

            return View();
        }
    }
}