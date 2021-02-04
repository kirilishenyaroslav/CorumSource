using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{
    public class OrdersReportViewModel : BaseViewModel
    {
        public string CntName { get; set; }

        public int CntOrders { get; set; }
       
        public List<string> BalanceKeepersName { set; get; }

        public List<int?> BalanceKeepers { set; get; }

        //public IList<int> BalanceKeepersPlan { set; get; }

        //public IList<int> BalanceKeepersUrgent { set; get; }

    }

    public class PayerNameViewModel
    {
        public string PayerName { get; set; }

        public int PayerCntAll { get; set; }

        public int PayerCntPlan { get; set; }

        public int PayerCntUrgent { get; set; }

    }

}
