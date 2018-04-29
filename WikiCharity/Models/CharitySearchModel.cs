using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiCharity.Models
{
    public class CharitySearchModel
    {
        public string state {get;set;}
        public string size { get; set; }
        public string tax { get; set; }
        public string benes { get; set; }

        public string name { get; set; }
    }
}