using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{    
        public class TruckReportViewModel : OrderUsedCarViewModel
    {

        public string Shipper { get; set; }

        public string Consignee { get; set; }

        public string ShipperName { get; set; }

        public long ShipperId { get; set; }

        public string TruckDescription { get; set; }

        public bool isShipper { get; set; }

        public DateTime? PlanDateTime  { get; set; }

        public DateTime? FactDateTime  { get; set; }

        public string IsShipperString  { get; set; }

        public string PlanTime { get; set; }

        public string FactTime { get; set; }

        public string PlanDate { get; set; }

        public string FactDate { get; set; }

        public string Address { get; set; }

        public string BalanceKeeper { get; set; }

        public string CreatorByUserName { get; set; }
        
        public string DateFactConsignee { get; set; }

        public string TimeFactConsignee { get; set; }
        


    }
}
