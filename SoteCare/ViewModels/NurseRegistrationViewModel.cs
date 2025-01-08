using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SoteCare.ViewModels
{
    public class NurseRegistrationViewModel
    {

        [Display(Name = "Käyttäjänimi")]
        public string Username { get; set; }

        [Display(Name = "Salasana")]
        public string Password { get; set; }

        [Display(Name = "Koko nimi")]
        public string FullName { get; set; }

        [Display(Name = "Sähköposti")]
        public string Email { get; set; }

        [Display(Name = "Puhelinnumero")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Etunimi")]
        public string FirstName { get; set; }

        [Display(Name = "Sukunimi")]
        public string LastName { get; set; }

        [Display(Name = "Osasto")]
        public string Department { get; set; }
    }
}