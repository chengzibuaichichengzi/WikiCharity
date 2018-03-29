using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WikiCharity.Models
{
    public class EmailFormModel
    {
        [Required]
        public string name { get; set; }
        [Required, EmailAddress]
        public string email { get; set; }
        [Required]
        public string message { get; set; }
    }
}