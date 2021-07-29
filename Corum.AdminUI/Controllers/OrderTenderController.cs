using System.Collections.Generic;
using System.Web.Mvc;
using Corum.Models;
using Corum.Models.ViewModels.OrderConcurs;
using CorumAdminUI.Common;
using Corum.Models.ViewModels.Cars;
using System;
using Corum.Models.ViewModels;
using Corum.Common;
using Corum.Models.Tender;
using Corum.Models.ViewModels.Tender;
using System.Collections;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using CorumAdminUI;
using Newtonsoft.Json;
using Corum.Models.Interfaces;
using Corum.Models.ViewModels.Orders;
using Corum.RestRenderModels;
using Corum.ReportsUI;
using CorumAdminUI.HangFireTasks;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace CorumAdminUI.Controllers
{
    [Authorize]
    public partial class OrderTenderController : CorumBaseController
    {
        static long OrderID;
        protected IReportRenderer report;

        public OrderTenderController()
        {
            report = DependencyResolver.Current.GetService<IReportRenderer>();
        }

        [HttpGet]
        public ActionResult TenderReport(int? orderId, long? dataStart, long? dataEnd, bool? active, string processValue)
        {
            var model = context.GetRegisterTenders();
            ViewBag.processValue = (processValue != null) ? processValue : "null";
            ViewBag.filterShare = (processValue != null) ? "true" : "false";
            ViewBag.shareTendersfromRegistyTenders = context.ShareTendersFromRegistyTenders();
            if (orderId != null && dataStart != null && dataEnd != null && active != false)
            {
                ViewBag.orderId = orderId;
                ViewBag.dataStart = dataStart;
                ViewBag.dataEnd = dataEnd;
                ViewBag.active = "true";
            }
            else
            {
                ViewBag.orderId = "null";
                ViewBag.dataStart = "null";
                ViewBag.dataEnd = "null";
                ViewBag.active = "false";
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult UpdateStatusOrderTender(int tenderNumber)
        {
            UpdateRegisterStatusTender updateDeserializedClass = new UpdateRegisterStatusTender();
            try
            {
                NameValueCollection allAppSettings = ConfigurationManager.AppSettings;
                BaseClient clientbase = new BaseClient($"{allAppSettings["ApiGetTenderId"]}{tenderNumber}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                var JSONresponse = new GetApiTendAjax().GetCallAsync(clientbase).Result.ResponseMessage;
                RequestJSONDeserializedToModel myDeserializedClass = JsonConvert.DeserializeObject<RequestJSONDeserializedToModel>(JSONresponse);
                if (myDeserializedClass.success)
                {
                    updateDeserializedClass = context.UpdateCLStatusTenderOrder(myDeserializedClass, tenderNumber);

                    // Вытягивание данных о контрагентах из aps tender
                    Dictionary<long, List<RegisterTenderContragent>> contragents = new Dictionary<long, List<RegisterTenderContragent>>();
                    if (myDeserializedClass.data.lots[0].items.Count != 0 && Int32.Parse(myDeserializedClass.data.process) >= 8)
                    {
                        foreach (var items in myDeserializedClass.data.lots[0].items)
                        {
                            BaseClient clientbaseOffer = new BaseClient($"{allAppSettings["ApiUrlGetOffer"]}{items.tenderItemUuid}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                            var JSONresponseContragent = $"{{\"data\":{new GetApiTendAjax().GetCallAsync(clientbaseOffer).Result.ResponseMessage}}}";
                            RequestJSONContragentModel myDeserializedClassContragent = JsonConvert.DeserializeObject<RequestJSONContragentModel>(JSONresponseContragent);
                            List<RequestJSONContragentMainData> listContAgentModelJSONDesiarized = new List<RequestJSONContragentMainData>();
                            int SupplierIdWinnerContragent = 0;
                            foreach (var item in myDeserializedClassContragent.Data)
                            {
                                BaseClient clientbaseSuppContragent = new BaseClient($"{allAppSettings["ApiUrlGetSuppContrAgent"]}{item.SupplierId}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                                var JSONresponseContAgentModel = new GetApiTendAjax().GetCallAsync(clientbaseSuppContragent).Result.ResponseMessage;
                                RequestJSONContragentMainData myDeserializedClassContragentModel = JsonConvert.DeserializeObject<RequestJSONContragentMainData>(JSONresponseContAgentModel);
                                listContAgentModelJSONDesiarized.Add(myDeserializedClassContragentModel);
                                if ((item.IsWinner != null) ? true : false)
                                {
                                    SupplierIdWinnerContragent = item.SupplierId;
                                }
                            }
                            List<ContrAgentModel> listContragentModels = new List<ContrAgentModel>();
                            ContrAgentModel isWinnerContragent = new ContrAgentModel();
                            listContragentModels = context.GetAgentModels(listContAgentModelJSONDesiarized, myDeserializedClassContragent);   // Список всех контрагентов, которые принимали участие в тендере
                            isWinnerContragent = context.GetWinnerContragent(listContragentModels, SupplierIdWinnerContragent); // Информация о контрагенте-победителе
                            List<RegisterTenderContragent> tenderContragents = new List<RegisterTenderContragent>();
                            foreach (var el in listContragentModels)
                            {
                                double costOfCarWithoutNDS = 0d;
                                double costOfCarWithNDS = 0d;
                                int paymentDelay = 0;
                                try
                                {
                                    for (int i = 0; i < el.listCritariaValues.Count; i++)
                                    {
                                        if (costOfCarWithoutNDS == 0)
                                        {
                                            costOfCarWithoutNDS = (el.listCritariaValues[i].Id == 61) ? Int64.Parse(el.listCritariaValues[i].Value.ToString()) : 0d;
                                        }
                                        if (paymentDelay == 0)
                                        {
                                            paymentDelay = (el.listCritariaValues[i].Id == 66) ? Int32.Parse(el.listCritariaValues[i].Value.ToString()) : 0;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                }
                                if (el.SupplierId != isWinnerContragent.SupplierId)
                                {
                                    RegisterTenderContragent registerTenderContragent = new RegisterTenderContragent()
                                    {
                                        OrderId = Int64.Parse(myDeserializedClass.data.tenderExternalN.Remove(myDeserializedClass.data.tenderExternalN.IndexOf('-'))),
                                        tenderNumber = Int32.Parse(myDeserializedClass.data.tenderNumber),
                                        itemExternalNumber = Int64.Parse(items.itemExternalN),
                                        ContragentName = el.OwnershipTypeName + ' ' + el.SupplierName,
                                        ContragentIdAps = el.SupplierId,
                                        DateUpdateInfo = DateTime.Now,
                                        IsWinner = false,
                                        EDRPOUContragent = Int64.Parse(el.SupplierEdrpou),
                                        emailContragent = (el.ContactEmail != null)? el.ContactEmail: el.SupplierCEOContragent.EMail,
                                        transportUnitsProposed = 1,
                                        acceptedTransportUnits = null,
                                        costOfCarWithoutNDS = costOfCarWithoutNDS,
                                        costOfCarWithNDS = costOfCarWithNDS,
                                        PaymentDelay = paymentDelay,
                                        tenderItemUuid = Guid.Parse(items.tenderItemUuid),
                                        nmcName = items.nmcName
                                    };
                                    tenderContragents.Add(registerTenderContragent);
                                }
                                else
                                {
                                    RegisterTenderContragent registerTenderContragent = new RegisterTenderContragent()
                                    {
                                        OrderId = Int64.Parse(myDeserializedClass.data.tenderExternalN.Remove(myDeserializedClass.data.tenderExternalN.IndexOf('-'))),
                                        tenderNumber = Int32.Parse(myDeserializedClass.data.tenderNumber),
                                        itemExternalNumber = Int64.Parse(items.itemExternalN),
                                        ContragentName = el.OwnershipTypeName + ' ' + el.SupplierName,
                                        ContragentIdAps = el.SupplierId,
                                        DateUpdateInfo = DateTime.Now,
                                        IsWinner = true,
                                        EDRPOUContragent = Int64.Parse(el.SupplierEdrpou),
                                        emailContragent = (el.ContactEmail != null) ? el.ContactEmail : el.SupplierCEOContragent.EMail,
                                        transportUnitsProposed = 1,
                                        acceptedTransportUnits = null,
                                        costOfCarWithoutNDS = costOfCarWithoutNDS,
                                        costOfCarWithNDS = costOfCarWithNDS,
                                        PaymentDelay = paymentDelay,
                                        tenderItemUuid = Guid.Parse(items.tenderItemUuid),
                                        nmcName = items.nmcName
                                    };
                                    tenderContragents.Add(registerTenderContragent);
                                }
                            }
                            contragents.Add(Int64.Parse(items.itemExternalN), tenderContragents);
                        }
                    }
                    context.UpdateDataRegisterContragents(contragents);
                }
            }
            catch
            {
            }
            return new JsonpResult
            {
                Data = updateDeserializedClass,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult UpdateAllStatusOrderTender()
        {
            Debug.WriteLine("Вход в контроллер");
            Task.WaitAll(Task.Run(AsyncUpdateTenderRegisty));
            Debug.WriteLine("Продолжение главного потока");
            return RedirectToAction("Index", "Home");
        }
        private async Task AsyncUpdateTenderRegisty()
        {
            Debug.WriteLine("Вход в асинхронный метод");
            HangFireTasks.HangFireTasks task = new HangFireTasks.HangFireTasks();
            await task.ListTasks();
            Debug.WriteLine("Выход из асинхронного метода");
        }

        [HttpPost]
        public ActionResult ResultsTenderUpdate(TenderRegistryUpdate registryUpdate)
        {
            context.UpdateRegisterTenders(Convert.ToInt32(registryUpdate.tenderNumber), registryUpdate.resultsTender);
            return Json("{\"success\":true}");
        }

        [HttpPost]
        public ActionResult RemainingTimeUpdate(RemainingTimeUpdate timeUpdate)
        {
            context.RemainingTime(timeUpdate.TimeList);
            return Json("{\"success\":true}");
        }

        [HttpPost]
        public ActionResult SendNotificationTender(TenderSumOrderId tenderSumOrder)
        {
            RegisterTenders modifRegisterTenders = new RegisterTenders();
            OrderID = Convert.ToInt64(tenderSumOrder.OrderId);
            Dictionary<string, string> otherParams = new Dictionary<string, string>();
            DateTimeOffset localTimeStart, otherTimeStart, localTimeEnd, otherTimeEnd;
            dynamic tenderForma = null;
            var routePointsLoadinfo = context.getLoadPoints(OrderID, true).ToList();
            var routePointsUnloadinfo = context.getLoadPoints(OrderID, false).ToList();
            int num = 0;
            try
            {
                TendFormDeserializedJSON tendFormDeserializedJSON = tenderSumOrder.ListItemsModelTenderForm;
                num = (routePointsLoadinfo.Count < routePointsUnloadinfo.Count) ? routePointsUnloadinfo.Count : routePointsLoadinfo.Count;
                switch (num)
                {
                    case 0:
                        tenderForma = new TenderForma<PropAliasValuesOne>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 1:
                        tenderForma = new TenderForma<PropAliasValuesOne>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 2:
                        tenderForma = new TenderForma<PropAliasValuesTwo>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 3:
                        tenderForma = new TenderForma<PropAliasValuesThree>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 4:
                        tenderForma = new TenderForma<PropAliasValuesFour>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 5:
                        tenderForma = new TenderForma<PropAliasValuesFive>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    default:
                        tenderForma = new TenderForma<PropAliasValuesOne>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                }

                tenderForma.data.InitializedAfterDeserialized();
                tenderForma.data.tenderName = tenderSumOrder.ListItemsModelTenderForm.TenderName;
                localTimeStart = new DateTimeOffset(DateTime.Parse(tenderSumOrder.ListItemsModelTenderForm.DateStart));
                localTimeEnd = new DateTimeOffset(DateTime.Parse(tenderSumOrder.ListItemsModelTenderForm.DateEnd));
                otherTimeStart = localTimeStart.ToUniversalTime();
                otherTimeEnd = localTimeEnd.ToUniversalTime();
                tenderForma.data.dateStart = otherTimeStart.ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                tenderForma.data.dateEnd = otherTimeEnd.ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                otherParams = tenderForma.otherParams;
            }
            catch (Exception e)
            {

            }
            NameValueCollection allAppSettings = ConfigurationManager.AppSettings;
            var appsett = allAppSettings["SwitchToMultipleTenders"];
            if (context.IsRegisterTendersExist(OrderID, Boolean.Parse(appsett)))
            {
                BaseClient clientbase = new BaseClient(allAppSettings["ApiUrl"], allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                var response = new PostApiTender<PropAliasValuesOne>().GetCallAsync(clientbase, tenderForma, num).Result.ResponseMessage;
                try
                {
                    byte[] dataOrder = OrderAsExcelData((int)OrderID);
                    RequestJSONDeserializedToModel myDeserializedClass = JsonConvert.DeserializeObject<RequestJSONDeserializedToModel>(response);
                    DateTime nowDateTime = DateTime.Now;
                    TimeSpan timeSpan = myDeserializedClass.data.dateEnd - nowDateTime;
                    RegisterTenders registerTenders = new RegisterTenders()
                    {
                        OrderId = OrderID,
                        TenderUuid = System.Guid.Parse(myDeserializedClass.data.tenderUuid),
                        tenderNumber = Convert.ToInt32(myDeserializedClass.data.tenderNumber),
                        industryId = myDeserializedClass.data.industryId,
                        industryName = myDeserializedClass.data.industryName,
                        dateStart = myDeserializedClass.data.dateStart,
                        dateEnd = myDeserializedClass.data.dateEnd,
                        mode = (byte)myDeserializedClass.data.mode,
                        process = Convert.ToByte(myDeserializedClass.data.process),
                        stageMode = myDeserializedClass.data.stageMode,
                        stageNumber = (byte)myDeserializedClass.data.stageNumber,
                        subCompanyId = myDeserializedClass.data.subCompanyId,
                        subCompanyName = myDeserializedClass.data.subCompanyName,
                        downloadAddress = otherParams["DOWNLOAD_ADDRESS"],
                        unloadAddress = otherParams["UNLOADING_ADDRESS"],
                        downloadDataRequired = DateTime.Parse(otherParams["DOWNLOADDATEREQUIRED"]),
                        unloadDataRequired = DateTime.Parse(otherParams["UNLOADINGDATEREQUIRED"]),
                        routeOrder = otherParams["ROUTE"],
                        cargoName = otherParams["CARGO_NAME"],
                        lotState = myDeserializedClass.data.lots[0].lotState,
                        resultsTender = null,
                        tenderOwnerPath = myDeserializedClass.data.tenderOwnerPath,
                        remainingTime = timeSpan.ToString(),
                        dateCreate = myDeserializedClass.data.dateCreate
                    };
                    context.AddNewDataTender(registerTenders);
                    HttpClientApi clientbaseAddFile = new HttpClientApi($"{allAppSettings["ApiUrlAddFile"]}{registerTenders.tenderNumber}&suppVisible=1", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                    new AddFilePostApiTender().GetCallAsync(clientbaseAddFile, dataOrder, $"OrderReport{OrderID}.xlsx");
                    modifRegisterTenders = context.GetRegisterTenders().Find(x => x.tenderNumber == registerTenders.tenderNumber);
                }
                catch (Exception e)
                {

                }
                return new JsonpResult
                {
                    Data = new { modifRegisterTenders, response },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                return Json("{\"success\":false,\"isLoadMultiple\":false}");
            }
        }

        public byte[] OrderAsExcelData(int id)
        {
            OrderBaseViewModel OrderTypeModel = null;
            var orderInfo = context.getOrder(id);
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

                string ContractName = context.getContactName(OrderTypeModel.Id);
                List<OrderUsedCarViewModel> carList = context.getOrderCarsInfo(OrderTypeModel.Id).ToList();
                byte[] fileContents;
                fileContents = report.OrderRenderReport<OrderBaseViewModel>(OrderTypeModel, extOrderTypeModel1, AcceptDate, orderClientInfo, Param, AdressFrom, AdressTo, ContractName, extOrderTypeModel2, OrderType, carList);

                return fileContents;
            }

            return null;
        }
    }
}