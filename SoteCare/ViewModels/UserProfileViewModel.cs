using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SoteCare.ViewModels
{
    public class UserProfileViewModel
    {
        // User Profile Information
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        // Password Change Information
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        // Assigned Patients
        public List<AssignedPatientViewModel> AssignedPatients { get; set; }
    }
    public class AssignedPatientViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string DoctorName { get; set; }  // Doctor's name who assigned the patient
    }
}   