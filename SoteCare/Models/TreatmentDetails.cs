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
    using System.ComponentModel.DataAnnotations;  // Add this for Display attribute

    public partial class TreatmentDetails
    {
        [Display(Name = "Hoitotieto")]
        public int TreatmentDetailID { get; set; }

        [Display(Name = "Hoito")]
        public int TreatmentID { get; set; }

        [Display(Name = "L��ke")]
        public int MedicationID { get; set; }

        [Display(Name = "Annos")]
        public Nullable<int> DosageID { get; set; }

        [Display(Name = "L��k�ri")]
        public Nullable<int> DoctorID { get; set; }

        public virtual Dosages Dosages { get; set; }
        public virtual Medications Medications { get; set; }
        public virtual Treatment Treatment { get; set; }
        public virtual Doctors Doctors { get; set; }
    }
}

