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

    public partial class PatientHistory
    {
        [Display(Name = "Historia")]
        public int HistoryID { get; set; }
        [Display(Name = "Potilas")]
        public int PatientID { get; set; }
        [Display(Name = "Oireen nimi")]
        public string ConditionName { get; set; }
        [Display(Name = "Hoidon tiedot")]
        public string TreatmentDetails { get; set; }
        [Display(Name = "Leikkauspäivämäärä")]
        public Nullable<System.DateTime> SurgeryDate { get; set; }
        [Display(Name = "Huomiot")]
        public string Notes { get; set; }
        [Display(Name = "Hoitaja")]
        public Nullable<int> NurseID { get; set; }
        [Display(Name = "Lääkäri")]
        public Nullable<int> DoctorID { get; set; }
    
        public virtual Patients Patients { get; set; }
        public virtual Nurses Nurses { get; set; }
        public virtual Doctors Doctors { get; set; }
    }
}
