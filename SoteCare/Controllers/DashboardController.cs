using SoteCare.Attributes;
using SoteCare.Models;
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
            // Ensure that the session contains user role and user ID, if not redirect to login
            if (Session["UserID"] == null || Session["Role"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch UserID and Role from the session
            int userId = (int)Session["UserID"];
            string userRole = Session["Role"].ToString();

            // General data for all users (Doctors and Nurses)
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

            var doctors = db.Doctors.ToList();  // This line ensures doctors are fetched

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
                // Fetch patients assigned to the current doctor
                var doctorPatients = db.Patients.Where(p => p.DoctorID == userId).ToList();
                ViewBag.DoctorPatients = doctorPatients;

                return View("Index");
            }
            else if (userRole == "Nurse")
            {
                // Fetch patients assigned to the current nurse
                var nursePatients = db.Patients.Where(p => p.NurseID == userId).ToList();
                ViewBag.NursePatients = nursePatients;

                return View("Index");
            }
            else
            {
                return View("Index"); // Default view for other roles or no roles
            }
        }
    }
}


