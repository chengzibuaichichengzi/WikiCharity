using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiCharity.Models
{
    public class CharityModel
    {
        public string ABN { get; set; }
        public string CharityName { get; set; }
        public string OtherName { get; set; }
        public bool BRC { get; set; } = false;

        public bool ConductedActivity { get; set; } = false;
        public string MainActivity { get; set; }

        public string State { get; set; }
        public string TownCity { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public string Website { get; set; }
        public string RegisDate { get; set; }
        public string Size { get; set; }
        
        public List<string> activities { get; set; } = new List<string>();

        public List<string> operateStates { get; set; } = new List<string>();

        public string description { get; set; }

        public List<string> beneficiaries { get; set; } = new List<string>();
        public bool ABNStatus { get; set; } = false;
        public bool DGR { get; set; } = false;

        public int StaffFull { get; set; }
        public int StaffPart { get; set; }
        public int StaffCasual { get; set; }
        public int StaffVolun { get; set; }

        
    }
}