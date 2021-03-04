using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Corum.Models
{
    public class NavigationInfo
    {
        public int? PageSize { get; set; }
        public string SearchResult { get; set; }
        public List<SelectListItem> PageSizeTemplates { get; set; }
    }

    public class OrdersNavigationInfo : NavigationInfo
    {
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
        public string FilterOrderDateBeg { get; set; }
        public string FilterOrderDateBegRaw { get; set; }
        public string FilterOrderDateEnd { get; set; }
        public string FilterOrderDateEndRaw { get; set; }

        public string FilterOrderDate { get; set; }
        public string FilterOrderDateRaw { get; set; }

        public bool UseAcceptDateFilter { get; set; }
        public string FilterAcceptDateBeg { get; set; }
        public string FilterAcceptDateBegRaw { get; set; }
        public string FilterAcceptDateEnd { get; set; }
        public string FilterAcceptDateEndRaw { get; set; }


        public bool UseOrderExDateFilter { get; set; }
        public string FilterOrderExDateBeg { get; set; }
        public string FilterOrderExDateBegRaw { get; set; }
        public string FilterOrderExDateEnd { get; set; }
        public string FilterOrderExDateEndRaw { get; set; }


        public bool UseOrderEndDateFilter { get; set; }
        public string FilterOrderEndDateBeg { get; set; }
        public string FilterOrderEndDateBegRaw { get; set; }
        public string FilterOrderEndDateEnd { get; set; }
        public string FilterOrderEndDateEndRaw { get; set; }

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

        public string OrgName { get; set; }
        public int OrgId  { get; set; }    
        public DateTime ReportDate { get; set; } 

        public int? IdGroup { get; set; }
        public string Id { get; set; }
        public string IdTree { get; set; }
        
    }

    public class OrderNavigationInfo : NavigationInfo
    {
        public bool ShowAll { get; set; }
        public long OrderId { get; set; }
    }

    public class PipelinesNavigationInfo : NavigationInfo
    {
        public int? OrderTypeId { get; set; }
    }

    public class RestNavigationInfo : NavigationInfo
    {
        public int snapshotId { get; set; }
        public bool PriceForEndConsumer { get; set; }
        public bool PriceForFirstReciver { get; set; }
        public bool PlanFullCost { get; set; }
        public bool PlanChangableCost { get; set; }
        public bool FactFullCosts { get; set; }
        public bool FactChangableCosts { get; set; }
        public bool BalancePrice { get; set; }
        public string CurrentGroupFieldName { get; set; }
        public string CurrentGroupFieldNameDescription { get; set; }
        
        public string FilterStorageId { get; set; }
        public string FilterCenterId { get; set; }
        public string FilterRecieverPlanId { get; set; }
        public string FilterRecieverFactId { get; set; }
        public string FilterKeeperId { get; set; }
        public string FilterProducerId { get; set; }
        public string FilterOrderProjectId { get; set; }
        public string FilterProductBarcodeId { get; set; }

        public bool UseStorageFilter { get; set; }
        public bool UseCenterFilter { get; set; }
        public bool UseRecieverPlanFilter { get; set; }
        public bool UseRecieverFactFilter { get; set; }
        public bool UseKeeperFilter { get; set; }
        public bool UseProducerFilter { get; set; }
        public bool UseOrderProjectFilter { get; set; }
        public bool UseProductBarcodeFilter { get; set; }

        public int IsPrihodDocs { get; set; }

        public bool BeforeColBlock { get; set; }
        public bool PrihodColBlock { get; set; }
        public bool RashodColBlock { get; set; }
        public bool AfterColBlock { get; set; }
    }


    public static class NavigationProperties
    {
        public static List<SelectListItem> GetPageSizeTemplates(int templateValue)
        {
            var valuesList = new int[] { 10, 25, 50, 100 };

            return (from value in valuesList
                let ifSelected = ((value == templateValue) ? true : false)
                select new SelectListItem
                {
                    Text = value.ToString(), Value = value.ToString(), Selected = ifSelected
                }).ToList();
        }
    }
    public class OrderCarsNavigationInfo : OrdersNavigationInfo
    {
       public string userId { get; set; }
        public bool isAdmin { get; set; }
        public bool UseOrderIdFilter { get; set; }
        public bool UseExpeditorIdFilter { get; set; }
        public bool UseContractExpBkInfoFilter { get; set; }
        public bool UseCarrierInfoFilter { get; set; }
        public bool UseContractInfoFilter { get; set; }
        public bool UseCarModelInfoFilter { get; set; }
        public bool UseCarRegNumFilter { get; set; }
        public bool UseCarCapacityFilter { get; set; }
        public bool UseCarDriverInfoFilter { get; set; }
        public bool UseDriverCardInfoFilter { get; set; }
        public bool UseDriverContactInfoFilter { get; set; }
        public bool UseCommentsFilter { get; set; }
        public bool UseFactShipperFilter { get; set; }
        public bool UseFactConsigneeFilter { get; set; }

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

        public string FilterFactShipperBeg { get; set; }
        public string FilterFactShipperBegRaw { get; set; }
        public string FilterFactShipperEnd { get; set; }
        public string FilterFactShipperEndRaw { get; set; }
                   
        public string FilterFactConsigneeBeg { get; set; }
        public string FilterFactConsigneeBegRaw { get; set; }
        public string FilterFactConsigneeEnd { get; set; }
        public string FilterFactConsigneeEndRaw { get; set; }
    }

    public class ContractNavigationInfo : NavigationInfo
    {
        public int? carOwnerId { get; set; }
    }

    public class FAQNavigationInfo : NavigationInfo
    {
        public int? GroupeId { get; set; }
    }

    public class ContractSpecNavigationInfo : NavigationInfo
    {
        public int? groupeSpecId { get; set; }
    }

    public class GroupesSpecNavigationInfo : NavigationInfo
    {
        public int? contractId { get; set; }
    }

    public class RoutesNavigationInfo : NavigationInfo
    {
        public int? orgId { get; set; }
    }

    public class ProjectNavigationInfo : NavigationInfo
    {
        public bool Code { get; set; }
        public bool Description { get; set; }
        public bool Comments { get; set; }
        public bool ProjectTypeName { get; set; }
        public bool ProjectCFOName { get; set; }
        public bool ProjectOrderer { get; set; }
        public bool ConstructionDesc { get; set; }
        public bool PlanCount { get; set; }
        public bool isActive { get; set; }
        public bool ManufacturingEnterprise { get; set; }
        public bool NumOrder { get; set; }
        public bool DateOpenOrder { get; set; }
        public bool PlanPeriodForMP { get; set; }
        public bool PlanPeriodForComponents { get; set; }
        public bool PlanPeriodForSGI { get; set; }
        public bool PlanPeriodForTransportation { get; set; }
        public bool PlanDeliveryToConsignee { get; set; }
        public bool DeliveryBasic { get; set; }
        public bool ShipperName { get; set; }
        public bool ConsigneeName { get; set; }
        public bool CanShowManufacture { get; set; }
    }

    public class ConcursNavigationInfo : NavigationInfo
    {
        public int Id { get; set; }

        public bool ShowAll { get; set; }

        public long OrderId { get; set; }

        public string FilterOrderDateBeg { set; get; }
        public string FilterOrderDateBegRaw { set; get; }

        public string FilterOrderDateEnd { set; get; }
        public string FilterOrderDateEndRaw { set; get; }
    }

}
