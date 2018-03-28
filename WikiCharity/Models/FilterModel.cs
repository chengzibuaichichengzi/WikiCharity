using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WikiCharity.Models
{
    public class FilterModel
    {
        public string name { get; set; }
        public string beneficial { get; set; }
        public IEnumerable<SelectListItem> beneficials { get; set; }
        public string isDGR { get; set; }
        public IEnumerable<SelectListItem> isDGRs { get; set; }
        public string size { get; set; }
        public IEnumerable<SelectListItem> sizes { get; set; }
        public string state { get; set; }
        public IEnumerable<SelectListItem> states { get; set; }
        public string countNum { get; set; }

    }
}