using Corum.Models;
using System.Reflection;
using Corum.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Corum.Models.ViewModels.Orders
{
   public class OrderFilterSettingsModel : BaseViewModel
    {
        public int? Id { set; get; }
        public string NameFilter { set; get; }
        //public int? StatusId { set; get; }
        public string StatusId { set; get; }
        public string CreatorId { set; get; }
        public string ExecuterId { set; get; }
        //public int? TypeId { set; get; }
        public string TypeId { set; get; }
        //public long? ClientId { set; get; }
        public string ClientId { set; get; }
        public string PayerId { set; get; }
        public string OrgFromId { set; get; }
        public string OrgToId { set; get; }
        public int? PriorityType { set; get; }
        public double DeltaDateBeg { set; get; }
        public double DeltaDateEnd { set; get; }
        public double DeltaDateBegEx { set; get; }
        public double DeltaDateEndEx { set; get; }
        public bool UseStatusFilter { set; get; }
        public bool UseCreatorFilter { set; get; }
        public bool UseExecuterFilter { set; get; }
        public bool UseClientFilter { set; get; }
        public bool UseTypeFilter { set; get; }
        public bool UsePriorityFilter { set; get; }
        public bool UseDateFilter { set; get; }
        public bool UseExDateFilter { set; get; }

        public string FilterOrderExecuterName { get; set; }
        public string FilterStatusName { get; set; }
        public string FilterOrderCreatorName { get; set; }
        public string FilterOrderTypeName { get; set; }
        public string FilterOrderClientName { get; set; }

        public string FilterOrderDateBeg { get; set; }
        public string FilterOrderDateBegRaw { get; set; }
        public string FilterOrderDateEnd { get; set; }
        public string FilterOrderDateEndRaw { get; set; }

        public string FilterOrderExDateBeg { get; set; }
        public string FilterOrderExDateBegRaw { get; set; }
        public string FilterOrderExDateEnd { get; set; }
        public string FilterOrderExDateEndRaw { get; set; }


        public string FilterOrderEndDateBeg { get; set; }
        public string FilterOrderEndDateBegRaw { get; set; }
        public string FilterOrderEndDateEnd { get; set; }
        public string FilterOrderEndDateEndRaw { get; set; }

        public string UserCurrentId { set; get; }

        public string FilterOrderPayerId { get; set; }
        public bool UseOrderPayerFilter { get; set; }
        public string FilterOrderPayerName { get; set; }

        public string FilterOrderOrgFromId { get; set; }
        public bool UseOrderOrgFromFilter { get; set; }
        public string FilterOrderOrgFromName { get; set; }

        public string FilterOrderOrgToId { get; set; }
        public bool UseOrderOrgToFilter { get; set; }
        public string FilterOrderOrgToName { get; set; }
    }
}
