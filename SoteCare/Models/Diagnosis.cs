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
    
    public partial class Diagnosis
    {
        public int DiagnosisID { get; set; }
        public Nullable<int> PatientID { get; set; }
        public string DiagnosisName { get; set; }
        public Nullable<System.DateTime> DiagnosisDate { get; set; }
        public string Notes { get; set; }
    
        public virtual Patients Patient { get; set; }
    }
}
