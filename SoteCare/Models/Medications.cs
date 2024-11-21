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
    
    public partial class Medications
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medications()
        {
            this.Dosages = new HashSet<Dosages>();
            this.PatientMedications = new HashSet<PatientMedication>();
            this.Treatments = new HashSet<Treatment>();
        }
    
        public int MedicationID { get; set; }
        public string MedicationName { get; set; }
        public Nullable<int> DoctorID { get; set; }
        public string RefillStatus { get; set; }
        public string MedicationStatus { get; set; }
        public string Allergies { get; set; }
        public string Comments { get; set; }
        public Nullable<int> MedicationListID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dosages> Dosages { get; set; }
        public virtual MedicationList MedicationList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientMedication> PatientMedications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Treatment> Treatments { get; set; }
    }
}
