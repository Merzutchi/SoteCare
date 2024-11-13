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
                    context.Medications.Add(new Medications 
                    {   MedicationName = "Bisoprolol Orion", 
                        Dosage = dosage, 
                        Frequency = "1x päivässä", 
                        RouteOfAdministration = "Suun kautta"
                    });                   
                }

                foreach (var dosage in NitrosidDosages)
                {
                    context.Medications.Add(new Medications
                    {
                        MedicationName = "Nitrosid",
                        Dosage = dosage,
                        Frequency = "2x päivässä",
                        RouteOfAdministration = "Suun Kautta"
                    });
                }

                foreach (var dosage in DiforminRetardDosages)
                {
                    context.Medications.Add(new Medications
                    {
                        MedicationName = "Diformin Retard",
                        Dosage = dosage,
                        Frequency = "3x päivässä",
                        RouteOfAdministration = "Suun kautta"
                    });
                }

                foreach (var dosage in JardianceDosages)
                {
                    context.Medications.Add(new Medications
                    {
                        MedicationName = "Jardiance",
                        Dosage = dosage,
                        Frequency = "2x päivässä",
                        RouteOfAdministration = "Suun kautta"
                    });
                }

                foreach (var dosage in ParatabsDosages)
                {
                    context.Medications.Add(new Medications
                    {
                        MedicationName = "Paratabs",
                        Dosage = dosage,
                        Frequency = "3x päivässä",
                        RouteOfAdministration = "Suun kautta"
                    });
                }

                foreach (var dosage in DisperinDosages)
                {
                    context.Medications.Add(new Medications
                    {
                        MedicationName = "Disperin",
                        Dosage = dosage,
                        Frequency = "1x päivässä",
                        RouteOfAdministration = "Suun kautta"
                    });
                }

                foreach (var dosage in EssitalopramDosages)
                {
                    context.Medications.Add(new Medications
                    {
                        MedicationName = "Nitrosid",
                        Dosage = dosage,
                        Frequency = "1x päivässä",
                        RouteOfAdministration = "Suun kautta"
                    });
                }

                context.SaveChanges();
            }
            return RedirectToAction("Index"); 
        }
    }
}