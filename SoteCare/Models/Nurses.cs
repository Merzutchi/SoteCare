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

    public partial class Nurses
    {
        [Display(Name = "HoitajaID")]
        public int NurseID { get; set; }

        [Display(Name = "Etunimi")]
        public string FirstName { get; set; }

        [Display(Name = "Sukunimi")]
        public string LastName { get; set; }

        [Display(Name = "Osasto")]
        public string Department { get; set; }

        [Display(Name = "Puhelinnumero")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Sähköposti")]
        public string Email { get; set; }
    }
}
