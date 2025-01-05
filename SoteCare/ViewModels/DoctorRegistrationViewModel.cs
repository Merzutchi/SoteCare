using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoteCare.ViewModels
{
    public class DoctorRegistrationViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
    }
}