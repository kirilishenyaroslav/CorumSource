using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderStatusHistoryViewModel
    {
        public long Id { get; set; }
        public long  OrderId { get; set; }
        public System.DateTime EditedDateTime { get; set; }
        public int OldStatusId { get; set; }
        public string OldStatusName { get; set; }
        public int NewStatusId { get; set; }
        public string NewStatusName { get; set; }
        public string CreatedByUser { get; set; }
        public string CreateByUserName { get; set; }
        public string StatusChangeComment { get; set; }
    }
}
