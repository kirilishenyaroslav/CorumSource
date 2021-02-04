using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderCountriesViewModel
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public bool IsDefault { get; set; }
    }
}
