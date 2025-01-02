using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class UsersController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Users Usernames, passwords
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Username,Password,Role,FullName,Email,PhoneNumber,DateOfBirth,IsActive")] Users users)
        {
            if (ModelState.IsValid)
            {
                // Hash the password before saving
                users.Password = HashPassword(users.Password);

                // Add the user to the database
                db.Users.Add(users);
                db.SaveChanges();

                // Store user info in the session after successful registration
                Session["UserID"] = users.UserID;
                Session["FullName"] = users.FullName;
                Session["Role"] = users.Role;

                // Redirect based on the role
                if (users.Role == "Teacher")
                {
                    return RedirectToAction("Index", "Dashboard"); // Redirect to the teacher's dashboard
                }
                else if (users.Role == "Doctor")
                {
                    return RedirectToAction("Index", "Dashboard"); // Redirect to the doctor's dashboard
                }
                else if (users.Role == "Nurse")
                {
                    return RedirectToAction("Index", "Dashboard"); // Redirect to the nurse's dashboard
                }

                // Redirect to a default page for other roles
                return RedirectToAction("Index", "Dashboard");
            }

            return View(users); // If the model is not valid, return to the form
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Username,Password,Role,FullName,Email,PhoneNumber,DateOfBirth,IsActive")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "UserID,Username,Password,Role,FullName,Email,PhoneNumber,DateOfBirth,IsActive")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
                    // Store user info in the session
                    Session["UserID"] = user.UserID;
                    Session["FullName"] = user.FullName;
                    Session["Role"] = user.Role;

                    // Redirect based on role
                    if (user.Role == "Teacher")  // Teacher (Doctor) role
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (user.Role == "Student")  // Student (Nurse) role
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }

            ViewBag.ErrorMessage = "Invalid username or password";
            return View(); // Show error if invalid login
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Clear session on logout
            Session.Clear();

            // Redirect to the Home controller's Index action (the home page)
            return RedirectToAction("Index", "Home");
        }

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

            // Assuming the user is a nurse, load the nurse information as well
            var nurse = db.Nurses.FirstOrDefault(n => n.Users.Any(u => u.UserID == userId));

            // Check for other roles (e.g., Doctor) if needed
            var doctor = db.Doctors.FirstOrDefault(d => d.Users.Any(u => u.UserID == userId));

            ViewBag.Nurse = nurse;
            ViewBag.Doctor = doctor;

            return View(user); // Pass the user object to the view
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            { 
                // Compute hash from the password string.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string.
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public ActionResult NurseDetails(int id)
        {
            // Fetch the nurse by ID, including the related Users
            var nurse = db.Nurses.Include(n => n.Users).FirstOrDefault(n => n.NurseID == id);

            if (nurse == null)
            {
                return HttpNotFound("Nurse not found");
            }

            // Access UserID from the associated Users collection
            var user = nurse.Users.FirstOrDefault();  // Get the first related user

            if (user != null)
            {
                var userId = user.UserID;  // Now you can safely access the UserID
                return View(user);  // Pass the user object to the view
            }

            return HttpNotFound("User not found");
        }

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
                // Hash the password before saving it
                users.Password = HashPassword(users.Password);

                // The Role will be assigned based on the input
                // If it's a teacher, assign the role as 'Teacher' (Doctor)
                // If it's a student, assign the role as 'Student' (Nurse)
                db.Users.Add(users);
                db.SaveChanges();

                // Redirect to login page after successful registration
                return RedirectToAction("Login");
            }
            return View(users);
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

