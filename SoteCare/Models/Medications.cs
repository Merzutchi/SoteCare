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
            this.Treatment = new HashSet<Treatment>();
            this.Dosages = new HashSet<Dosage>();
        }
    
        public int MedicationID { get; set; }
        public int PatientID { get; set; }
        public string MedicationName { get; set; }        
        public Nullable<int> DoctorID { get; set; }
        public string RefillStatus { get; set; }
        public string MedicationStatus { get; set; }
        public string Allergies { get; set; }
        public string Comments { get; set; }
    
        public virtual Patients Patients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Treatment> Treatment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dosage> Dosages { get; set; }
    }
}
