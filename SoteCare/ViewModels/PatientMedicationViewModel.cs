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
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DoctorName { get; set; }
        public string RouteOfAdministration { get; set; }
    }
}