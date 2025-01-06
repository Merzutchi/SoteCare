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
                // Compares the hashed password with the one stored in the database
                if (user.Password == HashPassword(password))  
                {
                    // Stores user info in session after successful login
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
                        Session["FullName"] = "User";  // For other roles
                    }

                    // Redirects to the dashboard based on the user's role
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
                        return RedirectToAction("Index", "Home");  // Default page for other roles
                    }
                }
            }

            // Shows error message if username/password is incorrect or account is inactive
            ViewBag.ErrorMessage = "Invalid username or password, or your account is inactive.";
            return View();
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
                // Ensures FullName is not null or empty before splitting
                var nameParts = string.IsNullOrWhiteSpace(model.FullName) ? new string[] { "" } : model.FullName.Split(' ');
                var firstName = nameParts[0];
                var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

                var user = new Users
                {
                    Username = model.Username,
                    Password = HashPassword(model.Password),
                    Role = "Doctor",
                    IsActive = true
                };

                db.Users.Add(user);
                db.SaveChanges();

                // Create the doctor record in the Doctors table
                var doctor = new Doctors
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Specialization = model.Specialization,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserID = user.UserID,
                    FullName = firstName + " " + lastName // Set the FullName explicitly
                };

                db.Doctors.Add(doctor);
                db.SaveChanges();

                // After saving the doctor, link the user to the doctor
                user.DoctorID = doctor.DoctorID;
                db.SaveChanges();

                // Set session variables
                Session["UserID"] = user.UserID;
                Session["FullName"] = doctor.FullName;  // Now FullName is correctly set in the session
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
                // Check if FullName is provided and not null
                if (string.IsNullOrWhiteSpace(model.FullName))
                {
                    ModelState.AddModelError("FullName", "Full Name is required");
                    return View(model); // Return to view with error
                }

                // Split FullName into FirstName and LastName
                var nameParts = model.FullName.Split(' ');
                var firstName = nameParts[0];
                var lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : string.Empty; // Join remaining parts to form last name

                // Create a new User for the nurse
                var user = new Users
                {
                    Username = model.Username,
                    Password = HashPassword(model.Password),
                    Role = "Nurse",  // Sets role as Nurse
                    IsActive = true,
                    // Optional: Save FullName in the Users table if needed
                    // FullName = model.FullName
                };

                db.Users.Add(user);
                db.SaveChanges();

                // Create the nurse record
                var nurse = new Nurses
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserID = user.UserID // Links the user to this nurse
                };

                db.Nurses.Add(nurse);
                db.SaveChanges();

                // After saving nurse details, link the User to the Nurse
                user.NurseID = nurse.NurseID;
                db.SaveChanges();

                // Log the user in after registration
                Session["UserID"] = user.UserID;
                Session["FullName"] = nurse.FirstName + " " + nurse.LastName;  // Use FirstName and LastName from Nurse model
                Session["Role"] = user.Role;

                return RedirectToAction("Index", "Dashboard");
            }

            return View(model); // Return the view with error if model is invalid
        }

        // GET: Account/UserProfile
        public ActionResult UserProfile()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");  // Redirects if not logged in
            }

            var userId = (int)Session["UserID"];
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            //ViewModel with default values
            var userProfileViewModel = new UserProfileViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                IsActive = user.IsActive,
                Role = user.Role
            };

            // Fetches FullName, Email, and PhoneNumber from Doctors or Nurses based on the role
            if (user.Role == "Doctor")
            {
                var doctor = db.Doctors.SingleOrDefault(d => d.UserID == user.UserID);
                if (doctor != null)
                {
                    userProfileViewModel.FullName = doctor.FirstName + " " + doctor.LastName;
                    userProfileViewModel.Email = doctor.Email;  // From Doctors table
                    userProfileViewModel.PhoneNumber = doctor.PhoneNumber;  // From Doctors table
                }
            }
            else if (user.Role == "Nurse")
            {
                var nurse = db.Nurses.SingleOrDefault(n => n.UserID == user.UserID);
                if (nurse != null)
                {
                    userProfileViewModel.FullName = nurse.FirstName + " " + nurse.LastName;
                    userProfileViewModel.Email = nurse.Email;  // From Nurses table
                    userProfileViewModel.PhoneNumber = nurse.PhoneNumber;  // From Nurses table
                }
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
                    // If password change is requested, update the password
                    if (!string.IsNullOrEmpty(model.Password) && model.Password == model.ConfirmPassword)
                    {
                        user.Password = HashPassword(model.Password); // Hashes the password
                    }

                    // Updates user profile information in the Users table
                    db.SaveChanges();

                    // Updates the doctor or nurse table based on role
                    if (user.Role == "Doctor")
                    {
                        var doctor = db.Doctors.SingleOrDefault(d => d.UserID == user.UserID);
                        if (doctor != null)
                        {
                            doctor.FirstName = model.FullName.Split(' ')[0];  // Splits full name to first name
                            doctor.LastName = model.FullName.Split(' ').Length > 1 ? model.FullName.Split(' ')[1] : string.Empty;  // Split full name to last name
                            doctor.Email = model.Email;  // Updates email
                            doctor.PhoneNumber = model.PhoneNumber;  // Updates phone number
                        }
                    }
                    else if (user.Role == "Nurse")
                    {
                        var nurse = db.Nurses.SingleOrDefault(n => n.UserID == user.UserID);
                        if (nurse != null)
                        {
                            nurse.FirstName = model.FullName.Split(' ')[0];  // Splits full name to first name
                            nurse.LastName = model.FullName.Split(' ').Length > 1 ? model.FullName.Split(' ')[1] : string.Empty;  // Split full name to last name
                            nurse.Email = model.Email;  // Updates email
                            nurse.PhoneNumber = model.PhoneNumber;  // Updates phone number
                        }
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
                return RedirectToAction("Login");  // Redirects to login if user is not logged in
            }

            var userId = (int)Session["UserID"];
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            //populates the ViewModel 
            var userProfileViewModel = new UserProfileViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                Role = user.Role,
                IsActive = user.IsActive  // Don't allow editing IsActive
            };

            // Load email and phone from the relevant table (Doctors or Nurses)
            if (user.Role == "Doctor")
            {
                var doctor = db.Doctors.SingleOrDefault(d => d.UserID == user.UserID);
                if (doctor != null)
                {
                    userProfileViewModel.Email = doctor.Email;
                    userProfileViewModel.PhoneNumber = doctor.PhoneNumber;
                }
            }
            else if (user.Role == "Nurse")
            {
                var nurse = db.Nurses.SingleOrDefault(n => n.UserID == user.UserID);
                if (nurse != null)
                {
                    userProfileViewModel.Email = nurse.Email;
                    userProfileViewModel.PhoneNumber = nurse.PhoneNumber;
                }
            }

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
                    // Updates user profile information (but not IsActive)
                    user.Username = model.Username;

                    //updates the FullName, Email, and PhoneNumber in the table (Doctors or Nurses)
                    if (user.Role == "Doctor")
                    {
                        var doctor = db.Doctors.SingleOrDefault(d => d.UserID == user.UserID);
                        if (doctor != null)
                        {
                            doctor.FirstName = model.FullName.Split(' ')[0];  // First name from FullName
                            doctor.LastName = model.FullName.Split(' ').Length > 1 ? model.FullName.Split(' ')[1] : string.Empty;  // Last name from FullName
                            doctor.Email = model.Email;
                            doctor.PhoneNumber = model.PhoneNumber;
                        }
                    }
                    else if (user.Role == "Nurse")
                    {
                        var nurse = db.Nurses.SingleOrDefault(n => n.UserID == user.UserID);
                        if (nurse != null)
                        {
                            nurse.FirstName = model.FullName.Split(' ')[0];  // First name from FullName
                            nurse.LastName = model.FullName.Split(' ').Length > 1 ? model.FullName.Split(' ')[1] : string.Empty;  // Last name from FullName
                            nurse.Email = model.Email;
                            nurse.PhoneNumber = model.PhoneNumber;
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction("UserProfile");
                }
            }

            // If the model is invalid, returns to the form with error messages
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Clear the session data and log the user out
            Session.Clear();
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