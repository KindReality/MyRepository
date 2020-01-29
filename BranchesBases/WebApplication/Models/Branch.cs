using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Branch
    {
        public int BranchID { get; set; }
        public string  Name { get; set; }
        public ICollection<Base> Bases { get; set; }
    }
}
