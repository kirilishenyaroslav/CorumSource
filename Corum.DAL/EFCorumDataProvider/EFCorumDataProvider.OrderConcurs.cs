using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corum.Models;
using Corum.Common;
using Corum.Models.ViewModels.OrderConcurs;
using Corum.DAL.Mappings;
using Corum.DAL.Entity;
using Corum.Models.ViewModels.Cars;
using System.Globalization;



namespace Corum.DAL
{

    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {
        public static string userId;
        public static bool status;
        public IEnumerable<CompetitiveListStepViewModel> getAvialiableStepsForList(long orderId, int? tenderNumber)
        {
            if (tenderNumber != null)
            {
                var currentStep = getCurrentStatusForList(orderId, tenderNumber);
                if (currentStep != null)
                {
                    return db.OrderConcursSteps.Where(x => x.Id <= currentStep.StepId + 1).Select(Mapper.Map);
                }
                return db.OrderConcursSteps.Where(x => x.Id == 1).Select(Mapper.Map);
            }
            else
            {
                return getAvialiableStepsForList(orderId);
            }
        }
        public IEnumerable<CompetitiveListStepViewModel> getAvialiableStepsForList(long orderId)
        {
            var currentStep = getCurrentStatusForList(orderId);
            if (currentStep != null)
            {
                return db.OrderConcursSteps.Where(x => x.Id <= currentStep.StepId + 1).Select(Mapper.Map);
            }
            return db.OrderConcursSteps.Where(x => x.Id == 1).Select(Mapper.Map);
        }

        public IQueryable<CompetetiveListStepsInfoViewModel> getTimeLineForList(long orderId)
        {

            List<CompetetiveListStepsInfoViewModel> cL = new List<CompetetiveListStepsInfoViewModel>();
            cL.AddRange(db.OrderConcursListsSteps
                .Where(osh => osh.OrderId == orderId)
                .Select(Mapper.Map)
                .OrderBy(x => x.Id));

            foreach (var orderItem in cL)
            {
                if ((cL.FirstOrDefault(osh => osh.Id == orderItem.Id - 1)) != null)
                {
                    orderItem.PreviousStepId = cL.FirstOrDefault(osh => osh.Id == orderItem.Id - 1).StepId;
                    orderItem.PreviousStepFullCode =
                        db.OrderConcursSteps.First(x => x.Id == orderItem.PreviousStepId).StepNameFull;
                }
                else
                {
                    orderItem.PreviousStepId = null;
                    orderItem.PreviousStepFullCode = "";
                }
            }

            return cL.AsQueryable();
            //   var concursList = db.OrderConcursListsSteps.Where(x => x.OrderId == orderId).OrderBy(x=>x.Id).Select(Mapper.Map).ToList();            
        }

        public List<CompetetiveListStepsInfoViewModel> listCurrentStatuses(long orderId)
        {
            List<CompetetiveListStepsInfoViewModel> list = new List<CompetetiveListStepsInfoViewModel>();
            IQueryable<RegisterTenders> registers = db.RegisterTenders.Where(x => x.OrderId == orderId);
            IEnumerable<int> tendersNumbers = registers.Select(x => x.tenderNumber);
            foreach (var item in tendersNumbers)
            {
                var currentStep =
                    db.OrderConcursListsSteps.Where(x => x.tenderNumber == item).OrderByDescending(x => x.Datetimevalue).FirstOrDefault();

                if (currentStep != null)
                {
                    list.Add(Mapper.Map_(currentStep, item));
                }
            }
            return list;
        }


        public Dictionary<int, IQueryable<OrderCompetitiveListViewModel>> listDisplayValues(long orderId, string userId)
        {
            Dictionary<int, IQueryable<OrderCompetitiveListViewModel>> dic = new Dictionary<int, IQueryable<OrderCompetitiveListViewModel>>();
            IQueryable<RegisterTenders> registers = db.RegisterTenders.Where(x => x.OrderId == orderId);
            IEnumerable<int> tendersNumbers = registers.Select(x => x.tenderNumber);
            foreach (var item in tendersNumbers)
            {
                var it = getOrderCompetitiveList(userId, orderId, item);
                if (it != null)
                {
                    dic[item] = it;
                }
            }
            return dic;
        }

        public Dictionary<int, IEnumerable<CompetitiveListStepViewModel>> list_listStatuses(long orderId)
        {
            Dictionary<int, IEnumerable<CompetitiveListStepViewModel>> dic = new Dictionary<int, IEnumerable<CompetitiveListStepViewModel>>();
            IQueryable<RegisterTenders> registers = db.RegisterTenders.Where(x => x.OrderId == orderId);
            IEnumerable<int> tendersNumbers = registers.Select(x => x.tenderNumber);
            foreach (var item in tendersNumbers)
            {
                var it = getAvialiableStepsForList(orderId, item);
                if (it != null)
                {
                    dic[item] = it;
                }
            }
            return dic;
        }


        public int? getTenderNumber(long orderId)
        {
            int tenderNumber = 0;
            IQueryable<RegisterTenders> registers = db.RegisterTenders.Where(x => x.OrderId == orderId);
            Dictionary<int, bool> ts = new Dictionary<int, bool>();
            if (registers.ToList().Count == 0)
            {
                return null;
            }
            foreach (var items in registers)
            {
                if (items.process == 9)
                {
                    ts.Add(items.tenderNumber, false);
                }
                else
                {
                    ts.Add(items.tenderNumber, true);
                }
            }
            tenderNumber = ts.Where(x => x.Value == true).OrderByDescending(x => x.Value).FirstOrDefault().Key;
            if (tenderNumber != null && tenderNumber == 0)
            {
                tenderNumber = ts.Where(x => x.Value == false).OrderByDescending(x => x.Value).FirstOrDefault().Key;
            }
            return tenderNumber;
        }
        public CompetetiveListStepsInfoViewModel getCurrentStatusForList(long orderId, int? tenderNumber)
        {
            if (tenderNumber != null)
            {
                var currentStep =
                    db.OrderConcursListsSteps.Where(x => x.tenderNumber == tenderNumber).OrderByDescending(x => x.Datetimevalue).FirstOrDefault();
                try
                {
                    if (status)
                    {
                        var stepInf = db.OrderConcursSteps.Where(x => x.Id == 3).FirstOrDefault();
                        currentStep.OrderConcursSteps = stepInf;
                        currentStep.StepId = stepInf.Id;
                    }
                }
                catch { }
                if (currentStep != null)
                {
                    if (currentStep.AspNetUsers != null && currentStep.OrderConcursSteps != null)
                    {
                        return Mapper.Map_(currentStep, tenderNumber);
                    }
                    else
                    {
                        var stepInfo = db.OrderConcursSteps.Where(x => x.Id == currentStep.StepId).FirstOrDefault();
                        var aspNetusers = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                        currentStep.OrderConcursSteps = stepInfo;
                        currentStep.AspNetUsers = aspNetusers;
                        return Mapper.Map_(currentStep, tenderNumber);
                    }
                }
                {
                    var first = db.OrderConcursSteps.Where(x => x.Id == 1).Select(Mapper.Map).FirstOrDefault();
                    return new CompetetiveListStepsInfoViewModel()
                    {
                        OrderId = orderId,
                        StepId = 1,
                        StepFullCode = first.StepFullName,
                        StepShortCode = first.StepShortName,
                        tenderNumber = tenderNumber
                    };
                }
            }
            else
            {
                return getCurrentStatusForList(orderId);
            }
        }
        public CompetetiveListStepsInfoViewModel getCurrentStatusForList(long orderId)
        {
            var currentStep =
                db.OrderConcursListsSteps.Where(x => x.OrderId == orderId).OrderByDescending(x => x.Id).FirstOrDefault();
            try
            {
                if (status)
                {
                    var stepInf = db.OrderConcursSteps.Where(x => x.Id == 3).FirstOrDefault();
                    currentStep.OrderConcursSteps = stepInf;
                    currentStep.StepId = stepInf.Id;
                }
            }
            catch { }
            if (currentStep != null)
            {
                if (currentStep.AspNetUsers != null && currentStep.OrderConcursSteps != null)
                {
                    return Mapper.Map(currentStep);
                }
                else
                {
                    var stepInfo = db.OrderConcursSteps.Where(x => x.Id == currentStep.StepId).FirstOrDefault();
                    var aspNetusers = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                    currentStep.OrderConcursSteps = stepInfo;
                    currentStep.AspNetUsers = aspNetusers;
                    return Mapper.Map(currentStep);
                }
            }
            {
                var first = db.OrderConcursSteps.Where(x => x.Id == 1).Select(Mapper.Map).FirstOrDefault();
                return new CompetetiveListStepsInfoViewModel()
                {
                    OrderId = orderId,
                    StepId = 1,
                    StepFullCode = first.StepFullName,
                    StepShortCode = first.StepShortName
                };
            }
        }

        public void getCurrentStatusForListKL(long orderId, string userId, int? tenderNumber)
        {
            EFCorumDataProvider.userId = userId;
            //EFCorumDataProvider.status = status;
            getCurrentStatusForList(orderId, tenderNumber);
        }

        public long SaveListStatus(CompetetiveListStepsInfoViewModel newStatusInfo)
        {
            var aspNetusers = db.AspNetUsers.Where(x => x.Id == newStatusInfo.userId).FirstOrDefault();
            var info = new OrderConcursListsSteps()
            {
                OrderId = newStatusInfo.OrderId,
                UserId = newStatusInfo.userId,
                StepId = newStatusInfo.StepId,
                Datetimevalue = DateTime.Now,
                tenderNumber = newStatusInfo.tenderNumber,
                AspNetUsers = aspNetusers
            };
            try
            {

                db.OrderConcursListsSteps.Add(info);
                db.SaveChanges();
            }
            catch (Exception e)
            {
            }

            return info.Id;
        }

        private string getRouteInfo(int? TripType, string Country, string City, string Adress)
        {
            string RouteInfo = "";
            if (TripType == 2)
                RouteInfo = string.Concat(Country, ", ", City, ", ", Adress);
            else
                RouteInfo = string.Concat(City, ", ", Adress);

            return RouteInfo;
        }
        public CompetitiveListViewModel getCompetitiveListInfo(long OrderId, int? tenderNumber)
        {
            if (tenderNumber != null)
            {
                var orderInfo = Mapper.Map(db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == OrderId));
                var passInfo = db.OrdersPassengerTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == OrderId);
                var truckInfo = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == OrderId);
                var payers = db.BalanceKeepers.AsNoTracking().FirstOrDefault(x => x.Id == orderInfo.PayerId);
                var currentStep = getCurrentStatusForList(OrderId, tenderNumber);
                var OrderTypeFullInfo = getOrderType(orderInfo.OrderType);

                CompetitiveListViewModel compList = new CompetitiveListViewModel();
                compList.Id = orderInfo.Id;
                compList.OrderDate = orderInfo.OrderDate;
                compList.OrderType = orderInfo.OrderType;
                compList.CreatedByUserName = getUser(orderInfo.CreatedByUser).displayName;
                compList.currentStep = currentStep;

                compList.TimeRoute = orderInfo.TimeRoute;
                compList.TimeSpecialVehicles = orderInfo.TimeSpecialVehicles;

                string cityFrom = "", cityTo = "";

                int VehicleTypeId = 0;
                string VehicleTypeName = "";
                bool IsTruck = false;
                int FilterPayerId = orderInfo.PayerId;

                int tripType = 0;
                if ((orderInfo.OrderType == 4) || (orderInfo.OrderType == 5) || (orderInfo.OrderType == 7))
                {
                    var truckTypeInfo = db.OrderTruckTransport.FirstOrDefault(or => or.OrderId == OrderId);
                    tripType = truckTypeInfo.TripType ?? 0;

                    cityFrom = truckTypeInfo.ShipperCity;
                    cityTo = truckTypeInfo.ConsigneeCity;

                    IsTruck = true;
                    VehicleTypeId = truckTypeInfo.VehicleTypeId ?? 0;
                    VehicleTypeName = db.OrderVehicleTypes.FirstOrDefault(u => u.Id == VehicleTypeId)?.VehicleTypeName;

                }

                if ((orderInfo.OrderType == 1) || (orderInfo.OrderType == 3) || (orderInfo.OrderType == 6))
                {
                    var passTypeInfo = db.OrdersPassengerTransport.FirstOrDefault(or => or.OrderId == OrderId);
                    tripType = passTypeInfo.TripType ?? 0;

                    cityFrom = passTypeInfo.FromCity;
                    cityTo = passTypeInfo.ToCity;
                }

                compList.CityFrom = cityFrom;
                compList.CityTo = cityTo;

                compList.VehicleTypeId = VehicleTypeId;
                compList.VehicleTypeName = VehicleTypeName;

                compList.IsTruck = IsTruck;

                // tripType = tripType + 1;
                compList.tripTypeName = db.RouteTypes.AsNoTracking().FirstOrDefault(x => x.Id == tripType).NameRouteType;
                compList.TripType = tripType;
                string SpecTypeId = "";
                compList.SpecificationType = "";
                var Spec = db.OrderBaseSpecification.AsNoTracking().Where(x => x.OrderId == OrderId).ToList();

                foreach (var spec in Spec)
                {
                    if (spec.Id != null)
                    {
                        if (SpecTypeId.Length > 0)
                        {
                            SpecTypeId += ",";
                            compList.SpecificationType += ",";
                        }
                        SpecTypeId = string.Concat(SpecTypeId, spec.SpecificationId);
                        compList.SpecificationType = string.Concat(compList.SpecificationType,
                            spec.SpecificationTypes.SpecificationType);
                    }
                }

                if (payers != null)
                    compList.PayerName = payers.BalanceKeeper;

                if (orderInfo != null)
                {
                    compList.TotalDistanceLenght = Convert.ToDecimal(orderInfo.TotalDistanceLenght.Replace(".", ","));
                    compList.PriorityType = orderInfo.PriorityType;
                }
                if ((orderInfo.OrderType == 4) || (orderInfo.OrderType == 5) || (orderInfo.OrderType == 7))
                {
                    if (truckInfo != null)
                    {
                        compList.TruckDescription = truckInfo.TruckDescription;
                        compList.Weight = (truckInfo.Weight ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA"));
                        compList.DimenssionH = Convert.ToDouble(truckInfo.DimenssionH ?? 0);
                        compList.DimenssionL = Convert.ToDouble(truckInfo.DimenssionL ?? 0);
                        compList.DimenssionW = Convert.ToDouble(truckInfo.DimenssionW ?? 0);
                        compList.Dimenssion = string.Concat(compList.DimenssionH.ToString(), " * ",
                            compList.DimenssionL.ToString(), " * ", compList.DimenssionW.ToString());

                        compList.BoxingDescription = truckInfo.BoxingDescription;

                        compList.FromDate = truckInfo.FromShipperDatetime.Value.ToString("dd.MM.yyyy");
                        compList.FromDateRaw = DateTimeConvertClass.getString(truckInfo.FromShipperDatetime.Value);

                        compList.ToDate = truckInfo.ToConsigneeDatetime.Value.ToString("dd.MM.yyyy");
                        compList.ToDateRaw = DateTimeConvertClass.getString(truckInfo.ToConsigneeDatetime.Value);

                        compList.Route = string.Concat(compList.FromInfo, " - ", compList.ToInfo);
                        compList.CarNumber = orderInfo.CarNumber;

                        var shipperCountryName =
                            db.Countries.FirstOrDefault(u => u.Сode == truckInfo.ShipperCountryId)?.Name;
                        compList.ShipperCountryName = shipperCountryName;

                        compList.FromInfo = getRouteInfo(truckInfo.TripType, compList.ShipperCountryName, truckInfo.ShipperCity, truckInfo.ShipperAdress);

                        /*if (truckInfo.TripType == 2)
                            compList.FromInfo = string.Concat(compList.ShipperCountryName, ", ",
                                truckInfo.ShipperCity, ", ", truckInfo.ShipperAdress);
                        else
                            compList.FromInfo = string.Concat(truckInfo.ShipperCity, ", ",
                                truckInfo.ShipperAdress);
                                */

                        var consigneeCountryName =
                            db.Countries.FirstOrDefault(u => u.Сode == truckInfo.ConsigneeCountryId)?.Name;
                        compList.ConsigneeCountryName = consigneeCountryName;

                        compList.ToInfo = getRouteInfo(truckInfo.TripType, compList.ConsigneeCountryName, truckInfo.ConsigneeCity, truckInfo.ConsigneeAdress);

                        /*  if (truckInfo.TripType == 2)
                              compList.ToInfo = string.Concat(compList.ConsigneeCountryName, ", ",
                                  truckInfo.ConsigneeCity, ", ", truckInfo.ConsigneeAdress);
                          else
                              compList.ToInfo = string.Concat(truckInfo.ConsigneeCity, ", ",
                                  truckInfo.ConsigneeAdress);
                          */
                        compList.CarNumber = orderInfo.CarNumber;

                    }
                }

                if ((orderInfo.OrderType == 1) || (orderInfo.OrderType == 3) || (orderInfo.OrderType == 6))
                {
                    compList.TruckDescription = "";
                    compList.Weight = "";
                    compList.BoxingDescription = "";
                    compList.Dimenssion = "";

                    if (passInfo != null)
                    {
                        compList.FromDate = passInfo.StartDateTimeOfTrip.ToString("dd.MM.yyyy");
                        compList.FromDateRaw = DateTimeConvertClass.getString(passInfo.StartDateTimeOfTrip);

                        compList.ToDate = passInfo.FinishDateTimeOfTrip.ToString("dd.MM.yyyy");
                        compList.ToDateRaw = DateTimeConvertClass.getString(passInfo.FinishDateTimeOfTrip);

                        var fromCountryName = db.Countries.FirstOrDefault(u => u.Сode == passInfo.FromCountry)?.Name;
                        compList.ShipperCountryName = fromCountryName;

                        compList.FromInfo = getRouteInfo(compList.TripType, compList.ShipperCountryName, passInfo.FromCity, passInfo.AdressFrom);

                        /*if (compList.TripType == 2)
                            compList.FromInfo = string.Concat(compList.ShipperCountryName, ", ", passInfo.FromCity, ", ",
                                passInfo.AdressFrom);
                        else
                            compList.FromInfo = string.Concat(passInfo.FromCity, ", ", passInfo.AdressFrom);*/

                        var consigneeCountryName = db.Countries.FirstOrDefault(u => u.Сode == passInfo.ToCountry)?.Name;
                        compList.ConsigneeCountryName = consigneeCountryName;

                        compList.FromInfo = getRouteInfo(compList.TripType, compList.ConsigneeCountryName, passInfo.ToCity, passInfo.AdressTo);

                        /*if (compList.TripType == 2)
                            compList.ToInfo = string.Concat(compList.ConsigneeCountryName, ", ",
                                passInfo.ToCity, ", ", passInfo.AdressTo);
                        else
                            compList.ToInfo = string.Concat(passInfo.ToCity, ", ",
                                passInfo.AdressTo);*/

                        compList.CarNumber = 1;
                    }
                }

                compList.Route = string.Concat(compList.FromInfo, " - ", compList.ToInfo);

                compList.FilterTripTypeId = string.IsNullOrEmpty(tripType.ToString()) ? "" : tripType.ToString();

                if (compList.FilterTripTypeId == "") compList.UseTripTypeFilter = false;
                else compList.UseTripTypeFilter = true;

                compList.FilterSpecificationTypeId = string.IsNullOrEmpty(SpecTypeId.ToString())
                    ? ""
                    : SpecTypeId.ToString();
                if (compList.FilterSpecificationTypeId == "") compList.UseSpecificationTypeFilter = false;
                else compList.UseSpecificationTypeFilter = true;

                compList.FilterVehicleTypeId = string.IsNullOrEmpty(VehicleTypeId.ToString())
                    ? ""
                    : VehicleTypeId.ToString();
                if (compList.FilterVehicleTypeId == "") compList.UseVehicleTypeFilter = false;
                else compList.UseVehicleTypeFilter = true;

                compList.FilterPayerId = string.IsNullOrEmpty(FilterPayerId.ToString()) ? "" : FilterPayerId.ToString();
                if (compList.FilterPayerId == "") compList.UsePayerFilter = false;
                else compList.UsePayerFilter = true;

                compList.OrderExecuterName = getUser(orderInfo.OrderExecuter).displayName;
                compList.OrderTypename = OrderTypeFullInfo.TypeName;
                return compList;
            }
            else
            {
                return getCompetitiveListInfo(OrderId);
            }
        }
        public CompetitiveListViewModel getCompetitiveListInfo(long OrderId)
        {
            var orderInfo = Mapper.Map(db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == OrderId));
            var passInfo = db.OrdersPassengerTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == OrderId);
            var truckInfo = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == OrderId);
            var payers = db.BalanceKeepers.AsNoTracking().FirstOrDefault(x => x.Id == orderInfo.PayerId);
            var currentStep = getCurrentStatusForList(OrderId);
            var OrderTypeFullInfo = getOrderType(orderInfo.OrderType);

            CompetitiveListViewModel compList = new CompetitiveListViewModel();
            compList.Id = orderInfo.Id;
            compList.OrderDate = orderInfo.OrderDate;
            compList.OrderType = orderInfo.OrderType;
            compList.CreatedByUserName = getUser(orderInfo.CreatedByUser).displayName;
            compList.currentStep = currentStep;

            compList.TimeRoute = orderInfo.TimeRoute;
            compList.TimeSpecialVehicles = orderInfo.TimeSpecialVehicles;

            string cityFrom = "", cityTo = "";

            int VehicleTypeId = 0;
            string VehicleTypeName = "";
            bool IsTruck = false;
            int FilterPayerId = orderInfo.PayerId;

            int tripType = 0;
            if ((orderInfo.OrderType == 4) || (orderInfo.OrderType == 5) || (orderInfo.OrderType == 7))
            {
                var truckTypeInfo = db.OrderTruckTransport.FirstOrDefault(or => or.OrderId == OrderId);
                tripType = truckTypeInfo.TripType ?? 0;

                cityFrom = truckTypeInfo.ShipperCity;
                cityTo = truckTypeInfo.ConsigneeCity;

                IsTruck = true;
                VehicleTypeId = truckTypeInfo.VehicleTypeId ?? 0;
                VehicleTypeName = db.OrderVehicleTypes.FirstOrDefault(u => u.Id == VehicleTypeId)?.VehicleTypeName;

            }

            if ((orderInfo.OrderType == 1) || (orderInfo.OrderType == 3) || (orderInfo.OrderType == 6))
            {
                var passTypeInfo = db.OrdersPassengerTransport.FirstOrDefault(or => or.OrderId == OrderId);
                tripType = passTypeInfo.TripType ?? 0;

                cityFrom = passTypeInfo.FromCity;
                cityTo = passTypeInfo.ToCity;
            }

            compList.CityFrom = cityFrom;
            compList.CityTo = cityTo;

            compList.VehicleTypeId = VehicleTypeId;
            compList.VehicleTypeName = VehicleTypeName;

            compList.IsTruck = IsTruck;

            // tripType = tripType + 1;
            compList.tripTypeName = db.RouteTypes.AsNoTracking().FirstOrDefault(x => x.Id == tripType).NameRouteType;
            compList.TripType = tripType;
            string SpecTypeId = "";
            compList.SpecificationType = "";
            var Spec = db.OrderBaseSpecification.AsNoTracking().Where(x => x.OrderId == OrderId).ToList();

            foreach (var spec in Spec)
            {
                if (spec.Id != null)
                {
                    if (SpecTypeId.Length > 0)
                    {
                        SpecTypeId += ",";
                        compList.SpecificationType += ",";
                    }
                    SpecTypeId = string.Concat(SpecTypeId, spec.SpecificationId);
                    compList.SpecificationType = string.Concat(compList.SpecificationType,
                        spec.SpecificationTypes.SpecificationType);
                }
            }

            if (payers != null)
                compList.PayerName = payers.BalanceKeeper;

            if (orderInfo != null)
            {
                compList.TotalDistanceLenght = Convert.ToDecimal(orderInfo.TotalDistanceLenght.Replace(".", ","));
                compList.PriorityType = orderInfo.PriorityType;
            }
            if ((orderInfo.OrderType == 4) || (orderInfo.OrderType == 5) || (orderInfo.OrderType == 7))
            {
                if (truckInfo != null)
                {
                    compList.TruckDescription = truckInfo.TruckDescription;
                    compList.Weight = (truckInfo.Weight ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA"));
                    compList.DimenssionH = Convert.ToDouble(truckInfo.DimenssionH ?? 0);
                    compList.DimenssionL = Convert.ToDouble(truckInfo.DimenssionL ?? 0);
                    compList.DimenssionW = Convert.ToDouble(truckInfo.DimenssionW ?? 0);
                    compList.Dimenssion = string.Concat(compList.DimenssionH.ToString(), " * ",
                        compList.DimenssionL.ToString(), " * ", compList.DimenssionW.ToString());

                    compList.BoxingDescription = truckInfo.BoxingDescription;

                    compList.FromDate = truckInfo.FromShipperDatetime.Value.ToString("dd.MM.yyyy");
                    compList.FromDateRaw = DateTimeConvertClass.getString(truckInfo.FromShipperDatetime.Value);

                    compList.ToDate = truckInfo.ToConsigneeDatetime.Value.ToString("dd.MM.yyyy");
                    compList.ToDateRaw = DateTimeConvertClass.getString(truckInfo.ToConsigneeDatetime.Value);

                    compList.Route = string.Concat(compList.FromInfo, " - ", compList.ToInfo);
                    compList.CarNumber = orderInfo.CarNumber;

                    var shipperCountryName =
                        db.Countries.FirstOrDefault(u => u.Сode == truckInfo.ShipperCountryId)?.Name;
                    compList.ShipperCountryName = shipperCountryName;

                    compList.FromInfo = getRouteInfo(truckInfo.TripType, compList.ShipperCountryName, truckInfo.ShipperCity, truckInfo.ShipperAdress);

                    /*if (truckInfo.TripType == 2)
                        compList.FromInfo = string.Concat(compList.ShipperCountryName, ", ",
                            truckInfo.ShipperCity, ", ", truckInfo.ShipperAdress);
                    else
                        compList.FromInfo = string.Concat(truckInfo.ShipperCity, ", ",
                            truckInfo.ShipperAdress);
                            */

                    var consigneeCountryName =
                        db.Countries.FirstOrDefault(u => u.Сode == truckInfo.ConsigneeCountryId)?.Name;
                    compList.ConsigneeCountryName = consigneeCountryName;

                    compList.ToInfo = getRouteInfo(truckInfo.TripType, compList.ConsigneeCountryName, truckInfo.ConsigneeCity, truckInfo.ConsigneeAdress);

                    /*  if (truckInfo.TripType == 2)
                          compList.ToInfo = string.Concat(compList.ConsigneeCountryName, ", ",
                              truckInfo.ConsigneeCity, ", ", truckInfo.ConsigneeAdress);
                      else
                          compList.ToInfo = string.Concat(truckInfo.ConsigneeCity, ", ",
                              truckInfo.ConsigneeAdress);
                      */
                    compList.CarNumber = orderInfo.CarNumber;

                }
            }

            if ((orderInfo.OrderType == 1) || (orderInfo.OrderType == 3) || (orderInfo.OrderType == 6))
            {
                compList.TruckDescription = "";
                compList.Weight = "";
                compList.BoxingDescription = "";
                compList.Dimenssion = "";

                if (passInfo != null)
                {
                    compList.FromDate = passInfo.StartDateTimeOfTrip.ToString("dd.MM.yyyy");
                    compList.FromDateRaw = DateTimeConvertClass.getString(passInfo.StartDateTimeOfTrip);

                    compList.ToDate = passInfo.FinishDateTimeOfTrip.ToString("dd.MM.yyyy");
                    compList.ToDateRaw = DateTimeConvertClass.getString(passInfo.FinishDateTimeOfTrip);

                    var fromCountryName = db.Countries.FirstOrDefault(u => u.Сode == passInfo.FromCountry)?.Name;
                    compList.ShipperCountryName = fromCountryName;

                    compList.FromInfo = getRouteInfo(compList.TripType, compList.ShipperCountryName, passInfo.FromCity, passInfo.AdressFrom);

                    /*if (compList.TripType == 2)
                        compList.FromInfo = string.Concat(compList.ShipperCountryName, ", ", passInfo.FromCity, ", ",
                            passInfo.AdressFrom);
                    else
                        compList.FromInfo = string.Concat(passInfo.FromCity, ", ", passInfo.AdressFrom);*/

                    var consigneeCountryName = db.Countries.FirstOrDefault(u => u.Сode == passInfo.ToCountry)?.Name;
                    compList.ConsigneeCountryName = consigneeCountryName;

                    compList.FromInfo = getRouteInfo(compList.TripType, compList.ConsigneeCountryName, passInfo.ToCity, passInfo.AdressTo);

                    /*if (compList.TripType == 2)
                        compList.ToInfo = string.Concat(compList.ConsigneeCountryName, ", ",
                            passInfo.ToCity, ", ", passInfo.AdressTo);
                    else
                        compList.ToInfo = string.Concat(passInfo.ToCity, ", ",
                            passInfo.AdressTo);*/

                    compList.CarNumber = 1;
                }
            }

            compList.Route = string.Concat(compList.FromInfo, " - ", compList.ToInfo);

            compList.FilterTripTypeId = string.IsNullOrEmpty(tripType.ToString()) ? "" : tripType.ToString();

            if (compList.FilterTripTypeId == "") compList.UseTripTypeFilter = false;
            else compList.UseTripTypeFilter = true;

            compList.FilterSpecificationTypeId = string.IsNullOrEmpty(SpecTypeId.ToString())
                ? ""
                : SpecTypeId.ToString();
            if (compList.FilterSpecificationTypeId == "") compList.UseSpecificationTypeFilter = false;
            else compList.UseSpecificationTypeFilter = true;

            compList.FilterVehicleTypeId = string.IsNullOrEmpty(VehicleTypeId.ToString())
                ? ""
                : VehicleTypeId.ToString();
            if (compList.FilterVehicleTypeId == "") compList.UseVehicleTypeFilter = false;
            else compList.UseVehicleTypeFilter = true;

            compList.FilterPayerId = string.IsNullOrEmpty(FilterPayerId.ToString()) ? "" : FilterPayerId.ToString();
            if (compList.FilterPayerId == "") compList.UsePayerFilter = false;
            else compList.UsePayerFilter = true;

            compList.OrderExecuterName = getUser(orderInfo.OrderExecuter).displayName;
            compList.OrderTypename = OrderTypeFullInfo.TypeName;
            return compList;
        }

        public IQueryable<SpecificationListViewModel> GetSpecBySearchString(string searchTerm, long OrderId,
            bool UseTripTypeFilter, string FilterTripTypeId,
            bool UseSpecificationTypeFilter, string FilterSpecificationTypeId, bool UseVehicleTypeFilter,
            string FilterVehicleTypeId, bool UsePayerFilter, string FilterPayerId, bool UseRouteFilter, int AlgorithmId)
        {
            var _FilterTripTypeId = string.IsNullOrEmpty(FilterTripTypeId) ? "-1" : FilterTripTypeId;
            var _FilterSpecificationTypeId = string.IsNullOrEmpty(FilterSpecificationTypeId)
                ? "0"
                : FilterSpecificationTypeId;
            var _FilterVehicleTypeId = string.IsNullOrEmpty(FilterVehicleTypeId) ? "0" : FilterVehicleTypeId;
            var _FilterPayerId = string.IsNullOrEmpty(FilterPayerId) ? "0" : FilterPayerId;

            List<SpecificationListViewModel> SpecInfo = new List<SpecificationListViewModel>();

            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == OrderId);
            var AcceptDate = GetAcceptOnlyDate(orderInfo.Id);
            DateTime FilterAcceptDate = string.IsNullOrEmpty(AcceptDate)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime2(AcceptDate);

            bool isTruck = false;
            int VehicleTypeId = 0;
            int TypeSpecId = orderInfo.TypeSpecId ?? 0;
            int PayerId = orderInfo.PayerId ?? 0;

            if ((orderInfo.OrderType == 4) || (orderInfo.OrderType == 5) || (orderInfo.OrderType == 7))
            {
                isTruck = true;
            }

            int tripType = 0;

            if ((orderInfo.OrderType == 4) || (orderInfo.OrderType == 5) || (orderInfo.OrderType == 7))
            {
                var truckTypeInfo = db.OrderTruckTransport.FirstOrDefault(or => or.OrderId == OrderId);
                tripType = truckTypeInfo.TripType ?? 0;
                VehicleTypeId = truckTypeInfo.VehicleTypeId ?? 0;
            }
            if ((orderInfo.OrderType == 1) || (orderInfo.OrderType == 3) || (orderInfo.OrderType == 6))
            {
                var passTypeInfo = db.OrdersPassengerTransport.FirstOrDefault(or => or.OrderId == OrderId);
                tripType = passTypeInfo.TripType ?? 0;
            }

            //tripType = tripType + 1;
            //  var tripTypeName = db.RouteTypes.AsNoTracking().FirstOrDefault(x => x.Id == OrderId).NameRouteType;
            if (AlgorithmId == 1)
            {
                _FilterTripTypeId = string.IsNullOrEmpty(tripType.ToString()) ? "-1" : tripType.ToString();
                if (_FilterTripTypeId == "-1") UseTripTypeFilter = false;
                else UseTripTypeFilter = true;
                _FilterSpecificationTypeId = string.IsNullOrEmpty(TypeSpecId.ToString()) ? "0" : TypeSpecId.ToString();
                if (_FilterSpecificationTypeId == "0") UseSpecificationTypeFilter = false;
                else UseSpecificationTypeFilter = true;
                _FilterVehicleTypeId = string.IsNullOrEmpty(VehicleTypeId.ToString()) ? "0" : VehicleTypeId.ToString();
                if (_FilterVehicleTypeId == "0") UseVehicleTypeFilter = false;
                else UseVehicleTypeFilter = true;
                _FilterPayerId = string.IsNullOrEmpty(PayerId.ToString()) ? "0" : PayerId.ToString();
                if (_FilterPayerId == "0") UsePayerFilter = false;
                else UsePayerFilter = true;
            }

            long? RouteId = orderInfo.RouteId;

            var SpecItems = db.GetSpecifications(FilterAcceptDate, isTruck, UseTripTypeFilter, _FilterTripTypeId,
                UseSpecificationTypeFilter, _FilterSpecificationTypeId,
                UseVehicleTypeFilter, _FilterVehicleTypeId, UsePayerFilter, _FilterPayerId, UseRouteFilter, RouteId
                /*tripType, VehicleTypeId, TypeSpecId*/).ToList();
            int i = 1;
            foreach (var spec in SpecItems)
            {
                //добавляем только если спецификации еще нет в списке конкурентного листа
                var concurs =
                    db.OrderCompetitiveList.FirstOrDefault(o => o.OrderId == OrderId && o.SpecificationId == spec.Id
                        /*&& o.GenId == spec.UsedRateId*/);
                if (concurs == null)
                {
                    SpecificationListViewModel SpecInfoItem = new SpecificationListViewModel();
                    SpecInfoItem.Id = spec.Id;
                    SpecInfoItem.GenId = spec.UsedRateId ?? 1; //i;
                    i++;

                    //        SpecInfoItem.tripTypeName = tripTypeName;
                    SpecInfoItem.IsForwarder = (spec.BalanceKeeperId != null) ? true : false;

                    if (SpecInfoItem.IsForwarder)
                        SpecInfoItem.ExpeditorName = spec.CarrierName1;
                    else
                        SpecInfoItem.ExpeditorName = spec.CarrierName;

                    SpecInfoItem.NameGroupeSpecification = spec.NameGroupSpec;

                    SpecInfoItem.DaysDelay = spec.DaysDelay;
                    SpecInfoItem.NameSpecification = spec.SpecName;
                    //SpecInfoItem.FreightName = spec.FreightName;
                    SpecInfoItem.CarryCapacity = spec.CarryCapacity;
                    SpecInfoItem.NameIntervalType = spec.NameIntervalType;
                    //SpecInfoItem.MovingTypeName = spec.MovingTypeName;
                    SpecInfoItem.FreightName = spec.SpecificationType;
                    SpecInfoItem.VehicleTypeName = spec.VehicleTypeName;
                    SpecInfoItem.isTruck = isTruck;

                    SpecInfoItem.UsedRateName = spec.UsedRateName;
                    SpecInfoItem.UsedRateId = spec.UsedRateId ?? 0;
                    SpecInfoItem.RateValue = spec.RateValue;
                    SpecInfoItem.RateValue = spec.RateValue;

                    //SpecInfoItem.DeparturePoint = spec.DeparturePoint;
                    //SpecInfoItem.ArrivalPoint = spec.ArrivalPoint;
                    SpecInfoItem.RouteName = spec.RouteName;

                    SpecInfoItem.FilterTripTypeId = _FilterTripTypeId;
                    SpecInfoItem.FilterSpecificationTypeId = _FilterSpecificationTypeId;
                    SpecInfoItem.FilterVehicleTypeId = _FilterVehicleTypeId;
                    SpecInfoItem.FilterPayerId = _FilterPayerId;

                    SpecInfoItem.UseTripTypeFilter = UseTripTypeFilter;
                    SpecInfoItem.UseSpecificationTypeFilter = UseSpecificationTypeFilter;
                    SpecInfoItem.UseVehicleTypeFilter = UseVehicleTypeFilter;
                    SpecInfoItem.UsePayerFilter = UsePayerFilter;
                    SpecInfoItem.UseRouteFilter = UseRouteFilter;
                    SpecInfoItem.NDSTax = (spec.NDSTax ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA"));

                    try
                    {
                        var carOwner = db.CarOwners.Where(x => x.CarrierName.Contains(spec.CarrierName1)).FirstOrDefault();
                        if (carOwner != null)
                        {
                            SpecInfoItem.edrpou_aps = carOwner.edrpou_aps;
                            SpecInfoItem.email_aps = carOwner.email_aps;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    SpecInfo.Add(SpecInfoItem);
                }
            }

            var result = SpecInfo.OrderBy(o => o.UsedRateName).ThenBy(o => o.NameIntervalType).ToList();

            return result

                .AsQueryable();
        }

        public List<SpecificationListViewModel> GetSpecifications(string searchTerm, int pageSize, int pageNum,
            long OrderId, bool UseTripTypeFilter, string FilterTripTypeId,
            bool UseSpecificationTypeFilter, string FilterSpecificationTypeId, bool UseVehicleTypeFilter,
            string FilterVehicleTypeId, bool UsePayerFilter, string FilterPayerId, bool UseRouteFilter, int AlgorithmId)
        {
            return GetSpecBySearchString(searchTerm, OrderId, UseTripTypeFilter, FilterTripTypeId,
                UseSpecificationTypeFilter, FilterSpecificationTypeId, UseVehicleTypeFilter, FilterVehicleTypeId,
                UsePayerFilter, FilterPayerId, UseRouteFilter, AlgorithmId)
                //.Skip(pageSize * (pageNum - 1))
                //.Take(pageSize)
                .ToList();
        }

        public int GetSpecificationsCount(string searchTerm, long OrderId, bool UseTripTypeFilter,
            string FilterTripTypeId,
            bool UseSpecificationTypeFilter, string FilterSpecificationTypeId, bool UseVehicleTypeFilter,
            string FilterVehicleTypeId, bool UsePayerFilter, string FilterPayerId, bool UseRouteFilter, int AlgorithmId)
        {
            return GetSpecBySearchString(searchTerm, OrderId, UseTripTypeFilter, FilterTripTypeId,
                UseSpecificationTypeFilter, FilterSpecificationTypeId, UseVehicleTypeFilter, FilterVehicleTypeId,
                UsePayerFilter, FilterPayerId, UseRouteFilter, AlgorithmId).Count();
        }

        private string GetZeroTarif(int GenId, decimal? RateKm, decimal? TotalDistanceLength, decimal? RateHour,
           int? TimeRoute, decimal? RateMachineHour, int? TimeSpecialVehicles)
        {
            string Comment = "";
            if (GenId == 1)
            {
                if ((RateKm > 0) && ((TotalDistanceLength ?? 0) == 0))
                {
                    Comment = "Расчет не произведен! Не введено \"Расстояние, км\"";
                }
            }
            if ((GenId == 2) || (GenId == 3))
            {
                if ((RateKm > 0) && ((TotalDistanceLength ?? 0) == 0))
                {
                    Comment = "Расчет не произведен! Не введено \"Расстояние, км\"";
                }
                if ((RateHour > 0) && ((TimeRoute ?? 0) == 0))
                {
                    if (Comment == "")
                        Comment = "Расчет не произведен! Не введено \"Время нахождения на маршруте\"";
                    else
                        Comment = string.Concat(Comment,
                            "Не введено \"Время нахождения на маршруте\"");
                }
                if ((RateMachineHour > 0) && ((TimeSpecialVehicles ?? 0) == 0))
                {
                    if (Comment == "")
                        Comment = "Расчет не произведен! Не введено \"Время работы спецтранспорта\"";
                    else
                        Comment = string.Concat(Comment,
                            "Не введено \"Время работы спецтранспорта\"");
                }
            }
            return Comment;
        }

        public bool IsContainTender(int? tenderNumber)
        {
            var tender = db.OrderCompetitiveList.Where(x => x.tenderNumber == tenderNumber).FirstOrDefault();
            return (tender != null) ? false : true;
        }
        public long NewSpecification(SpecificationListViewModel model, string userId, int? tenderNumber)
        {
            var cs = db.ContractSpecifications.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);
            OrderCompetitiveList concurs = null;

            if (cs != null)
            {
                var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == model.OrderId);

                decimal _CarCostDog = 0;
                //если фрахт, то _CarCostDog - это общая стоим-ть фрахта
                // если фрахт - то длину маршрута брать не из спецификации, а из заявки
                if /*((cs.IsFreight == true) &&*/ (model.UsedRateId == 1)
                    _CarCostDog = (cs.RateTotalFreight ?? 0) /* Convert.ToInt32(orderInfo.TotalDistanceLength ?? 0)*/;
                else if (model.UsedRateId == 2)
                {
                    _CarCostDog = (cs.RateKm ?? 0) * (orderInfo.TotalDistanceLength ?? 0);
                }
                else if (model.UsedRateId == 3)
                {
                    decimal routeTime = (orderInfo.TimeRoute ?? 0); /// (decimal)3600000.0;
                    decimal specialVehiclesTime = (orderInfo.TimeSpecialVehicles ?? 0) / (decimal)3600000.0;
                    //сумма= км*тариф_грн.км+часы*тариф_грн.час+м_час*тариф_грн.м.час
                    _CarCostDog = (cs.RateKm ?? 0) * (orderInfo.TotalDistanceLength ?? 0) + (cs.RateHour ?? 0) * routeTime +
                                  (cs.RateMachineHour ?? 0) * specialVehiclesTime;
                }

                string Comments = GetZeroTarif(model.UsedRateId, cs.RateKm, orderInfo.TotalDistanceLength, cs.RateHour,
                    orderInfo.TimeRoute, cs.RateMachineHour, orderInfo.TimeSpecialVehicles);


                if (!(string.IsNullOrEmpty(Comments))) _CarCostDog = 0;

                concurs = new OrderCompetitiveList()
                {
                    OrderId = model.OrderId,
                    ExpeditorName = model.ExpeditorName,
                    CarryCapacity = model.CarryCapacity,
                    DaysDelay = model.DaysDelay,
                    CarCostDog = Math.Round(_CarCostDog, MidpointRounding.AwayFromZero),
                    SpecificationId = model.Id,
                    GenId = model.UsedRateId,
                    IsChange = false,
                    NDS = cs.NDSTax ?? 0,
                    tenderNumber = tenderNumber,
                    CarsOffered = model.transportUnitsProposed,
                    CarsAccepted = model.acceptedTransportUnits,
                    CarCost7 = 0,
                    DaysDelayStep2 = model.DaysDelay,
                    CarCost = Math.Round((decimal)model.costOfCarWithoutNDS, MidpointRounding.AwayFromZero),
                    Comments = model.note,
                    IsSelectedId = model.IsWinner,
                    itemDescription = model.itemDescription,
                    cargoWeight = model.cargoWeight,
                    emailContragent = model.emailContragent
                };

                db.OrderCompetitiveList.Add(concurs);

                db.SaveChanges();

                if (!db.OrderConcursListsSteps.Any(x => x.OrderId == model.OrderId && x.StepId == 3))
                {
                    var StepInfo = new OrderConcursListsSteps()
                    {
                        OrderId = model.OrderId,
                        StepId = 1,
                        UserId = userId,
                        Datetimevalue = DateTime.Now,
                        tenderNumber = tenderNumber
                    };

                    db.OrderConcursListsSteps.Add(StepInfo);
                    db.SaveChanges();
                }
            }
            else
            {
                var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == model.OrderId);
                try
                {
                    concurs = new OrderCompetitiveList()
                    {
                        OrderId = model.OrderId,
                        ExpeditorName = model.ExpeditorName,
                        CarryCapacity = model.CarryCapacity,
                        DaysDelay = model.DaysDelay,
                        CarCostDog = 0,
                        SpecificationId = model.Id,
                        GenId = model.UsedRateId,
                        IsChange = false,
                        NDS = (cs != null)? cs.NDSTax ?? 0: 0,
                        tenderNumber = tenderNumber,
                        CarsOffered = model.transportUnitsProposed,
                        CarsAccepted = model.acceptedTransportUnits,
                        CarCost7 = 0,
                        DaysDelayStep2 = model.DaysDelay,
                        CarCost = Math.Round((decimal)model.costOfCarWithoutNDS, MidpointRounding.AwayFromZero),
                        Comments = model.note,
                        IsSelectedId = model.IsWinner,
                        itemDescription = model.itemDescription,
                        cargoWeight = model.cargoWeight,
                        emailContragent = model.emailContragent
                    };
                }
                catch (Exception e)
                {
                }


                db.OrderCompetitiveList.Add(concurs);

                db.SaveChanges();

                if (!db.OrderConcursListsSteps.Any(x => x.OrderId == model.OrderId && x.StepId == 1))
                {
                    var StepInfo = new OrderConcursListsSteps()
                    {
                        OrderId = model.OrderId,
                        StepId = 1,
                        UserId = userId,
                        Datetimevalue = DateTime.Now,
                        tenderNumber = tenderNumber
                    };

                    db.OrderConcursListsSteps.Add(StepInfo);
                    db.SaveChanges();
                }
            }


            return concurs.Id;

        }

        public long NewSpecification(SpecificationListViewModel model, string userId)
        {
            var cs = db.ContractSpecifications.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == model.OrderId);

            decimal _CarCostDog = 0;
            //если фрахт, то _CarCostDog - это общая стоим-ть фрахта
            // если фрахт - то длину маршрута брать не из спецификации, а из заявки
            if /*((cs.IsFreight == true) &&*/ (model.UsedRateId == 1)
                _CarCostDog = (cs.RateTotalFreight ?? 0) /* Convert.ToInt32(orderInfo.TotalDistanceLength ?? 0)*/;
            else if (model.UsedRateId == 2)
            {
                _CarCostDog = (cs.RateKm ?? 0) * (orderInfo.TotalDistanceLength ?? 0);
            }
            else if (model.UsedRateId == 3)
            {
                decimal routeTime = (orderInfo.TimeRoute ?? 0); /// (decimal)3600000.0;
                decimal specialVehiclesTime = (orderInfo.TimeSpecialVehicles ?? 0) / (decimal)3600000.0;
                //сумма= км*тариф_грн.км+часы*тариф_грн.час+м_час*тариф_грн.м.час
                _CarCostDog = (cs.RateKm ?? 0) * (orderInfo.TotalDistanceLength ?? 0) + (cs.RateHour ?? 0) * routeTime +
                              (cs.RateMachineHour ?? 0) * specialVehiclesTime;
            }

            string Comments = GetZeroTarif(model.UsedRateId, cs.RateKm, orderInfo.TotalDistanceLength, cs.RateHour,
                orderInfo.TimeRoute, cs.RateMachineHour, orderInfo.TimeSpecialVehicles);


            if (!(string.IsNullOrEmpty(Comments))) _CarCostDog = 0;

            var concurs = new OrderCompetitiveList()
            {
                OrderId = model.OrderId,
                ExpeditorName = model.ExpeditorName,
                CarryCapacity = model.CarryCapacity,
                DaysDelay = model.DaysDelay,
                CarCostDog = Math.Round(_CarCostDog, MidpointRounding.AwayFromZero),
                SpecificationId = model.Id,
                GenId = model.UsedRateId,
                IsChange = false,
                NDS = cs.NDSTax ?? 0,
                Comments = Comments
            };

            db.OrderCompetitiveList.Add(concurs);

            db.SaveChanges();

            if (!db.OrderConcursListsSteps.Any(x => x.OrderId == model.OrderId && x.StepId == 1))
            {
                var StepInfo = new OrderConcursListsSteps()
                {
                    OrderId = model.OrderId,
                    StepId = 1,
                    UserId = userId,
                    Datetimevalue = DateTime.Now
                };

                db.OrderConcursListsSteps.Add(StepInfo);
                db.SaveChanges();
            }

            return concurs.Id;

        }

        public bool DeleteConcurs(long id)
        {

            var concurs = db.OrderCompetitiveList.FirstOrDefault(o => o.Id == id);
            try
            {
                if (concurs != null)
                {
                    db.OrderCompetitiveList.Remove(concurs);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
            return true;
        }

        public void UpdateConcurs(OrderCompetitiveListViewModel model)
        {
            var concurs = db.OrderCompetitiveList.FirstOrDefault(o => o.Id == model.Id);
            if (concurs == null) return;

            concurs.CarsOffered = model.CarsOffered;
            concurs.CarsAccepted = model.CarsAccepted;
            concurs.NDS = Convert.ToDecimal(model.NDS.Replace(".", ","));
            concurs.CarCost = Convert.ToDecimal(model.CarCost.Replace(".", ","));
            concurs.Prepayment = model.Prepayment;
            concurs.DaysDelayStep1 = model.DaysDelayStep1;
            concurs.DaysDelayStep2 = model.DaysDelayStep2;

            concurs.Prepayment2 = Convert.ToDecimal(model.Prepayment2.Replace(".", ","));
            if (model.PrepaymentEffect2 != null)
                concurs.PrepaymentEffect2 = Convert.ToDecimal((model.PrepaymentEffect2).Replace(".", ","));

            concurs.Comments = model.Comments;
            concurs.IsSelectedId = model.IsSelectedId;

            var currentStep = getCurrentStatusForList(concurs.OrderId);

            if (currentStep.StepId == 2) concurs.CarCost7 = Convert.ToDecimal(model.CarCost7.Replace(".", ","));


            if ((model.IsFreight == false) && (model.IsChange == false))
            {
                concurs.IsChange = true;
            }
            else
                concurs.CarCostDog = concurs.CarCostDog;
            /*- concurs.CarCostDog * ((Convert.ToDecimal(model.NDS.Replace(".", ",")) / 100));*/

            //concurs.GenId = concurs.GenId;

            db.SaveChanges();
        }

        //определение стоимости одного автомобиля в зависимости от этапа
        private decimal GetCarCost(int StepId, string CarCostDog, string CarCost7, string CarCost)
        {
            decimal carCostValue = 0;
            if (StepId == 1) carCostValue = Convert.ToDecimal(CarCostDog.Replace(".", ","));
            if (StepId == 2) carCostValue = Convert.ToDecimal(CarCost7.Replace(".", ","));
            if (StepId >= 3) carCostValue = Convert.ToDecimal(CarCost.Replace(".", ","));
            return carCostValue;
        }

        //определение договорной отсрочки платежей в зависимости от этапа
        private int GetDaysDelay(int StepId, int DaysDelay, int DaysDelayStep1, int DaysDelayStep2)
        {
            int daysDelayStep = 0;
            if (StepId == 1) daysDelayStep = DaysDelay;
            if (StepId == 2) daysDelayStep = DaysDelayStep1;
            if (StepId >= 3) daysDelayStep = DaysDelayStep2;
            return daysDelayStep;
        }

        private double GetDiscountRateInfo(DateTime OrderDate)
        {
            double DiscountRateInfo = 0;
            if ((db.ConcursDiscountRate.Any()) && (OrderDate != null))
            {
                var DiscountInfo =
                    db.ConcursDiscountRate.AsNoTracking()
                        .FirstOrDefault(x => x.DateBeg <= OrderDate && x.DateEnd >= OrderDate);
                if (DiscountInfo != null)
                    DiscountRateInfo = (double)(DiscountInfo.DiscountRate ?? 0);
                else DiscountRateInfo = 0;
            }
            return DiscountRateInfo;
        }

        private void GetCalculateFields(DateTime OrderDate, long OrderId,
                                        string Prepayment2, int? Prepayment, int? DaysDelay,
                                        int DaysDelayStep1, int DaysDelayStep2,
                                        string CarCostDog, int CarsOffered, string CarCost, ref string CarCost7,
                                        out string DelayEffect, out string PrepaymentEffect, out string CarCostWithMoneyCost)
        {
            double discountRateInfo = GetDiscountRateInfo(OrderDate);

            var currentStep = getCurrentStatusForList(OrderId);

            if (currentStep.StepId == 1)
                CarCost7 =
                    (Convert.ToDecimal(CarCostDog.Replace(".", ",")) * CarsOffered).ToString("F2");

            decimal carCostValue = GetCarCost(currentStep.StepId, CarCostDog, CarCost7,
                CarCost);
            int daysDelayStep = GetDaysDelay(currentStep.StepId, DaysDelay ?? 0, DaysDelayStep1,
                DaysDelayStep2);

            //<столбец 10> = (<столбец 6>-<столбец 12 «сумма предоплаты»>)*<процентная ставка>*<столбец 9(0 Этап)>/365
            //<столбец 10> = (<столбец 7>-<столбец 12 «сумма предоплаты»>)*<процентная ставка>*<столбец 9(1 этап)>/365
            //<столбец 10> = (<столбец 8>-<столбец 12 «сумма предоплаты»>)*<процентная ставка>*<столбец 9(2 этап)>/365
            DelayEffect = ((carCostValue - (Convert.ToDecimal(Prepayment2.Replace(".", ",")))) *
                                     ((decimal)discountRateInfo / 100) * daysDelayStep / 365).ToString("F2");

            //< столбец 13 эффект от предоплаты > = -(минус)(< столбец 12 «сумма предоплаты»>)*< процентная ставка > *< столбец 11 >/ 365
            PrepaymentEffect =
                (Convert.ToDecimal(
                    -((Convert.ToDouble(Prepayment2.Replace(".", ","))) * (discountRateInfo / 100) *
                      (Prepayment ?? 0)) / 365)).ToString("F2");

            //<столбец 15(0 этап)> = <столбец 6>-<столбец 10(0 этап)>-<столбец 13>
            //<столбец 15(1 этап)> = <столбец 7>-<столбец 10(1 этап)>-<столбец 13>
            //<столбец 15(2 этап)> = <столбец 8>-<столбец 10(2 этап)>-<столбец 13> 
            CarCostWithMoneyCost = (carCostValue - Convert.ToDecimal(DelayEffect.Replace(".", ",")) -
                                              Convert.ToDecimal(PrepaymentEffect.Replace(".", ","))).ToString(
                                                  "F2");
        }

        //расчет значений столбцов конкурентного листа
        private OrderCompetitiveListViewModel CalculateCompetitiveList(long Id)
        {
            OrderCompetitiveListViewModel orderItem = new OrderCompetitiveListViewModel();
            orderItem = Mapper.Map(db.OrderCompetitiveList.AsNoTracking().FirstOrDefault(u => u.Id == Id));
            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderItem.OrderId);

            /* var currentStep = getCurrentStatusForList(orderItem.OrderId);                                    
             if (currentStep.StepId == 1)
                 orderItem.CarCost7 =
                     (Convert.ToDecimal(orderItem.CarCostDog.Replace(".", ","))*orderItem.CarsOffered).ToString("F2");
             else
                 orderItem.CarCost7 = orderItem.CarCost7 ?? "0";

             decimal carCostValue = GetCarCost(currentStep.StepId, orderItem.CarCostDog, orderItem.CarCost7,
                 orderItem.CarCost);
             int daysDelayStep = GetDaysDelay(currentStep.StepId, orderItem.DaysDelay ?? 0, orderItem.DaysDelayStep1,
                 orderItem.DaysDelayStep2);

             //<столбец 10> = (<столбец 6>-<столбец 12 «сумма предоплаты»>)*<процентная ставка>*<столбец 9(0 Этап)>/365
             //<столбец 10> = (<столбец 7>-<столбец 12 «сумма предоплаты»>)*<процентная ставка>*<столбец 9(1 этап)>/365
             //<столбец 10> = (<столбец 8>-<столбец 12 «сумма предоплаты»>)*<процентная ставка>*<столбец 9(2 этап)>/365
             orderItem.DelayEffect = ((carCostValue - (Convert.ToDecimal(orderItem.Prepayment2.Replace(".", ","))))*
                                      ((decimal) discountRateInfo/100)*daysDelayStep/365).ToString("F2");

             //< столбец 13 эффект от предоплаты > = -(минус)(< столбец 12 «сумма предоплаты»>)*< процентная ставка > *< столбец 11 >/ 365
             orderItem.PrepaymentEffect =
                 (Convert.ToDecimal(
                     -((Convert.ToDouble(orderItem.Prepayment2.Replace(".", ",")))*(discountRateInfo/100)*
                       (orderItem.Prepayment ?? 0))/365)).ToString("F2");

             //<столбец 15(0 этап)> = <столбец 6>-<столбец 10(0 этап)>-<столбец 13>
             //<столбец 15(1 этап)> = <столбец 7>-<столбец 10(1 этап)>-<столбец 13>
             //<столбец 15(2 этап)> = <столбец 8>-<столбец 10(2 этап)>-<столбец 13> 
             orderItem.CarCostWithMoneyCost = (carCostValue - Convert.ToDecimal(orderItem.DelayEffect.Replace(".", ",")) -
                                               Convert.ToDecimal(orderItem.PrepaymentEffect.Replace(".", ","))).ToString(
                                                   "F2");
 */

            if (orderItem.CarCost == null) orderItem.CarCost = "0";
            if (orderItem.CarCostDog == null) orderItem.CarCostDog = "0";
            if (orderItem.CarCost7 == null) orderItem.CarCost7 = "0";

            string DelayEffect, PrepaymentEffect, CarCostWithMoneyCost, CarCost7;
            CarCost7 = orderItem.CarCost7;
            GetCalculateFields(orderInfo.OrderDate, orderItem.OrderId,
                orderItem.Prepayment2, orderItem.Prepayment, orderItem.DaysDelay,
                orderItem.DaysDelayStep1, orderItem.DaysDelayStep2,
                orderItem.CarCostDog, orderItem.CarsOffered, orderItem.CarCost, ref CarCost7,
                out DelayEffect, out PrepaymentEffect, out CarCostWithMoneyCost);

            orderItem.DelayEffect = DelayEffect;
            orderItem.PrepaymentEffect = PrepaymentEffect;
            orderItem.CarCostWithMoneyCost = CarCostWithMoneyCost;
            orderItem.CarCost7 = CarCost7;

            //if (orderItem.CarCostWithMoneyCost == null) orderItem.CarCostWithMoneyCost = "0";           
            var totalDistanceLength = orderInfo.TotalDistanceLength.Value;

            if ((orderInfo != null) && (totalDistanceLength > 0))

                orderItem.AverageCost =
                    (Convert.ToDecimal(orderItem.CarCostWithMoneyCost.Replace(".", ",")) /
                     orderInfo.TotalDistanceLength.Value).ToString("F2");

            orderItem.Comments = orderItem.Comments ?? "";
            if (orderItem.Comments.Length >= 50)
                orderItem.Comments_Cut = orderItem.Comments.Substring(0, 49) + "...";
            else orderItem.Comments_Cut = orderItem.Comments;

            return orderItem;
        }

        private string GetSelectedItem(int GenId)
        {
            //выбранная ячейка
            string SelectedItem = "";

            //"Д" - если услуга договорная
            if (GenId == 5)
            {
                SelectedItem = "Д";
            }
            //"Ф" - если услуга фрахт/ фиксированный

            if (GenId == 1)
            {
                SelectedItem = "Ф";
            }
            //"Т" - если услуга тариф/ не фиксированный
            if ((GenId >= 2) && (GenId <= 4))
            {
                SelectedItem = "Т";
            }

            return SelectedItem;
        }

        private string GetNameCarCostDog(int GenId, decimal? RateKm, decimal? TotalDistanceLength, decimal? RateHour,
            int? TimeRoute, decimal? RateMachineHour, int? TimeSpecialVehicles)
        {
            string NameCarCostDog = "";

            if (GenId == 5)
            {
                NameCarCostDog = "договорной";
            }
            //"Ф" - если услуга фрахт/ фиксированный
            if (GenId == 1)
            {
                NameCarCostDog = "фрахт";
            }
            //"Т" - если услуга тариф/ не фиксированный
            if ((GenId >= 2) && (GenId <= 4))
            {
                if (GenId == 3)
                {
                    if (RateKm > 0)
                        NameCarCostDog = RateKm.ToString() + " грн/км * " +
                                         (TotalDistanceLength ?? 0).ToString() + " км";
                    if (RateHour > 0)
                    {
                        decimal routeTime = (TimeRoute ?? 0); // (decimal)3600000.0

                        if (NameCarCostDog == "")
                            NameCarCostDog = RateHour.ToString() + " грн/час * " + routeTime.ToString() +
                                             " час";
                        else
                            NameCarCostDog = NameCarCostDog + " + " + RateHour.ToString() +
                                             " грн/час * " + routeTime.ToString() + " час";
                    }

                    if (RateMachineHour > 0)
                    {
                        decimal specialVehiclesTime = (TimeSpecialVehicles ?? 0) / (decimal)3600000.0;

                        if (NameCarCostDog == "")
                        {
                            NameCarCostDog = RateMachineHour.ToString() + "  грн м-час * " +
                                             specialVehiclesTime.ToString() + " м-час";
                        }
                        else
                            NameCarCostDog = string.Concat(NameCarCostDog, " + ",
                                (RateMachineHour ?? 0).ToString(), "  грн м-час * ",
                                specialVehiclesTime.ToString(), " м-час");
                    }
                }
                else if (GenId == 2)
                    NameCarCostDog = RateHour.ToString() + " грн/км * " + (TotalDistanceLength ?? 0).ToString() + " км";
            }
            return NameCarCostDog;

        }

        public OrderCompetitiveListViewModel getConcurs(long Id)
        {
            OrderCompetitiveListViewModel orderItem = new OrderCompetitiveListViewModel();
            orderItem = Mapper.Map(db.OrderCompetitiveList.AsNoTracking().FirstOrDefault(u => u.Id == Id));

            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderItem.OrderId);
            // var truckInfo = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderInfo.Id);
            var currentStep = getCurrentStatusForList(orderItem.OrderId);
            var cs = db.ContractSpecifications.AsNoTracking().FirstOrDefault(x => x.Id == orderItem.SpecificationId);

            orderItem.currentStep = currentStep;

            //если первый раз
            if (orderItem.IsChange == false)
            {
                //decimal _CarCostDog = 0;
                orderItem.IsRateKm = false;
                orderItem.RouteLength = "0";
                orderItem.IsFreight = true;
                if (cs != null)
                {
                    //если тариф грн/км, а длина пути в спецификации не указана
                    if ((orderItem.GenId == 2) && (cs.RateKm > 0) && ((cs.RouteLength ?? 0) == 0))
                    {
                        orderItem.IsRateKm = true;
                        orderItem.RateKm = (cs.RateKm ?? 00).
                            ToString(CultureInfo.CreateSpecificCulture("uk-UA"));
                    }
                }
            }

            if (orderItem.CarCostDog == null) orderItem.CarCostDog = "0";
            orderItem.DaysDelay = orderItem.DaysDelay ?? 0;
            orderItem.DaysDelaySteps = string.Concat((orderItem.DaysDelay ?? 0), " / ", orderItem.DaysDelayStep1,
                    " / ", orderItem.DaysDelayStep2);
            if (orderItem.CarCost == null) orderItem.CarCost = "0";
            if (orderItem.DelayEffect == null) orderItem.DelayEffect = "0";
            if (orderItem.PrepaymentEffect == null) orderItem.PrepaymentEffect = "0";
            if (orderItem.PrepaymentEffect2 == null) orderItem.PrepaymentEffect2 = "0";
            if (orderItem.NDS == null) orderItem.NDS = "0";

            OrderCompetitiveListViewModel calcItem = CalculateCompetitiveList(Id);

            orderItem.CarCost7 = calcItem.CarCost7;
            orderItem.DelayEffect = calcItem.DelayEffect;
            orderItem.PrepaymentEffect = calcItem.PrepaymentEffect;
            orderItem.CarCostWithMoneyCost = calcItem.CarCostWithMoneyCost;
            orderItem.AverageCost = calcItem.AverageCost;
            orderItem.Comments = calcItem.Comments;

            return orderItem;
        }

        public IQueryable<OrderCompetitiveListViewModel> getOrderCompetitiveList(string userId, long OrderId, int? tenderNumber)
        {
            if (tenderNumber != null)
            {
                List<OrderCompetitiveListViewModel> cL = new List<OrderCompetitiveListViewModel>();
                cL.AddRange(
                    db.OrderCompetitiveList.AsNoTracking()
                        .Where(osh => osh.tenderNumber == tenderNumber)
                        .Select(Mapper.Map)
                        .OrderByDescending(o => o.Id));

                foreach (var orderItem in cL)
                {
                    var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderItem.OrderId);
                    var truckInfo = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderInfo.Id);
                    var currentStep = getCurrentStatusForList(OrderId);

                    orderItem.currentStep = currentStep;

                    int TypeVehicleId = 0;
                    string VehicleTypeName = "";
                    bool isTruck = false;

                    if ((orderInfo.OrderType == 4) || (orderInfo.OrderType == 5) || (orderInfo.OrderType == 7))
                    {
                        TypeVehicleId = truckInfo.VehicleTypeId ?? 0;
                        VehicleTypeName = db.OrderVehicleTypes.FirstOrDefault(u => u.Id == TypeVehicleId)?.VehicleTypeName;
                        isTruck = true;
                    }

                    orderItem.isTruck = isTruck;
                    orderItem.TypeVehicleId = TypeVehicleId;
                    orderItem.VehicleTypeName = VehicleTypeName;

                    if (orderItem.CarCostDog == null) orderItem.CarCostDog = "0";
                    orderItem.DaysDelay = orderItem.DaysDelay ?? 0;
                    orderItem.DaysDelaySteps = string.Concat((orderItem.DaysDelay ?? 0), " / ", orderItem.DaysDelayStep1,
                        " / ", orderItem.DaysDelayStep2);
                    if (orderItem.CarCost == null) orderItem.CarCost = "0";
                    if (orderItem.DelayEffect == null) orderItem.DelayEffect = "0";
                    if (orderItem.PrepaymentEffect == null) orderItem.PrepaymentEffect = "0";
                    if (orderItem.PrepaymentEffect2 == null) orderItem.PrepaymentEffect2 = "0";

                    OrderCompetitiveListViewModel calcItem = CalculateCompetitiveList(orderItem.Id);

                    orderItem.CarCost7 = calcItem.CarCost7;
                    orderItem.DelayEffect = calcItem.DelayEffect;
                    orderItem.PrepaymentEffect = calcItem.PrepaymentEffect;
                    orderItem.CarCostWithMoneyCost = calcItem.CarCostWithMoneyCost;
                    orderItem.AverageCost = calcItem.AverageCost;
                    orderItem.Comments = calcItem.Comments;
                    orderItem.Comments_Cut = calcItem.Comments_Cut;

                    if (orderItem.CarCostWithMoneyCost == null) orderItem.CarCostWithMoneyCost = "0";

                    //название услуги
                    var cs = db.ContractSpecifications.AsNoTracking().FirstOrDefault(x => x.Id == orderItem.SpecificationId);
                    if (cs != null)
                    {
                        var sn = db.SpecificationNames.AsNoTracking().FirstOrDefault(x => x.Id == cs.NameId);

                        orderItem.NameSpecification = "";
                        if (sn.SpecName != null)
                            orderItem.NameSpecification = sn.SpecName;

                        //выбранная ячейка
                        orderItem.SelectedItem = "";

                        //если запись выбрана то показывать следующие мнемоники при условии что в 4 столбце количество автомобилей больше 0                
                        if ((orderItem.IsSelectedId) && (orderItem.CarsAccepted > 0))
                        {
                            orderItem.SelectedItem = GetSelectedItem(orderItem.GenId);
                        }

                        //как рассчиталась сумма
                        orderItem.NameCarCostDog = GetNameCarCostDog(orderItem.GenId, cs.RateKm, orderInfo.TotalDistanceLength,
                            cs.RateHour, orderInfo.TimeRoute, cs.RateMachineHour, orderInfo.TimeSpecialVehicles);

                        //разукрашка, если нулевые значения в тарифе (т.е. комментарий не пустой)
                        string Comments = GetZeroTarif(orderItem.GenId, cs.RateKm, orderInfo.TotalDistanceLength,
                            cs.RateHour, orderInfo.TimeRoute, cs.RateMachineHour, orderInfo.TimeSpecialVehicles);

                        orderItem.IsZeroTarif = !(string.IsNullOrEmpty(Comments));
                    }
                    else
                    {
                        var sn = db.RegisterTenderContragents.Where(x => x.tenderNumber == tenderNumber).Where(x => x.ContragentName.Contains(orderItem.ExpeditorName)).FirstOrDefault();
                        if (sn != null)
                        {
                            orderItem.NameSpecification = "";
                            if (sn.nmcName != null)
                                orderItem.NameSpecification = sn.nmcName;

                            //выбранная ячейка
                            orderItem.SelectedItem = "";

                            //если запись выбрана то показывать следующие мнемоники при условии что в 4 столбце количество автомобилей больше 0                
                            if ((orderItem.IsSelectedId) && (orderItem.CarsAccepted > 0))
                            {
                                orderItem.SelectedItem = GetSelectedItem(orderItem.GenId);
                            }
                        }

                    }
                    var sn_ = db.RegisterTenderContragents.Where(x => x.tenderNumber == tenderNumber).Where(x => x.ContragentName.Contains(orderItem.ExpeditorName)).FirstOrDefault();
                    if (sn_ != null)
                    {

                        orderItem.DaysDelay = sn_.PaymentDelay;
                        orderItem.ExpeditorName = sn_.ContragentName;
                        orderItem.NameSpecification = sn_.nmcName;
                    }


                }

                var result =
                    cL.OrderBy(o => Convert.ToDecimal(o.CarCostDog.Replace(".", ","))).ThenBy(o => o.GenId).ToList();

                return result.AsQueryable();

            }
            else
            {
                return getOrderCompetitiveList(userId, OrderId);
            }
        }

        public IQueryable<OrderCompetitiveListViewModel> getOrderCompetitiveList(string userId, long OrderId)
        {
            List<OrderCompetitiveListViewModel> cL = new List<OrderCompetitiveListViewModel>();
            cL.AddRange(
                db.OrderCompetitiveList.AsNoTracking()
                    .Where(osh => osh.OrderId == OrderId)
                    .Select(Mapper.Map)
                    .OrderByDescending(o => o.Id));

            foreach (var orderItem in cL)
            {
                var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderItem.OrderId);
                var truckInfo = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderInfo.Id);
                var currentStep = getCurrentStatusForList(OrderId);

                orderItem.currentStep = currentStep;

                int TypeVehicleId = 0;
                string VehicleTypeName = "";
                bool isTruck = false;

                if ((orderInfo.OrderType == 4) || (orderInfo.OrderType == 5) || (orderInfo.OrderType == 7))
                {
                    TypeVehicleId = truckInfo.VehicleTypeId ?? 0;
                    VehicleTypeName = db.OrderVehicleTypes.FirstOrDefault(u => u.Id == TypeVehicleId)?.VehicleTypeName;
                    isTruck = true;
                }

                orderItem.isTruck = isTruck;
                orderItem.TypeVehicleId = TypeVehicleId;
                orderItem.VehicleTypeName = VehicleTypeName;

                if (orderItem.CarCostDog == null) orderItem.CarCostDog = "0";
                orderItem.DaysDelay = orderItem.DaysDelay ?? 0;
                orderItem.DaysDelaySteps = string.Concat((orderItem.DaysDelay ?? 0), " / ", orderItem.DaysDelayStep1,
                    " / ", orderItem.DaysDelayStep2);
                if (orderItem.CarCost == null) orderItem.CarCost = "0";
                if (orderItem.DelayEffect == null) orderItem.DelayEffect = "0";
                if (orderItem.PrepaymentEffect == null) orderItem.PrepaymentEffect = "0";
                if (orderItem.PrepaymentEffect2 == null) orderItem.PrepaymentEffect2 = "0";

                OrderCompetitiveListViewModel calcItem = CalculateCompetitiveList(orderItem.Id);

                orderItem.CarCost7 = calcItem.CarCost7;
                orderItem.DelayEffect = calcItem.DelayEffect;
                orderItem.PrepaymentEffect = calcItem.PrepaymentEffect;
                orderItem.CarCostWithMoneyCost = calcItem.CarCostWithMoneyCost;
                orderItem.AverageCost = calcItem.AverageCost;
                orderItem.Comments = calcItem.Comments;
                orderItem.Comments_Cut = calcItem.Comments_Cut;

                if (orderItem.CarCostWithMoneyCost == null) orderItem.CarCostWithMoneyCost = "0";

                //название услуги
                var cs = db.ContractSpecifications.AsNoTracking().FirstOrDefault(x => x.Id == orderItem.SpecificationId);
                var sn = db.SpecificationNames.AsNoTracking().FirstOrDefault(x => x.Id == cs.NameId);

                orderItem.NameSpecification = "";
                if (sn.SpecName != null)
                    orderItem.NameSpecification = sn.SpecName;

                //выбранная ячейка
                orderItem.SelectedItem = "";

                //если запись выбрана то показывать следующие мнемоники при условии что в 4 столбце количество автомобилей больше 0                
                if ((orderItem.IsSelectedId) && (orderItem.CarsAccepted > 0))
                {
                    orderItem.SelectedItem = GetSelectedItem(orderItem.GenId);
                }

                //как рассчиталась сумма
                orderItem.NameCarCostDog = GetNameCarCostDog(orderItem.GenId, cs.RateKm, orderInfo.TotalDistanceLength,
                    cs.RateHour, orderInfo.TimeRoute, cs.RateMachineHour, orderInfo.TimeSpecialVehicles);

                //разукрашка, если нулевые значения в тарифе (т.е. комментарий не пустой)
                string Comments = GetZeroTarif(orderItem.GenId, cs.RateKm, orderInfo.TotalDistanceLength,
                    cs.RateHour, orderInfo.TimeRoute, cs.RateMachineHour, orderInfo.TimeSpecialVehicles);

                orderItem.IsZeroTarif = !(string.IsNullOrEmpty(Comments));

            }

            var result =
                cL.OrderBy(o => Convert.ToDecimal(o.CarCostDog.Replace(".", ","))).ThenBy(o => o.GenId).ToList();

            return result.AsQueryable();
        }

        public List<SpecificationTypesViewModel> GetSpecificationTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetSpecificationTypesBySearchString(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public int GetSpecificationTypesCount(string searchTerm)
        {
            return GetSpecificationTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<SpecificationTypesViewModel> GetSpecificationTypesBySearchString(string searchTerm)
        {
            return
                db.SpecificationTypes
                    .AsNoTracking()
                    .Where(
                        s =>
                            ((s.SpecificationType.Contains(searchTerm) || s.SpecificationType.StartsWith(searchTerm) ||
                              s.SpecificationType.EndsWith(searchTerm))))
                    .Select(Mapper.Map)
                    .OrderBy(o => o.Id)
                    .AsQueryable();
        }

        public DateTime getConcursHistoryHeader(long OrderId)
        {
            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == OrderId);
            return orderInfo.OrderDate;
            //return string.Concat(orderInfo.OrderDate.AddMonths(-3).ToString("dd.MM.yyyy"), " по ",
            //   orderInfo.OrderDate.ToString("dd.MM.yyyy"));
        }

        public IQueryable<OrderCompetitiveListViewModel> getConcursHistory(long Id, bool ShowAll, long OrderId, DateTime FilterOrderDateBeg,
                                                             DateTime FilterOrderDateEnd)
        {
            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == OrderId);
            // var headerInfo = getCompetitiveListInfo(OrderId);
            int? SpecificationId = null;
            int? RouteTypeId = null;
            int? IntervalTypeId = null;

            // var currentStep = getCurrentStatusForList(OrderId);
            // double discountRateInfo = GetDiscountRateInfo(orderInfo.OrderDate);

            if (!(ShowAll))
            {
                var concurs = db.OrderCompetitiveList.FirstOrDefault(o => o.Id == Id);
                var cs = db.ContractSpecifications.AsNoTracking().FirstOrDefault(x => x.Id == concurs.SpecificationId);
                SpecificationId = concurs.SpecificationId;
                RouteTypeId = cs.RouteTypeId;
                IntervalTypeId = cs.IntervalTypeId;
            }

            //DateTime EndDate = orderInfo.OrderDate;
            var SpecItems =
                    db.GetConcursHistory(/*orderInfo.OrderDate.AddMonths(-3), EndDate,*/FilterOrderDateBeg, FilterOrderDateEnd, SpecificationId, RouteTypeId,
                        IntervalTypeId, ShowAll).ToList();

            string DelayEffect, PrepaymentEffect, CarCostWithMoneyCost, CarCost7Str;

            List<OrderCompetitiveListViewModel> cL = new List<OrderCompetitiveListViewModel>();
            int CntNameSpecification = (from x in SpecItems
                                        select x.NameId).Distinct().Count();

            foreach (var spec in SpecItems)
            {
                OrderCompetitiveListViewModel c = new OrderCompetitiveListViewModel();

                var concursStep = db.OrderCompetitiveList.Select(Mapper.Map).FirstOrDefault(o => o.Id == spec.Id);
                c.CntNameSpecification = CntNameSpecification;
                c.ExpeditorName = spec.ExpeditorName;
                //Акцептованная цена - это приведенная Стомость одного автомобиля рассчитанная - при условии (Финальный этап и Столбец4"Акцептовано тр-ных единиц">0) и 
                //(если(Столбец8>0;Столбец8; иначе если(Столбец7>0;Столбец7;  иначе если(Столбец6>0;Столбец6; 0) 
                if (spec.CarCost > 0)
                    c.AcceptPrice = spec.CarCost;
                else if (spec.CarCost7 > 0)
                    c.AcceptPrice = spec.CarCost7 ?? 0;
                else if (spec.CarCostDog > 0)
                    c.AcceptPrice = spec.CarCostDog;
                else c.AcceptPrice = 0;
                //"Отсрочка" - логика как в расчете Акцептованной цены см.выше
                if (spec.DaysDelayStep2 > 0)
                    c.AcceptDaysDelay = spec.DaysDelayStep2 ?? 0;
                else if (spec.DaysDelayStep1 > 0)
                    c.AcceptDaysDelay = spec.DaysDelayStep1 ?? 0;
                else if (spec.DaysDelay > 0)
                    c.AcceptDaysDelay = spec.DaysDelay ?? 0;
                else c.AcceptDaysDelay = 0;

                // CarCost7Str = concursStep.CarCost7;

                if (concursStep.CarCost == null) concursStep.CarCost = "0";
                if (concursStep.CarCostDog == null) concursStep.CarCostDog = "0";
                CarCost7Str = concursStep.CarCost7 ?? "0";

                GetCalculateFields(orderInfo.OrderDate, spec.OrderId,
                    concursStep.Prepayment2, concursStep.Prepayment, concursStep.DaysDelay,
                    concursStep.DaysDelayStep1, concursStep.DaysDelayStep2,
                    concursStep.CarCostDog, concursStep.CarsOffered, concursStep.CarCost, ref CarCost7Str,
                    out DelayEffect, out PrepaymentEffect, out CarCostWithMoneyCost);

                c.HistoryOrderId = spec.OrderId;
                var orderItemInfo = Mapper.Map(db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == spec.OrderId));

                c.TotalDistanceLenght = Convert.ToDecimal(orderItemInfo.TotalDistanceLenght.Replace(".", ","));

                //c.OrderDate = orderItemInfo.OrderDate;            

                int tripType = 0;
                string FromInfo = "";
                string ToInfo = "";
                if ((orderItemInfo.OrderType == 1) || (orderItemInfo.OrderType == 3) || (orderItemInfo.OrderType == 6))
                {
                    var passInfo = db.OrdersPassengerTransport.FirstOrDefault(or => or.OrderId == c.HistoryOrderId);
                    if (passInfo != null)
                    {
                        c.FromDate = passInfo.StartDateTimeOfTrip.ToString("dd.MM.yyyy");
                        c.ToDate = passInfo.FinishDateTimeOfTrip.ToString("dd.MM.yyyy");

                        tripType = passInfo.TripType ?? 0;

                        var fromCountryName = db.Countries.FirstOrDefault(u => u.Сode == passInfo.FromCountry)?.Name;
                        FromInfo = getRouteInfo(tripType, fromCountryName, passInfo.FromCity, passInfo.AdressFrom);

                        var consigneeCountryName = db.Countries.FirstOrDefault(u => u.Сode == passInfo.ToCountry)?.Name;
                        ToInfo = getRouteInfo(tripType, consigneeCountryName, passInfo.ToCity, passInfo.AdressTo);
                    }
                }

                if ((orderItemInfo.OrderType == 4) || (orderItemInfo.OrderType == 5) || (orderItemInfo.OrderType == 7))
                {
                    var truckInfo = db.OrderTruckTransport.FirstOrDefault(or => or.OrderId == c.HistoryOrderId);
                    if (truckInfo != null)
                    {
                        c.FromDate = truckInfo.FromShipperDatetime.Value.ToString("dd.MM.yyyy");
                        c.ToDate = truckInfo.ToConsigneeDatetime.Value.ToString("dd.MM.yyyy");

                        tripType = truckInfo.TripType ?? 0;
                        var shipperCountryName = db.Countries.FirstOrDefault(u => u.Сode == truckInfo.ShipperCountryId)?.Name;
                        FromInfo = getRouteInfo(tripType, shipperCountryName, truckInfo.ShipperCity, truckInfo.ShipperAdress);

                        var consigneeCountryName = db.Countries.FirstOrDefault(u => u.Сode == truckInfo.ConsigneeCountryId)?.Name;
                        ToInfo = getRouteInfo(tripType, consigneeCountryName, truckInfo.ConsigneeCity, truckInfo.ConsigneeAdress);
                    }
                }

                c.Route = string.Concat(FromInfo, " - ", ToInfo);

                // c.CarCostWithMoneyCost = CarCostWithMoneyCost;               
                c.CarCostWithMoneyCost = (c.AcceptPrice - Convert.ToDecimal(DelayEffect.Replace(".", ",")) -
                                              Convert.ToDecimal(PrepaymentEffect.Replace(".", ","))).ToString("F2");

                c.NameSpecification = spec.SpecName;

                cL.Add(c);
            }

            return cL.OrderBy(x => x.AcceptPrice).AsQueryable();
        }

        //ставки дисконтирования
        public IQueryable<ConcursDiscountRateModel> GetConcursDiscountRate()
        {
            return db.ConcursDiscountRate
                .AsNoTracking()
                .Select(Mapper.Map)
                .OrderByDescending(o => o.Id)
                .AsQueryable();
        }

        public ConcursDiscountRateModel GetConcursDiscountRate(long Id)
        {
            return Mapper.Map(db.ConcursDiscountRate.AsNoTracking().FirstOrDefault(c => c.Id == Id));
        }

        public void UpdateDiscountRate(ConcursDiscountRateModel model)
        {
            var discountRateInfo = db.ConcursDiscountRate.FirstOrDefault(c => c.Id == model.Id);
            if (discountRateInfo == null) return;

            discountRateInfo.DiscountRate = Convert.ToDecimal(model.DiscountRate.Replace(".", ","));
            discountRateInfo.DateBeg = DateTimeConvertClass.getDateTime(model.DateBegRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.DateBegRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.DateBegRaw));
            discountRateInfo.DateEnd = DateTimeConvertClass.getDateTime(model.DateEndRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.DateEndRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.DateEndRaw));

            db.SaveChanges();
        }

        public bool DeleteDiscountRate(long Id)
        {
            var discountRateInfo = db.ConcursDiscountRate.FirstOrDefault(o => o.Id == Id);

            if (discountRateInfo != null)
            {
                db.ConcursDiscountRate.Remove(discountRateInfo);
                db.SaveChanges();
            }
            return true;

        }

        public void AddDiscountRate(ConcursDiscountRateModel model)
        {

            var discountRateInfo = new ConcursDiscountRate()
            {
                DiscountRate = Convert.ToDecimal(model.DiscountRate.Replace(".", ",")),
                DateBeg = DateTimeConvertClass.getDateTime(model.DateBegRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.DateBegRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.DateBegRaw)),
                DateEnd = DateTimeConvertClass.getDateTime(model.DateEndRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.DateEndRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.DateEndRaw))
            };

            db.ConcursDiscountRate.Add(discountRateInfo);
            db.SaveChanges();
        }

        public void CloneConcurs(long Id, string userId)
        {
            var cs = db.OrderCompetitiveList.AsNoTracking().FirstOrDefault(x => x.Id == Id);

            var concurs = new OrderCompetitiveList()
            {
                OrderId = cs.OrderId,
                CarsAccepted = cs.CarsAccepted,
                NDS = cs.NDS,
                CarCostDog = cs.CarCostDog,
                CarCost = cs.CarCost,
                DaysDelay = cs.DaysDelay,
                Prepayment = cs.Prepayment,
                PrepaymentEffect = cs.PrepaymentEffect,
                Comments = cs.Comments,
                ExpeditorName = cs.ExpeditorName,
                CarryCapacity = cs.CarryCapacity,
                IsSelectedId = cs.IsSelectedId,
                Prepayment2 = cs.Prepayment2,
                PrepaymentEffect2 = cs.PrepaymentEffect2,
                SpecificationId = cs.SpecificationId,
                IsChange = cs.IsChange,
                GenId = cs.GenId,
                CarsOffered = cs.CarsOffered,
                CarCost7 = cs.CarCost7,
                DaysDelayStep1 = cs.DaysDelayStep1,
                DaysDelayStep2 = cs.DaysDelayStep2
            };

            db.OrderCompetitiveList.Add(concurs);

            db.SaveChanges();

        }


    }
}