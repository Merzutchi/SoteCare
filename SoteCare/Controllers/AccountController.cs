using SoteCare.Models;
using SoteCare.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SoteCare.Controllers
{
    public class AccountController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.SingleOrDefault(u => u.Username == username);

            if (user != null && user.IsActive)
            {
                if (user.Password == HashPassword(password))
                {
                    Session["UserID"] = user.UserID;
                    Session["Role"] = user.Role;

                    // Fetches FullName based on Role
                    if (user.Role == "Doctor")
                    {
                        var doctor = db.Doctors.SingleOrDefault(d => d.UserID == user.UserID);
                        if (doctor != null)
                        {
                            Session["FullName"] = doctor.FirstName + " " + doctor.LastName;
                        }
                    }
                    else if (user.Role == "Nurse")
                    {
                        var nurse = db.Nurses.SingleOrDefault(n => n.UserID == user.UserID);
                        if (nurse != null)
                        {
                            Session["FullName"] = nurse.FirstName + " " + nurse.LastName;
                        }
                    }
                    else
                    {
                        Session["FullName"] = "User";
                    }

                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ViewBag.ErrorMessage = "Virheellinen käyttäjänimi tai salasana, tai sinun käyttäjänimi ei ole käytössä.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Kirjautumisen uloskirjautuminen
            Session.Clear(); // Poistaa istunnon tiedot
            FormsAuthentication.SignOut(); // Poistaa kirjautumisen evästeen
            return RedirectToAction("Index", "Home"); // Ohjaa kirjautumissivulle
        }

        // GET: Account/RegisterDoctor
        public ActionResult RegisterDoctor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterDoctor(DoctorRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users
                {
                    Username = model.FirstName + " " + model.LastName,
                    Password = HashPassword(model.Password),
                    Role = "Doctor",
                    IsActive = true
                };

                db.Users.Add(user);
                db.SaveChanges();

                var doctor = new Doctors
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Specialization = model.Specialization,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserID = user.UserID
                };

                db.Doctors.Add(doctor);
                db.SaveChanges();

                user.DoctorID = doctor.DoctorID;
                db.SaveChanges();

                Session["UserID"] = user.UserID;
                Session["FullName"] = doctor.FirstName + " " + doctor.LastName;
                Session["Role"] = user.Role;

                return RedirectToAction("Index", "Dashboard");
            }
            return View(model);
        }

        // GET: Account/RegisterNurse
        public ActionResult RegisterNurse()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterNurse(NurseRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users
                {
                    Username = model.FirstName + " " + model.LastName,
                    Password = HashPassword(model.Password),
                    Role = "Nurse",
                    IsActive = true
                };

                db.Users.Add(user);
                db.SaveChanges();

                var nurse = new Nurses
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserID = user.UserID
                };

                db.Nurses.Add(nurse);
                db.SaveChanges();

                user.NurseID = nurse.NurseID;
                db.SaveChanges();

                Session["UserID"] = user.UserID;
                Session["FullName"] = nurse.FirstName + " " + nurse.LastName;
                Session["Role"] = user.Role;

                return RedirectToAction("Index", "Dashboard");
            }
            return View(model);
        }

        public ActionResult UserProfile()
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

            var userProfileViewModel = new UserProfileViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                IsActive = user.IsActive,
                Role = user.Role
            };

            if (user.Role == "Doctor")
            {
                var doctor = db.Doctors.SingleOrDefault(d => d.UserID == user.UserID);
                if (doctor != null)
                {
                    userProfileViewModel.FullName = doctor.FirstName + " " + doctor.LastName;
                    userProfileViewModel.Email = doctor.Email;
                    userProfileViewModel.PhoneNumber = doctor.PhoneNumber;
                }
            }
            else if (user.Role == "Nurse")
            {
                var nurse = db.Nurses.SingleOrDefault(n => n.UserID == user.UserID);
                if (nurse != null)
                {
                    userProfileViewModel.FullName = nurse.FirstName + " " + nurse.LastName;
                    userProfileViewModel.Email = nurse.Email;
                    userProfileViewModel.PhoneNumber = nurse.PhoneNumber;

                    var assignedPatients = db.PatientNurseAssignment
                        .Where(a => a.NurseID == nurse.NurseID)
                        .Include(a => a.Patients)
                        .Include(a => a.Doctors)
                        .Select(a => new AssignedPatientViewModel
                        {
                            FirstName = a.Patients.FirstName,
                            LastName = a.Patients.LastName,
                            AssignmentDate = a.AssignmentDate,
                            DoctorName = a.Doctors.FirstName + " " + a.Doctors.LastName
                        })
                        .ToList();

                    userProfileViewModel.AssignedPatients = assignedPatients;
                }
            }
            return View(userProfileViewModel);
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}