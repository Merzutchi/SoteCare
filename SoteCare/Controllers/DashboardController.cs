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
            int userId = 0;
            if (Session["UserID"] != null)
            {
                userId = (int)Session["UserID"];
            }

            // Fetch role and FullName from session
            string userRole = Session["Role"] as string;
            string userFullName = Session["FullName"] as string;

            // Pass FullName to ViewBag for display in the dashboard
            ViewBag.UserFullName = userFullName;

            // General data for all users
            ViewBag.TotalPatients = db.Patients.Count();

            // Fix for AddMonths: Calculate the date in memory
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            ViewBag.NewPatients = db.Patients.Count(p => p.DateOfBirth > oneMonthAgo);

            ViewBag.TotalMedications = db.Medications.Count();
            ViewBag.ActiveMedications = db.PatientMedications.Count(m => m.EndDate == null || m.EndDate > DateTime.Now);
            ViewBag.ActiveTreatments = db.Treatment.Count(t => t.EndDate == null || t.EndDate > DateTime.Now);
            ViewBag.CompletedTreatments = db.Treatment.Count(t => t.EndDate < DateTime.Now);

            // Recent patients for dashboard display
            ViewBag.RecentPatients = db.Patients
                .OrderByDescending(p => p.PatientID)
                .Take(5)
                .ToList();

            // List of all doctors for general dashboard data
            ViewBag.Doctors = db.Doctors.ToList();

            // Role-specific data
            if (userRole == "Doctor")
            {
                // Fetch patients assigned to the logged-in doctor
                var doctorPatients = db.Patients
                    .Where(p => p.DoctorID == userId)
                    .ToList();
                ViewBag.DoctorPatients = doctorPatients;

                // Fetch recent observations created by the logged-in doctor
                var doctorObservations = db.Observations
                    .Where(o => o.CreatedBy == userId)
                    .OrderByDescending(o => o.CreatedDate)
                    .Take(5) // Limits to the 5 most recent observations
                    .Select(o => new ObservationViewModel
                    {
                        ObservationID = o.ObservationID,
                        ObservationText = o.ObservationText,
                        // Fetch the creator's full name from Doctors table
                        CreatedByName = db.Users
                            .Where(u => u.UserID == o.CreatedBy)
                            .Join(db.Doctors, u => u.UserID, d => d.UserID, (u, d) => new { u, d })
                            .Select(x => x.d.FirstName + " " + x.d.LastName) // Accessing the Doctor's FirstName and LastName
                            .FirstOrDefault(),
                        CreatedDate = o.CreatedDate ?? DateTime.Now
                    })
                    .ToList();
                ViewBag.Observations = doctorObservations;
            }
            else if (userRole == "Nurse")
            {
                // Fetch patients assigned to the logged-in nurse
                var nursePatients = db.PatientNurseAssignment
                    .Where(a => a.NurseID == userId)  // Match NurseID with logged-in user ID
                    .Select(a => a.Patients)          // Fetch the related Patient records
                    .ToList();
                ViewBag.NursePatients = nursePatients;

                // Fetch recent observations assigned to the logged-in nurse
                var nurseObservations = db.Observations
                    .Where(o => o.AssignedTo == userId)
                    .OrderByDescending(o => o.CreatedDate)
                    .Take(5) // Limits to the 5 most recent observations
                    .Select(o => new ObservationViewModel
                    {
                        ObservationID = o.ObservationID,
                        ObservationText = o.ObservationText,
                        // Fetch the creator's full name from Nurses table
                        CreatedByName = db.Users
                            .Where(u => u.UserID == o.CreatedBy)
                            .Join(db.Nurses, u => u.UserID, n => n.UserID, (u, n) => new { u, n })
                            .Select(x => x.n.FirstName + " " + x.n.LastName) // Accessing the Nurse's FirstName and LastName
                            .FirstOrDefault(),
                        CreatedDate = o.CreatedDate ?? DateTime.Now
                    })
                    .ToList();
                ViewBag.Observations = nurseObservations;
            }

            return View();
        }
    }
}