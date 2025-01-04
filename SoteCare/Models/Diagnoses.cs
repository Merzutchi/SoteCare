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

    public partial class Diagnoses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Diagnoses()
        {
            this.Treatment = new HashSet<Treatment>();
        }

        [Display(Name = "Diagnoosi ID")]
        public int DiagnosisID { get; set; }

        [Display(Name = "Potilas ID")]
        public Nullable<int> PatientID { get; set; }

        [Display(Name = "Diagnoosi")]
        public string DiagnosisName { get; set; }

        [Display(Name = "Diagnoosi p�iv�m��r�")]
        public Nullable<System.DateTime> DiagnosisDate { get; set; }

        [Display(Name = "Muistiinpanot")]
        public string Notes { get; set; }

        [Display(Name = "L��k�rin nimi")]
        public string DoctorName { get; set; }

        [Display(Name = "L��k�ri ID")]
        public Nullable<int> DoctorID { get; set; }
    
        public virtual Patients Patients { get; set; }
        public virtual Doctors Doctors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Treatment> Treatment { get; set; }
    }
}
