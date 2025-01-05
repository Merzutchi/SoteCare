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
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = (int)Session["UserID"];
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Fetch assigned patients for the logged-in user based on their role
            var userProfileViewModel = new UserProfileViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Role = user.Role
            };

            if (user.Role == "Doctor")
            {
                // Fetch patients assigned to the current doctor
                userProfileViewModel.AssignedPatients = db.Patients
                    .Where(p => p.DoctorID == user.DoctorID)
                    .ToList();
            }
            else if (user.Role == "Nurse")
            {
                // Fetch patients assigned to the current nurse
                userProfileViewModel.AssignedPatients = db.Patients
                    .Where(p => p.NurseID == user.NurseID)
                    .ToList();
            }

            return View(userProfileViewModel);
        }
    }
}


