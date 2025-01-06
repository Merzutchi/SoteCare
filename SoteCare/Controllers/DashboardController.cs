using SoteCare.Attributes;
using SoteCare.Models;
using SoteCare.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    [AuthorizeUser]  // Ensure the user is logged in
    public class DashboardController : Controller
    {
        private readonly PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            // Ensures Session["UserID"] is not null and cast it to int
            int userId = 0;
            if (Session["UserID"] != null)
            {
                userId = (int)Session["UserID"];
            }

            // Fetches role from session
            string userRole = Session["Role"] as string;

            // Fetches FullName from session
            string userFullName = Session["FullName"] as string;
            ViewBag.UserFullName = userFullName;  // Passes to ViewBag

            // General data for all users
            var totalPatients = db.Patients.Count();
            ViewBag.TotalPatients = totalPatients;

            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var newPatients = db.Patients.Count(p => p.DateOfBirth > oneMonthAgo);
            ViewBag.NewPatients = newPatients;

            var totalMedications = db.Medications.Count();
            ViewBag.TotalMedications = totalMedications;

            var activeMedications = db.PatientMedications.Count(m => m.EndDate == null || m.EndDate > DateTime.Now);
            ViewBag.ActiveMedications = activeMedications;

            var activeTreatments = db.Treatment.Count(t => t.EndDate == null || t.EndDate > DateTime.Now);
            ViewBag.ActiveTreatments = activeTreatments;

            var completedTreatments = db.Treatment.Count(t => t.EndDate < DateTime.Now);
            ViewBag.CompletedTreatments = completedTreatments;

            var recentPatients = db.Patients.OrderByDescending(p => p.PatientID).Take(5).ToList();
            ViewBag.RecentPatients = recentPatients;

            var doctors = db.Doctors.ToList();
            ViewBag.Doctors = doctors;

            // Role-based data
            if (userRole == "Doctor")
            {
                var doctorPatients = db.Patients.Where(p => p.DoctorID == userId).ToList(); // Use userId here
                ViewBag.DoctorPatients = doctorPatients;
            }

            if (userRole == "Nurse")
            {
                var nursePatients = db.Patients.Where(p => p.NurseID == userId).ToList(); // Use userId here
                ViewBag.NursePatients = nursePatients;
            }

            return View();
        }

    }
}