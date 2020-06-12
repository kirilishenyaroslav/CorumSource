using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderNotificationTypesViewModel : BaseViewModel
    {
        public int Id { get; set; }

        
        public string TypeName { get; set; }
    }
}
