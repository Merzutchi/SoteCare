//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoteCare.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Users
    {
        [Display(Name = "KäyttäjäID")]
        public int UserID { get; set; }

        [Display(Name = "Käyttäjätunnus")]
        public string Username { get; set; }

        [Display(Name = "Salasana")]
        public string Password { get; set; }

        [Display(Name = "Rooli")]
        public string Role { get; set; }

        [Display(Name = "Koko nimi")]
        public string FullName { get; set; }

        [Display(Name = "Sähköposti")]
        public string Email { get; set; }

        [Display(Name = "Puhelinnumero")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Syntymäaika")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
