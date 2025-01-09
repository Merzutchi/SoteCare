using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.ViewModels
{
    public class AssignPatientToNurseViewModel
    {
        public List<SelectListItem> UnassignedPatients { get; set; }
        public List<SelectListItem> Nurses { get; set; }
    }    
}