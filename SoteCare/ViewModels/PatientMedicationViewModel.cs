using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoteCare.ViewModels
{
    public class PatientMedicationViewModel
    {
        public int PatientMedicationID { get; set; }
        public string MedicationName { get; set; }
        public string DosageAmount { get; set; }
        public string SingleDose { get; set; } // Esim. "500mg"
        public int? DailyFrequency { get; set; } // Esim. "3 kertaa päivässä"
        public string AdministrationTimes { get; set; } // Esim. "08:00, 14:00, 20:00"
        public DateTime? StartDate { get; set; }  
        public DateTime? EndDate { get; set; }
        public string DoctorName { get; set; }
        public string RouteOfAdministration { get; set; }
        public string MedicationType { get; set; } // "Säännöllinen" tai "Tarvittaessa"
    }
}