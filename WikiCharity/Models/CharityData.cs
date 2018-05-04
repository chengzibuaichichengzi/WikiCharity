using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiCharity.Models
{
    public class CharityData
    {
        public List<Charity> charities { get; set; }
        public int TotalRecords { get; set; }
    }
}