using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Cars;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels.Admin;
using Corum.Models.ViewModels.Customers;
using Corum.Models.ViewModels.OrderConcurs;
using Corum.Models.Tender;




namespace Corum.Models
{
    
    public class OrdersReportsNavigationResult
    {
        public OrdersNavigationResult<BaseReportViewModel> BaseReport { get; set; }
        public OrdersNavigationResult<StatusReportViewModel> StatusReport { get; set; }
        public OrdersNavigationResult<OrdersReportViewModel> OrdersReport { get; set; }
        public OrdersNavigationResult<FinalReportViewModel> FinalReport { get; set; }        
        public OrdersNavigationResult<TruckViewModel> TruckReport { get; set; }        
        public OrdersNavigationResult<TruckViewModel> TruckReportDetail { get; set; }
        
        public string JSONData { get; set; }       

        public bool UseOrderDateFilter { get; set; }
        public string FilterOrderDateBeg { get; set; }
        public string FilterOrderDateBegRaw { get; set; }
        public string FilterOrderDateEnd { get; set; }
        public string FilterOrderDateEndRaw { get; set; }

        public string FilterOrderDate { get; set; }
        public string FilterOrderDateRaw { get; set; }

        public string FilterOrderTypeId { get; set; }
        public string FilterOrderTypeName { get; set; }
        public bool UseOrderTypeFilter { get; set; }

        public string FilterTripTypeId { get; set; }
        public string FilterTripTypeName { get; set; }
        public bool UseTripTypeFilter { get; set; }

        public string FilterOrderClientId { get; set; }
        public string FilterOrderClientName { get; set; }
        public bool UseOrderClientFilter { get; set; }

        public int PageNumber { get; set; }

        public int DateType { get; set; }

        public bool UseAcceptDateFilter { get; set; }
        public string FilterAcceptDateBeg { get; set; }
        public string FilterAcceptDateBegRaw { get; set; }
        public string FilterAcceptDateEnd { get; set; }
        public string FilterAcceptDateEndRaw { get; set; }

        public string OrgName { get; set; }

        public int OrgId  { get; set; }

        public DateTime ReportDate { get; set; }

        public string Address { get; set; }

        public int? IdGroup { get; set; }
        public string Id { get; set; }
        public string IdTree { get; set; }
    }


    public class NavigationResult<T> where T : class
    {
        private ICorumDataProvider context;
        private string userId = "";        
        public NavigationInfo RequestParams { get; set; }                
        public IQueryable<T> DisplayValues { get; set; }
        
        public NavigationResult(NavigationInfo request,                                
                                string userId)
        {
            try
            {
                context = DependencyResolver.Current.GetService<ICorumDataProvider>();
                this.userId = userId;
                var def_count = context.getCurrentPageSizeByUser(userId);
                var currentPageSize = ProcessPageSizeValue(def_count, request.PageSize ?? def_count);


                RequestParams = new NavigationInfo()
                {
                    PageSize = currentPageSize,                                       
                    PageSizeTemplates = NavigationProperties.GetPageSizeTemplates(currentPageSize)                    
                };
            }
            catch 
            {
                throw;
            }
        }
                
        private int ProcessPageSizeValue(int OldPageSizeValue, int NewPageSizeValue)
        {            
            if (OldPageSizeValue != NewPageSizeValue)
            {
                context.setCurrentPageSizeForUser(this.userId, NewPageSizeValue);
            }

            return NewPageSizeValue;
        }                                
    }


    public class PipelinesNavigationResult<T> : NavigationResult<T> where T : class
    {
        public List<OrderTypeViewModel> AvailiableTypes { get; set; }
        public int OrderTypeId { get; set; }

        public PipelinesNavigationResult(NavigationInfo request, string userId) : base(request, userId)
        {

        }
    }
    
    public class OrdersNavigationResult<T> : NavigationResult<T> where T : class
    {
        public List<OrderTypeViewModel> AvailiableTypes { get; set; }
        public ICorumDataProvider context { get; set; }

        public List<CompetetiveListStepsInfoViewModel> Timeline { get; set; } 
        public IQueryable<T> DataDisplayValues { get; set; }       
        public String IdTree { get; set; }  

        public OrdersNavigationResult(NavigationInfo request, string userId) : base(request, userId)
        {
            DriftDate = true;
            AcceptDate = false;
            ExecuteDate = false;
            isTransport = true;
        }
        
        public bool isTransport { get; set; }
        public bool isChrome { get; set; }

        public int SumPlanCarNumber { set; get; }
        public int SumFactCarNumber { set; get; }

        [Display(Name = @"Дата создания черновика")]
        public bool DriftDate { get; set; }

        [Display(Name = @"Дата утверждения")]
        public bool AcceptDate { get; set; }

        [Display(Name = @"Дата начала выполнения")]
        public bool ExecuteDate { get; set; }

        public string FilterOrderExecuterId { get; set; }
        public string FilterOrderExecuterName { get; set; }
        public bool UseOrderExecuterFilter { get; set; }

        public string FilterOrderPayerId { get; set; }
        public string FilterOrderPayerName { get; set; }        
        public bool UseOrderPayerFilter { get; set; }

        public string FilterOrderOrgFromId { get; set; }
        public string FilterOrderOrgFromName { get; set; }
        public bool UseOrderOrgFromFilter { get; set; }

        public string FilterOrderOrgToId { get; set; }
        public string FilterOrderOrgToName { get; set; }
        public bool UseOrderOrgToFilter { get; set; }

        public string FilterStatusId { get; set; }
        public string FilterStatusName { get; set; }
        public bool UseStatusFilter { get; set; }

        public string FilterOrderCreatorId { get; set; }
        public string FilterOrderCreatorName { get; set; }
        public bool UseOrderCreatorFilter { get; set; }

        public string FilterOrderTypeId { get; set; }
        public string FilterOrderTypeName { get; set; }
        public bool UseOrderTypeFilter { get; set; }

        public string FilterTripTypeId { get; set; }
        public string FilterTripTypeName { get; set; }
        public bool UseTripTypeFilter { get; set; }

        public bool UseFinalStatusFilter { get; set; }
        public bool FilterFinalStatus { get; set; }

        public string FilterOrderClientId { get; set; }
        public string FilterOrderClientName { get; set; }
        public bool UseOrderClientFilter { get; set; }   

        public int FilterOrderPriority { get; set; }        
        public bool UseOrderPriorityFilter { get; set; }

        public bool UseOrderDateFilter { get; set; }
        public string FilterOrderDateBeg { get; set; }
        public string FilterOrderDateBegRaw { get; set; }
        public string FilterOrderDateEnd { get; set; }
        public string FilterOrderDateEndRaw { get; set; }

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

        public bool UseOrderProjectFilter { get; set; }
        public string FilterOrderProjectId { get; set; }
        public string FilterOrderProjectCode { get; set; }

        public string OrgName { get; set; }
       /* public int OrgId { get; set; }
        public DateTime ReportDate { get; set; }
        public String Address { get; set; }
        public int IdGroup { get; set; }
        public String CurrentId { get; set; }*/
        
        public List<string> FinalStatuses { get; set; }

        public Dictionary<string, int> orderFinalStatusesDict { get; set; }
        
    }

    public class OrderNavigationResult<T> : NavigationResult<T> where T : class
    {
        public UserViewModel userInfo;

        public List<TenderServices> tenderServices { get; set; }
        public OrderBaseViewModel orderInfo { get; set; }

        public CompetitiveListViewModel CompetitiveListInfo { get; set; }

        public IEnumerable<CompetitiveListStepViewModel> listStatuses { get; set; }

        public CompetetiveListStepsInfoViewModel currentStatus { get; set; }

        public ConcursDiscountRateModel discountRate { get; set; }

        public string  headerInfo { get; set; }

        public long OrderId { get; set; }

        public long Id { get; set; }

        public bool ShowAll { get; set; }

        [Display(Name = "с")]
        public string FilterOrderDateBeg { set; get; }
        public string FilterOrderDateBegRaw { set; get; }

        [Display(Name = "по")]
        public string FilterOrderDateEnd { set; get; }
        public string FilterOrderDateEndRaw { set; get; }

        public OrderNavigationResult(NavigationInfo request, string userId): base(request, userId)
        {

        }
    }

    public class RestsNavigationResult<T> : NavigationResult<T> where T : class
    {
        public RestsNavigationResult(NavigationInfo request,string userId): base(request, userId)
        {
            PriceForEndConsumer = false;
            PriceForFirstReciver = false;
            PlanFullCost = false;
            PlanChangableCost = false;
            FactFullCosts = false;
            FactChangableCosts = false;
            BalancePrice = true;
        }

        public T DisplayTotalValues { get; set; }
        public string CurrentGroupFieldName { get; set; }
        public string CurrentGroupFieldNameDescription { get; set; }
        public Dictionary<string, string> AvialableFields { get; set; }
        public SnapshotInfoViewModel SnapshotInfo { set; get; }

        [Display(Name = @"Цена конечного потребителя (ЦКП)")]
        public bool PriceForEndConsumer { get; set; }

        [Display(Name = @"Цена на 1 покупателя (Ц1П)")]
        public bool PriceForFirstReciver { get; set; }

        [Display(Name = @"Себестоимость плановая полная (СПП)")]
        public bool PlanFullCost { get; set; }

        [Display(Name = @"Себестоимость плановая переменная (СППР)")]
        public bool PlanChangableCost { get; set; }
        [Display(Name = @"Себестоимость фактическая полная (СФП)")]
        public bool FactFullCosts { get; set; }

        [Display(Name = @"Себестоимость фактическая переменная (СФПР)")]
        public bool FactChangableCosts { get; set; }

        [Display(Name = @"Балансовая стоимость (БС)")]
        public bool BalancePrice { get; set; }

        public string FilterStorageId { get; set; }
        public string FilterCenterId { get; set; }
        public string FilterRecieverPlanId { get; set; }
        public string FilterRecieverFactId { get; set; }
        public string FilterKeeperId { get; set; }
        public string FilterProducerId { get; set; }
        public string FilterOrderProjectId { get; set; }
        public string FilterOrderProjectCode { get; set; }

        public bool UseStorageFilter { get; set; }
        public bool UseCenterFilter { get; set; }
        public bool UseRecieverPlanFilter { get; set; }
        public bool UseRecieverFactFilter { get; set; }
        public bool UseKeeperFilter { get; set; }
        public bool UseProducerFilter { get; set; }
        public bool UseOrderProjectFilter { get; set; }

        public int IsPrihodDocs { get; set; }

        public bool BeforeColBlock { get; set; }
        public bool PrihodColBlock { get; set; }
        public bool RashodColBlock { get; set; }
        public bool AfterColBlock { get; set; }

        public bool DetailItemsExists { get; set; }
    }

    public class ImportErrorsNavigationResult<T> : NavigationResult<T> where T : class
    {
        public ImportErrorsNavigationResult(NavigationInfo request, string userId) : base(request, userId)
        {
            
        }

        public ImportErrorsInfo errorCommonInfo { set; get; }
    }

    public class ContractNavigationResult<T> : NavigationResult<T> where T : class
    {
        public ContractsViewModel contractInfo { get; set; }
        public bool isChrome { get; set; }

        public CarOwnersAccessViewModel carOwnerInfo { get; set; }
        public bool isForwarder { get; set; }
        public bool isMainMenu { get; set; }

        public ContractNavigationResult(NavigationInfo request, string userId)
            : base(request, userId)
        {

        }
    }

    public class OrderCarsNavigationResult<T> : OrdersNavigationResult<T> where T : class
    {
        
        public bool UseOrderIdFilter { get; set; }
        public string FilterOrderIdFilter { get; set; }
        public string FilterOrderIdName { get; set; }

        public bool UseExpeditorIdFilter { get; set; }
        public string FilterExpeditorIdFilter { get; set; }
        public string FilterOrderExpeditorName { get; set; }

        public bool UseContractExpBkInfoFilter { get; set; }
        public string FilterContractExpBkInfoFilter { get; set; }
        public string FilterContractExpBkInfoName { get; set; }

        public bool UseCarrierInfoFilter { get; set; }
        public string FilterCarrierInfoFilter { get; set; }
        public string FilterCarrierInfoName { get; set; }

        public bool UseContractInfoFilter { get; set; }
        public string FilterContractInfoFilter { get; set; }
        public string FilterContractInfoName { get; set; }

        public bool UseCarModelInfoFilter { get; set; }
        public string FilterCarModelInfoFilter { get; set; }
        public string FilterCarModelInfoName { get; set; }

        public bool UseCarRegNumFilter { get; set; }
        public string FilterCarRegNumFilter { get; set; }
        public string FilterCarRegNumName { get; set; }

        public bool UseCarCapacityFilter { get; set; }
        public string FilterCarCapacityFilter { get; set; }
        public string FilterCarCapacityName { get; set; }

        public bool UseCarDriverInfoFilter { get; set; }
        public string FilterCarDriverInfoFilter { get; set; }
        public string FilterCarDriverInfoName { get; set; }

        public bool UseDriverCardInfoFilter { get; set; }
        public string FilterDriverCardInfoFilter { get; set; }
        public string FilterDriverCardInfoName { get; set; }

        public bool UseDriverContactInfoFilter { get; set; }
        public string FilterDriverContactInfoFilter { get; set; }
        public string FilterDriverContactInfoName { get; set; }

        public bool UseCommentsFilter { get; set; }
        public string FilterCommentsFilter { get; set; }
        public string FilterCommentsName { get; set; }

        public bool UseFactShipperFilter { get; set; }
        public string FilterFactShipperBeg { get; set; }
        public string FilterFactShipperBegRaw { get; set; }
        public string FilterFactShipperEnd { get; set; }
        public string FilterFactShipperEndRaw { get; set; }

        public bool UseFactConsigneeFilter { get; set; }
        public string FilterFactConsigneeBeg { get; set; }
        public string FilterFactConsigneeBegRaw { get; set; }
        public string FilterFactConsigneeEnd { get; set; }
        public string FilterFactConsigneeEndRaw { get; set; }
        
        /*public bool UseOrderExDateFilter { get; set; }
        public string FilterOrderExDateBeg { get; set; }
        public string FilterOrderExDateBegRaw { get; set; }
        public string FilterOrderExDateEnd { get; set; }
        public string FilterOrderExDateEndRaw { get; set; }


        public bool UseOrderEndDateFilter { get; set; }
        public string FilterOrderEndDateBeg { get; set; }
        public string FilterOrderEndDateBegRaw { get; set; }
        public string FilterOrderEndDateEnd { get; set; }
        public string FilterOrderEndDateEndRaw { get; set; }
        */


        public OrderCarsNavigationResult(NavigationInfo request, string userId)
            : base(request, userId)
        {

        }
    }
    public class FAQNavigationResult<T> : NavigationResult<T> where T : class
    {
        public List<FAQGroupesViewModel> AvailiableGroupes { get; set; }
        public int GroupeId { get; set; }
        public string NameFAQGroup { get; set; }

        public FAQNavigationResult(NavigationInfo request, string userId) : base(request, userId)
        {

        }
    }

    public class UserMessagesNavigationResult<T> : NavigationResult<T> where T : class
    {
        public IQueryable<UserMessagesViewModel> AvailiableMessagesIn { get; set; }
        public IQueryable<UserMessagesViewModel> AvailiableMessagesOut { get; set; }
        public bool? IsMsgIn { get; set; }

        public UserMessagesNavigationResult(NavigationInfo request, string userId) : base(request, userId)
        {

        }
    }

    public class ContractSpecNavigationResult<T> : NavigationResult<T> where T : class
    {
        public ContractSpecificationsViewModel contractSpecInfo { get; set; }
        public ContractsViewModel contractInfo { get; set; }
        public GroupesSpecificationsViewModel groupeSpecInfo { get; set; }
        public bool isMainMenu;

        public ContractSpecNavigationResult(NavigationInfo request, string userId)
            : base(request, userId)
        {

        }
    }

    public class GroupesSpecNavigationResult<T> : NavigationResult<T> where T : class
    {
        public GroupesSpecificationsViewModel groupeSpecInfo { get; set; }
        public ContractsViewModel contractInfo { get; set; }
        public bool isMainMenu;

        public GroupesSpecNavigationResult(NavigationInfo request, string userId)
            : base(request, userId)
        {

        }
    }

    public class OrganizationNavigationResult<T> : NavigationResult<T> where T : class
    {
        public long? OrgIdFocus { get; set; }

        public OrganizationNavigationResult(NavigationInfo request, string userId)
            : base(request, userId)
        {

        }
    }

    public class ProjectNavigationResult<T> : NavigationResult<T> where T : class
    {
        public ProjectNavigationResult(NavigationInfo request, string userId) : base(request, userId)
        {
            Code = true;
            Description = true;
            ProjectTypeName = true;
            ProjectCFOName = true;
            ProjectOrderer = true;
            ConstructionDesc = true;
            PlanCount = true;
            ManufacturingEnterprise = true;
            isActive = true;
            Comments = false;
            NumOrder = false;
            DateOpenOrder = false;
            PlanPeriodForMP = false;
            PlanPeriodForComponents = false;
            PlanPeriodForSGI = false;
            PlanPeriodForTransportation = false;
            PlanDeliveryToConsignee = false;
            DeliveryBasic = false;
            ShipperName = false;
            ConsigneeName = false;
        }

        public T DisplayTotalValues { get; set; }

        [Display(Name = @"Код проекта")]
        public bool Code { get; set; }

        [Display(Name = @"Наименование проекта")]
        public bool Description { get; set; }

        [Display(Name = @"Комментарии")]
        public bool Comments { get; set; }

        [Display(Name = @"Тип проекта")]
        public bool ProjectTypeName { get; set; }

        [Display(Name = @"ЦФО проекта")]
        public bool ProjectCFOName { get; set; }

        [Display(Name = @"Заказчик проекта")]
        public bool ProjectOrderer { get; set; }

        [Display(Name = @"Обозначение конструкции")]
        public bool ConstructionDesc { get; set; }

        [Display(Name = @"План производства")]
        public bool PlanCount { get; set; }

        [Display(Name = @"Доступен для выбора в заявках")]
        public bool isActive { get; set; }

        [Display(Name = @"Производственное предприятие ")]
        public bool ManufacturingEnterprise { get; set; }

        [Display(Name = @"Номер заказа в производство")]
        public bool NumOrder { get; set; }

        [Display(Name = @"Дата открытия заказа в производство")]
        public bool DateOpenOrder { get; set; }

        [Display(Name = @"Плановый срок обеспечения м.п.")]
        public bool PlanPeriodForMP { get; set; }

        [Display(Name = @"Плановый срок обеспечения комплектующими")]
        public bool PlanPeriodForComponents { get; set; }

        [Display(Name = @"План.срок на СГИ")]
        public bool PlanPeriodForSGI { get; set; }

        [Display(Name = @"План.срок подачи транспорта")]
        public bool PlanPeriodForTransportation { get; set; }

        [Display(Name = @"Плановая дата доставки Грузополучателю")]
        public bool PlanDeliveryToConsignee { get; set; }

        [Display(Name = @"Базис поставки правила ИНКОТРЕРМС")]
        public bool DeliveryBasic { get; set; }

        [Display(Name = @"Грузоотправитель")]
        public bool ShipperName { get; set; }

        [Display(Name = @"Грузополучатель")]
        public bool ConsigneeName { get; set; }

        public bool CanShowManufacture { get; set; }


    }

}