using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public partial class RegisterTenderContragent
    {
        public int Id { get; set; }
        public long OrderId { get; set; }
        public int tenderNumber { get; set; }
        public long itemExternalNumber { get; set; }
        public string ContragentName { get; set; }
        public int ContragentIdAps { get; set; }
        public System.DateTime DateUpdateInfo { get; set; }
        public bool IsWinner { get; set; }
        public long EDRPOUContragent { get; set; }
        public string emailContragent { get; set; }
        public int transportUnitsProposed { get; set; }
        public Nullable<int> acceptedTransportUnits { get; set; }
        public double costOfCarWithoutNDS { get; set; }
        public double costOfCarWithNDS { get; set; }
        public int PaymentDelay { get; set; }
        public System.Guid tenderItemUuid { get; set; }
        public string nmcName { get; set; }
    }
}
