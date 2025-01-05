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
            // Check if user is logged in (This is redundant because of the AuthorizeUser attribute, 
            // but left here for context on additional logic you might want to do)
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Account");
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

            var doctors = db.Doctors.ToList();  // This line ensures doctors are fetched
            ViewBag.TotalPatients = totalPatients;
            ViewBag.NewPatients = newPatients;
            ViewBag.TotalMedications = totalMedications;
            ViewBag.ActiveMedications = activeMedications;
            ViewBag.ActiveTreatments = activeTreatments;
            ViewBag.CompletedTreatments = completedTreatments;
            ViewBag.RecentPatients = recentPatients;
            ViewBag.Doctors = doctors;  // Assign doctors to ViewBag

            // Role-based content
            if (userRole == "Doctor")
            {
                // Fetch patients assigned to the current doctor
                var doctorPatients = db.Patients.Where(p => p.DoctorID == (int)Session["UserID"]).ToList();
                ViewBag.DoctorPatients = doctorPatients;

                // Return the general dashboard view for doctors (same as for others, but with doctor-specific data)
                return View("Index");
            }
            else if (userRole == "Nurse")
            {
                // Fetch patients assigned to the current nurse
                var nursePatients = db.Patients.Where(p => p.NurseID == (int)Session["UserID"]).ToList();
                ViewBag.NursePatients = nursePatients;

                // Return the general dashboard view for nurses
                return View("Index"); // Same view, just with nurse-specific data
            }
            else
            {
                // Default view for other roles or no roles
                return View("Index");
            }
        }
    }
}


