using SoteCare.Attributes;
using SoteCare.Models;
using SoteCare.ViewModels;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    [AuthorizeUser]  // Ensures the user is logged in
    public class DashboardController : Controller
    {
        private readonly PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            int userId = 0;
            if (Session["UserID"] != null)
            {
                userId = (int)Session["UserID"];
            }

            // Fetches user role and FullName from session
            string userRole = Session["Role"] as string;
            string userFullName = Session["FullName"] as string;

            // Passes FullName to ViewBag to display in dashboard
            ViewBag.UserFullName = userFullName;

            // General data for all users
            ViewBag.TotalPatients = db.Patients.Count();
            DateTime fewDaysAgo = DateTime.Now.AddDays(-2); // Adjust -2 to the number of days you want
            ViewBag.NewPatients = db.Patients.Count(p => p.DateOfBirth > fewDaysAgo);
            //DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            //ViewBag.NewPatients = db.Patients.Count(p => p.DateOfBirth > oneMonthAgo);

            ViewBag.TotalMedications = db.Medications.Count();
            ViewBag.ActiveMedications = db.PatientMedications.Count(m => m.EndDate == null || m.EndDate > DateTime.Now);
            ViewBag.ActiveTreatments = db.Treatment.Count(t => t.EndDate == null || t.EndDate > DateTime.Now);
            ViewBag.CompletedTreatments = db.Treatment.Count(t => t.EndDate < DateTime.Now);

            // Recent patients for dashboard display
            ViewBag.RecentPatients = db.Patients
                    .Include(p => p.PatientRooms) // Lataa potilaan huonetiedot
                    .OrderByDescending(p => p.PatientID)
                    .Take(5)
                    .ToList();


            // List of all doctors for general dashboard data
            ViewBag.Doctors = db.Doctors.ToList();

            // Role-specific data
            if (userRole == "Doctor")
            {
                // Fetches patients assigned to the logged-in doctor
                var doctorPatients = db.Patients
                    .Where(p => p.DoctorID == userId)
                    .ToList();
                ViewBag.DoctorPatients = doctorPatients;
            }
            else if (userRole == "Nurse")
            {
                // Fetches patients assigned to the logged-in nurse
                var nursePatients = db.PatientNurseAssignment
                    .Where(a => a.NurseID == userId)  
                    .Select(a => a.Patients)         
                    .ToList();
                ViewBag.NursePatients = nursePatients;
            }
            return View();
        }
    }
}