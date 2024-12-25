using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoteCare.ViewModels
{
    public class VFunctionChart
    {
        public int PatientID { get; set; } // Added PatientID property
        public string PatientName { get; set; } // Name of the patient
        public List<string> Dates { get; set; } // X-axis labels (dates/times)

        // Data for each vital function
        public List<int> HeartRates { get; set; }
        public List<int> SystolicBP { get; set; }
        public List<int> DiastolicBP { get; set; }
        public List<int> RespiratoryRates { get; set; }
        public List<decimal> Temperatures { get; set; }
        public List<decimal> OxygenSaturations { get; set; }

        public VFunctionChart()
        {
            Dates = new List<string>();
            HeartRates = new List<int>();
            SystolicBP = new List<int>();
            DiastolicBP = new List<int>();
            RespiratoryRates = new List<int>();
            Temperatures = new List<decimal>();
            OxygenSaturations = new List<decimal>();
        }
    }
}