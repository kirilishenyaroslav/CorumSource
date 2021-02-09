using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{
    
    public class FinalReportViewModel : BaseViewModel
    {
        public List<string> OrderStatusName { get; set; }

        public List<int> OrderStatus { get; set; }
        
        public int CntAllNotFinal { get; set; }

        public int CntAll { get; set; }


    }
}
