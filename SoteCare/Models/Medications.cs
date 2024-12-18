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

    public partial class Medications
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medications()
        {
            this.Dosages = new HashSet<Dosages>();
            this.PatientMedications = new HashSet<PatientMedications>();
            this.Treatment = new HashSet<Treatment>();
            this.TreatmentDetails = new HashSet<TreatmentDetails>();
        }

        [Display(Name = "L��keID")]
        public int MedicationID { get; set; }

        [Display(Name = "L��kkeen nimi")]
        public string MedicationName { get; set; }

        [Display(Name = "L��k�riID")]
        public Nullable<int> DoctorID { get; set; }

        [Display(Name = "Lis�ys/t�ytt�??")]
        public string RefillStatus { get; set; }

        [Display(Name = "L��kkeen status???")]
        public string MedicationStatus { get; set; }

        [Display(Name = "Allergiat")]
        public string Allergies { get; set; }

        [Display(Name = "Kommentit")]
        public string Comments { get; set; }

        [Display(Name = "L��kelistaID")]
        public Nullable<int> MedicationListID { get; set; }

        [Display(Name = "L��kkeen tyyppi")]
        public string MedicationType { get; set; }

        [Display(Name = "L��kkeen kuvaus???")]
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dosages> Dosages { get; set; }
        public virtual MedicationLists MedicationLists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientMedications> PatientMedications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Treatment> Treatment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreatmentDetails> TreatmentDetails { get; set; }
    }
}
