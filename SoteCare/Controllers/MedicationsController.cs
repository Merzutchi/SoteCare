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
            using (var context = new PatientRecordDataEntities())
            {
                try
                {
                    var medications = context.Medications.ToList();
                    return View(medications);
                }
                catch (Exception ex)
                {
                    // Log or display the exception message
                    return View("Error", new HandleErrorInfo(ex, "Medications", "Index"));
                }
            }
        }
    }
}
       