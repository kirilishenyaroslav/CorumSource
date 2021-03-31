using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    // Создание модели компании заказчика
    public class BalanceKeepers
    {
        public int Id { get; set; }
        public string BalanceKeeper { get; set; }
        public Nullable<int> subCompanyId { get; set; }
    }
}
