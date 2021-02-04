using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{

    public class FactCarsFilter
    {
        public string userId { get; set; }
        public bool? isAdmin { get; set; }
        public bool? UseOrderIdFilter { get; set; }
        public bool? UseExpeditorIdFilter { get; set; }
        public bool? UseContractExpBkInfoFilter { get; set; }
        public bool? UseCarrierInfoFilter { get; set; }
        public bool? UseContractInfoFilter { get; set; }
        public bool? UseCarModelInfoFilter { get; set; }
        public bool? UseCarRegNumFilter { get; set; }
        public bool? UseCarCapacityFilter { get; set; }
        public bool? UseCarDriverInfoFilter { get; set; }
        public bool? UseDriverCardInfoFilter { get; set; }
        public bool? UseDriverContactInfoFilter { get; set; }
        public bool? UseCommentsFilter { get; set; }
        public bool? UseFactShipperFilter { get; set; }
        public bool? UseFactConsigneeFilter { get; set; }
        public bool? UseOrderExDateFilter { get; set; }
        public bool? UseOrderEndDateFilter { get; set; }

        public string FilterOrderIdFilter { get; set; }
        public string FilterExpeditorIdFilter { get; set; }
        public string FilterContractExpBkInfoFilter { get; set; }
        public string FilterCarrierInfoFilter { get; set; }
        public string FilterContractInfoFilter { get; set; }
        public string FilterCarModelInfoFilter { get; set; }
        public string FilterCarRegNumFilter { get; set; }
        public string FilterCarCapacityFilter { get; set; }
        public string FilterCarDriverInfoFilter { get; set; }
        public string FilterDriverCardInfoFilter { get; set; }
        public string FilterDriverContactInfoFilter { get; set; }
        public string FilterCommentsFilter { get; set; }
        public DateTime FilterFactShipperBeg { get; set; }
        public DateTime FilterFactShipperEnd { get; set; }
        public DateTime FilterFactConsigneeBeg { get; set; }
        public DateTime FilterFactConsigneeEnd { get; set; }
       
        public DateTime FilterOrderExDateBeg { get; set; }        
        public DateTime FilterOrderExDateEnd { get; set; }        
        
        public DateTime FilterOrderEndDateBeg { get; set; }        
        public DateTime FilterOrderEndDateEnd { get; set; }   
        
        /////
          public bool DriftDate { get; set; }
        public bool AcceptDate { get; set; }
        public bool ExecuteDate { get; set; }

        public string FilterOrderExecuterId { get; set; }
        public bool UseOrderExecuterFilter { get; set; }

        public string FilterStatusId { get; set; }
        public bool UseStatusFilter { get; set; }

        public string FilterOrderCreatorId { get; set; }
        public bool UseOrderCreatorFilter { get; set; }

        public string FilterOrderTypeId { get; set; }
        public bool UseOrderTypeFilter { get; set; }

        public string FilterTripTypeId { get; set; }
        public bool UseTripTypeFilter { get; set; }

        public string FilterOrderClientId { get; set; }
        public bool UseOrderClientFilter { get; set; }

        public int FilterOrderPriority { get; set; }
        public bool UseOrderPriorityFilter { get; set; }

        public bool UseOrderDateFilter { get; set; }
        public DateTime FilterOrderDateBeg { get; set; }
        public string FilterOrderDateBegRaw { get; set; }
        public DateTime FilterOrderDateEnd { get; set; }
        public string FilterOrderDateEndRaw { get; set; }

        public bool UseAcceptDateFilter { get; set; }
        public string FilterAcceptDateBeg { get; set; }
        public string FilterAcceptDateBegRaw { get; set; }
        public string FilterAcceptDateEnd { get; set; }
        public string FilterAcceptDateEndRaw { get; set; }
   
        public bool UseFinalStatusFilter { get; set; }
        public bool FilterFinalStatus { get; set; }

        public bool UseOrderProjectFilter { get; set; }
        public string FilterOrderProjectId { get; set; }

        public int PageNumber { get; set; }

        public bool isPassOrders { get; set; }

        public int? DateType { get; set; }

        public string FilterOrderPayerId { get; set; }
        public bool UseOrderPayerFilter { get; set; }

        public string FilterOrderOrgFromId { get; set; }
        public bool UseOrderOrgFromFilter { get; set; }

        public string FilterOrderOrgToId { get; set; }
        public bool UseOrderOrgToFilter { get; set; }     
    }
}
