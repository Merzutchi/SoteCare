using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public List<SoteCare.Models.Patients> AssignedPatients { get; set; }
    }
}