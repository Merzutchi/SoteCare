using SoteCare.Models;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.SingleOrDefault(u => u.Username == username);
            if (user != null)
            {
                // Compare hashed password
                if (user.Password == HashPassword(password))
                {
                    // Store user info in session
                    Session["UserID"] = user.UserID;
                    Session["FullName"] = user.FullName;
                    Session["Role"] = user.Role;

                    // Redirect based on role
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ViewBag.ErrorMessage = "Invalid username or password";
            return View(); // Show error if invalid login
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password,Role,FullName,Email,PhoneNumber,DateOfBirth,IsActive")] Users users)
        {
            if (ModelState.IsValid)
            {
                // Role mapping
                if (users.Role == "Teacher") users.Role = "Doctor";
                if (users.Role == "Student") users.Role = "Nurse";

                // Hash the password before saving
                users.Password = HashPassword(users.Password);

                db.Users.Add(users);
                db.SaveChanges();

                // Automatically log in the user
                Session["UserID"] = users.UserID;
                Session["FullName"] = users.FullName;
                Session["Role"] = users.Role;

                // Redirect to the Dashboard based on the role
                return RedirectToAction("Index", "Dashboard");
            }

            return View(users); // Return to the registration form if the model is invalid
        }

        // GET: User Profile
        public ActionResult UserProfile()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");  // Redirect to login page if not logged in
            }

            var userId = (int)Session["UserID"];
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Depending on the user's role, fetch additional information
            ViewBag.Role = user.Role;

            return View(user); // Pass the user object to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Clear session on logout
            Session.Clear();

            // Redirect to the Home controller's Index action
            return RedirectToAction("Index", "Home");
        }

        //method to hash password
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute hash from the password string
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
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