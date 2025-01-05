﻿using SoteCare.Models;
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

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            // Find the user from the database
            var user = db.Users.SingleOrDefault(u => u.Username == username);
            if (user != null)
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
            // Show error message if username or password is incorrect
            ViewBag.ErrorMessage = "Invalid username or password";
            return View();
        }

        // GET: Register
        public ActionResult Register()
        {
            return View(new Users()); // Pass an empty Users object to the view
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password,Role,FullName,Email,IsActive")] Users users)
        {
            if (ModelState.IsValid)
            {
                // Hash the password before saving
                users.Password = HashPassword(users.Password);

                // Add the new user to the database
                db.Users.Add(users);
                db.SaveChanges();

                // Log the user in after registration
                Session["UserID"] = users.UserID;
                Session["FullName"] = users.FullName;
                Session["Role"] = users.Role;

                // Redirect the user to the dashboard based on their role
                return RedirectToAction("Index", "Dashboard");
            }

            // If validation failed, return to the registration form with error messages
            return View(users);
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

        // Logout action to clear the session and redirect to login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Clear session to log the user out
            Session.Clear();

            // Redirect the user to the login page after logout
            return RedirectToAction("Login", "Account");
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