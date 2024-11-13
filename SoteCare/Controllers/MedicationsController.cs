using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class MedicationsController : Controller
    {
        // GET: Medications
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddMedication() 
        {
            using (var context = new PatientRecordDataEntities()) 
            {
                var BisoprololDosages = new List<string> { "2,5mg", "5 mg", "10 mg" };
                var NitrosidDosages = new List<string> { "5 mg", "10 mg", "20 mg" };
                var DiforminRetardDosages = new List<string> { "500 mg", "750 mg", "1000 mg" };
                var JardianceDosages = new List<string> { "10 mg", "25 mg" };
                var ParatabsDosages = new List<string> { "500 mg", "1000 mg" };
                var DisperinDosages = new List<string> { "100 mg", "500 mg" };
                var EssitalopramDosages = new List<string> { "5 mg", "10 mg", "20 mg" };



                foreach (var dosage in BisoprololDosages)
                {
                    var existingMedication = context.Medications.FirstOrDefault(m => m.MedicationName == "Bisoprolol Orion" && m.Dosage == dosage);

                    if (existingMedication == null)
                    {
                        context.Medications.Add(new Medications
                        {
                            MedicationName = "Bisoprolol Orion",
                            Dosage = dosage,
                            Frequency = "1x päivässä",
                            RouteOfAdministration = "Suun kautta"
                        });
                    }
                }

                foreach (var dosage in NitrosidDosages)
                {
                    var existingMedication = context.Medications .FirstOrDefault(m => m.MedicationName == "Nitrosid" && m.Dosage == dosage);

                    if (existingMedication == null) 
                    {
                        context.Medications.Add(new Medications
                        {
                            MedicationName = "Nitrosid",
                            Dosage = dosage,
                            Frequency = "2x päivässä",
                            RouteOfAdministration = "Suun Kautta"
                        });
                    }                   
                }

                foreach (var dosage in DiforminRetardDosages)
                {
                    var existingMedication = context.Medications.FirstOrDefault(m => m.MedicationName == "Diformin Retard" && m.Dosage == dosage);

                    if (existingMedication == null)
                    {
                        context.Medications.Add(new Medications
                        {
                            MedicationName = "Diformin Retard",
                            Dosage = dosage,
                            Frequency = "3x päivässä",
                            RouteOfAdministration = "Suun kautta"
                        });
                    }                      
                }

                foreach (var dosage in JardianceDosages)
                {
                    var existingMedication = context.Medications.FirstOrDefault(m => m.MedicationName == "Jardiance" && m.Dosage == dosage);

                    if (existingMedication == null) 
                    {
                        context.Medications.Add(new Medications
                        {
                            MedicationName = "Jardiance",
                            Dosage = dosage,
                            Frequency = "2x päivässä",
                            RouteOfAdministration = "Suun kautta"
                        });
                    }                       
                }

                foreach (var dosage in ParatabsDosages)
                {
                    var existingMedication = context.Medications.FirstOrDefault(m => m.MedicationName == "Paratabs" && m.Dosage == dosage);

                    if (existingMedication == null) 
                    {
                        context.Medications.Add(new Medications
                        {
                            MedicationName = "Paratabs",
                            Dosage = dosage,
                            Frequency = "3x päivässä",
                            RouteOfAdministration = "Suun kautta"
                        });
                    }                       
                }

                foreach (var dosage in DisperinDosages)
                {
                    var existingMedication = context.Medications.FirstOrDefault(m => m.MedicationName == "Disperin" && m.Dosage == dosage);

                    if (existingMedication == null) 
                    {
                        context.Medications.Add(new Medications
                        {
                            MedicationName = "Disperin",
                            Dosage = dosage,
                            Frequency = "1x päivässä",
                            RouteOfAdministration = "Suun kautta"
                        });
                    }                        
                }

                foreach (var dosage in EssitalopramDosages)
                {
                    var existingMedication = context.Medications.FirstOrDefault(m => m.MedicationName == "Essitalopram" && m.Dosage == dosage);

                    if (existingMedication == null) 
                    {
                        context.Medications.Add(new Medications
                        {
                            MedicationName = "Essitalopram",
                            Dosage = dosage,
                            Frequency = "1x päivässä",
                            RouteOfAdministration = "Suun kautta"
                        });
                    }                      
                }

                context.SaveChanges();
            }
            return RedirectToAction("Index"); 
        }
    }
}