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
    using System.Globalization;
    using System.Web.Mvc;

    public class VitalFunctions
    {
        public int VitalFunctionID { get; set; }
        public int PatientID { get; set; }
        public System.DateTime DateTime { get; set; }
        public Nullable<int> HeartRate { get; set; }
        public Nullable<int> SystolicBloodPressure { get; set; }
        public Nullable<int> DiastolicBloodPressure { get; set; }
        public Nullable<int> RespiratoryRate { get; set; }
        public decimal? Temperature { get; set; }
        public Nullable<decimal> OxygenSaturation { get; set; }
        public Nullable<int> NurseID { get; set; }
    
        public virtual Patients Patients { get; set; }
        public virtual Nurses Nurses { get; set; }
    }

    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value != null)
            {
                var attemptedValue = value.AttemptedValue;

                // Try parsing with invariant culture (dot as separator)
                if (decimal.TryParse(attemptedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                {
                    return result;
                }

                // Try parsing with current culture (comma as separator)
                if (decimal.TryParse(attemptedValue, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                {
                    return result;
                }

                // Add model state error if parsing fails
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid temperature format.");
            }

            return null;
        }
    }
}
