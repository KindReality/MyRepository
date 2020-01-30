using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Base
    {
        public int BaseID { get; set; }
        [Display(Name = "Base Name")]
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Branch Branch { get; set; }
    }
}
