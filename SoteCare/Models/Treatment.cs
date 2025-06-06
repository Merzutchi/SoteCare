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
    
    public partial class Treatment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Treatment()
        {
            this.TreatmentDetails = new HashSet<TreatmentDetails>();
        }
    
        public int TreatmentID { get; set; }
        public int PatientID { get; set; }
        public Nullable<int> MedicationID { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string TreatmentType { get; set; }
        public string Notes { get; set; }
        public Nullable<int> DiagnosisID { get; set; }
        public Nullable<int> DoctorID { get; set; }
    
        public virtual Diagnoses Diagnoses { get; set; }
        public virtual Medications Medications { get; set; }
        public virtual Patients Patients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreatmentDetails> TreatmentDetails { get; set; }
    }
}
