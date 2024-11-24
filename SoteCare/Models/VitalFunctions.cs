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
    
    public partial class VitalFunctions
    {
        public int VitalFunctionID { get; set; }
        public int PatientID { get; set; }
        public System.DateTime DateTime { get; set; } = DateTime.Now;
        public Nullable<int> HeartRate { get; set; }
        public Nullable<int> SystolicBloodPressure { get; set; }
        public Nullable<int> DiastolicBloodPressure { get; set; }
        public Nullable<int> RespiratoryRate { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public Nullable<decimal> OxygenSaturation { get; set; }
    
        public virtual Patients Patients { get; set; }
    }
}
