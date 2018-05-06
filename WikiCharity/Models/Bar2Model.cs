using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiCharity.Models
{
    public class Bar2Model
    {
        public string year { get; set; }
        public double TotalAssets { get; set; } = 0;

        public double TotalLia { get; set; } = 0;
    }
}