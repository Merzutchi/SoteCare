using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoteCare.ViewModels
{
    public class PHViewModel
    {
        public string PatientName { get; set; }
        public List<DiagnosisViewModel> Diagnoses { get; set; }
        public List<TreatmentViewModel> Treatments { get; set; }
        public List<MedicationViewModel> Medications { get; set; }
    }

    public class DiagnosisViewModel
    {
        public string DiagnosisName { get; set; }
        public DateTime? DiagnosisDate { get; set; }
        public string Notes { get; set; }
    }

    public class TreatmentViewModel
    {
        public string TreatmentType { get; set; }
        public string TreatmentDetails { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class MedicationViewModel
    {
        public string MedicationName { get; set; }
        public DateTime StartDate { get; set; }
    }
}