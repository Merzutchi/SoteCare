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
    
    public partial class PatientMedications
    {
        public int PatientMedicationID { get; set; }
        public Nullable<int> PatientID { get; set; }
        public Nullable<int> MedicationID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> DoctorID { get; set; }
        public Nullable<int> DosageID { get; set; }
        public string Notes { get; set; }
        public string AdministrationTimes { get; set; }
        public Nullable<int> DailyFrequency { get; set; }
        public string MedicationType { get; set; }
        public string SingleDose { get; set; }
    
        public virtual Doctors Doctors { get; set; }
        public virtual Dosages Dosages { get; set; }
        public virtual Medications Medications { get; set; }
        public virtual Patients Patients { get; set; }
    }
}
