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
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Users"); // Redirect to login page if no role is found
            }

            string userRole = Session["Role"].ToString();
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            // General data for all users (Doctors and Nurses)
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

            // Role-based content
            if (userRole == "Doctor")
            {
                // Add doctor-specific data here
                var doctorPatients = db.Patients.Where(p => p.DoctorID == (int)Session["UserID"]).ToList();
                ViewBag.DoctorPatients = doctorPatients;

                // Display relevant view for doctors
                return View("DoctorDashboard");
            }
            else if (userRole == "Nurse")
            {
                // Add nurse-specific data here
                var nursePatients = db.Patients.Where(p => p.NurseID == (int)Session["UserID"]).ToList();
                ViewBag.NursePatients = nursePatients;

                // Display relevant view for nurses
                return View("NurseDashboard");
            }
            else
            {
                // Default view for other roles or no roles
                return View("GeneralDashboard");
            }
        }
    }
}