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
using System.IO;
using System.Text.RegularExpressions;

namespace CorumAdminUI.HangFireTasks
{
    public class UpdateDataTableContragents
    {
        protected Corum.Models.ICorumDataProvider context;
        public UpdateDataTableContragents()
        {
            context = DependencyResolver.Current.GetService<ICorumDataProvider>();
        }
        public void UpdateStatusContragentsData(int tenderNumber)
        {
            UpdateRegisterStatusTender updateDeserializedClass = new UpdateRegisterStatusTender();
            CompetetiveListStepsInfoViewModel currentStatus = new CompetetiveListStepsInfoViewModel();

            long OrderID = 0;
            try
            {
                NameValueCollection allAppSettings = ConfigurationManager.AppSettings;
                BaseClient clientbase = new BaseClient($"{allAppSettings["ApiGetTenderId"]}{tenderNumber}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                var JSONresponse = new GetApiTendAjax().GetCallAsync(clientbase).Result.ResponseMessage;
                RequestJSONDeserializedToModel myDeserializedClass = JsonConvert.DeserializeObject<RequestJSONDeserializedToModel>(JSONresponse);
                if (myDeserializedClass.success)
                {
                    updateDeserializedClass = context.UpdateCLStatusTenderOrder(myDeserializedClass, tenderNumber);
                    if (Int32.Parse(updateDeserializedClass.process) >= 8)
                    {
                        string cargoWeight = null;
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
                                    try
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
                                    catch (Exception e)
                                    {
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
                                    double costOfCarWithoutNDSToNull = 0d;
                                    int paymentDelay = 0;
                                    string note = null;
                                    string itemsDescription = items.itemNote;
                                    try
                                    {
                                        foreach (var it in items.propValues)
                                        {
                                            if (it.ВесТ != null)
                                            {
                                                cargoWeight = it.ВесТ;
                                                break;
                                            }
                                        }

                                        for (int i = 0; i < el.listCritariaValues.Count; i++)
                                        {
                                            if (costOfCarWithoutNDS == 0)
                                            {
                                                costOfCarWithoutNDS = (el.listCritariaValues[i].Id == 61) ? Double.Parse(el.listCritariaValues[i].Value.ToString()) : 0d;
                                            }
                                            if (costOfCarWithoutNDSToNull == 0)
                                            {
                                                costOfCarWithoutNDSToNull = (el.listCritariaValues[i].Id == 64) ? Double.Parse(el.listCritariaValues[i].Value.ToString()) : 0d;
                                            }
                                            if (paymentDelay == 0)
                                            {
                                                paymentDelay = (el.listCritariaValues[i].Id == 66) ? Int32.Parse(el.listCritariaValues[i].Value.ToString()) : 0;
                                            }
                                            if (note == null)
                                            {
                                                note = (el.listCritariaValues[i].Id == 510) ? el.listCritariaValues[i].Value.ToString() : null;
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                    if (isWinnerContragent != null && el.SupplierId != isWinnerContragent.SupplierId)
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
                                            emailContragent = (el.ContactEmail != null) ? el.ContactEmail : el.SupplierCEOContragent.EMail,
                                            transportUnitsProposed = 1,
                                            acceptedTransportUnits = 0,
                                            costOfCarWithoutNDS = costOfCarWithoutNDS,
                                            costOfCarWithoutNDSToNull = costOfCarWithoutNDSToNull,
                                            PaymentDelay = paymentDelay,
                                            tenderItemUuid = Guid.Parse(items.tenderItemUuid),
                                            nmcName = items.nmcName,
                                            note = note,
                                            itemDescription = itemsDescription,
                                            cargoWeight = cargoWeight
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
                                            acceptedTransportUnits = 1,
                                            costOfCarWithoutNDS = costOfCarWithoutNDS,
                                            costOfCarWithoutNDSToNull = costOfCarWithoutNDSToNull,
                                            PaymentDelay = paymentDelay,
                                            tenderItemUuid = Guid.Parse(items.tenderItemUuid),
                                            nmcName = items.nmcName,
                                            note = note,
                                            itemDescription = itemsDescription,
                                            cargoWeight = cargoWeight
                                        };
                                        tenderContragents.Add(registerTenderContragent);
                                    }
                                }
                                contragents.Add(Int64.Parse(items.itemExternalN), tenderContragents);
                            }
                        }
                        context.UpdateDataRegisterContragents(contragents);
                        List<SpecificationListViewModel> specificationListViews = new List<SpecificationListViewModel>();
                        int AlgorithmId = 2;
                        long OrderId = Int64.Parse(myDeserializedClass.data.tenderExternalN.Remove(myDeserializedClass.data.tenderExternalN.IndexOf('-')));
                        OrderID = OrderId;
                        context.SaveListStatus(new CompetetiveListStepsInfoViewModel()
                        {
                            StepId = 3,
                            OrderId = OrderID,
                            userId = allAppSettings["UserId"],
                            tenderNumber = tenderNumber
                        });
                        context.getCurrentStatusForListKL(OrderID, allAppSettings["UserId"], tenderNumber);
                        currentStatus = context.getCurrentStatusForList(OrderID, tenderNumber);
                        var CompetitiveListInfo_ = context.getCompetitiveListInfo(OrderId);
                        var specificationList = context.GetSpecifications(null, 5, 1, OrderId, true, CompetitiveListInfo_.FilterTripTypeId,
                                   false, null, true, CompetitiveListInfo_.FilterVehicleTypeId, true, CompetitiveListInfo_.FilterPayerId,
                                   false, AlgorithmId);

                        foreach (var it in contragents)
                        {
                            for (int i = 0; i < it.Value.Count; i++)
                            {
                                SpecificationListViewModel model = new SpecificationListViewModel();

                                var spec = specificationList.Find(x => x.edrpou_aps == it.Value[i].EDRPOUContragent);
                                if (spec != null)
                                {
                                    model.OrderId = (int)OrderId;
                                    model.tenderNumber = tenderNumber;
                                    model.CarryCapacity = specificationList[0].CarryCapacity;
                                    model.DaysDelay = it.Value[i].PaymentDelay;
                                    model.ExpeditorName = it.Value[i].ContragentName;
                                    model.NameSpecification = it.Value[i].nmcName;
                                    model.itemExternalNumber = it.Value[i].itemExternalNumber;
                                    model.ContragentIdAps = it.Value[i].ContragentIdAps;
                                    model.DateUpdateInfo = it.Value[i].DateUpdateInfo;
                                    model.IsWinner = it.Value[i].IsWinner;
                                    model.EDRPOUContragent = it.Value[i].EDRPOUContragent;
                                    model.emailContragent = it.Value[i].emailContragent;
                                    model.transportUnitsProposed = it.Value[i].transportUnitsProposed;
                                    model.acceptedTransportUnits = it.Value[i].acceptedTransportUnits;
                                    model.costOfCarWithoutNDS = it.Value[i].costOfCarWithoutNDS;
                                    model.tenderItemUuid = it.Value[i].tenderItemUuid;
                                    model.costOfCarWithoutNDSToNull = it.Value[i].costOfCarWithoutNDSToNull;
                                    model.note = it.Value[i].note;
                                    model.ArrivalPoint = spec.ArrivalPoint;
                                    model.CanBeDelete = spec.CanBeDelete;
                                    model.CarryCapacityId = spec.CarryCapacityId;
                                    model.CarryCapacityVal = spec.CarryCapacityVal;
                                    model.ContragentName = spec.ContragentName;
                                    model.DeparturePoint = spec.DeparturePoint;
                                    model.edrpou_aps = spec.edrpou_aps;
                                    model.email_aps = spec.email_aps;
                                    model.FilterPayerId = spec.FilterPayerId;
                                    model.FilterSpecificationTypeId = spec.FilterSpecificationTypeId;
                                    model.FilterTripTypeId = spec.FilterTripTypeId;
                                    model.FilterVehicleTypeId = spec.FilterVehicleTypeId;
                                    model.FreightName = spec.FreightName;
                                    model.GenId = spec.GenId;
                                    model.GroupeSpecId = spec.GroupeSpecId;
                                    model.Id = spec.Id;
                                    model.IntervalTypeId = spec.IntervalTypeId;
                                    model.IntervalTypeName = spec.IntervalTypeName;
                                    model.IsForwarder = spec.IsForwarder;
                                    model.IsFreight = spec.IsFreight;
                                    model.isTruck = spec.isTruck;
                                    model.MovingType = spec.MovingType;
                                    model.MovingTypeName = spec.MovingTypeName;
                                    model.NameGroupeSpecification = spec.NameGroupeSpecification;
                                    model.NameIntervalType = spec.NameIntervalType;
                                    model.NDSTax = spec.NDSTax;
                                    model.nmcName = spec.nmcName;
                                    model.PaymentDelay = spec.PaymentDelay;
                                    model.RateHour = spec.RateHour;
                                    model.RateKm = spec.RateKm;
                                    model.RateMachineHour = spec.RateMachineHour;
                                    model.RateTotalFreight = spec.RateTotalFreight;
                                    model.RateValue = spec.RateValue;
                                    model.returnurl = spec.returnurl;
                                    model.RouteLength = spec.RouteLength;
                                    model.RouteName = spec.RouteName;
                                    model.RouteTypeId = spec.RouteTypeId;
                                    model.RouteTypeName = spec.RouteTypeName;
                                    model.tripTypeName = spec.tripTypeName;
                                    model.UsedRateId = spec.UsedRateId;
                                    model.UsedRateName = spec.UsedRateName;
                                    model.UsePayerFilter = spec.UsePayerFilter;
                                    model.UseRouteFilter = spec.UseRouteFilter;
                                    model.UseSpecificationTypeFilter = spec.UseSpecificationTypeFilter;
                                    model.UseTripTypeFilter = spec.UseTripTypeFilter;
                                    model.UseVehicleTypeFilter = spec.UseVehicleTypeFilter;
                                    model.VehicleTypeName = spec.VehicleTypeName;
                                    model.itemDescription = it.Value[i].itemDescription;
                                    model.cargoWeight = cargoWeight;
                                    specificationListViews.Add(model);
                                }
                                else
                                {
                                    SpecificationListViewModel instance = new SpecificationListViewModel()
                                    {
                                        OrderId = (int)OrderId,
                                        GenId = specificationList[0].GenId,
                                        tenderNumber = tenderNumber,
                                        CarryCapacity = specificationList[0].CarryCapacity,
                                        DaysDelay = it.Value[i].PaymentDelay,
                                        ExpeditorName = it.Value[i].ContragentName,
                                        FilterPayerId = specificationList[0].FilterPayerId,
                                        FilterSpecificationTypeId = specificationList[0].FilterSpecificationTypeId,
                                        FilterTripTypeId = specificationList[0].FilterTripTypeId,
                                        FilterVehicleTypeId = specificationList[0].FilterVehicleTypeId,
                                        FreightName = specificationList[0].FreightName,
                                        GroupeSpecId = specificationList[0].GroupeSpecId,
                                        IntervalTypeId = specificationList[0].IntervalTypeId,
                                        IsForwarder = specificationList[0].IsForwarder,
                                        IsFreight = specificationList[0].IsFreight,
                                        NDSTax = specificationList[0].NDSTax,
                                        NameGroupeSpecification = specificationList[0].NameGroupeSpecification,
                                        NameSpecification = it.Value[i].nmcName,
                                        RateValue = specificationList[0].RateValue,
                                        RouteTypeId = specificationList[0].RouteTypeId,
                                        UsePayerFilter = specificationList[0].UsePayerFilter,
                                        UseRouteFilter = specificationList[0].UseRouteFilter,
                                        UseSpecificationTypeFilter = specificationList[0].UseSpecificationTypeFilter,
                                        UseTripTypeFilter = specificationList[0].UseTripTypeFilter,
                                        UseVehicleTypeFilter = specificationList[0].UseVehicleTypeFilter,
                                        UsedRateId = specificationList[0].UsedRateId,
                                        UsedRateName = specificationList[0].UsedRateName,
                                        VehicleTypeName = specificationList[0].VehicleTypeName,
                                        edrpou_aps = it.Value[i].EDRPOUContragent,
                                        email_aps = it.Value[i].emailContragent,
                                        isTruck = specificationList[0].isTruck,
                                        itemExternalNumber = it.Value[i].itemExternalNumber,
                                        ContragentIdAps = it.Value[i].ContragentIdAps,
                                        DateUpdateInfo = it.Value[i].DateUpdateInfo,
                                        IsWinner = it.Value[i].IsWinner,
                                        EDRPOUContragent = it.Value[i].EDRPOUContragent,
                                        emailContragent = it.Value[i].emailContragent,
                                        transportUnitsProposed = it.Value[i].transportUnitsProposed,
                                        acceptedTransportUnits = it.Value[i].acceptedTransportUnits,
                                        costOfCarWithoutNDS = it.Value[i].costOfCarWithoutNDS,
                                        tenderItemUuid = it.Value[i].tenderItemUuid,
                                        costOfCarWithoutNDSToNull = it.Value[i].costOfCarWithoutNDSToNull,
                                        note = it.Value[i].note,
                                        itemDescription = it.Value[i].itemDescription,
                                        cargoWeight = cargoWeight
                                    };
                                    specificationListViews.Add(instance);
                                }
                            }
                        }
                        if (specificationListViews.Count != 0)
                        {
                            if (context.IsContainTender(tenderNumber))
                            {
                                foreach (var model in specificationListViews)
                                {
                                    context.NewSpecification(model, allAppSettings["UserId"], tenderNumber);
                                }
                            }
                        }
                    }
                }
                var listStatuses = context.getAvialiableStepsForList(OrderID, tenderNumber);
                List<CompetitiveListStepViewModel> listStKL = new List<CompetitiveListStepViewModel>();
                foreach (var el in listStatuses)
                {
                    listStKL.Add(el);
                }
                if (currentStatus != null && listStKL.Count <= 2)
                {
                    context.SaveListStatus(new CompetetiveListStepsInfoViewModel()
                    {
                        StepId = 3,
                        OrderId = OrderID,
                        userId = allAppSettings["UserId"],
                        tenderNumber = tenderNumber
                    });
                    context.getCurrentStatusForListKL(OrderID, allAppSettings["UserId"], tenderNumber);
                    currentStatus = context.getCurrentStatusForList(OrderID, tenderNumber);
                    listStatuses = context.getAvialiableStepsForList(OrderID, tenderNumber);
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}