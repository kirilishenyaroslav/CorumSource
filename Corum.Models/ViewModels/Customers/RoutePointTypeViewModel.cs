using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Customers
{
    public class RoutePointTypeViewModel
    {
        public int Id { get; set; }

        public string FullNamePointType { get; set; }

        public string ShortNamePointType { get; set; }
    }
}
