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
            var oneMonthAgo = DateTime.Now.AddMonths(-1); 

            var totalPatients = db.Patients.Count();
            var newPatients = db.Patients
                                .Where(p => p.DateOfBirth > oneMonthAgo) 
                                .Count();

            var totalMedications = db.Medications.Count();
            var activeMedications = db.PatientMedications
                                .Count(m => m.EndDate == null || m.EndDate > DateTime.Now);

            var activeTreatments = db.Treatment
                                .Count(t => t.EndDate == null || t.EndDate > DateTime.Now);
            var completedTreatments = db.Treatment
                                .Count(t => t.EndDate < DateTime.Now);

            var recentPatients = db.Patients
                                .OrderByDescending(p => p.PatientID)
                                .Take(5)
                                .ToList();

            var doctors = db.Doctors.ToList();

            ViewBag.TotalPatients = totalPatients;
            ViewBag.NewPatients = newPatients;

            ViewBag.TotalMedications = totalMedications;
            ViewBag.ActiveMedications = activeMedications;

            ViewBag.ActiveTreatments = activeTreatments;
            ViewBag.CompletedTreatments = completedTreatments;

            ViewBag.RecentPatients = recentPatients;
            ViewBag.Doctors = doctors;

            return View();
        }
    }
}