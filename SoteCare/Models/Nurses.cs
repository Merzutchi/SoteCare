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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nurses()
        {
            this.Patients = new HashSet<Patients>();
            this.VitalFunctions = new HashSet<VitalFunctions>();
            this.PatientHistory = new HashSet<PatientHistory>();
            this.PatientNurseAssignment = new HashSet<PatientNurseAssignment>();
        }
        [Display(Name = "Hoitaja")]
        public int NurseID { get; set; }
        [Display(Name = "K�ytt�j�")]
        public Nullable<int> UserID { get; set; }
        [Display(Name = "Etunimi")]
        public string FirstName { get; set; }
        [Display(Name = "Sukunimi")]
        public string LastName { get; set; }
        [Display(Name = "Puhelinnumero")]
        public string PhoneNumber { get; set; }
        [Display(Name = "S�hk�posti")]
        public string Email { get; set; }
        [Display(Name = "Aktiivisuus")]
        public bool IsActive { get; set; }
    
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patients> Patients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VitalFunctions> VitalFunctions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientHistory> PatientHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientNurseAssignment> PatientNurseAssignment { get; set; }
    }
}
