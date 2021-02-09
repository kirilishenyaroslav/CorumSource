using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Web.Routing;
using Corum.Models;
using Corum.Models.ViewModels;
using Corum.Common;
using Corum.RestRenderModels;
using Corum.Models.Interfaces;
using CorumAdminUI.Controllers;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels.OrderConcurs;


namespace Corum.ReportsUI
{
    [Authorize]
    public class ExportToExcelController : CorumBaseController
    {
        protected IReportRenderer report;


        public ExportToExcelController()
        {
            report = DependencyResolver.Current.GetService<IReportRenderer>();
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult ConcursAsExcel(OrderNavigationInfo navInfo)
        {
            var concurs = context.getOrderCompetitiveList(userId, navInfo.OrderId);

            var Data = new RestDataInfo<OrderCompetitiveListViewModel>();

            Data.Rows.AddRange(concurs);


            var concursHeader = context.getCompetitiveListInfo(navInfo.OrderId);

            //Поднимаем из БД базовую информацию о заявке
            OrderBaseViewModel OrderTypeModel = null;
            var orderInfo = context.getOrder(navInfo.OrderId);
            //OrdersPassTransportViewModel extOrderTypeModel1 = null;
            var extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
            var extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
            int OrderType = 6;
            string AdressFrom, AdressTo;
            AdressFrom = "";
            AdressTo = "";
            if (orderInfo != null)
            {
                var DefaultCounty = context.getDefaultCountry();
                switch (orderInfo.OrderType)
                {
                    case 1:
                    case 3:
                    case 6:
                        OrderType = 6;
                        OrderTypeModel = orderInfo.ConvertTo<OrdersPassTransportViewModel>();
                        extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
                        context.getPassTrasportOrderData(ref extOrderTypeModel1);

                        if (((OrdersPassTransportViewModel) OrderTypeModel).CountryFrom == 0)
                        {
                            ((OrdersPassTransportViewModel) OrderTypeModel).CountryFrom = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel) OrderTypeModel).CountryFromName = DefaultCounty.CountryName;
                        }
                        if (((OrdersPassTransportViewModel) OrderTypeModel).CountryTo == 0)
                        {
                            ((OrdersPassTransportViewModel) OrderTypeModel).CountryTo = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel) OrderTypeModel).CountryToName = DefaultCounty.CountryName;
                        }
                        ((OrdersPassTransportViewModel) OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                        ((OrdersPassTransportViewModel) OrderTypeModel).DefaultCountryName = DefaultCounty.CountryName;
                        AdressFrom = context.GetFromInfoForExport(extOrderTypeModel1.Id);
                        AdressTo = context.GetToInfoForExport(extOrderTypeModel1.Id);
                        break;
                    case 4:
                    case 5:
                    case 7:
                        OrderType = 7;
                        OrderTypeModel = orderInfo.ConvertTo<OrdersTruckTransportViewModel>();
                        extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
                        context.getTruckTrasportOrderData(ref extOrderTypeModel2);

                        if ((((OrdersTruckTransportViewModel) OrderTypeModel).ShipperCountryId == 0) ||
                            (((OrdersTruckTransportViewModel) OrderTypeModel).TripType < 2))
                        {
                            ((OrdersTruckTransportViewModel) OrderTypeModel).ShipperCountryId = DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel) OrderTypeModel).ShipperCountryName =
                                DefaultCounty.CountryName;
                        }
                        if ((((OrdersTruckTransportViewModel) OrderTypeModel).ConsigneeCountryId == 0) ||
                            (((OrdersTruckTransportViewModel) OrderTypeModel).TripType < 2))
                        {
                            ((OrdersTruckTransportViewModel) OrderTypeModel).ConsigneeCountryId = DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel) OrderTypeModel).ConsigneeCountryName =
                                DefaultCounty.CountryName;
                        }

                        ((OrdersTruckTransportViewModel) OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                        ((OrdersTruckTransportViewModel) OrderTypeModel).DefaultCountryName = DefaultCounty.CountryName;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            byte[] fileContents = null;

            var Params = new RestParamsInfo();
            Params.Language = Request.UserLanguages[0];
            if (OrderType == 6)
            Params.MainHeader = "Конкурентный лист автоперевозки к Заявке на легковой автотранспорт";// + OrderTypeModel.Id.ToString();
            else if (OrderType == 7)
                Params.MainHeader = "Конкурентный лист автоперевозки к Заявке на грузовой автотранспорт";// + OrderTypeModel.Id.ToString();

            fileContents = report.ConcursRenderReport<OrderCompetitiveListViewModel>(Data, concursHeader, Params, OrderTypeModel, extOrderTypeModel1, extOrderTypeModel2);
            //fileContents = report.OrderRenderReport<OrderBaseViewModel>(OrderTypeModel, extOrderTypeModel1, AcceptDate, orderClientInfo, Param, AdressFrom, AdressTo, ContractName, extOrderTypeModel2, OrderType, carList);
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ConcursReport.xlsx");

        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult FinalReportAsExcel(OrdersNavigationInfo navInfo)
        {

            if (string.IsNullOrEmpty(navInfo.FilterOrderClientId))
            {
                navInfo.UseOrderClientFilter = false;
            }
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                navInfo.UseOrderTypeFilter = false;
            }

            if (navInfo.DateType == null) navInfo.DateType = 0;

            if (navInfo.DateType == 0)
            {
                navInfo.UseOrderDateFilter = true;
                navInfo.UseAcceptDateFilter = false;
            }
            else
            {
                navInfo.UseOrderDateFilter = false;
                navInfo.UseAcceptDateFilter = true;
            }

            if (navInfo.FilterOrderDateBeg == null)
            {
                navInfo.FilterOrderDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
            }
            if (navInfo.FilterOrderDateEnd == null)
            {
                navInfo.FilterOrderDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));
            }

            if (navInfo.FilterAcceptDateBeg == null)
            {
                navInfo.FilterAcceptDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterAcceptDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
            }
            if (navInfo.FilterAcceptDateEnd == null)
            {
                navInfo.FilterAcceptDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterAcceptDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));
            }

            var orders = context.getFinalReport(userId,                
                    this.isAdmin,
                    navInfo.UseOrderClientFilter,
                    navInfo.UseOrderTypeFilter,
                    navInfo.UseTripTypeFilter,
                    navInfo.FilterOrderClientId,
                    navInfo.FilterOrderTypeId,
                    navInfo.FilterTripTypeId,                    
                    string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw),
                string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw)
                    ? DateTime.Now
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw),
                string.IsNullOrEmpty(navInfo.FilterAcceptDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateBegRaw),
                string.IsNullOrEmpty(navInfo.FilterAcceptDateEndRaw)
                    ? DateTime.Now
                    : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateEndRaw),
                navInfo.UseOrderDateFilter,
                navInfo.UseAcceptDateFilter,

                navInfo.isPassOrders).ToList();

            List<string> orderStatus = new List<string>();
            orderStatus = orders.SelectMany(o => o.OrderStatusName).Distinct().ToList();

            var Data = new RestDataInfo<IDictionary<string, Object>>();


            foreach (var order in orders)
            {
                var orderForReport = new ExpandoObject() as IDictionary<string, Object>;

                orderForReport.Add("CntAll", order.CntAll);
                orderForReport.Add("CntAllNotFinal", order.CntAllNotFinal);
                int i = 3;
                foreach (var status in order.OrderStatus)
                {

                    orderForReport.Add("NewProp" + i.ToString(), status);
                    i++;
                }
                Data.Rows.Add(orderForReport);
            }


            byte[] fileContents = null;

            var Params = new RestParamsInfo();
            Params.Language = Request.UserLanguages[0];
            var DateBeg = DateTime.Now;
            var DateEnd = DateTime.Now;

            if (navInfo.UseOrderDateFilter)
            {
                DateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw);            
                DateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw);
            }
        else if (navInfo.UseAcceptDateFilter)
            {
                DateBeg = string.IsNullOrEmpty(navInfo.FilterAcceptDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateBegRaw);            
                DateEnd = string.IsNullOrEmpty(navInfo.FilterAcceptDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateEndRaw);
            }

            var DateBegRaw = DateBeg.ToString("dd.MM.yyyy");
            var DateEndRaw = DateEnd.ToString("dd.MM.yyyy");
            Params.MainHeader = "Отчет выполнения заявок на транспортные услуги за " + DateBegRaw + " - " + DateEndRaw;

            fileContents = report.FinalReportRenderReport<IDictionary<string, Object>>(Data, Params, orderStatus);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FinalReport.xlsx");

        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult OrdersReportAsExcel(OrdersNavigationInfo navInfo)
        {

            if (string.IsNullOrEmpty(navInfo.FilterOrderClientId)) { navInfo.UseOrderClientFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId)) { navInfo.UseOrderTypeFilter = false; }

            if (navInfo.DateType == null) navInfo.DateType = 0;

            if (navInfo.DateType == 0)
            {
                navInfo.UseOrderDateFilter = true;
                navInfo.UseAcceptDateFilter = false;
            }
            else
            {
                navInfo.UseOrderDateFilter = false;
                navInfo.UseAcceptDateFilter = true;
            }

            if (navInfo.FilterOrderDateBeg == null)
            {
                navInfo.FilterOrderDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
            }
            if (navInfo.FilterOrderDateEnd == null)
            {
                navInfo.FilterOrderDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));
            }

            if (navInfo.FilterAcceptDateBeg == null)
            {
                navInfo.FilterAcceptDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterAcceptDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
            }
            if (navInfo.FilterAcceptDateEnd == null)
            {
                navInfo.FilterAcceptDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterAcceptDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));
            }

            var orders = context.getOrdersReport(userId,
                                                this.isAdmin,
                                                navInfo.UseOrderClientFilter,
                                                navInfo.UseOrderTypeFilter,
                                                navInfo.UseTripTypeFilter,
                                                navInfo.FilterOrderClientId,
                                                navInfo.FilterOrderTypeId,
                                                navInfo.FilterTripTypeId, 
                                                string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw) ? DateTime.Now.AddDays(-7) : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw),
                                                string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw) ? DateTime.Now : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw),
                                                string.IsNullOrEmpty(navInfo.FilterAcceptDateBegRaw)
                                                ? DateTime.Now.AddDays(-7)
                                                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateBegRaw),
                                             string.IsNullOrEmpty(navInfo.FilterAcceptDateEndRaw)
                                                ? DateTime.Now
                                                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateEndRaw),
                                             navInfo.UseOrderDateFilter,
                                             navInfo.UseAcceptDateFilter,

                                                navInfo.isPassOrders).ToList();


            List<string> balanceKeepers = new List<string>();
            balanceKeepers = orders.SelectMany(o => o.BalanceKeepersName).Distinct().ToList();


            var Data = new RestDataInfo<IDictionary<string, Object>>();

            foreach (var order in orders)
            {
                var orderForReport = new ExpandoObject() as IDictionary<string, Object>;
                orderForReport.Add("CntName", order.CntName);
                orderForReport.Add("CntOrders", order.CntOrders);
                int i = 3;
                foreach (var status in order.BalanceKeepers)
                {

                    orderForReport.Add("NewProp" + i.ToString(), status);
                    i++;
                }
                Data.Rows.Add(orderForReport);
            }

            byte[] fileContents = null;

            var Params = new RestParamsInfo();
            Params.Language = Request.UserLanguages[0];

            var DateBeg = DateTime.Now;
            var DateEnd = DateTime.Now;

            if (navInfo.UseOrderDateFilter)
            {
                DateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw);
                DateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw);
            }
            else if (navInfo.UseAcceptDateFilter)
            {
                DateBeg = string.IsNullOrEmpty(navInfo.FilterAcceptDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateBegRaw);
                DateEnd = string.IsNullOrEmpty(navInfo.FilterAcceptDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateEndRaw);
            }

            var DateBegRaw = DateBeg.ToString("dd.MM.yyyy");
            var DateEndRaw = DateEnd.ToString("dd.MM.yyyy");
            Params.MainHeader = "Отчет выполнения заявок на транспортные услуги за " + DateBegRaw + " - " + DateEndRaw;

            fileContents = report.OrdersReportRenderReport<IDictionary<string, Object>>(Data, Params, balanceKeepers);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrdersReport.xlsx");

        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult BaseReportAsExcel(OrdersNavigationInfo navInfo)
        {

            if (string.IsNullOrEmpty(navInfo.FilterOrderClientId)) { navInfo.UseOrderClientFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId)) { navInfo.UseOrderTypeFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterTripTypeId)) { navInfo.UseTripTypeFilter = false; }

            if (navInfo.DateType == null) navInfo.DateType = 0;

            if (navInfo.DateType == 0)
            {
                navInfo.UseOrderDateFilter = true;
                navInfo.UseAcceptDateFilter = false;
            }
            else
            {
                navInfo.UseOrderDateFilter = false;
                navInfo.UseAcceptDateFilter = true;
            }

            if (navInfo.FilterOrderDateBeg == null)
            {
                navInfo.FilterOrderDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
            }
            if (navInfo.FilterOrderDateEnd == null)
            {
                navInfo.FilterOrderDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));
            }

            if (navInfo.FilterAcceptDateBeg == null)
            {
                navInfo.FilterAcceptDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterAcceptDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
            }
            if (navInfo.FilterAcceptDateEnd == null)
            {
                navInfo.FilterAcceptDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterAcceptDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));
            }

            var orders = context.getBaseReport(userId,
                                                this.isAdmin,
                                                navInfo.UseOrderClientFilter,
                                                navInfo.UseOrderTypeFilter,
                                                navInfo.UseTripTypeFilter,
                                                navInfo.FilterOrderClientId,
                                                navInfo.FilterOrderTypeId,
                                                navInfo.FilterTripTypeId,
                                                string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw) ? DateTime.Now.AddDays(-7) : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw),
                                                string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw) ? DateTime.Now : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw),
                                                string.IsNullOrEmpty(navInfo.FilterAcceptDateBegRaw)
                                                ? DateTime.Now.AddDays(-7)
                                                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateBegRaw),
                                             string.IsNullOrEmpty(navInfo.FilterAcceptDateEndRaw)
                                                ? DateTime.Now
                                                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateEndRaw),
                                             navInfo.UseOrderDateFilter,
                                             navInfo.UseAcceptDateFilter,

                                                navInfo.isPassOrders).ToList();

            int SumPlanCarNumber = 0;
            int SumFactCarNumber = 0;
            SumPlanCarNumber = orders.Sum(item => item.CarNumber);
            SumFactCarNumber = orders.Sum(item => item.FactCarNumber);

            //получаем типы заявок
            var orderTypes = orders.Select(o => o.OrderType).Distinct().ToList();

            List<string> orderFinalStatuses = new List<string>();
            foreach (var OrderType in orderTypes)
            {
                var OrderStatuses = context.getFinalPipelineSteps(userId, OrderType);

                orderFinalStatuses.AddRange(OrderStatuses);
            }
            List<string> FinalStatuses = new List<string>();

            //только различные
            FinalStatuses = orderFinalStatuses.Distinct().ToList();
            //отсортировали
            FinalStatuses = FinalStatuses.OrderBy(q => q).ToList();

            //Есть список фин. статусов, теперь нужно пройти и собрать по ним суммы
            Dictionary<string, int> orderFinalStatusesDict = new Dictionary<string, int>();
            int StatusSumm = 0;
            foreach (var FinalStatus in FinalStatuses)
            {
                var StatusList = orders.Where(ri => ri.CurrentOrderStatusName == FinalStatus).ToList();
                StatusSumm = StatusList.Count;
                orderFinalStatusesDict.Add(FinalStatus, StatusSumm);
            }


            var Data = new RestDataInfo<BaseReportViewModel>();

            Data.Rows.AddRange(orders);

            byte[] fileContents = null;

            var Params = new RestParamsInfo();
            Params.Language = Request.UserLanguages[0];
            var DateBeg = DateTime.Now;
            var DateEnd = DateTime.Now;

            if (navInfo.UseOrderDateFilter)
            {
                DateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw);
                DateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw);
            }
            else if (navInfo.UseAcceptDateFilter)
            {
                DateBeg = string.IsNullOrEmpty(navInfo.FilterAcceptDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateBegRaw);
                DateEnd = string.IsNullOrEmpty(navInfo.FilterAcceptDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateEndRaw);
            }

            var DateBegRaw = DateBeg.ToString("dd.MM.yyyy");
            var DateEndRaw = DateEnd.ToString("dd.MM.yyyy");
            Params.MainHeader = "Отчет выполнения заявок на транспортные услуги за " + DateBegRaw + " - " + DateEndRaw;
            //fileContents = report.RenderReport(Header, Data, Footer, Params);

            fileContents = report.BaseReportRenderReport<BaseReportViewModel>(Data, Params, orderFinalStatusesDict, SumPlanCarNumber, SumFactCarNumber);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaseReport.xlsx");

        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult AllOrdersAsExcel(OrdersNavigationInfo navInfo)
        {
            //Поднимаем из БД базовую информацию о заявке

            if (string.IsNullOrEmpty(navInfo.FilterStatusId)) { navInfo.UseStatusFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderCreatorId)) { navInfo.UseOrderCreatorFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId)) { navInfo.UseOrderTypeFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderClientId)) { navInfo.UseOrderClientFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderExecuterId)) { navInfo.UseOrderExecuterFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderPayerId)) { navInfo.UseOrderPayerFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderOrgFromId)) { navInfo.UseOrderOrgFromFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderOrgToId)) { navInfo.UseOrderOrgToFilter = false; }

            if ((!navInfo.UseStatusFilter)
             && (!navInfo.UseOrderCreatorFilter)
             && (!navInfo.UseOrderTypeFilter)
             && (!navInfo.UseOrderClientFilter)
             && (!navInfo.UseOrderExecuterFilter)
             && (!navInfo.UseOrderPriorityFilter)
             && (!navInfo.UseOrderDateFilter)
             && (!navInfo.UseOrderExDateFilter)
             && (!navInfo.UseOrderEndDateFilter)
             && (!navInfo.UseFinalStatusFilter)
             && (!navInfo.UseOrderProjectFilter)
              /* && (!navInfo.UseOrderPayerFilter)
               && (!navInfo.UseOrderOrgFromFilter)
               && (!navInfo.UseOrderOrgToFilter)*/)
            {
                navInfo.UseOrderDateFilter = true;

                navInfo.FilterOrderDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
                navInfo.FilterOrderDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));

                navInfo.UseFinalStatusFilter = true;
                navInfo.FilterFinalStatus = false;

            }

            if (!string.IsNullOrEmpty(navInfo.FilterStatusId))
            {
                string[] idList = navInfo.FilterStatusId.Split(new char[] { ',' });
                if ((idList.Length == 1) && (Convert.ToInt32(idList[0]) == 0))
                {
                    navInfo.UseStatusFilter = false;
                }
            }

            var AllOrders = context.getOrders(true,
                userId,
                this.isAdmin,
                navInfo.UseStatusFilter,
                navInfo.FilterStatusId,
                navInfo.UseOrderCreatorFilter,
                navInfo.FilterOrderCreatorId,
                navInfo.UseOrderTypeFilter,
                navInfo.FilterOrderTypeId,
                navInfo.UseOrderClientFilter,
                navInfo.FilterOrderClientId,
                navInfo.UseOrderPriorityFilter,
                navInfo.FilterOrderPriority,
                navInfo.UseOrderDateFilter,
                string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw),
            
                
                string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw)
                    ? DateTime.Now
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw),
                navInfo.UseOrderExDateFilter,
                string.IsNullOrEmpty(navInfo.FilterOrderExDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderExDateBegRaw),
                string.IsNullOrEmpty(navInfo.FilterOrderExDateEndRaw)
                    ? DateTime.Now
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderExDateEndRaw),

                 navInfo.UseOrderEndDateFilter,
                string.IsNullOrEmpty(navInfo.FilterOrderEndDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderEndDateBegRaw),
                string.IsNullOrEmpty(navInfo.FilterOrderEndDateEndRaw)
                    ? DateTime.Now
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderEndDateEndRaw),

                navInfo.FilterOrderExecuterId,
                navInfo.UseOrderExecuterFilter,
                navInfo.UseFinalStatusFilter,
                navInfo.FilterFinalStatus,
                navInfo.UseOrderProjectFilter,
                navInfo.FilterOrderProjectId,
                navInfo.UseOrderPayerFilter,
                navInfo.FilterOrderPayerId,
                navInfo.FilterOrderOrgFromId,
                navInfo.UseOrderOrgFromFilter,
                navInfo.FilterOrderOrgToId,
                navInfo.UseOrderOrgToFilter
                );

            List<OrdersTruckTransportViewModel> ordersList = new List<OrdersTruckTransportViewModel>();
            List<OrdersPassTransportViewModel> ordersPassList = new List<OrdersPassTransportViewModel>();


            foreach (var order in AllOrders)
            {
                var DefaultCounty = context.getDefaultCountry();

                //заявки легковые
                if ((order.OrderType == 1) || (order.OrderType == 3) || (order.OrderType == 6))
                {
                    OrdersPassTransportViewModel orderPassItem = new OrdersPassTransportViewModel();

                    OrderBaseViewModel OrderPassTypeModel = null;
                    var orderPassInfo = context.getOrder(order.Id);                    
                    var extOrderTypeModel1 = (OrderPassTypeModel as OrdersPassTransportViewModel);

                    int OrderPassType = 6;
                    string AdressPassFrom, AdressPassTo;
                    AdressPassFrom = "";
                    AdressPassTo = "";

                    if (orderPassInfo != null)
                    {
                        OrderPassTypeModel = orderPassInfo.ConvertTo<OrdersPassTransportViewModel>();
                        extOrderTypeModel1 = (OrderPassTypeModel as OrdersPassTransportViewModel);
                        context.getPassTrasportOrderData(ref extOrderTypeModel1);
                        orderPassItem = extOrderTypeModel1;

                        if (((OrdersPassTransportViewModel)OrderPassTypeModel).CountryFrom == 0)
                        {
                            ((OrdersPassTransportViewModel)OrderPassTypeModel).CountryFrom = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderPassTypeModel).CountryFromName =
                                DefaultCounty.CountryName;

                            orderPassItem.CountryFrom = DefaultCounty.Id;
                            orderPassItem.CountryFromName = DefaultCounty.CountryName;
                        }
                        if (((OrdersPassTransportViewModel)OrderPassTypeModel).CountryTo == 0)
                        {
                            ((OrdersPassTransportViewModel)OrderPassTypeModel).CountryTo = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderPassTypeModel).CountryToName = DefaultCounty.CountryName;

                            orderPassItem.CountryTo = DefaultCounty.Id;
                            orderPassItem.CountryToName = DefaultCounty.CountryName;
                        }

                        ((OrdersPassTransportViewModel)OrderPassTypeModel).DefaultCountry = DefaultCounty.Id;
                        ((OrdersPassTransportViewModel)OrderPassTypeModel).DefaultCountryName = DefaultCounty.CountryName;

                        orderPassItem.DefaultCountry = DefaultCounty.Id;
                        orderPassItem.DefaultCountryName = DefaultCounty.CountryName;
                                                
                        ordersPassList.Add(orderPassItem);
                    }
                }
                else
                //заявки грузовые
                if ((order.OrderType == 4) || (order.OrderType == 5) || (order.OrderType == 7))
                {
                    OrdersTruckTransportViewModel orderItem = new OrdersTruckTransportViewModel();
                    OrdersTruckTransportViewModel OrderTypeModel = null;
                    var orderInfo = context.getOrder(order.Id);
                    // var extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
                    var extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
                    int OrderType = 7;
                    string AdressFrom, AdressTo;
                    AdressFrom = "";
                    AdressTo = "";

                    if (orderInfo != null)
                    {
                        OrderTypeModel = orderInfo.ConvertTo<OrdersTruckTransportViewModel>();
                        extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
                        context.getTruckTrasportOrderData(ref extOrderTypeModel2);
                        orderItem = extOrderTypeModel2;

                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryId == 0) ||
                            (((OrdersTruckTransportViewModel)OrderTypeModel).TripType < 2))
                        {
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryId = DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryName =
                                DefaultCounty.CountryName;

                            orderItem.ShipperCountryId = DefaultCounty.Id;
                            orderItem.ShipperCountryName = DefaultCounty.CountryName;
                        }
                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryId == 0) ||
                            (((OrdersTruckTransportViewModel)OrderTypeModel).TripType < 2))
                        {
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryId =
                                DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryName =
                                DefaultCounty.CountryName;

                            orderItem.ConsigneeCountryId = DefaultCounty.Id;
                            orderItem.ConsigneeCountryName = DefaultCounty.CountryName;
                        }

                        ((OrdersTruckTransportViewModel)OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                        ((OrdersTruckTransportViewModel)OrderTypeModel).DefaultCountryName =
                            DefaultCounty.CountryName;

                        orderItem.DefaultCountry = DefaultCounty.Id;
                        orderItem.DefaultCountryName = DefaultCounty.CountryName;

                        ordersList.Add(orderItem);
                    }
                }
            }
            
            var Param = new RestParamsInfo();
            Param.Language = Request.UserLanguages[0];
            string MainHeader = "";
            Param.MainHeader = MainHeader;
            var reportName = "OrderReport.xlsx";
            
            byte[] fileContents;

            fileContents = report.AllOrderRenderReport<OrderBaseViewModel>(ordersPassList.OrderBy(x => x.Id).ToList(),
                ordersList.OrderBy(x => x.Id).ToList(), Param);
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportName);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult OrdersAsExcel(bool isTransport, OrdersNavigationInfo navInfo)
        {

            if (string.IsNullOrEmpty(navInfo.FilterStatusId)) { navInfo.UseStatusFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderCreatorId)) { navInfo.UseOrderCreatorFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId)) { navInfo.UseOrderTypeFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderClientId)) { navInfo.UseOrderClientFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderExecuterId)) { navInfo.UseOrderExecuterFilter = false; }
            if (!string.IsNullOrEmpty(navInfo.FilterStatusId))
            {
                string[] idList = navInfo.FilterStatusId.Split(new char[] { ',' });
                if ((idList.Length == 1) && (Convert.ToInt32(idList[0]) == 0))
                {
                    navInfo.UseStatusFilter = false;
                }
            }

            var orders = context.getOrders(isTransport,
                                                    userId,
                                                    this.isAdmin,
                                                    navInfo.UseStatusFilter,
                                                    navInfo.FilterStatusId,
                                                    navInfo.UseOrderCreatorFilter,
                                                    navInfo.FilterOrderCreatorId,
                                                    navInfo.UseOrderTypeFilter,
                                                    navInfo.FilterOrderTypeId,
                                                    navInfo.UseOrderClientFilter,
                                                    navInfo.FilterOrderClientId,
                                                    navInfo.UseOrderPriorityFilter,
                                                    navInfo.FilterOrderPriority,
                                                    navInfo.UseOrderDateFilter,
                                                    string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw) ? DateTime.Now.AddDays(-7) : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw),
                                                    string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw) ? DateTime.Now : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw),
                                                    navInfo.UseOrderExDateFilter,
                                                    string.IsNullOrEmpty(navInfo.FilterOrderExDateBegRaw) ? DateTime.Now.AddDays(-7) : DateTimeConvertClass.getDateTime(navInfo.FilterOrderExDateBegRaw),
                                                    string.IsNullOrEmpty(navInfo.FilterOrderExDateEndRaw) ? DateTime.Now : DateTimeConvertClass.getDateTime(navInfo.FilterOrderExDateEndRaw),

                                                    navInfo.UseOrderEndDateFilter,
                                                    string.IsNullOrEmpty(navInfo.FilterOrderEndDateBegRaw) ? DateTime.Now.AddDays(-7) : DateTimeConvertClass.getDateTime(navInfo.FilterOrderEndDateBegRaw),
                                                    string.IsNullOrEmpty(navInfo.FilterOrderEndDateEndRaw) ? DateTime.Now : DateTimeConvertClass.getDateTime(navInfo.FilterOrderEndDateEndRaw),

                                                    navInfo.FilterOrderExecuterId,
                                                    navInfo.UseOrderExecuterFilter,
                                                    navInfo.UseFinalStatusFilter,
                                                    navInfo.FilterFinalStatus,
                                                    navInfo.UseOrderProjectFilter,
                                                    navInfo.FilterOrderProjectId,
                                                    navInfo.UseOrderPayerFilter,
                                                    navInfo.FilterOrderPayerId,
                                                    navInfo.FilterOrderOrgFromId,
                                                    navInfo.UseOrderOrgFromFilter,
                                                    navInfo.FilterOrderOrgToId,
                                                    navInfo.UseOrderOrgToFilter
                                                    ).ToList();

            byte[] fileContents = null;

            var Header = new RestHeaderInfo();

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Плательщик перевозки",
                columnOrder = 1,
                columnField = "PayerName",
                columnWidth = 50,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Дата фактической постановки авто",
                columnOrder = 2,
                columnField = "StartExecuteDate",
                columnWidth = 50,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Пункт отправления",
                columnOrder = 2,
                columnField = "StartExecuteDate",
                columnWidth = 50,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            var Data = new RestDataInfo<OrderBaseViewModel>();
            Data.Rows.AddRange(orders);
            var Footer = new RestFooterInfo();
            var Params = new RestParamsInfo();
            Params.Language = Request.UserLanguages[0];

            fileContents = report.RenderReport(Header, Data, Footer, Params);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult OrderAsExcel(int id)
        {
            //Поднимаем из БД базовую информацию о заявке
            OrderBaseViewModel OrderTypeModel = null;
            var orderInfo = context.getOrder(id);
            //OrdersPassTransportViewModel extOrderTypeModel1 = null;
            var extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
            var extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
            int OrderType = 6;
            string AdressFrom, AdressTo;
            AdressFrom = "";
            AdressTo = "";
            if (orderInfo != null)
            {
                var DefaultCounty = context.getDefaultCountry();
                switch (orderInfo.OrderType)
                {
                    case 1:
                    case 3:
                    case 6:
                        OrderType = 6;
                        OrderTypeModel = orderInfo.ConvertTo<OrdersPassTransportViewModel>();
                        extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
                        context.getPassTrasportOrderData(ref extOrderTypeModel1);

                        if (((OrdersPassTransportViewModel)OrderTypeModel).CountryFrom == 0)
                        {
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFrom = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFromName = DefaultCounty.CountryName;
                        }
                        if (((OrdersPassTransportViewModel)OrderTypeModel).CountryTo == 0)
                        {
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryTo = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryToName = DefaultCounty.CountryName;
                        }

                       ((OrdersPassTransportViewModel)OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                        ((OrdersPassTransportViewModel)OrderTypeModel).DefaultCountryName = DefaultCounty.CountryName;

                        AdressFrom = context.GetFromInfoForExport(extOrderTypeModel1.Id);
                        AdressTo = context.GetToInfoForExport(extOrderTypeModel1.Id);

                        break;

                    case 4:
                    case 5:
                    case 7:
                        OrderType = 7;
                        OrderTypeModel = orderInfo.ConvertTo<OrdersTruckTransportViewModel>();
                        extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
                        context.getTruckTrasportOrderData(ref extOrderTypeModel2);

                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryId == 0) || (((OrdersTruckTransportViewModel)OrderTypeModel).TripType < 2))
                        {
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryId = DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryName = DefaultCounty.CountryName;
                        }
                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryId == 0) || (((OrdersTruckTransportViewModel)OrderTypeModel).TripType < 2))
                        {
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryId = DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryName = DefaultCounty.CountryName;
                        }

                       ((OrdersTruckTransportViewModel)OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                        ((OrdersTruckTransportViewModel)OrderTypeModel).DefaultCountryName = DefaultCounty.CountryName;

                        break;

                    default:
                        throw new NotImplementedException();
                }

                var Param = new RestParamsInfo();
                Param.Language = Request.UserLanguages[0];
                string MainHeader = "";
                if (OrderType == 6)
                    MainHeader = "Заявка на транспортные средства категории В ";
                else if (OrderType == 7)
                    MainHeader = "ЗАЯВКА НА ПЕРЕВОЗКУ ГРУЗОВ ПО МАРШРУТУ  № " + OrderTypeModel.Id.ToString();

                Param.MainHeader = MainHeader;

                string AcceptDate = context.GetAcceptDate(id);

                OrderClientsViewModel orderClientInfo = context.getClient(OrderTypeModel.ClientId);

                var reportName = "OrderReport " + OrderTypeModel.Id.ToString() + ".xlsx";

                string ContractName = context.getContactName(OrderTypeModel.Id);
                List<OrderUsedCarViewModel> carList = context.getOrderCarsInfo(OrderTypeModel.Id).ToList();
                byte[] fileContents;
                fileContents = report.OrderRenderReport<OrderBaseViewModel>(OrderTypeModel, extOrderTypeModel1, AcceptDate, orderClientInfo, Param, AdressFrom, AdressTo, ContractName, extOrderTypeModel2, OrderType, carList);

                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportName);
            }

            return null;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult SummaryReportAsExcel(RestNavigationInfo navInfo)
        {
            var Header = new RestHeaderInfo();

            string FirstColumnName = "";
            bool getComment = context.GetCommentColumnName(true, navInfo.CurrentGroupFieldName.ToString(), ref FirstColumnName);

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = FirstColumnName,
                columnOrder = 2,
                columnField = "groupItem",
                columnWidth = 20,
                ColumnBlockStart = false,
                ColumnBlockEnd = true,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Начальное кол-во",
                columnOrder = 3,
                columnField = "QuantityBefore",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Начальная масса, кг.",
                columnOrder = 4,
                columnField = "WeightBefore",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            if (navInfo.BalancePrice == true)
            {

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Начальная сумма в БС (грн. в ценах учета)",
                    columnOrder = 5,
                    columnField = "BP_Before",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Приход сумма в БС, (грн. в ценах учета)",
                    columnOrder = 14,
                    columnField = "BP_Prihod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Расход сумма в БС, (грн. в ценах учета)",
                    columnOrder = 23,
                    columnField = "BP_Rashod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Конечная сумма в БС, (грн. в ценах учета)",
                    columnOrder = 32,
                    columnField = "BP_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PriceForEndConsumer == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Начальная сумма в ЦКП (грн. в ценах учета)",
                    columnOrder = 6,
                    columnField = "PE_Before",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Приход сумма в ЦКП, (грн. в ценах учета)",
                    columnOrder = 15,
                    columnField = "PE_Prihod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Расход сумма в ЦКП, (грн. в ценах учета)",
                    columnOrder = 24,
                    columnField = "PE_Rashod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Конечная сумма в ЦКП, (грн. в ценах учета)",
                    columnOrder = 33,
                    columnField = "PE_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PriceForFirstReciver == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Начальная сумма в ЦПП (грн. в ценах учета)",
                    columnOrder = 7,
                    columnField = "PF_Before",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Приход сумма в ЦПП, (грн. в ценах учета)",
                    columnOrder = 16,
                    columnField = "PF_Prihod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Расход сумма в ЦПП, (грн. в ценах учета)",
                    columnOrder = 25,
                    columnField = "PF_Rashod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Конечная сумма в ЦПП, (грн. в ценах учета)",
                    columnOrder = 34,
                    columnField = "PF_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PlanFullCost == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Начальная сумма в CПП (грн. в ценах учета)",
                    columnOrder = 8,
                    columnField = "PCP_Before",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Приход сумма в CПП, (грн. в ценах учета)",
                    columnOrder = 17,
                    columnField = "PCP_Prihod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Расход сумма в CПП, (грн. в ценах учета)",
                    columnOrder = 26,
                    columnField = "PCP_Rashod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Конечная сумма в CПП, (грн. в ценах учета)",
                    columnOrder = 35,
                    columnField = "PCP_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }


            if (navInfo.PlanChangableCost == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Начальная сумма в CППР (грн. в ценах учета)",
                    columnOrder = 9,
                    columnField = "PCPC_Before",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Приход сумма в CППР, (грн. в ценах учета)",
                    columnOrder = 18,
                    columnField = "PCPC_Prihod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Расход сумма в CППР, (грн. в ценах учета)",
                    columnOrder = 27,
                    columnField = "PCPC_Rashod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Конечная сумма в CППР, (грн. в ценах учета)",
                    columnOrder = 36,
                    columnField = "PCPC_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.FactFullCosts == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Начальная сумма в CФП (грн. в ценах учета)",
                    columnOrder = 10,
                    columnField = "FCP_Before",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Приход сумма в CФП, (грн. в ценах учета)",
                    columnOrder = 19,
                    columnField = "FCP_Prihod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Расход сумма в CФП, (грн. в ценах учета)",
                    columnOrder = 28,
                    columnField = "FCP_Rashod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Конечная сумма в CФП, (грн. в ценах учета)",
                    columnOrder = 37,
                    columnField = "FCP_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.FactChangableCosts == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Начальная сумма в CФПР (грн. в ценах учета)",
                    columnOrder = 11,
                    columnField = "FCPC_Before",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Приход сумма в CФПР, (грн. в ценах учета)",
                    columnOrder = 20,
                    columnField = "FCPC_Prihod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Расход сумма в CФПР, (грн. в ценах учета)",
                    columnOrder = 29,
                    columnField = "FCPC_Rashod",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Конечная сумма в CФПР, (грн. в ценах учета)",
                    columnOrder = 38,
                    columnField = "FCPC_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Приход кол-во",
                columnOrder = 12,
                columnField = "QuantityPrihod",
                columnWidth = 12,
                ColumnBlockStart = true,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Приход масса, кг.",
                columnOrder = 13,
                columnField = "MassPrihod",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Расход кол-во",
                columnOrder = 21,
                columnField = "QuantityRashod",
                columnWidth = 12,
                ColumnBlockStart = true,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Расход масса, кг.",
                columnOrder = 22,
                columnField = "MassRashod",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Конечное кол-во",
                columnOrder = 30,
                columnField = "QuantityAfter",
                columnWidth = 12,
                ColumnBlockStart = true,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Конечная масса, кг.",
                columnOrder = 31,
                columnField = "WeightAfter",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            var order_items = context.GetGroupRestsBySnapShotId(navInfo.snapshotId, navInfo.CurrentGroupFieldName,
             new GroupItemFilters()
             {
                 FilterStorageId = navInfo.FilterStorageId,
                 FilterCenterId = navInfo.FilterCenterId,
                 FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                 FilterRecieverFactId = navInfo.FilterRecieverFactId,
                 FilterKeeperId = navInfo.FilterKeeperId,

                 UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                 UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                 UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                 UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                 UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
             });

            var Data = new RestDataInfo<GroupItemRestViewModel>();

            Data.Rows.AddRange(order_items);
            /***************************************************************************************************************/

            //************************** итоговые цифры   ******************************************************************/
            SnapshotInfoViewModel maxAvialiableSnapshot = null;

            maxAvialiableSnapshot = navInfo.snapshotId > 0 ?
                context.GetScreenShotById(navInfo.snapshotId) : context.GetMaxScreenShot();

            var columns = context.GetRestColumnsForGroupReport(navInfo.CurrentGroupFieldName);

            if (string.IsNullOrEmpty(navInfo.CurrentGroupFieldName))
            {
                var defColumn = columns.First();
                navInfo.CurrentGroupFieldName = defColumn.Key;
                navInfo.CurrentGroupFieldNameDescription = defColumn.Value;
            }
            else
            {
                navInfo.CurrentGroupFieldNameDescription = columns.FirstOrDefault(c => c.Key == navInfo.CurrentGroupFieldName).Value;
            }
            var model = new RestsNavigationResult<GroupItemRestViewModel>(navInfo, userId)
            {
                DisplayValues = context.GetGroupRestsBySnapShotId(maxAvialiableSnapshot.Id, navInfo.CurrentGroupFieldName,
                new GroupItemFilters()
                {
                    FilterStorageId = navInfo.FilterStorageId,
                    FilterCenterId = navInfo.FilterCenterId,
                    FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                    FilterRecieverFactId = navInfo.FilterRecieverFactId,
                    FilterKeeperId = navInfo.FilterKeeperId,
                    FilterProducerId = navInfo.FilterProducerId,

                    UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                    UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                    UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                    UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                    UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                    UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter
                }),
                //Price columns settings
                PriceForEndConsumer = navInfo.PriceForEndConsumer,
                PriceForFirstReciver = navInfo.PriceForFirstReciver,
                PlanFullCost = navInfo.PlanFullCost,
                PlanChangableCost = navInfo.PlanChangableCost,
                FactFullCosts = navInfo.FactFullCosts,
                FactChangableCosts = navInfo.FactChangableCosts,
                BalancePrice = navInfo.BalancePrice,
                SnapshotInfo = maxAvialiableSnapshot,

                //Group by column settings
                AvialableFields = columns,
                CurrentGroupFieldName = navInfo.CurrentGroupFieldName,
                CurrentGroupFieldNameDescription = navInfo.CurrentGroupFieldNameDescription,

                //Filter settings
                FilterStorageId = navInfo.FilterStorageId,
                FilterCenterId = navInfo.FilterCenterId,
                FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                FilterRecieverFactId = navInfo.FilterRecieverFactId,
                FilterKeeperId = navInfo.FilterKeeperId,
                FilterProducerId = navInfo.FilterProducerId,

                UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter,

                //Blocks columns settings
                BeforeColBlock = true,
                PrihodColBlock = true,
                RashodColBlock = true,
                AfterColBlock = true
            };

            if (!((model.BalancePrice) ||
                (model.PriceForEndConsumer) ||
                (model.PriceForFirstReciver) ||
                (model.PlanFullCost) ||
                (model.PlanChangableCost) ||
                (model.FactFullCosts) ||
                (model.FactChangableCosts)))
            {
                model.BalancePrice = true;
            }

            model.DisplayTotalValues = model.DisplayValues
                                           .GroupBy(ri => navInfo.CurrentGroupFieldName)
                                            .Select(g => new GroupItemRestViewModel
                                            {
                                                Position = int.MaxValue,
                                                groupItem = "ВСЕГО:",
                                                QuantityBefore = g.Sum(s => s.QuantityBefore),
                                                QuantityAfter = g.Sum(s => s.QuantityAfter),
                                                WeightBefore = g.Sum(s => s.WeightBefore),
                                                WeightAfter = g.Sum(s => s.WeightAfter),
                                                PE_Before = g.Sum(s => s.PE_Before),
                                                PF_Before = g.Sum(s => s.PF_Before),
                                                PCP_Before = g.Sum(s => s.PCP_Before),
                                                PCPC_Before = g.Sum(s => s.PCPC_Before),
                                                FCP_Before = g.Sum(s => s.FCP_Before),
                                                FCPC_Before = g.Sum(s => s.FCPC_Before),
                                                BP_Before = g.Sum(s => s.BP_Before),
                                                PE_After = g.Sum(s => s.PE_After),
                                                PF_After = g.Sum(s => s.PF_After),
                                                PCP_After = g.Sum(s => s.PCP_After),
                                                PCPC_After = g.Sum(s => s.PCPC_After),
                                                FCP_After = g.Sum(s => s.FCP_After),
                                                FCPC_After = g.Sum(s => s.FCPC_After),
                                                BP_After = g.Sum(s => s.BP_After),

                                                QuantityPrihod = g.Sum(s => s.QuantityPrihod),
                                                MassPrihod = g.Sum(s => s.MassPrihod),
                                                PE_Prihod = g.Sum(s => s.PE_Prihod),
                                                PF_Prihod = g.Sum(s => s.PF_Prihod),
                                                PCP_Prihod = g.Sum(s => s.PCP_Prihod),
                                                PCPC_Prihod = g.Sum(s => s.PCPC_Prihod),
                                                FCP_Prihod = g.Sum(s => s.FCP_Prihod),
                                                FCPC_Prihod = g.Sum(s => s.FCPC_Prihod),
                                                BP_Prihod = g.Sum(s => s.BP_Prihod),

                                                QuantityRashod = g.Sum(s => s.QuantityRashod),
                                                MassRashod = g.Sum(s => s.MassRashod),
                                                PE_Rashod = g.Sum(s => s.PE_Rashod),
                                                PF_Rashod = g.Sum(s => s.PF_Rashod),
                                                PCP_Rashod = g.Sum(s => s.PCP_Rashod),
                                                PCPC_Rashod = g.Sum(s => s.PCPC_Rashod),
                                                FCP_Rashod = g.Sum(s => s.FCP_Rashod),
                                                FCPC_Rashod = g.Sum(s => s.FCPC_Rashod),
                                                BP_Rashod = g.Sum(s => s.BP_Rashod)

                                            }).FirstOrDefault();


            var Footer = new RestFooterInfo();
            Footer.Footers.Add(2, model.DisplayTotalValues.groupItem.ToString());
            Footer.Footers.Add(3, model.DisplayTotalValues.QuantityBefore);
            Footer.Footers.Add(4, model.DisplayTotalValues.WeightBefore);

            if (navInfo.BalancePrice == true)
            {
                Footer.Footers.Add(5, model.DisplayTotalValues.BP_Before);
                Footer.Footers.Add(14, model.DisplayTotalValues.BP_Prihod);
                Footer.Footers.Add(23, model.DisplayTotalValues.BP_Rashod);
                Footer.Footers.Add(32, model.DisplayTotalValues.BP_After);
            }

            if (navInfo.PriceForEndConsumer == true)
            {
                Footer.Footers.Add(6, model.DisplayTotalValues.PE_Before);
                Footer.Footers.Add(15, model.DisplayTotalValues.PE_Prihod);
                Footer.Footers.Add(24, model.DisplayTotalValues.PE_Rashod);
                Footer.Footers.Add(33, model.DisplayTotalValues.PE_After);
            }

            if (navInfo.PriceForFirstReciver == true)
            {
                Footer.Footers.Add(7, model.DisplayTotalValues.PF_Before);
                Footer.Footers.Add(16, model.DisplayTotalValues.PF_Prihod);
                Footer.Footers.Add(25, model.DisplayTotalValues.PF_Rashod);
                Footer.Footers.Add(34, model.DisplayTotalValues.PF_After);
            }

            if (navInfo.PlanFullCost == true)
            {
                Footer.Footers.Add(8, model.DisplayTotalValues.PCP_Before);
                Footer.Footers.Add(17, model.DisplayTotalValues.PCP_Prihod);
                Footer.Footers.Add(26, model.DisplayTotalValues.PCP_Rashod);
                Footer.Footers.Add(35, model.DisplayTotalValues.PCP_After);
            }

            if (navInfo.PlanChangableCost == true)
            {
                Footer.Footers.Add(9, model.DisplayTotalValues.PCPC_Before);
                Footer.Footers.Add(18, model.DisplayTotalValues.PCPC_Prihod);
                Footer.Footers.Add(27, model.DisplayTotalValues.PCPC_Rashod);
                Footer.Footers.Add(36, model.DisplayTotalValues.PCPC_After);
            }

            if (navInfo.FactFullCosts == true)
            {
                Footer.Footers.Add(10, model.DisplayTotalValues.FCP_Before);
                Footer.Footers.Add(19, model.DisplayTotalValues.FCP_Prihod);
                Footer.Footers.Add(28, model.DisplayTotalValues.FCP_Rashod);
                Footer.Footers.Add(37, model.DisplayTotalValues.FCP_After);
            }

            if (navInfo.FactChangableCosts == true)
            {
                Footer.Footers.Add(11, model.DisplayTotalValues.FCPC_Before);
                Footer.Footers.Add(20, model.DisplayTotalValues.FCPC_Prihod);
                Footer.Footers.Add(29, model.DisplayTotalValues.FCPC_Rashod);
                Footer.Footers.Add(38, model.DisplayTotalValues.FCPC_After);
            }

            Footer.Footers.Add(12, model.DisplayTotalValues.QuantityPrihod);
            Footer.Footers.Add(13, model.DisplayTotalValues.MassPrihod);

            Footer.Footers.Add(21, model.DisplayTotalValues.QuantityRashod);
            Footer.Footers.Add(22, model.DisplayTotalValues.MassRashod);

            Footer.Footers.Add(30, model.DisplayTotalValues.QuantityAfter);
            Footer.Footers.Add(31, model.DisplayTotalValues.WeightAfter);

            var Param = new RestParamsInfo();
            Param.Language = Request.UserLanguages[0];

            string MainHeader = "Сводная форма по остаткам за период: ";
            string ActualDateBeg = "";
            string ActualDateEnd = "";
            bool getDate = context.GetDateSnapshot(navInfo.snapshotId, ref ActualDateBeg, ref ActualDateEnd);

            Param.MainHeader = MainHeader + ActualDateBeg + " - " + ActualDateEnd;

            if ((navInfo.UseCenterFilter == false) &&
                 (navInfo.UseKeeperFilter == false) &&
                 (navInfo.UseProducerFilter == false) &&
                 (navInfo.UseRecieverFactFilter == false) &&
                 (navInfo.UseRecieverPlanFilter == false) &&
                 (navInfo.UseStorageFilter == false))
                Param.Params.Add("Активный фильтр: Все данные информационного снимка", "");
            else
            {
                if (navInfo.UseCenterFilter == true)
                {
                    Param.Params.Add("Активный фильтр по ЦФО: ", navInfo.FilterCenterId);
                }

                if (navInfo.UseKeeperFilter == true)
                {
                    Param.Params.Add("Активный фильтр по балансодержателю: ", navInfo.FilterKeeperId);
                }

                if (navInfo.UseProducerFilter == true)
                {
                    Param.Params.Add("Активный фильтр по производителю: ", navInfo.FilterProducerId);
                }

                if (navInfo.UseRecieverFactFilter == true)
                {
                    Param.Params.Add("Активный фильтр по грузополучателю(факт): ", navInfo.FilterRecieverFactId);
                }

                if (navInfo.UseRecieverPlanFilter == true)
                {
                    Param.Params.Add("Активный фильтр по грузополучателю(план): ", navInfo.FilterRecieverPlanId);
                }

                if (navInfo.UseStorageFilter == true)
                {
                    Param.Params.Add("Активный фильтр по складу: ", navInfo.FilterStorageId);
                }
            }

            byte[] fileContents;
            fileContents = report.RenderReport<GroupItemRestViewModel>(Header, Data, Footer, Param);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SummaryReport.xlsx");
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult ReestrReportAsExcel(RestNavigationInfo navInfo)
        {
            var Header = new RestHeaderInfo();

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Склад",
                columnOrder = 2,
                columnField = "Storage",
                columnWidth = 35,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Продукт (шифр/артикул)",
                columnOrder = 3,
                columnField = "ProductFullName",
                columnWidth = 40,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Чертеж",
                columnOrder = 4,
                columnField = "Figure",
                columnWidth = 15,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Внутрений заказ ПП",
                columnOrder = 5,
                columnField = "InnerOrderNum",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Вес, (1 ед./кг.)",
                columnOrder = 6,
                columnField = "Weight",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Кол-во на конец периода",
                columnOrder = 7,
                columnField = "QuantityAfter",
                columnWidth = 12,
                ColumnBlockStart = true,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Масса, (кг.) на начало периода",
                columnOrder = 8,
                columnField = "MassAfter",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            if (navInfo.BalancePrice == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в БС, (грн. в ценах учета)",
                    columnOrder = 9,
                    columnField = "BP_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PriceForEndConsumer == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в ЦКП, (грн. в ценах учета)",
                    columnOrder = 10,
                    columnField = "PE_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PriceForFirstReciver == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в ЦПП, (грн. в ценах учета)",
                    columnOrder = 11,
                    columnField = "PF_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PlanFullCost == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в CПП, (грн. в ценах учета)",
                    columnOrder = 12,
                    columnField = "PCPC_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PlanChangableCost == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в CППР, (грн. в ценах учета)",
                    columnOrder = 13,
                    columnField = "PCPC_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.FactFullCosts == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в CФП, (грн. в ценах учета)",
                    columnOrder = 14,
                    columnField = "FCP_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.FactChangableCosts == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в CФПР, (грн. в ценах учета)",
                    columnOrder = 15,
                    columnField = "FCPC_After",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Клиент (Тип грузополучателя)",
                columnOrder = 16,
                columnField = "RecieverGroupPlan",
                columnWidth = 20,
                ColumnBlockStart = true,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Номер заказа",
                columnOrder = 17,
                columnField = "OrderNum",
                columnWidth = 10,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Готовность к реализации/Статус резервирования",
                columnOrder = 18,
                columnField = "FullSellStatus",
                columnWidth = 20,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            var order_items = context.GetRestsByScreenShotId(navInfo.snapshotId,
                new GroupItemFilters()
                {
                    FilterStorageId = navInfo.FilterStorageId,
                    FilterCenterId = navInfo.FilterCenterId,
                    FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                    FilterRecieverFactId = navInfo.FilterRecieverFactId,
                    FilterKeeperId = navInfo.FilterKeeperId,

                    UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                    UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                    UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                    UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                    UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                });

            var Data = new RestDataInfo<RestViewModel>();

            Data.Rows.AddRange(order_items);
            /***************************************************************************************************************/

            //************************** итоговые цифры   ******************************************************************/
            SnapshotInfoViewModel maxAvialiableSnapshot = null;

            maxAvialiableSnapshot = navInfo.snapshotId > 0 ?
                 context.GetScreenShotById(navInfo.snapshotId) : context.GetMaxScreenShot();

            var model = new RestsNavigationResult<RestViewModel>(navInfo, userId)
            {
                DisplayValues = context.GetRestsByScreenShotId(maxAvialiableSnapshot.Id,
                new GroupItemFilters()
                {
                    FilterStorageId = navInfo.FilterStorageId,
                    FilterCenterId = navInfo.FilterCenterId,
                    FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                    FilterRecieverFactId = navInfo.FilterRecieverFactId,
                    FilterKeeperId = navInfo.FilterKeeperId,
                    FilterProducerId = navInfo.FilterProducerId,

                    UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                    UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                    UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                    UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                    UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                    UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter
                }),

                PriceForEndConsumer = navInfo.PriceForEndConsumer,
                PriceForFirstReciver = navInfo.PriceForFirstReciver,
                PlanFullCost = navInfo.PlanFullCost,
                PlanChangableCost = navInfo.PlanChangableCost,
                FactFullCosts = navInfo.FactFullCosts,
                FactChangableCosts = navInfo.FactChangableCosts,
                BalancePrice = navInfo.BalancePrice,
                SnapshotInfo = maxAvialiableSnapshot,

                FilterStorageId = navInfo.FilterStorageId,
                FilterCenterId = navInfo.FilterCenterId,
                FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                FilterRecieverFactId = navInfo.FilterRecieverFactId,
                FilterKeeperId = navInfo.FilterKeeperId,
                FilterProducerId = navInfo.FilterProducerId,

                UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter
            };

            if (!((model.BalancePrice) ||
                (model.PriceForEndConsumer) ||
                (model.PriceForFirstReciver) ||
                (model.PlanFullCost) ||
                (model.PlanChangableCost) ||
                (model.FactFullCosts) ||
                (model.FactChangableCosts)))
            {
                model.BalancePrice = true;
            }

            model.DisplayTotalValues = model.DisplayValues
                                          .GroupBy(ri => navInfo.CurrentGroupFieldName)
                                           .Select(g => new RestViewModel
                                           {
                                               QuantityAfter = g.Sum(s => s.QuantityAfter),
                                               Weight = g.Sum(s => s.QuantityAfter * s.Weight),
                                               PE_After = g.Sum(s => s.PE_After),
                                               PF_After = g.Sum(s => s.PF_After),
                                               PCP_After = g.Sum(s => s.PCP_After),
                                               PCPC_After = g.Sum(s => s.PCPC_After),
                                               FCP_After = g.Sum(s => s.FCP_After),
                                               FCPC_After = g.Sum(s => s.FCPC_After),
                                               BP_After = g.Sum(s => s.BP_After)
                                           }).FirstOrDefault();

            var Footer = new RestFooterInfo();
            Footer.Footers.Add(2, "ВСЕГО:");
            Footer.Footers.Add(7, model.DisplayTotalValues.QuantityAfter);
            Footer.Footers.Add(8, model.DisplayTotalValues.Weight);

            if (navInfo.BalancePrice == true)
            {
                Footer.Footers.Add(9, model.DisplayTotalValues.BP_After);
            }

            if (navInfo.PriceForEndConsumer == true)
            {
                Footer.Footers.Add(10, model.DisplayTotalValues.PE_After);
            }

            if (navInfo.PriceForFirstReciver == true)
            {
                Footer.Footers.Add(11, model.DisplayTotalValues.PF_After);
            }

            if (navInfo.PlanFullCost == true)
            {
                Footer.Footers.Add(12, model.DisplayTotalValues.PCPC_After);
            }

            if (navInfo.PlanChangableCost == true)
            {
                Footer.Footers.Add(13, model.DisplayTotalValues.PCPC_After);
            }

            if (navInfo.FactFullCosts == true)
            {
                Footer.Footers.Add(14, model.DisplayTotalValues.FCP_After);
            }

            if (navInfo.FactChangableCosts == true)
            {
                Footer.Footers.Add(15, model.DisplayTotalValues.FCPC_After);
            }

            var Param = new RestParamsInfo();
            Param.Language = Request.UserLanguages[0];

            string MainHeader = "Реестр остатков за период: ";
            string ActualDateBeg = "";
            string ActualDateEnd = "";
            bool getDate = context.GetDateSnapshot(navInfo.snapshotId, ref ActualDateBeg, ref ActualDateEnd);

            Param.MainHeader = MainHeader + ActualDateBeg + " - " + ActualDateEnd;

            if ((navInfo.UseCenterFilter == false) &&
                (navInfo.UseKeeperFilter == false) &&
                (navInfo.UseProducerFilter == false) &&
                (navInfo.UseRecieverFactFilter == false) &&
                (navInfo.UseRecieverPlanFilter == false) &&
                (navInfo.UseStorageFilter == false))
                Param.Params.Add("Активный фильтр: Все данные информационного снимка", "");
            else
            {
                if (navInfo.UseCenterFilter == true)
                {
                    Param.Params.Add("Активный фильтр по ЦФО: ", navInfo.FilterCenterId);
                }

                if (navInfo.UseKeeperFilter == true)
                {
                    Param.Params.Add("Активный фильтр по балансодержателю: ", navInfo.FilterKeeperId);
                }

                if (navInfo.UseProducerFilter == true)
                {
                    Param.Params.Add("Активный фильтр по производителю: ", navInfo.FilterProducerId);
                }

                if (navInfo.UseRecieverFactFilter == true)
                {
                    Param.Params.Add("Активный фильтр по грузополучателю(факт): ", navInfo.FilterRecieverFactId);
                }

                if (navInfo.UseRecieverPlanFilter == true)
                {
                    Param.Params.Add("Активный фильтр по грузополучателю(план): ", navInfo.FilterRecieverPlanId);
                }

                if (navInfo.UseStorageFilter == true)
                {
                    Param.Params.Add("Активный фильтр по складу: ", navInfo.FilterStorageId);
                }
            }

            byte[] fileContents;
            fileContents = report.RenderReport<RestViewModel>(Header, Data, Footer, Param);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReestrReport.xlsx");
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult DocsReportAsExcel(RestNavigationInfo navInfo)
        {
            var Header = new RestHeaderInfo();

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Реквизиты документа",
                columnOrder = 2,
                columnField = "DocType",
                columnWidth = 40,
                ColumnBlockStart = false,
                ColumnBlockEnd = true,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Товар",
                columnOrder = 3,
                columnField = "Product",
                columnWidth = 20,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Артикул (шифр)",
                columnOrder = 4,
                columnField = "Shifr",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Чертеж",
                columnOrder = 5,
                columnField = "Figure",
                columnWidth = 15,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Внутрений заказ ПП",
                columnOrder = 6,
                columnField = "InnerOrderNum",
                columnWidth = 15,
                ColumnBlockStart = false,
                ColumnBlockEnd = true,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Кол-во",
                columnOrder = 7,
                columnField = "Quantity",
                columnWidth = 12,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Масса,(кг.)",
                columnOrder = 8,
                columnField = "Mass",
                columnWidth = 15,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 0
            });

            if (navInfo.BalancePrice == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в БС, (грн. в ценах учета)",
                    columnOrder = 9,
                    columnField = "BP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }


            if (navInfo.PriceForEndConsumer == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в ЦКП, (грн. в ценах учета)",
                    columnOrder = 10,
                    columnField = "PE",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PriceForFirstReciver == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в ЦПП, (грн. в ценах учета)",
                    columnOrder = 11,
                    columnField = "PF",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PlanFullCost == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в CПП, (грн. в ценах учета)",
                    columnOrder = 12,
                    columnField = "PCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }


            if (navInfo.PlanChangableCost == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в CППР, (грн. в ценах учета)",
                    columnOrder = 13,
                    columnField = "PCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.FactFullCosts == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в CФП, (грн. в ценах учета)",
                    columnOrder = 14,
                    columnField = "FCP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.FactChangableCosts == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Сумма в CФПР, (грн. в ценах учета)",
                    columnOrder = 15,
                    columnField = "FCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Клиент (Тип грузополучателя)",
                columnOrder = 16,
                columnField = "RecieverGroupPlan",
                columnWidth = 20,
                ColumnBlockStart = true,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Номер заказа",
                columnOrder = 17,
                columnField = "OrderNum",
                columnWidth = 10,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            SnapshotInfoViewModel maxAvialiableSnapshot = null;

            maxAvialiableSnapshot = navInfo.snapshotId > 0 ?
                context.GetScreenShotById(navInfo.snapshotId) : context.GetMaxScreenShot();

            var order_items = context.GetDocsByScreenShotId(maxAvialiableSnapshot.Id,
                new GroupItemFilters()
                {
                    FilterStorageId = navInfo.FilterStorageId,
                    FilterCenterId = navInfo.FilterCenterId,
                    FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                    FilterRecieverFactId = navInfo.FilterRecieverFactId,
                    FilterKeeperId = navInfo.FilterKeeperId,
                    FilterProducerId = navInfo.FilterProducerId,

                    UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                    UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                    UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                    UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                    UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                    UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter,

                    IsPrihodDocs = navInfo.IsPrihodDocs
                });

            var Data = new RestDataInfo<DocViewModel>();

            Data.Rows.AddRange(order_items);
            /***************************************************************************************************************/

            //************************** итоговые цифры   ******************************************************************           
            var model = new RestsNavigationResult<DocViewModel>(navInfo, userId)
            {
                DisplayValues = context.GetDocsByScreenShotId(maxAvialiableSnapshot.Id,
                new GroupItemFilters()
                {
                    FilterStorageId = navInfo.FilterStorageId,
                    FilterCenterId = navInfo.FilterCenterId,
                    FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                    FilterRecieverFactId = navInfo.FilterRecieverFactId,
                    FilterKeeperId = navInfo.FilterKeeperId,
                    FilterProducerId = navInfo.FilterProducerId,

                    UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                    UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                    UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                    UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                    UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                    UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter,

                    IsPrihodDocs = navInfo.IsPrihodDocs
                }),
                //Price columns settings
                PriceForEndConsumer = navInfo.PriceForEndConsumer,
                PriceForFirstReciver = navInfo.PriceForFirstReciver,
                PlanFullCost = navInfo.PlanFullCost,
                PlanChangableCost = navInfo.PlanChangableCost,
                FactFullCosts = navInfo.FactFullCosts,
                FactChangableCosts = navInfo.FactChangableCosts,
                BalancePrice = navInfo.BalancePrice,
                SnapshotInfo = maxAvialiableSnapshot,


                //Filter settings
                FilterStorageId = navInfo.FilterStorageId,
                FilterCenterId = navInfo.FilterCenterId,
                FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                FilterRecieverFactId = navInfo.FilterRecieverFactId,
                FilterKeeperId = navInfo.FilterKeeperId,
                FilterProducerId = navInfo.FilterProducerId,

                UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter,

                IsPrihodDocs = navInfo.IsPrihodDocs,

                //Blocks columns settings
                BeforeColBlock = true,
                PrihodColBlock = true,
                RashodColBlock = true,
                AfterColBlock = true

            };

            if (!((model.BalancePrice) ||
                (model.PriceForEndConsumer) ||
                (model.PriceForFirstReciver) ||
                (model.PlanFullCost) ||
                (model.PlanChangableCost) ||
                (model.FactFullCosts) ||
                (model.FactChangableCosts)))
            {
                model.BalancePrice = true;
            }

            model.DisplayTotalValues = model.DisplayValues
                                           .GroupBy(ri => navInfo.CurrentGroupFieldName)
                                            .Select(g => new DocViewModel
                                            {
                                                Quantity = g.Sum(s => s.Quantity),
                                                Weight = g.Sum(s => s.Weight * s.Quantity),
                                                PE = g.Sum(s => s.PE),
                                                PF = g.Sum(s => s.PF),
                                                PCP = g.Sum(s => s.PCP),
                                                PCPC = g.Sum(s => s.PCPC),
                                                FCP = g.Sum(s => s.FCP),
                                                FCPC = g.Sum(s => s.FCPC),
                                                BP = g.Sum(s => s.BP)
                                            }).FirstOrDefault();


            var Footer = new RestFooterInfo();
            Footer.Footers.Add(2, "ВСЕГО:");
            Footer.Footers.Add(7, model.DisplayTotalValues.Quantity);
            Footer.Footers.Add(8, model.DisplayTotalValues.Weight);

            if (navInfo.BalancePrice == true)
            {
                Footer.Footers.Add(9, model.DisplayTotalValues.BP);
            }

            if (navInfo.PriceForEndConsumer == true)
            {
                Footer.Footers.Add(10, model.DisplayTotalValues.PE);
            }

            if (navInfo.PriceForFirstReciver == true)
            {
                Footer.Footers.Add(11, model.DisplayTotalValues.PF);
            }

            if (navInfo.PlanFullCost == true)
            {
                Footer.Footers.Add(12, model.DisplayTotalValues.PCP);
            }

            if (navInfo.PlanChangableCost == true)
            {
                Footer.Footers.Add(13, model.DisplayTotalValues.PCPC);
            }

            if (navInfo.FactFullCosts == true)
            {
                Footer.Footers.Add(14, model.DisplayTotalValues.FCP);
            }

            if (navInfo.FactChangableCosts == true)
            {
                Footer.Footers.Add(15, model.DisplayTotalValues.FCPC);
            }

            var Param = new RestParamsInfo();
            Param.Language = Request.UserLanguages[0];

            string DocType = (model.IsPrihodDocs == 1) ? "Приходные" : "Расходные";
            string MainHeader = DocType + " документы за период: ";
            string ActualDateBeg = "";
            string ActualDateEnd = "";
            bool getDate = context.GetDateSnapshot(navInfo.snapshotId, ref ActualDateBeg, ref ActualDateEnd);

            Param.MainHeader = MainHeader + ActualDateBeg + " - " + ActualDateEnd;

            if ((navInfo.UseCenterFilter == false) &&
                (navInfo.UseKeeperFilter == false) &&
                (navInfo.UseProducerFilter == false) &&
                (navInfo.UseRecieverFactFilter == false) &&
                (navInfo.UseRecieverPlanFilter == false) &&
                (navInfo.UseStorageFilter == false))
                Param.Params.Add("Активный фильтр: Все данные информационного снимка", "");
            else
            {
                if (navInfo.UseCenterFilter == true)
                {
                    Param.Params.Add("Активный фильтр по ЦФО: ", navInfo.FilterCenterId);
                }

                if (navInfo.UseKeeperFilter == true)
                {
                    Param.Params.Add("Активный фильтр по балансодержателю: ", navInfo.FilterKeeperId);
                }

                if (navInfo.UseProducerFilter == true)
                {
                    Param.Params.Add("Активный фильтр по производителю: ", navInfo.FilterProducerId);
                }

                if (navInfo.UseRecieverFactFilter == true)
                {
                    Param.Params.Add("Активный фильтр по грузополучателю(факт): ", navInfo.FilterRecieverFactId);
                }

                if (navInfo.UseRecieverPlanFilter == true)
                {
                    Param.Params.Add("Активный фильтр по грузополучателю(план): ", navInfo.FilterRecieverPlanId);
                }

                if (navInfo.UseStorageFilter == true)
                {
                    Param.Params.Add("Активный фильтр по складу: ", navInfo.FilterStorageId);
                }
            }

            byte[] fileContents;
            fileContents = report.RenderReport<DocViewModel>(Header, Data, Footer, Param);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DocsReport.xlsx");
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult BriefReportAsExcel(RestNavigationInfo navInfo)
        {
            var Header = new RestHeaderInfo();

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Дата",
                columnOrder = 2,
                columnField = "currentDate",
                columnWidth = 20,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 2
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Остатки ГП, масса (тн)",
                columnOrder = 3,
                columnField = "Mass_OnDate",
                columnWidth = 15,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            if (navInfo.BalancePrice == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Остатки ГП в БС, в ценах учета",
                    columnOrder = 4,
                    columnField = "BP_OnDate",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в БС в ценах реализации",
                    columnOrder = 13,
                    columnField = "ShipOnDate_BP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в БС в ценах реализации с начала периода",
                    columnOrder = 20,
                    columnField = "ShipForPeriod_BP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в БС в ценах реализации",
                    columnOrder = 29,
                    columnField = "ProdOnDate_BP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в БС в ценах реализации с начала периода",
                    columnOrder = 36,
                    columnField = "ProdByPeriod_BP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PriceForEndConsumer == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Остатки ГП в ЦКП, в ценах учета",
                    columnOrder = 5,
                    columnField = "PE_OnDate",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в ЦКП в ценах реализации",
                    columnOrder = 14,
                    columnField = "ShipOnDate_PE",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в ЦКП в ценах реализации с начала периода",
                    columnOrder = 21,
                    columnField = "ShipForPeriod_PE",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в ЦКП в ценах реализации",
                    columnOrder = 30,
                    columnField = "ProdOnDate_PE",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в ЦКП в ценах реализации с начала периода",
                    columnOrder = 37,
                    columnField = "ProdByPeriod_PE",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

            }

            if (navInfo.PriceForFirstReciver == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Остатки ГП в ЦПП, в ценах учета",
                    columnOrder = 6,
                    columnField = "PF_OnDate",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в ЦПП в ценах реализации",
                    columnOrder = 15,
                    columnField = "ShipOnDate_PF",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в ЦПП в ценах реализации с начала периода",
                    columnOrder = 22,
                    columnField = "ShipForPeriod_PF",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в ЦПП в ценах реализации",
                    columnOrder = 31,
                    columnField = "ProdOnDate_PF",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в ЦПП в ценах реализации с начала периода",
                    columnOrder = 38,
                    columnField = "ProdByPeriod_PF",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.PlanFullCost == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Остатки ГП в СПП, в ценах учета",
                    columnOrder = 7,
                    columnField = "PCP_OnDate",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в СПП в ценах реализации",
                    columnOrder = 16,
                    columnField = "ShipOnDate_PCP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в СПП в ценах реализации с начала периода",
                    columnOrder = 23,
                    columnField = "ShipForPeriod_PCP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в СПП в ценах реализации",
                    columnOrder = 32,
                    columnField = "ProdOnDate_PCP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в СПП в ценах реализации с начала периода",
                    columnOrder = 39,
                    columnField = "ProdByPeriod_PCP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

            }

            if (navInfo.PlanChangableCost == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Остатки ГП в СППР, в ценах учета",
                    columnOrder = 8,
                    columnField = "PCPC_OnDate",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в СППР в ценах реализации",
                    columnOrder = 17,
                    columnField = "ShipOnDate_PCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в СППР в ценах реализации с начала периода",
                    columnOrder = 24,
                    columnField = "ShipForPeriod_PCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в СППР в ценах реализации",
                    columnOrder = 33,
                    columnField = "ProdOnDate_PCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в СППР в ценах реализации с начала периода",
                    columnOrder = 40,
                    columnField = "ProdByPeriod_PCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.FactFullCosts == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Остатки ГП в СФП, в ценах учета",
                    columnOrder = 9,
                    columnField = "FCP_OnDate",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в СФП в ценах реализации",
                    columnOrder = 18,
                    columnField = "ShipOnDate_FCP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в СФП в ценах реализации с начала периода",
                    columnOrder = 25,
                    columnField = "ShipForPeriod_FCP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в СФП в ценах реализации",
                    columnOrder = 34,
                    columnField = "ProdOnDate_FCP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в СФП в ценах реализации с начала периода",
                    columnOrder = 41,
                    columnField = "ProdByPeriod_FCP",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            if (navInfo.FactChangableCosts == true)
            {
                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Остатки ГП в СФПР, в ценах учета",
                    columnOrder = 10,
                    columnField = "FCPC_OnDate",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в СФПР в ценах реализации",
                    columnOrder = 19,
                    columnField = "ShipOnDate_FCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Отгрузка ГП в СФПР в ценах реализации с начала периода",
                    columnOrder = 26,
                    columnField = "ShipForPeriod_FCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в СФПР в ценах реализации",
                    columnOrder = 35,
                    columnField = "ProdOnDate_FCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });

                Header.Headers.Add(new HeaderItemInfo()
                {
                    columnName = "Производство ГП в СФПР в ценах реализации с начала периода",
                    columnOrder = 42,
                    columnField = "ProdByPeriod_FCPC",
                    columnWidth = 20,
                    ColumnBlockStart = false,
                    ColumnBlockEnd = false,
                    ColumnType = 0
                });
            }

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Отгрузка ГП, масса (тн)",
                columnOrder = 11,
                columnField = "ShipOnDate_Mass",
                columnWidth = 20,
                ColumnBlockStart = true,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Отгрузка ГП с начала периода, масса (тн)",
                columnOrder = 12,
                columnField = "ShipForPeriod_Mass",
                columnWidth = 20,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Производство ГП, масса (тн)",
                columnOrder = 27,
                columnField = "ProdOnDate_Mass",
                columnWidth = 20,
                ColumnBlockStart = true,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            Header.Headers.Add(new HeaderItemInfo()
            {
                columnName = "Производство ГП с начала периода, масса (тн)",
                columnOrder = 28,
                columnField = "ProdByPeriod_Mass",
                columnWidth = 20,
                ColumnBlockStart = false,
                ColumnBlockEnd = false,
                ColumnType = 3
            });

            SnapshotInfoViewModel maxAvialiableSnapshot = null;

            maxAvialiableSnapshot = navInfo.snapshotId > 0 ?
                context.GetScreenShotById(navInfo.snapshotId) : context.GetMaxScreenShot();

            var order_items = context.GetBriefDataByScreenShotId(maxAvialiableSnapshot.Id,
                new GroupItemFilters()
                {
                    FilterStorageId = navInfo.FilterStorageId,
                    FilterCenterId = navInfo.FilterCenterId,
                    FilterRecieverPlanId = navInfo.FilterRecieverPlanId,
                    FilterRecieverFactId = navInfo.FilterRecieverFactId,
                    FilterKeeperId = navInfo.FilterKeeperId,
                    FilterProducerId = navInfo.FilterProducerId,

                    UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                    UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                    UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                    UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                    UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                    UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter
                });

            var Data = new RestDataInfo<BriefViewModel>();

            Data.Rows.AddRange(order_items);
            /***************************************************************************************************************/

            //************************** итоговые цифры   ******************************************************************                    
            var Footer = new RestFooterInfo();
            //  Footer.Footers.Add(2, "ВСЕГО:");

            var Param = new RestParamsInfo();
            Param.Language = Request.UserLanguages[0];

            string MainHeader = "Сводка за период: ";
            string ActualDateBeg = "";
            string ActualDateEnd = "";
            bool getDate = context.GetDateSnapshot(navInfo.snapshotId, ref ActualDateBeg, ref ActualDateEnd);

            Param.MainHeader = MainHeader + ActualDateBeg + " - " + ActualDateEnd;

            if ((navInfo.UseCenterFilter == false) &&
                (navInfo.UseKeeperFilter == false) &&
                (navInfo.UseProducerFilter == false) &&
                (navInfo.UseRecieverFactFilter == false) &&
                (navInfo.UseRecieverPlanFilter == false) &&
                (navInfo.UseStorageFilter == false))
                Param.Params.Add("Активный фильтр: Все данные информационного снимка", "");
            else
            {
                if (navInfo.UseCenterFilter == true)
                {
                    Param.Params.Add("Активный фильтр по ЦФО: ", navInfo.FilterCenterId);
                }

                if (navInfo.UseKeeperFilter == true)
                {
                    Param.Params.Add("Активный фильтр по балансодержателю: ", navInfo.FilterKeeperId);
                }

                if (navInfo.UseProducerFilter == true)
                {
                    Param.Params.Add("Активный фильтр по производителю: ", navInfo.FilterProducerId);
                }

                if (navInfo.UseRecieverFactFilter == true)
                {
                    Param.Params.Add("Активный фильтр по грузополучателю(факт): ", navInfo.FilterRecieverFactId);
                }

                if (navInfo.UseRecieverPlanFilter == true)
                {
                    Param.Params.Add("Активный фильтр по грузополучателю(план): ", navInfo.FilterRecieverPlanId);
                }

                if (navInfo.UseStorageFilter == true)
                {
                    Param.Params.Add("Активный фильтр по складу: ", navInfo.FilterStorageId);
                }
            }

            byte[] fileContents;
            fileContents = report.RenderReport<BriefViewModel>(Header, Data, Footer, Param);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BriefReport.xlsx");
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult StatusReportAsExcel(OrdersNavigationInfo navInfo)
        {

            if (string.IsNullOrEmpty(navInfo.FilterOrderClientId)) { navInfo.UseOrderClientFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId)) { navInfo.UseOrderTypeFilter = false; }

            if (navInfo.DateType == null) navInfo.DateType = 0;

            if (navInfo.DateType == 0)
            {
                navInfo.UseOrderDateFilter = true;
                navInfo.UseAcceptDateFilter = false;
            }
            else
            {
                navInfo.UseOrderDateFilter = false;
                navInfo.UseAcceptDateFilter = true;
            }

            if (navInfo.FilterOrderDateBeg == null)
            {
                navInfo.FilterOrderDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
            }
            if (navInfo.FilterOrderDateEnd == null)
            {
                navInfo.FilterOrderDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));
            }

            if (navInfo.FilterAcceptDateBeg == null)
            {
                navInfo.FilterAcceptDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterAcceptDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
            }
            if (navInfo.FilterAcceptDateEnd == null)
            {
                navInfo.FilterAcceptDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterAcceptDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));
            }

            var orders = context.getStatusReport(userId,
                                                this.isAdmin,
                                                navInfo.UseOrderClientFilter,
                                                navInfo.UseOrderTypeFilter,
                                                navInfo.UseTripTypeFilter,
                                                navInfo.FilterOrderClientId,
                                                navInfo.FilterOrderTypeId,
                                                navInfo.FilterTripTypeId,
                                                string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw) ? DateTime.Now.AddDays(-7) : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw),
                                                string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw) ? DateTime.Now : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw),
                                                string.IsNullOrEmpty(navInfo.FilterAcceptDateBegRaw)
                                                ? DateTime.Now.AddDays(-7)
                                                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateBegRaw),
                                             string.IsNullOrEmpty(navInfo.FilterAcceptDateEndRaw)
                                                ? DateTime.Now
                                                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateEndRaw),
                                             navInfo.UseOrderDateFilter,
                                             navInfo.UseAcceptDateFilter,

                                                navInfo.isPassOrders).ToList();

            //собираем итоги для отчета по плановым и срочным заявкам            
            List<string> statusOrderSumm = new List<string>();
            statusOrderSumm.Add(orders.Sum(item => item.CntAll).ToString());
            statusOrderSumm.Add(orders.Sum(item => item.CntZero).ToString());

            statusOrderSumm.Add(((int)orders.Average(item => item.CntZeroPercent)).ToString() + "%");
            statusOrderSumm.Add(orders.Sum(item => item.CntOne).ToString());
            statusOrderSumm.Add(((int)orders.Average(item => item.CntOnePercent)).ToString() + "%");

            var Data = new RestDataInfo<StatusReportViewModel>();

            Data.Rows.AddRange(orders);

            byte[] fileContents = null;

            var Params = new RestParamsInfo();
            Params.Language = Request.UserLanguages[0];
            var DateBeg = DateTime.Now;
            var DateEnd = DateTime.Now;

            if (navInfo.UseOrderDateFilter)
            {
                DateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw);
                DateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw);
            }
            else if (navInfo.UseAcceptDateFilter)
            {
                DateBeg = string.IsNullOrEmpty(navInfo.FilterAcceptDateBegRaw)
                    ? DateTime.Now.AddDays(-7)
                    : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateBegRaw);
                DateEnd = string.IsNullOrEmpty(navInfo.FilterAcceptDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterAcceptDateEndRaw);
            }

            var DateBegRaw = DateBeg.ToString("dd.MM.yyyy");
            var DateEndRaw = DateEnd.ToString("dd.MM.yyyy");
            Params.MainHeader = "Срочные и плановые заявки за " + DateBegRaw + " - " + DateEndRaw;

            fileContents = report.StatusReportRenderReport<StatusReportViewModel>(Data, Params, statusOrderSumm);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StatusReport.xlsx");

        }

        
        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public FileResult TruckReportAsExcel(OrdersNavigationInfo navInfo)
        {
             String Address = "", OrgName = "";
            IQueryable<TruckViewModel> orders = null;
            var modelTruckReport2 = (Session[navInfo.IdTree] as OrdersNavigationResult<TruckViewModel>);
            var modelTruckReport3 = new OrdersNavigationResult<TruckViewModel>(navInfo, userId);
            if (modelTruckReport2 != null)
            {
                var TruckInfo = modelTruckReport2.DataDisplayValues.ToList();
                modelTruckReport3 = new OrdersNavigationResult<TruckViewModel>(navInfo, userId)
                {                   
                    DisplayValues = context.getTruckReportDetails2(TruckInfo,
                    navInfo.IdGroup ?? 0,
                    navInfo.Id)
                };
                OrgName = context.getTruckReportTitle(TruckInfo,
               navInfo.IdGroup ?? 0, navInfo.Id, ref Address);

                orders = modelTruckReport3.DisplayValues;
                /* var model = new OrdersReportsNavigationResult
            {
                TruckReportDetail = modelTruckReport3,
                OrgName = OrgName,
                OrgId = 0,
                ReportDate = navInfo.ReportDate,
                Address = Address,
                IdGroup = navInfo.IdGroup ?? 0,
                Id = navInfo.Id
            };*/
                //return View(model);
            }
            else
            {                
                /*return RedirectToAction( "TruckReport", new RouteValueDictionary( 
    new { controller = "Orders", action = "TruckReport", UseOrderTypeFilter =  navInfo.UseOrderTypeFilter,
        FilterOrderTypeId =  navInfo.FilterOrderTypeId, FilterOrderDateRaw =  navInfo.FilterOrderDateRaw,
        UseOrderDateFilter = navInfo.UseOrderDateFilter} ) );  */
                             
            }

             // string orgName= context.GetOrganizationNameById(navInfo.OrgId);
            
           /* var orders = context.getTruckReportDetails(userId,
                                                this.isAdmin,                                               
                                                 navInfo.OrgId,
                                                 navInfo.ReportDate, navInfo.IdGroup ?? 0, navInfo.Id).ToList();*/

            var Otgruzka = orders.Where(x => x.isShipper == true).ToList();
            var Poluchenie = orders.Where(x => x.isShipper == false).ToList();
            int SumOtgruzka = Otgruzka.Count();
            int SumPoluchenie = Poluchenie.Count();
       
            var DataOtgruzka = new RestDataInfo<TruckViewModel>();

            DataOtgruzka.Rows.AddRange(Otgruzka);

            var DataPoluchenie = new RestDataInfo<TruckViewModel>();

            DataPoluchenie.Rows.AddRange(Poluchenie);

            byte[] fileContents = null;

            var Params = new RestParamsInfo();
            Params.Language = Request.UserLanguages[0];              
            Params.Address = Address;           

            Params.MainHeader = "Информация по " + OrgName + " на дату " +
                                navInfo.ReportDate.Date.ToString("dd.MM.yyyy") + Address;            

            fileContents = report.TruckReportRenderReport<TruckViewModel>(DataOtgruzka, DataPoluchenie, Params, SumOtgruzka, SumPoluchenie);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TruckReport.xlsx");

        }


    }
}
