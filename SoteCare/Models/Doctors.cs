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

    public partial class Doctors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctors()
        {
            this.Patients = new HashSet<Patients>();
            this.TreatmentDetails = new HashSet<TreatmentDetails>();
            this.Diagnoses = new HashSet<Diagnoses>();
            this.PatientHistory = new HashSet<PatientHistory>();
            this.PatientMedications = new HashSet<PatientMedications>();
            this.PatientNurseAssignment = new HashSet<PatientNurseAssignment>();
        }
    
        public int DoctorID { get; set; }
        public Nullable<int> UserID { get; set; }

        [Display(Name = "Etunimi")]
        public string FirstName { get; set; }

        [Display(Name = "Sukunimi")]
        public string LastName { get; set; }

        [Display(Name = "Erikoisuus")]
        public string Specialization { get; set; }

        [Display(Name = "Puhelinnumero")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Sähköposti")]
        public string Email { get; set; }

        [Display(Name = "Aktiivinen?")]
        public bool IsActive { get; set; }

        [Display(Name = "Koko nimi")]
        public string FullName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patients> Patients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreatmentDetails> TreatmentDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Diagnoses> Diagnoses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientHistory> PatientHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientMedications> PatientMedications { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientNurseAssignment> PatientNurseAssignment { get; set; }
    }
}
