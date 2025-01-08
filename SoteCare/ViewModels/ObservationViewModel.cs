using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoteCare.ViewModels
{
    public class ObservationViewModel
    {
        public int ObservationID { get; set; }
        public string ObservationText { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}