using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Corum.Models.ViewModels.OrderConcurs;
using Corum.RestRenderModels;
using Corum.Models.ViewModels.Orders;

namespace Corum.Models.Interfaces
{
    public partial interface IReportRenderer
    {
        byte[] RenderReport<T>(RestHeaderInfo Header, RestDataInfo<T> Data, RestFooterInfo Footer, RestParamsInfo Params);

        byte[] OrderRenderReport<T>(OrderBaseViewModel OrderTypeModel, OrdersPassTransportViewModel extOrderTypeModel, string AcceptDate, OrderClientsViewModel orderClientInfo, RestParamsInfo Params, string AdressFrom, string AdressTo, string ContractName, OrdersTruckTransportViewModel extOrderTypeModel2, int OrderType, List<OrderUsedCarViewModel> carList);

        byte[] BaseReportRenderReport<T>(RestDataInfo<T> Orders, RestParamsInfo Params, Dictionary<string, int> orderFinalStatusesDict, int SumPlanCarNumber, int SumFactCarNumber);

        byte[] StatusReportRenderReport<T>(RestDataInfo<T> Data, RestParamsInfo Params,
            List<string> statusOrderSumm);

        byte[] OrdersReportRenderReport<T>(RestDataInfo<T> Orders, RestParamsInfo Params, List<string> balanceKeepers);

        byte[] FinalReportRenderReport<T>(RestDataInfo<T> Orders, RestParamsInfo Params, List<string> orderStatus);

        byte[] AllOrderRenderReport<T>(List<OrdersPassTransportViewModel> ordersPassList, List<OrdersTruckTransportViewModel> truckOrders, RestParamsInfo Params);

        byte[] ConcursRenderReport<T>(RestDataInfo<T> Orders, CompetitiveListViewModel concursHeader, RestParamsInfo Params, OrderBaseViewModel OrderTypeModel, OrdersPassTransportViewModel extOrderTypeModel1, OrdersTruckTransportViewModel extOrderTypeModel2);

        byte[] TruckReportRenderReport<T>(RestDataInfo<T> DataOtgruzka, RestDataInfo<T> DataPoluchenie, RestParamsInfo Params, int SumOtgruzka, int SumPoluchenie);
    }
}