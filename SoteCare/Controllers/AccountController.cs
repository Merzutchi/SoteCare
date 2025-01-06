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
            // Find the user from the database
            var user = db.Users.SingleOrDefault(u => u.Username == username);

            if (user != null && user.IsActive)
            {
                // Compare the hashed password with the one stored in the database
                if (user.Password == HashPassword(password))
                {
                    // Store user info in session after successful login
                    Session["UserID"] = user.UserID;
                    Session["FullName"] = user.FullName;
                    Session["Role"] = user.Role;

                    // Redirect to the dashboard based on the user's role
                    if (user.Role == "Doctor")
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (user.Role == "Nurse")
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Dashboard"); // Default
                    }
                }
            }

            // Show error message if username/password is incorrect or account is inactive
            ViewBag.ErrorMessage = "Invalid username or password, or your account is inactive.";
            return View();
        }

        // GET: Account/RegisterDoctor
        public ActionResult RegisterDoctor()
        {
            return View();
        }

        // POST: Account/RegisterDoctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterDoctor(DoctorRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new User for the doctor
                var user = new Users
                {
                    Username = model.Username,
                    Password = HashPassword(model.Password),
                    Role = "Doctor", // Set role as doctor
                    FullName = model.FullName,  // Save the FullName in Users table
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    IsActive = true
                };

                db.Users.Add(user);
                db.SaveChanges();

                // Split FullName into FirstName and LastName
                var nameParts = model.FullName.Split(' ');
                var firstName = nameParts[0];
                var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

                // Now create the doctor record
                var doctor = new Doctors
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Specialization = model.Specialization,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email
                };

                db.Doctors.Add(doctor);
                db.SaveChanges();

                // After saving doctor details, link the User to the Doctor
                user.DoctorID = doctor.DoctorID;
                db.SaveChanges();

                // Log the user in after registration
                Session["UserID"] = user.UserID;
                Session["FullName"] = user.FullName;
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

        // POST: Account/RegisterNurse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterNurse(NurseRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new User for the nurse
                var user = new Users
                {
                    Username = model.Username,
                    Password = HashPassword(model.Password),
                    Role = "Nurse", // Set role as nurse
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    IsActive = true
                };

                db.Users.Add(user);
                db.SaveChanges();

                // Now create the nurse record
                var nurse = new Nurses
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email
                };

                db.Nurses.Add(nurse);
                db.SaveChanges();

                // After saving nurse details, link the User to the Nurse
                user.NurseID = nurse.NurseID;
                db.SaveChanges();

                // Log the user in after registration
                Session["UserID"] = user.UserID;
                Session["FullName"] = user.FullName;
                Session["Role"] = user.Role;

                return RedirectToAction("Index", "Dashboard");
            }

            return View(model);
        }

        // GET: Account/UserProfile
        public ActionResult UserProfile()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");  // Redirect if not logged in
            }

            var userId = (int)Session["UserID"];
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Create ViewModel with the user data
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

            // Fetch assigned patients for the logged-in user (depending on role)
            if (user.Role == "Doctor")
            {
                userProfileViewModel.AssignedPatients = db.Patients
                    .Where(p => p.DoctorID == user.UserID)
                    .ToList();
            }
            else if (user.Role == "Nurse")
            {
                userProfileViewModel.AssignedPatients = db.Patients
                    .Where(p => p.NurseID == user.UserID)
                    .ToList();
            }

            return View(userProfileViewModel);
        }

        // POST: Account/UserProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.UserID);
                if (user != null)
                {
                    // Update user profile information
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    // If password change is requested, update the password
                    if (!string.IsNullOrEmpty(model.Password) && model.Password == model.ConfirmPassword)
                    {
                        user.Password = HashPassword(model.Password); // Hash the password
                    }

                    db.SaveChanges();
                    return RedirectToAction("UserProfile");
                }
            }

            // If the model is invalid, return to the form with error messages
            return View(model);
        }

        // GET: Account/EditProfile
        public ActionResult EditProfile()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");  // Redirect to login if user is not logged in
            }

            var userId = (int)Session["UserID"];
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Create and populate the ViewModel 
            var userProfileViewModel = new UserProfileViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive, //still passes IsActive but don't allow editing
                Role = user.Role
            };

            return View(userProfileViewModel);  // Pass the ViewModel to the view
        }

        // POST: Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.UserID);
                if (user != null)
                {
                    // Update user profile information (but not IsActive)
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    db.SaveChanges();

                    // Redirect back to the profile page after saving changes
                    return RedirectToAction("UserProfile");
                }
            }

            // If the model is invalid, return the form with error messages
            return View(model);
        }

        // POST: Account/Deactivate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateAccount()
        {
            var userId = (int)Session["UserID"];
            var user = db.Users.Find(userId);

            if (user != null)
            {
                user.IsActive = false;  // Set the account as inactive

                // Save the changes
                db.SaveChanges();
            }

            // Log the user out after deactivating the account
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Logout action to clear the session and redirect to login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Clear session to log the user out
            Session.Clear();

            // Redirect the user to the login page after logout
            return RedirectToAction("Index", "Home");
        }

        //method to hash password
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute hash from the password string
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a hex string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Dispose the database context when done
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