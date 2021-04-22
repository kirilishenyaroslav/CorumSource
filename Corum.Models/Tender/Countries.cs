using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public class Countries
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Fullname { get; set; }
        public string alpha2 { get; set; }
        public string alpha3 { get; set; }
        public bool? IsDefault { get; set; }
    }
}
