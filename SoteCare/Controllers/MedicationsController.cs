using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace SoteCare.Controllers
{
    public class MedicationsController : Controller
    {
        private readonly PatientRecordDataEntities context = new PatientRecordDataEntities();

        public ActionResult MedicationDetails(int patientId)  //Views all meds for specific patient
        {
            var patient = context.Patients.Find(patientId); //Gets all assigned meds for patient
            if (patient == null)
            {
                return HttpNotFound();
            }
            var patientMedications = context.PatientMedications  //gets all available meds in system/db
                                            .Where(pm => pm.PatientID == patientId)   
                                            .Include(pm => pm.Medication)
                                            .ToList(); 

            var medications = context.Medications.ToList();   //passes data to view
            var data = new 
            {
                Patient = patient,
                PatientMedication = patientMedications,
                Medications = medications
            };
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientAssign (int patientId, int medicationId, string dosage = null)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(dosage))
                {
                    var defaultDosage = context.Dosages
                        .FirstOrDefault(d => d.MedicationID == medicationId);

                    dosage = defaultDosage?.DosageAmount ?? "No dosage assigned"; // Fallback if no dosage exists
                }
                var patientMedication = new PatientMedication
                {
                    PatientID = patientId,
                    MedicationID = medicationId,
                    Dosages = dosage,
                    StartDate = DateTime.Now
                };
                context.PatientMedications.Add(patientMedication);
                context.SaveChanges();
            }
        }

    }
}
