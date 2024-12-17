using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        private PatientRecordDataEntities db = new PatientRecordDataEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            // patient stats
            var totalPatients = db.Patients.Count();
            var oneYearAgo = DateTime.Now.AddYears(-1); 
            var newPatients = db.Patients.Where(p => p.DateOfBirth > oneYearAgo).Count();

            // recent patient activity
            var recentPatients = db.Patients
                .OrderByDescending(p => p.PatientID)
                .Take(5)
                .Select(p => new
                {
                    p.PatientID,
                    FullName = p.FirstName + " " + p.LastName,
                    p.DateOfBirth,
                    p.Gender
                })
                .ToList();

            // doctor list
            var doctors = db.Doctors.Select(d => new
            {
                FullName = d.FirstName + " " + d.LastName,
                d.Specialization,
                d.PhoneNumber,
                d.Email
            }).ToList();

            ViewBag.TotalPatients = totalPatients;
            ViewBag.NewPatients = newPatients;
            ViewBag.RecentPatients = recentPatients;
            ViewBag.Doctors = doctors;
            return View();
        }
    }
}
    
