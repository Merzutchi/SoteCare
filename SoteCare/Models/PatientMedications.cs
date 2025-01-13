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

    public partial class PatientMedications
    {
        [Display(Name = "Lääke")]
        public int PatientMedicationID { get; set; }

        [Display(Name = "Potilas")]
        public Nullable<int> PatientID { get; set; }

        [Display(Name = "Lääke")]
        public Nullable<int> MedicationID { get; set; }

        [Display(Name = "Aloituspäivämäärä")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Display(Name = "Lopetuspäivämäärä")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "Lääkäri")]
        public Nullable<int> DoctorID { get; set; }

        [Display(Name = "Annos")]
        public Nullable<int> DosageID { get; set; }

        [Display(Name = "Huomautukset")]
        public string Notes { get; set; }

        public virtual Medications Medications { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual Dosages Dosages { get; set; }
        public virtual Doctors Doctors { get; set; }
    }
}
