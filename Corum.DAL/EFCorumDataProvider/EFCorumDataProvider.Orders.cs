using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Corum.Models;
using Corum.Common;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Orders;
using Corum.DAL.Mappings;
using Corum.DAL.Entity;
using System.Data.Entity.Validation;
using Corum.Models.ViewModels.Cars;
using System.Globalization;
using GoogleMaps.LocationServices;
using Corum.Models.ViewModels.Customers;

namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {
        public IQueryable<ProjectTypeViewModel> getProjectTYpes()
        {
            return db.ProjectTypes
                           .AsNoTracking()
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();


        }

        public void AddDefaultObservers(long orderId, int orderTypeId)
        {
            //Get users from  
            var ordertypeinfo = db.OrderTypesBase.FirstOrDefault(x => x.Id == orderTypeId);
            if (ordertypeinfo != null)
            {
                var def_observers = db.AspNetUsers.Where(x => x.AspNetRoles.Any(y => y.Id == ordertypeinfo.UserRoleIdForExecuterData)).Distinct().ToList();
                foreach (var observer in def_observers)
                {
                    this.NewObserver(new OrderObserverViewModel()
                    {
                        OrderId = orderId,
                        observerId = observer.Id
                    });
                }

            }

        }


        public bool IsUserOrderLPRPerson(string userId, int orderTypeId)
        {
            var result = (from BP in db.OrderPipelineSteps
                          join UR in db.AspNetRoles on BP.AccessRoleId equals UR.Id
                          where BP.OrderTypeId == orderTypeId
                             && UR.AspNetUsers.Any(x => x.Id == userId)
                             && BP.StartDateForClient == true
                          select UR).Count();

            return result > 0;
        }


        public IQueryable<OrderUsedCarViewModel> getOrderCarsInfo(long OrderId)
        {
            return db.OrderUsedCars
                           .AsNoTracking()
                            .Where(x => x.OrderId == OrderId)
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();
        }

        public OrderUsedCarViewModel getUsedCarInfo(int Id)
        {
            return Mapper.Map(db.OrderUsedCars.AsNoTracking().FirstOrDefault(c => c.Id == Id));
        }

        public IQueryable<OrderNotificationViewModel> getNotifications(long OrderId)
        {
            return db.OrderNotifications
                           .AsNoTracking()
                            .Where(x => x.OrderId == OrderId)
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();
        }


        public IQueryable<OrderNotificationTypesViewModel> getNotificationTypes()
        {
            return db.OrderNotificationTypes
                           .AsNoTracking()
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();

        }


        public string GetPassInfo(long orderId, int orderType)
        {

            switch (orderType)
            {
                case 1:
                case 3:
                case 6:
                    var orderInfo1 = db.OrdersPassengerTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo1 != null)
                    {
                        return orderInfo1.PassInfo;
                    }

                    break;

                case 4:
                case 5:
                case 7:

                    var orderInfo2 = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo2 != null)
                    {
                        return orderInfo2.TruckDescription;
                    }

                    break;

                default:
                    return string.Empty;
            }



            return string.Empty;
        }

        public string GetFromInfo(long orderId, int orderType)
        {
            switch (orderType)
            {
                case 1:
                case 3:
                case 6:
                    var orderInfo1 = db.OrdersPassengerTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo1 != null)
                    {
                        return string.Concat(orderInfo1.Countries == null ? null : orderInfo1.Countries.Name, ", ", orderInfo1.FromCity, ", ", orderInfo1.AdressFrom, "<br>", "<hr>", orderInfo1.OrgFrom);
                    }

                    break;

                case 4:
                case 5:
                case 7:

                    var orderInfo2 = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo2 != null)
                    {
                        return string.Concat(orderInfo2.Countries1 == null ? null : orderInfo2.Countries1.Name, ", ", orderInfo2.ShipperCity, ", ", orderInfo2.ShipperAdress, "<br>", "<hr>", orderInfo2.Shipper);
                    }

                    break;

                default:
                    return string.Empty;
            }

            return string.Empty;

        }

        public string GetToInfo(long orderId, int orderType)
        {
            switch (orderType)
            {
                case 1:
                case 3:
                case 6:
                    var orderInfo1 = db.OrdersPassengerTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo1 != null)
                    {
                        return string.Concat(orderInfo1.Countries1 == null ? null : orderInfo1.Countries1.Name, ", ", orderInfo1.ToCity, ", ", orderInfo1.AdressTo, "<br>", "<hr>", orderInfo1.OrgTo);
                    }

                    break;

                case 4:
                case 5:
                case 7:

                    var orderInfo2 = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo2 != null)
                    {
                        return string.Concat(orderInfo2.Countries == null ? null : orderInfo2.Countries.Name, ", ", orderInfo2.ConsigneeCity, ", ", orderInfo2.ConsigneeAdress, "<br>", "<hr>", orderInfo2.Consignee);
                    }

                    break;

                default:
                    return string.Empty;
            }

            return string.Empty;
        }

        public string GetAutoCount(long orderId, int orderType)
        {
            switch (orderType)
            {
                case 1:
                case 3:
                case 6:
                    var orderInfo2 = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderId);
                    return string.Concat("план:", 1.ToString(), " ", "факт:", orderInfo2.OrderUsedCars.Count().ToString());
                case 4:
                case 5:
                case 7:

                    var orderInfo1 = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderId);
                    return string.Concat("план:", orderInfo1.CarNumber.Value.ToString(), " ", "факт:", orderInfo1.OrderUsedCars.Count().ToString());

                default:
                    return string.Empty;
            }
        }

        public DateTime GetStartDate(long orderId, int orderType)
        {
            switch (orderType)
            {
                case 1:
                case 3:
                case 6:
                    var orderInfo1 = db.OrdersPassengerTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo1 != null)
                    {
                        return orderInfo1.StartDateTimeOfTrip;

                    }

                    break;

                case 4:
                case 5:
                case 7:

                    var orderInfo2 = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo2 != null)
                    {
                        return orderInfo2.FromShipperDatetime ?? DateTime.MinValue;

                    }

                    break;

                default:
                    return DateTime.MinValue;
            }




            return DateTime.MinValue;
        }

        public DateTime GetFinishDate(long orderId, int orderType)
        {
            switch (orderType)
            {
                case 1:
                case 3:
                case 6:
                    var orderInfo1 = db.OrdersPassengerTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo1 != null)
                    {
                        return !orderInfo1.NeedReturn ? orderInfo1.FinishDateTimeOfTrip : orderInfo1.ReturnFinishDateTimeOfTrip.Value;

                    }

                    break;

                case 4:
                case 5:
                case 7:

                    var orderInfo2 = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo2 != null)
                    {
                        return orderInfo2.ToConsigneeDatetime ?? DateTime.MaxValue;

                    }

                    break;

                default:
                    return DateTime.MaxValue;
            }




            return DateTime.MaxValue;
        }

        public string GetAcceptDate(long orderId)
        {
            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderId);

            if (orderInfo != null)
            {
                var stepInfo = db.OrderPipelineSteps.FirstOrDefault(x => x.OrderTypeId == orderInfo.OrderType && x.StartDateForClient == true);
                if (stepInfo != null)
                {
                    var date = db.OrderStatusHistory.FirstOrDefault(x => x.OldStatus == stepInfo.FromStatus &&
                                     x.OrderId == orderId) == null ? null : db.OrderStatusHistory.FirstOrDefault(x => x.OldStatus == stepInfo.FromStatus &&
                                     x.OrderId == orderId).ChangeDateTime.ToString("dd.MM.yyyy hh:mm");

                    return date;
                }
            }

            return string.Empty;
        }
        public string GetStartExecuteDate(long orderId)
        {

            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderId);

            if (orderInfo != null)
            {
                var stepInfo = db.OrderPipelineSteps.FirstOrDefault(x => x.OrderTypeId == orderInfo.OrderType && x.StartDateForExecuter == true);
                if (stepInfo != null)
                {
                    return db.OrderStatusHistory.FirstOrDefault(x => x.OldStatus == stepInfo.FromStatus &&
                                     x.OrderId == orderId) == null ? null : db.OrderStatusHistory.FirstOrDefault(x => x.OldStatus == stepInfo.FromStatus &&
                                     x.OrderId == orderId).ChangeDateTime.ToString("dd.MM.yyyy hh:mm");
                }
            }

            return string.Empty;
        }


        public IQueryable<OrderClientBalanceKeeperViewModel> getBalanceKeepers(string userId)
        {
            return db.BalanceKeepers
                           .AsNoTracking()
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();
        }

        public IQueryable<OrderClientCFOViewModel> getCenters(string userId)
        {
            return db.Centers
                           .AsNoTracking()
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();
        }

        public IQueryable<OrderPipelineStepViewModel> getPipelineSteps(string userId, int OrderTypeId)
        {

            var query = (from pl in db.OrderPipelineSteps
                         join st in db.OrderStatuses on pl.FromStatus equals st.Id
                         join os in db.OrderStatuses on pl.ToStatus equals os.Id
                         where pl.OrderTypeId == OrderTypeId
                         select pl).Distinct();

            return query.Select(Mapper.Map).OrderBy(o => o.Id).AsQueryable();

        }

        public IQueryable<OrderObserverViewModel> getOrderObservers(long Id)
        {
            return db.OrderObservers
                           .AsNoTracking()
                            .Where(osh => osh.OrderId == Id)
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();
        }


        public IQueryable<OrderDocTypeViewModel> getAvailabbleDocTypes(string userId)
        {
            return db.OrdersDocTypes
                            .AsNoTracking()
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();
        }

        public IQueryable<OrderStatusViewModel> getAvailableOrderStatusesInPipeline(string userId, bool isAdmin, int currentStatus, int orderTypeId)
        {
            if (isAdmin)
            {
                var query = (from st in db.OrderStatuses
                             join pl in db.OrderPipelineSteps on st.Id equals pl.ToStatus
                             where pl.FromStatus == currentStatus
                                && pl.OrderTypeId == orderTypeId
                             select st).Distinct();

                return query.Select(Mapper.Map).OrderBy(o => o.Id).AsQueryable();
            }
            else
            {
                var query = (from st in db.OrderStatuses
                             join pl in db.OrderPipelineSteps on st.Id equals pl.ToStatus
                             join ro in db.AspNetRoles on pl.AccessRoleId equals ro.Id
                             where pl.FromStatus == currentStatus
                                && pl.OrderTypeId == orderTypeId
                                && (ro.AspNetUsers.Where(u => u.Id == userId).Count() > 0)
                             select st).Distinct();

                return query.Select(Mapper.Map).OrderBy(o => o.Id).AsQueryable();
            }
        }

        public IQueryable<OrderStatusViewModel> getPreviousOrderStatusesInPipeline(string userId, bool isAdmin, int currentStatus, int orderTypeId, long Id)
        {
            if (isAdmin)
            {
                var query = (from st in db.OrderStatuses
                             join pl in db.OrderPipelineSteps on st.Id equals pl.FromStatus
                             join sh in db.OrderStatusHistory on pl.FromStatus equals sh.OldStatus
                             where pl.ToStatus == currentStatus
                                && pl.OrderTypeId == orderTypeId
                                && sh.OrderId == Id
                             select st).Distinct();

                return query.Select(Mapper.Map).OrderBy(o => o.Id).AsQueryable();
            }
            else
            {
                var query = (from st in db.OrderStatuses
                             join pl in db.OrderPipelineSteps on st.Id equals pl.FromStatus
                             join sh in db.OrderStatusHistory on pl.FromStatus equals sh.OldStatus
                             join ro in db.AspNetRoles on pl.AccessRoleId equals ro.Id
                             where pl.ToStatus == currentStatus
                                && pl.OrderTypeId == orderTypeId
                                && sh.OrderId == Id
                                && (ro.AspNetUsers.Where(u => u.Id == userId).Count() > 0)
                             select st).Distinct();

                return query.Select(Mapper.Map).OrderBy(o => o.Id).AsQueryable();
            }
        }

        public IQueryable<OrderStatusViewModel> getAvailableOrderStatuses(string userId)
        {
            return db.OrderStatuses
                            .AsNoTracking()
                              .Select(Mapper.Map)
                               .OrderByDescending(o => o.Id)
                                .AsQueryable();
        }

        public IQueryable<OrderTypeViewModel> getAvailableOrderTypes(string userId, bool? IsTransport)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return db.OrderTypesBase
                            .AsNoTracking()
                             .Select(Mapper.Map)
                              .Where(x => ((IsTransport == null) || ((IsTransport != null) && (x.IsTransportType == IsTransport))))
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();

            }
            else
            {
                return (from OT in db.OrderTypesBase
                        join UR in db.AspNetRoles on OT.TypeAccessGroupId equals UR.Id
                        where UR.AspNetUsers.Any(x => x.Id == userId)
                           && ((IsTransport == null) || ((IsTransport != null) && (OT.IsTransportType == IsTransport)))
                        select OT).Select(Mapper.Map)
                                  .OrderByDescending(o => o.Id)
                                   .AsQueryable();
            }
        }

        public IQueryable<OrderClientsViewModel> getClients(string userId, string searchString = "")
        {
            var result = db.OrderClients
                            .AsNoTracking()
                            .Where(s => (((s.ClientName.Contains(searchString) || s.ClientName.StartsWith(searchString) || s.ClientName.EndsWith(searchString)))))
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();

            return (result != null) ? result : (new List<OrderClientsViewModel>()).AsQueryable();
        }



        public IQueryable<OrderClientsViewModel> getClientsInPipeline(string userId, string searchString = "")
        {
            var query = (from cl in db.OrderClients
                         join ro in db.AspNetRoles on cl.AccessRoleId equals ro.Id
                         where (ro.AspNetUsers.Where(u => u.Id == userId).Count() > 0)
                         select cl);

            return query
                .Where(s => (((s.ClientName.Contains(searchString) || s.ClientName.StartsWith(searchString) || s.ClientName.EndsWith(searchString)))))
                .Select(Mapper.Map)
                .OrderByDescending(o => o.Id)
                .AsQueryable();
        }

        public IQueryable<OrderBaseViewModel> getOrders(bool IsTransport,
                                                        string userId,
                                                        bool isAdmin,
                                                        bool UseStatusesFilter,
                                                        string FilterStatusId,
                                                        bool UseOrderCreatorFilter,
                                                        string FilterOrderCreatorId,
                                                        bool UseOrderTypeFilter,
                                                        string FilterOrderTypeId,
                                                        bool UseOrderClientFilter,
                                                        string FilterOrderClientId,
                                                        bool UseOrderPriorityFilter,
                                                        int FilterOrderPriority,
                                                        bool UseOrderDateFilter,
                                                        DateTime FilterOrderDateBeg,
                                                        DateTime FilterOrderDateEnd,
                                                        bool UseOrderExDateFilter,
                                                        DateTime FilterOrderExDateBeg,
                                                        DateTime FilterOrderExDateEnd,
                                                        bool UseOrderEndDateFilter,
                                                        DateTime FilterOrderEndDateBeg,
                                                        DateTime FilterOrderEndDateEnd,
                                                        string FilterOrderExecuterId,
                                                        bool UseOrderExecuterFilter,
                                                        bool UseFinalStatusFilter,
                                                        bool FilterFinalStatus,
                                                        bool UseOrderProjectFilter,
                                                        string FilterOrderProjectId,
                                                        bool UseOrderPayerFilter,
                                                        string FilterOrderPayerId,
                                                        string FilterOrderOrgFromId,
                                                        bool UseOrderOrgFromFilter,
                                                        string FilterOrderOrgToId,
                                                        bool UseOrderOrgToFilter)
        {


            var _FilterStatusId = string.IsNullOrEmpty(FilterStatusId) ? "0" : FilterStatusId;
            var _FilterOrderTypeId = string.IsNullOrEmpty(FilterOrderTypeId) ? "0" : FilterOrderTypeId;
            var _FilterOrderClientId = string.IsNullOrEmpty(FilterOrderClientId) ? "0" : FilterOrderClientId;
            var _FilterOrderProjectId = string.IsNullOrEmpty(FilterOrderProjectId) ? "0" : FilterOrderProjectId;
            var _FilterOrderPayerId = string.IsNullOrEmpty(FilterOrderPayerId) ? "0" : FilterOrderPayerId;
            var _FilterOrderOrgFromId = string.IsNullOrEmpty(FilterOrderOrgFromId) ? "" : FilterOrderOrgFromId;
            var _FilterOrderOrgToId = string.IsNullOrEmpty(FilterOrderOrgToId) ? "" : FilterOrderOrgToId;

            if (!string.IsNullOrEmpty(FilterOrderOrgFromId))
            {
                string[] idList = FilterOrderOrgFromId.Split(new char[] { ',' });
                string FilterOrderOrgFromName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderOrgFromName.Length > 0)
                    {
                        FilterOrderOrgFromName += ",";
                    }

                    var OrgFromName = GetOrganization(Convert.ToInt32(i));
                    FilterOrderOrgFromName = string.Concat(FilterOrderOrgFromName, string.Concat(OrgFromName?.Name));
                }
                _FilterOrderOrgFromId = FilterOrderOrgFromName;
            }

            if (!string.IsNullOrEmpty(FilterOrderOrgToId))
            {
                string[] idList = FilterOrderOrgToId.Split(new char[] { ',' });
                string FilterOrderOrgToName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderOrgToName.Length > 0)
                    {
                        FilterOrderOrgToName += ",";
                    }

                    var OrgToName = GetOrganization(Convert.ToInt32(i));
                    FilterOrderOrgToName = string.Concat(FilterOrderOrgToName, string.Concat(OrgToName?.Name));
                }
                _FilterOrderOrgToId = FilterOrderOrgToName;
            }

            var query = db.GetOrdersPipelineV3(userId,
                                             isAdmin,
                                             IsTransport,
                                             UseStatusesFilter,
                                             UseOrderCreatorFilter,
                                             UseOrderExecuterFilter,
                                             UseOrderTypeFilter,
                                             UseOrderClientFilter,
                                             UseOrderPriorityFilter,
                                             UseOrderDateFilter,
                                             UseOrderExDateFilter,
                                             UseOrderEndDateFilter,
                                             UseFinalStatusFilter,
                                             UseOrderProjectFilter,
                                             UseOrderPayerFilter,
                                             UseOrderOrgFromFilter,
                                             UseOrderOrgToFilter,
                                             _FilterStatusId,
                                             FilterOrderCreatorId,
                                             FilterOrderExecuterId,
                                             _FilterOrderTypeId,
                                             _FilterOrderClientId,
                                             FilterOrderPriority,
                                             FilterOrderDateBeg,
                                             FilterOrderDateEnd,
                                             FilterOrderExDateBeg,
                                             FilterOrderExDateEnd,
                                             FilterOrderEndDateBeg,
                                             FilterOrderEndDateEnd,
                                             FilterFinalStatus,
                                             _FilterOrderProjectId,
                                             _FilterOrderPayerId,
                                             _FilterOrderOrgFromId,
                                             _FilterOrderOrgToId
                                             ).ToList().AsQueryable();

            var result = query.Select(Mapper.Map).OrderByDescending(o => o.Id).AsQueryable();

            List<OrderBaseViewModel> orders = new List<OrderBaseViewModel>();
            foreach (var o in result)
            {
                OrderBaseViewModel OrdersInfoItem = new OrderBaseViewModel();

                var OrderTypeFullInfo = getAvailableOrderTypes(null, null).FirstOrDefault(t => t.Id == o.OrderType);

                OrdersInfoItem.AllowData = isAdmin || (UserHasRole(userId, OrderTypeFullInfo.UserRoleIdForCompetitiveList));

                OrdersInfoItem.Id = o.Id;
                OrdersInfoItem.OrderDate = o.OrderDate;
                OrdersInfoItem.OrderDateRaw = o.OrderDateRaw;
                OrdersInfoItem.CreatedByUser = o.CreatedByUser;
                OrdersInfoItem.CreatedByUserName = o.CreatedByUserName;
                OrdersInfoItem.CreateDatetime = o.CreateDatetime;
                OrdersInfoItem.OrderType = o.OrderType;
                OrdersInfoItem.OrderTypename = o.OrderTypename;
                OrdersInfoItem.OrderTypeShortName = o.OrderTypeShortName;
                OrdersInfoItem.CurrentOrderStatus = o.CurrentOrderStatus;
                OrdersInfoItem.CurrentOrderStatusColor = o.CurrentOrderStatusColor;
                OrdersInfoItem.CurrentOrderStatusName = o.CurrentOrderStatusName;
                OrdersInfoItem.CurrentStatusShortName = o.CurrentStatusShortName;
                OrdersInfoItem.FontColor = o.FontColor;
                OrdersInfoItem.BackgroundColor = o.BackgroundColor;
                OrdersInfoItem.OrderDescription = o.OrderDescription;
                OrdersInfoItem.ClientId = o.ClientId;
                OrdersInfoItem.ClientName = o.ClientName;
                OrdersInfoItem.ClientCenterName = o.ClientCenterName;
                OrdersInfoItem.CanBeDelete = o.CanBeDelete;
                OrdersInfoItem.Summ = o.Summ;
                OrdersInfoItem.UseNotifications = o.UseNotifications;
                OrdersInfoItem.CreatorContact = o.CreatorContact;
                OrdersInfoItem.CreatorPosition = o.CreatorPosition;
                OrdersInfoItem.PriorityType = o.PriorityType;
                OrdersInfoItem.OrderServiceDatetime = o.OrderServiceDatetime;
                OrdersInfoItem.IconFile = o.IconFile;
                OrdersInfoItem.IconDescription = o.IconDescription;
                OrdersInfoItem.OrderExecuter = o.OrderExecuter;
                OrdersInfoItem.OrderExecuterName = o.OrderExecuterName;
                OrdersInfoItem.PayerId = o.PayerId;
                OrdersInfoItem.PayerName = o.PayerName;
                OrdersInfoItem.ProjectId = o.ProjectId;
                OrdersInfoItem.ProjectNum = o.ProjectNum;
                OrdersInfoItem.ProjectDescription = o.ProjectDescription;
                OrdersInfoItem.CarNumber = o.CarNumber;
                OrdersInfoItem.TotalDistanceDescription = o.TotalDistanceDescription;
                OrdersInfoItem.TotalCost = o.TotalCost;
                OrdersInfoItem.TotalDistanceLenght = o.TotalDistanceLenght;
                OrdersInfoItem.IsTransport = o.IsTransport;
                OrdersInfoItem.IsPrivateOrder = o.IsPrivateOrder;
                OrdersInfoItem.ExecuterNotes = o.ExecuterNotes;

                orders.Add(OrdersInfoItem);
            }

            return orders.AsQueryable();
        }

        public OrderPipelineStepViewModel getStep(int Id)
        {
            return Mapper.Map(db.OrderPipelineSteps.FirstOrDefault(or => or.Id == Id));
        }

        public OrderBaseViewModel getOrder(long Id)
        {
            return Mapper.Map(db.OrdersBase.FirstOrDefault(or => or.Id == Id));
        }

        private OrdersTruckTransportViewModel getAdditionalPointsData(List<OrderAdditionalRoutePointModel> points, int loadType)
        {
            OrdersTruckTransportViewModel order = new OrdersTruckTransportViewModel();

            order.OrganizationLoadPoints = "";
            order.AddressLoadPoints = "";
            order.ContactsLoadPoints = "";
            //для загрузки начинаем отсчет доп. точек с 2
            int i = 2;
            //для выгрузки начинаем отсчет доп. точки с 1
            if (loadType == 2) i = 1;

            foreach (var point in points)
            {
                order.OrganizationLoadPoints = order.OrganizationLoadPoints + i.ToString() + ") " + point.NamePoint + " (" + point.CityPoint + ")" + "\n";
                if (order.TripType == 2)
                    order.AddressLoadPoints = order.AddressLoadPoints + i.ToString() + ") " + point.CountryPoint + " " + point.CityPoint + " " + point.AddressPoint + "\n";
                else
                    order.AddressLoadPoints = order.AddressLoadPoints + i.ToString() + ") " + point.CityPoint + " " + point.AddressPoint + "\n";
                order.ContactsLoadPoints = order.ContactsLoadPoints + i.ToString() + ") " + point.ContactPerson + " / " + point.ContactPersonPhone + "\n";
                i++;
            }

            return order;
        }

        public void getTruckTrasportOrderData(ref OrdersTruckTransportViewModel order)
        {
            var Id = order.Id;
            var TruckTypeInfo = db.OrderTruckTransport.FirstOrDefault(or => or.OrderId == Id);
            if (TruckTypeInfo != null) Mapper.Map(TruckTypeInfo, ref order);


            var pointLoads = getRoutePoints((long)order.RouteId).ToList();
            //  var LoadPoints = pointLoads.Where(x => x.RoutePointTypeId == 1).ToList();
            // var UnLoadPoints = pointLoads.Where(x => x.RoutePointTypeId == 4).ToList();
            order.LoadPoints = getLoadPoints(Id, true).ToList();
            order.UnLoadPoints = getLoadPoints(Id, false).ToList();
            //order.LoadPoints = pointLoads;

            var AdditionalLoadData = getAdditionalPointsData(order.LoadPoints, 1);

            order.OrganizationLoadPoints = AdditionalLoadData.OrganizationLoadPoints;
            order.AddressLoadPoints = AdditionalLoadData.AddressLoadPoints;
            order.ContactsLoadPoints = AdditionalLoadData.ContactsLoadPoints;

            var AdditionalUnLoadData = getAdditionalPointsData(order.UnLoadPoints, 2);
            order.OrganizationUnLoadPoints = AdditionalUnLoadData.OrganizationLoadPoints;
            order.AddressUnLoadPoints = AdditionalUnLoadData.AddressLoadPoints;
            order.ContactsUnLoadPoints = AdditionalUnLoadData.ContactsLoadPoints;

            order.CountUnLoadPoints = order.UnLoadPoints.Count();
            order.CountLoadAndUnLoadPoints = order.LoadPoints.Count() + order.UnLoadPoints.Count();
        }

        public void getPassTrasportOrderData(ref OrdersPassTransportViewModel order)
        {
            var Id = order.Id;
            var PassTypeInfo = db.OrdersPassengerTransport.FirstOrDefault(or => or.OrderId == Id);
            if (PassTypeInfo != null) Mapper.Map(PassTypeInfo, ref order);
        }

        public OrderAttachmentViewModel getAttachment(long Id)
        {
            return Mapper.MapWithBody(db.OrderAttachments.FirstOrDefault(or => or.Id == Id));
        }

        public OrderClientsViewModel getClient(long Id)
        {
            return Mapper.Map(db.OrderClients.FirstOrDefault(or => or.Id == Id));
        }

        public OrderClientBalanceKeeperViewModel getPayer(long Id)
        {
            return Mapper.Map(db.BalanceKeepers.FirstOrDefault(or => or.Id == Id));
        }

        public OrderStatusViewModel getStatus(int Id)
        {
            return Mapper.Map(db.OrderStatuses.FirstOrDefault(or => or.Id == Id));
        }

        public string getStatusName(string Id)
        {
            string[] idList = Id.Split(new char[] { ',' });
            string FilterStatusName = "";

            foreach (string i in idList)
            {
                FilterStatusName = string.Concat(FilterStatusName, getStatus(Convert.ToInt32(i))?.StatusName);
            }

            return FilterStatusName;
        }

        public bool SaveNotificationHistory(OrderNotificationViewModel model)
        {
            try
            {

                var historyRow = new OrderNotifications();

                if (historyRow != null)
                {
                    historyRow.Datetime = DateTime.Now;
                    historyRow.TypeId = 1;
                    historyRow.CreatedBy = model.CreatedBy;
                    historyRow.Body = model.Body;
                    historyRow.OrderId = model.OrderId;
                    historyRow.Reciever = model.Receiver;

                    db.OrderNotifications.Add(historyRow);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }



        }


        public bool UpdatePipelineStep(OrderPipelineStepViewModel model)
        {
            try
            {
                var orderPipelineStep = db.OrderPipelineSteps.FirstOrDefault(p => p.Id == model.Id);

                if (orderPipelineStep != null)
                {
                    orderPipelineStep.FromStatus = model.FromStatus;
                    orderPipelineStep.ToStatus = model.ToStatus;
                    orderPipelineStep.AccessRoleId = model.AccessRoleId;
                    orderPipelineStep.OrderTypeId = model.OrderTypeId;
                    orderPipelineStep.StartDateForClient = model.StartDateForClientLayer;
                    orderPipelineStep.StartDateForExecuter = model.StartDateForExecuterLayer;
                    orderPipelineStep.FinishOfTheProcess = model.FinishStatusForBP;

                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }


        }


        public bool NewPipelineStep(OrderPipelineStepViewModel model)
        {
            try
            {

                var orderPipelineStep = new OrderPipelineSteps();

                if (orderPipelineStep != null)
                {
                    orderPipelineStep.FromStatus = model.FromStatus;
                    orderPipelineStep.ToStatus = model.ToStatus;
                    orderPipelineStep.AccessRoleId = model.AccessRoleId;
                    orderPipelineStep.OrderTypeId = model.OrderTypeId;
                    orderPipelineStep.StartDateForClient = model.StartDateForClientLayer;
                    orderPipelineStep.StartDateForExecuter = model.StartDateForExecuterLayer;
                    orderPipelineStep.FinishOfTheProcess = model.FinishStatusForBP;

                    db.OrderPipelineSteps.Add(orderPipelineStep);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public bool NewObserver(OrderObserverViewModel model)
        {
            var orderObserver = new OrderObservers();

            if (orderObserver != null)
            {
                orderObserver.OrderId = model.OrderId;
                orderObserver.userId = model.observerId;
                db.OrderObservers.Add(orderObserver);
                db.SaveChanges();
            }

            return true;
        }


        public bool NewAttachment(OrderAttachmentViewModel model)
        {
            var orderAttachment = new OrderAttachments();

            if (orderAttachment != null)
            {
                orderAttachment.OrderId = model.OrderId;
                orderAttachment.DocDescription = model.DocDescription;
                orderAttachment.AddedByUser = model.AddedByUser;
                orderAttachment.AddedDateTime = model.AddedDateTime;
                orderAttachment.DocType = 1;//model.DocType;
                orderAttachment.DocBody = model.DocBody;
                orderAttachment.RealFileName = model.RealFileName;

                db.OrderAttachments.Add(orderAttachment);

                db.SaveChanges();
            }

            return true;

        }

        public bool NewClient(OrderClientsViewModel model)
        {
            var orderClient = new OrderClients();

            if (orderClient != null)
            {
                orderClient.ClientName = model.ClientName;
                orderClient.ClientCity = string.Empty;
                orderClient.ClientAddress = string.Empty;
                orderClient.AccessRoleId = model.AccessRoleId;
                orderClient.ClientCFOId = model.ClientCFOId;

                db.OrderClients.Add(orderClient);

                db.SaveChanges();
            }

            return true;
        }



        public long NewOrder(OrderBaseViewModel model)
        {
            var orderInfo = new OrdersBase();

            var routeTimeInt = DateTimeConvertClass.convertHoursToInt(model.TimeRoute);
            var specialVehiclesTimeInt = DateTimeConvertClass.convertTimeToInt(model.TimeSpecialVehicles);

            if (orderInfo != null)
            {
                orderInfo.CreatedByUser = model.CreatedByUser;
                orderInfo.CreateDatetime = model.CreateDatetime;
                orderInfo.OrderDate = DateTime.Now;
                orderInfo.OrderType = model.OrderType;
                orderInfo.CurrentOrderStatus = 1;
                orderInfo.OrderDescription = model.OrderDescription;
                orderInfo.ClientId = model.ClientId;
                orderInfo.Summ = model.Summ;
                orderInfo.UseNotifications = model.UseNotifications;
                orderInfo.CreatorContact = model.CreatorContact;
                orderInfo.CreatorPosition = model.CreatorPosition;
                orderInfo.PriotityType = model.PriorityType;
                orderInfo.OrderServiceDateTime = model.OrderServiceDatetime;
                orderInfo.OrderExecuter = model.OrderExecuter;
                orderInfo.PayerId = model.PayerId;
                orderInfo.ProjectId = (model.ProjectId == 0) ? (int?)null : (int?)model.ProjectId;
                orderInfo.CarNumber = model.CarNumber;
                orderInfo.DistanceDescription = model.TotalDistanceDescription;
                orderInfo.TotalPrice = Convert.ToDecimal(model.TotalCost.Replace(".", ","));
                orderInfo.TotalDistanceLength = Convert.ToDecimal(model.TotalDistanceLenght.Replace(".", ","));
                orderInfo.IsPrivateOrder = model.IsPrivateOrder;
                orderInfo.ExecuterNotes = model.ExecuterNotes;
                orderInfo.TypeSpecId = (model.TypeSpecId == 0) ? (int?)null : (int?)model.TypeSpecId;

                orderInfo.TimeRoute = routeTimeInt;
                orderInfo.TimeSpecialVehicles = specialVehiclesTimeInt;
                orderInfo.IsAdditionalRoutePoints = model.IsAdditionalRoutePoints;

                if (model.RouteId != 0)
                    orderInfo.RouteId = model.RouteId;

                db.OrdersBase.Add(orderInfo);

                if (db.SaveChanges() > 0)
                {
                    db.OrderStatusHistory.Add(new OrderStatusHistory()
                    {
                        ChangedByUser = model.CreatedByUser,
                        NewStatus = 1,//model.CurrentOrderStatus,
                        OldStatus = null,
                        ChangeDateTime = model.CreateDatetime,
                        OrderId = orderInfo.Id
                    });

                    db.SaveChanges();

                    db.OrderObservers.Add(new OrderObservers()
                    {
                        OrderId = orderInfo.Id,
                        userId = model.CreatedByUser
                    });

                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(orderInfo.OrderExecuter))
                    {
                        db.OrderObservers.Add(new OrderObservers()
                        {
                            OrderId = orderInfo.Id,
                            userId = model.OrderExecuter
                        });
                    }

                    db.SaveChanges();
                }


                var Specification = db.OrderBaseSpecification.Where(o => o.OrderId == model.Id).ToList();
                if (Specification.Count() > 0)
                {
                    foreach (var p in Specification)
                        db.OrderBaseSpecification.Remove(p);
                    db.SaveChanges();
                }
                if (model.handledItems?.Length > 0)
                {

                    foreach (string i in model.handledItems)
                    {
                        int SpecificationId = 0;
                        SpecificationId = Int32.Parse(i);
                        db.OrderBaseSpecification.Add(new OrderBaseSpecification()
                        {
                            OrderId = orderInfo.Id,
                            SpecificationId = Int32.Parse(i)
                        });
                        db.SaveChanges();
                    }
                }


                var Projects = db.OrderBaseProjects.Where(o => o.OrderId == model.Id).ToList();

                if (Projects.Count() > 0)
                {
                    foreach (var p in Projects)
                        db.OrderBaseProjects.Remove(p);
                    db.SaveChanges();
                }

                if (!string.IsNullOrEmpty(model.MultiProjectId))
                {
                    string[] idList = model.MultiProjectId.Split(new char[] { ',' });
                    if (idList.Length > 0)
                    {

                        foreach (string i in idList)
                        {
                            int ProjectId = 0;
                            ProjectId = Int32.Parse(i);
                            db.OrderBaseProjects.Add(new OrderBaseProjects()
                            {
                                OrderId = orderInfo.Id,
                                ProjectId = Int32.Parse(i)
                            });
                            db.SaveChanges();
                        }
                    }
                }
            }




            if (model is OrdersPassTransportViewModel)
            {
                model.Id = orderInfo.Id;
                NewPassengerTransport((OrdersPassTransportViewModel)model);
            }

            if (model is OrdersTruckTransportViewModel)
            {
                model.Id = orderInfo.Id;
                NewTruckTransport((OrdersTruckTransportViewModel)model);
            }

            return orderInfo.Id;
        }

        private bool NewTruckTransport(OrdersTruckTransportViewModel model)
        {
            try
            {
                if (db.Organization.Where(o => o.Name == model.Shipper).Count() == 0)
                {
                    db.Organization.Add(new Organization()
                    {
                        Name = model.Shipper,
                        City = model.ShipperCity,
                        Address = model.ShipperAdress,
                        CountryId = model.ShipperCountryId,
                        IsAuto = true
                    });
                    db.SaveChanges();
                }

                if (db.Organization.Where(o => o.Name == model.Consignee).Count() == 0)
                {
                    db.Organization.Add(new Organization()
                    {
                        Name = model.Consignee,
                        City = model.ConsigneeCity,
                        Address = model.ConsigneeAdress,
                        CountryId = model.ConsigneeCountryId,
                        IsAuto = true
                    });
                    db.SaveChanges();
                }

                var TruckTransInfo = new OrderTruckTransport();

                TruckTransInfo.OrderId = model.Id;
                TruckTransInfo.Shipper = model.Shipper;

                TruckTransInfo.ShipperAdress = model.ShipperAdress;

                TruckTransInfo.FromShipperDatetime = DateTimeConvertClass.getDateTime(model.FromShipperDateRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.FromShipperTimeRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.FromShipperTimeRaw));

                TruckTransInfo.Consignee = model.Consignee;


                TruckTransInfo.ConsigneeAdress = model.ConsigneeAdress;

                if (model.TripType < 2)
                {
                    TruckTransInfo.ConsigneeCountryId = model.DefaultCountry;
                    TruckTransInfo.ShipperCountryId = model.DefaultCountry;
                }
                else
                {
                    TruckTransInfo.ConsigneeCountryId = model.ConsigneeCountryId;
                    TruckTransInfo.ShipperCountryId = model.ShipperCountryId;
                }

                if (model.TripType < 1)
                {
                    TruckTransInfo.ShipperCity = model.ShipperCity;
                    TruckTransInfo.ConsigneeCity = model.ShipperCity;
                }
                else
                {
                    TruckTransInfo.ShipperCity = model.ShipperCity;
                    TruckTransInfo.ConsigneeCity = model.ConsigneeCity;
                }




                TruckTransInfo.ToConsigneeDatetime = DateTimeConvertClass.getDateTime(model.ToConsigneeDateRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.ToConsigneeTimeRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.ToConsigneeTimeRaw));

                TruckTransInfo.BoxingDescription = model.BoxingDescription;
                TruckTransInfo.TruckDescription = model.TruckDescription;
                TruckTransInfo.TripType = model.TripType;
                TruckTransInfo.Weight = Convert.ToDecimal(model.Weight.Replace(".", ","));
                TruckTransInfo.Volume = Convert.ToDecimal(model.Volume);
                TruckTransInfo.DimenssionL = Convert.ToDecimal(model.DimenssionL);
                TruckTransInfo.DimenssionW = Convert.ToDecimal(model.DimenssionW);
                TruckTransInfo.DimenssionH = Convert.ToDecimal(model.DimenssionH);
                TruckTransInfo.TruckTypeId = model.TruckTypeId;
                TruckTransInfo.VehicleTypeId = model.VehicleTypeId;
                TruckTransInfo.LoadingTypeId = model.LoadingTypeId;
                TruckTransInfo.UnloadingTypeId = model.UnloadingTypeId;
                TruckTransInfo.ShipperContactPerson = model.ShipperContactPerson;
                TruckTransInfo.ShipperContactPersonPhone = model.ShipperContactPersonPhone;
                TruckTransInfo.ConsigneeContactPerson = model.ConsigneeContactPerson;
                TruckTransInfo.ConsigneeContactPersonPhone = model.ConsigneeContactPersonPhone;
                TruckTransInfo.ShipperId = model.ShipperId;
                TruckTransInfo.ConsigneeId = model.ConsigneeId;
                TruckTransInfo.ShipperId = model.ShipperId != 0 ? model.ShipperId : db.Organization.FirstOrDefault(o => o.Name == model.Shipper).Id;
                TruckTransInfo.ConsigneeId = model.ConsigneeId != 0 ? model.ConsigneeId : db.Organization.FirstOrDefault(o => o.Name == model.Consignee).Id;

                db.OrderTruckTransport.Add(TruckTransInfo);
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return true;
        }

        private bool NewPassengerTransport(OrdersPassTransportViewModel model)
        {
            try
            {
                if (db.Organization.Where(o => o.Name == model.OrgFrom).Count() == 0)
                {
                    db.Organization.Add(new Organization()
                    {
                        Name = model.OrgFrom,
                        City = model.CityFrom,
                        Address = model.AdressFrom,
                        CountryId = model.CountryFrom,
                        IsAuto = true
                    });
                    db.SaveChanges();
                }

                if (db.Organization.Where(o => o.Name == model.OrgTo).Count() == 0)
                {
                    db.Organization.Add(new Organization()
                    {
                        Name = model.OrgTo,
                        City = model.CityTo,
                        Address = model.AdressTo,
                        CountryId = model.CountryTo,
                        IsAuto = true
                    });
                    db.SaveChanges();
                }


                var PassTransInfo = new OrdersPassengerTransport();

                PassTransInfo.OrderId = model.Id;
                PassTransInfo.AdressFrom = model.AdressFrom;
                PassTransInfo.AdressTo = model.AdressTo;

                PassTransInfo.OrgFrom = model.OrgFrom;
                PassTransInfo.OrgTo = model.OrgTo;

                if (model.TripType < 2)
                {
                    PassTransInfo.FromCountry = model.DefaultCountry;
                    PassTransInfo.ToCountry = model.DefaultCountry;
                }
                else
                {
                    PassTransInfo.FromCountry = model.CountryFrom;
                    PassTransInfo.ToCountry = model.CountryTo;
                }

                if (model.TripType < 1)
                {
                    PassTransInfo.FromCity = model.CityFrom;
                    PassTransInfo.ToCity = model.CityFrom;
                }
                else
                {
                    PassTransInfo.FromCity = model.CityFrom;
                    PassTransInfo.ToCity = model.CityTo;
                }

                PassTransInfo.StartDateTimeOfTrip = DateTimeConvertClass.getDateTime(model.StartDateTimeOfTripRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.StartDateTimeExOfTripRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.StartDateTimeExOfTripRaw));

                PassTransInfo.FinishDateTimeOfTrip = DateTimeConvertClass.getDateTime(model.FinishDateTimeOfTripRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.FinishDateTimeExOfTripRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.FinishDateTimeExOfTripRaw));

                PassTransInfo.NeedReturn = model.NeedReturn;

                if (model.NeedReturn)
                {
                    PassTransInfo.ReturnStartDateTimeOfTrip = DateTimeConvertClass.getDateTime(model.ReturnStartDateTimeOfTripRaw).
                                                           AddHours(DateTimeConvertClass.getHours(model.ReturnStartDateTimeExOfTripRaw)).
                                                           AddMinutes(DateTimeConvertClass.getMinutes(model.ReturnStartDateTimeExOfTripRaw));

                    PassTransInfo.ReturnFinishDateTimeOfTrip = DateTimeConvertClass.getDateTime(model.ReturnFinishDateTimeOfTripRaw).
                                                           AddHours(DateTimeConvertClass.getHours(model.ReturnFinishDateTimeExOfTripRaw)).
                                                           AddMinutes(DateTimeConvertClass.getMinutes(model.ReturnFinishDateTimeExOfTripRaw));

                }

                PassTransInfo.TripDescription = model.TripDescription;
                PassTransInfo.PassInfo = model.PassInfo;

                PassTransInfo.CarDetailInfo = model.CarDetailInfo;
                PassTransInfo.CarDriverFio = model.CarDriverFio;
                PassTransInfo.CarDriverContactInfo = model.CarDriverContactInfo;
                PassTransInfo.TripType = model.TripType;
                PassTransInfo.OrgFromId = model.OrgFromId != 0 ? model.OrgFromId : db.Organization.FirstOrDefault(o => o.Name == model.OrgFrom).Id;
                PassTransInfo.OrgToId = model.OrgToId != 0 ? model.OrgToId : db.Organization.FirstOrDefault(o => o.Name == model.OrgTo).Id;

                db.OrdersPassengerTransport.Add(PassTransInfo);
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return true;
        }



        public bool UpdateClient(OrderClientsViewModel model)
        {
            var orderClient = db.OrderClients.FirstOrDefault(o => o.Id == model.Id);

            if (orderClient != null)
            {
                orderClient.ClientName = model.ClientName;
                orderClient.ClientCity = string.Empty;
                orderClient.ClientAddress = string.Empty;
                orderClient.AccessRoleId = model.AccessRoleId;
                orderClient.ClientCFOId = model.ClientCFOId;

                db.SaveChanges();
            }

            return true;
        }


        public bool UpdateOrder(OrderBaseViewModel model)
        {
            var orderInfo = db.OrdersBase.FirstOrDefault(o => o.Id == model.Id);

            if (orderInfo != null)
            {
                var oldStatus = orderInfo.CurrentOrderStatus;

                if (oldStatus != model.CurrentOrderStatus)
                {
                    db.OrderStatusHistory.Add(new OrderStatusHistory()
                    {
                        ChangedByUser = model.CreatedByUser,
                        NewStatus = model.CurrentOrderStatus,
                        OldStatus = oldStatus,
                        ChangeDateTime = model.CreateDatetime,
                        StatusChangeComment = model.StatusChangeComment,
                        OrderId = model.Id
                    });

                    db.SaveChanges();
                }

                orderInfo.OrderDate = DateTimeConvertClass.getDateTime(model.OrderDateRaw);
                orderInfo.OrderType = model.OrderType;
                orderInfo.CurrentOrderStatus = model.CurrentOrderStatus;
                orderInfo.OrderDescription = model.OrderDescription;
                orderInfo.ClientId = model.ClientId;
                orderInfo.Summ = model.Summ;
                orderInfo.UseNotifications = model.UseNotifications;
                orderInfo.CreatorContact = model.CreatorContact;
                orderInfo.CreatorPosition = model.CreatorPosition;
                orderInfo.PriotityType = model.PriorityType;
                orderInfo.OrderServiceDateTime = model.OrderServiceDatetime;
                orderInfo.OrderExecuter = model.OrderExecuter;
                orderInfo.PayerId = model.PayerId;
                orderInfo.ProjectId = (model.ProjectId == 0) ? (int?)null : (int?)model.ProjectId;
                orderInfo.CarNumber = model.CarNumber;
                orderInfo.DistanceDescription = model.TotalDistanceDescription;
                orderInfo.TotalPrice = Convert.ToDecimal(model.TotalCost.Replace(".", ","));
                orderInfo.TotalDistanceLength = Convert.ToDecimal(model.TotalDistanceLenght.Replace(".", ","));
                orderInfo.IsPrivateOrder = model.IsPrivateOrder;
                orderInfo.ExecuterNotes = model.ExecuterNotes;
                orderInfo.TypeSpecId = orderInfo.TypeSpecId = (model.TypeSpecId == 0) ? (int?)null : (int?)model.TypeSpecId;

                orderInfo.TimeRoute = DateTimeConvertClass.convertHoursToInt(model.TimeRoute);
                orderInfo.TimeSpecialVehicles = DateTimeConvertClass.convertTimeToInt(model.TimeSpecialVehicles);

                orderInfo.IsAdditionalRoutePoints = model.IsAdditionalRoutePoints;
                orderInfo.RouteId = (model.RouteId == 0) ? (long?)null : (long?)model.RouteId;

                db.SaveChanges();

                if (!string.IsNullOrEmpty(orderInfo.OrderExecuter))
                {
                    if (db.OrderObservers.Where(o => o.OrderId == model.Id && o.userId == orderInfo.OrderExecuter).Count() == 0)
                    {
                        db.OrderObservers.Add(new OrderObservers()
                        {
                            OrderId = orderInfo.Id,
                            userId = model.OrderExecuter
                        });

                        db.SaveChanges();
                    }
                }

                var Specification = db.OrderBaseSpecification.Where(o => o.OrderId == model.Id).ToList();
                if (Specification.Count() > 0)
                {
                    foreach (var p in Specification)
                        db.OrderBaseSpecification.Remove(p);
                    db.SaveChanges();
                }

                if (model.handledItems != null)
                    if (model.handledItems.Length > 0)
                    {

                        foreach (string i in model.handledItems)
                        {
                            int SpecificationId = 0;
                            SpecificationId = Int32.Parse(i);
                            db.OrderBaseSpecification.Add(new OrderBaseSpecification()
                            {
                                OrderId = orderInfo.Id,
                                SpecificationId = Int32.Parse(i)
                            });
                            db.SaveChanges();
                        }
                    }



                var Projects = db.OrderBaseProjects.Where
                    (o => o.OrderId == model.Id).ToList();

                if (Projects.Count() > 0)
                {
                    foreach (var p in Projects)
                        db.OrderBaseProjects.Remove(p);
                    db.SaveChanges();
                }

                if (!string.IsNullOrEmpty(model.MultiProjectId))
                {
                    string[] idList = model.MultiProjectId.Split(new char[] { ',' });
                    if (idList.Length > 0)
                    {

                        foreach (string i in idList)
                        {
                            int ProjectId = 0;
                            ProjectId = Int32.Parse(i);
                            db.OrderBaseProjects.Add(new OrderBaseProjects()
                            {
                                OrderId = orderInfo.Id,
                                ProjectId = Int32.Parse(i)
                            });
                            db.SaveChanges();
                        }
                    }
                }

                if (model is OrdersPassTransportViewModel)
                {
                    UpdatePassengerTransport((OrdersPassTransportViewModel)model);
                }
                if (model is OrdersTruckTransportViewModel)
                {
                    UpdateTruckTransport((OrdersTruckTransportViewModel)model);
                }
            }

            return true;
        }
        private bool UpdateTruckTransport(OrdersTruckTransportViewModel model)
        {

            var TruckTransInfo = db.OrderTruckTransport.FirstOrDefault(o => o.OrderId == model.Id);

            try
            {
                if (TruckTransInfo == null)
                {
                    NewTruckTransport(model);
                }
                else
                {
                    if (db.Organization.Where(o => o.Name == model.Shipper).Count() == 0)
                    {
                        db.Organization.Add(new Organization()
                        {
                            Name = model.Shipper,
                            City = model.ShipperCity,
                            Address = model.ShipperAdress,
                            CountryId = model.ShipperCountryId,
                            IsAuto = true
                        });
                        db.SaveChanges();
                    }

                    if (db.Organization.Where(o => o.Name == model.Consignee).Count() == 0)
                    {
                        db.Organization.Add(new Organization()
                        {
                            Name = model.Consignee,
                            City = model.ConsigneeCity,
                            Address = model.ConsigneeAdress,
                            CountryId = model.ConsigneeCountryId,
                            IsAuto = true
                        });
                        db.SaveChanges();
                    }

                    TruckTransInfo.Shipper = model.Shipper;
                    TruckTransInfo.ShipperCountryId = model.ShipperCountryId;


                    TruckTransInfo.ShipperAdress = model.ShipperAdress;


                    if (model.TripType < 2)
                    {
                        TruckTransInfo.ConsigneeCountryId = model.DefaultCountry;
                        TruckTransInfo.ShipperCountryId = model.DefaultCountry;
                    }
                    else
                    {
                        TruckTransInfo.ConsigneeCountryId = model.ConsigneeCountryId;
                        TruckTransInfo.ShipperCountryId = model.ShipperCountryId;
                    }

                    if (model.TripType < 1)
                    {
                        TruckTransInfo.ShipperCity = model.ShipperCity;
                        TruckTransInfo.ConsigneeCity = model.ShipperCity;
                    }
                    else
                    {
                        TruckTransInfo.ShipperCity = model.ShipperCity;
                        TruckTransInfo.ConsigneeCity = model.ConsigneeCity;
                    }

                    TruckTransInfo.FromShipperDatetime = DateTimeConvertClass.getDateTime(model.FromShipperDateRaw).
                                                           AddHours(DateTimeConvertClass.getHours(model.FromShipperTimeRaw)).
                                                           AddMinutes(DateTimeConvertClass.getMinutes(model.FromShipperTimeRaw));

                    TruckTransInfo.Consignee = model.Consignee;
                    TruckTransInfo.ConsigneeAdress = model.ConsigneeAdress;

                    TruckTransInfo.ToConsigneeDatetime = DateTimeConvertClass.getDateTime(model.ToConsigneeDateRaw).
                                                           AddHours(DateTimeConvertClass.getHours(model.ToConsigneeTimeRaw)).
                                                           AddMinutes(DateTimeConvertClass.getMinutes(model.ToConsigneeTimeRaw));

                    TruckTransInfo.BoxingDescription = model.BoxingDescription;
                    TruckTransInfo.TruckDescription = model.TruckDescription;
                    TruckTransInfo.TripType = model.TripType;
                    TruckTransInfo.Weight = Convert.ToDecimal(model.Weight.Replace(".", ","));
                    TruckTransInfo.Volume = Convert.ToDecimal(model.Volume);
                    TruckTransInfo.DimenssionL = Convert.ToDecimal(model.DimenssionL);
                    TruckTransInfo.DimenssionW = Convert.ToDecimal(model.DimenssionW);
                    TruckTransInfo.DimenssionH = Convert.ToDecimal(model.DimenssionH);
                    TruckTransInfo.TruckTypeId = model.TruckTypeId;
                    TruckTransInfo.VehicleTypeId = model.VehicleTypeId;
                    TruckTransInfo.LoadingTypeId = model.LoadingTypeId;
                    TruckTransInfo.UnloadingTypeId = model.UnloadingTypeId;

                    TruckTransInfo.ShipperContactPerson = model.ShipperContactPerson;
                    TruckTransInfo.ShipperContactPersonPhone = model.ShipperContactPersonPhone;
                    TruckTransInfo.ConsigneeContactPerson = model.ConsigneeContactPerson;
                    TruckTransInfo.ConsigneeContactPersonPhone = model.ConsigneeContactPersonPhone;
                    //TruckTransInfo.ShipperId = (model.ShipperId == 0) ? (long?)null : (long?)model.ShipperId;
                    //TruckTransInfo.ConsigneeId = (model.ConsigneeId == 0) ? (long?)null : (long?)model.ConsigneeId;

                    TruckTransInfo.ShipperId = model.ShipperId != 0 ? model.ShipperId : db.Organization.FirstOrDefault(o => o.Name == model.Shipper).Id;
                    TruckTransInfo.ConsigneeId = model.ConsigneeId != 0 ? model.ConsigneeId : db.Organization.FirstOrDefault(o => o.Name == model.Consignee).Id;

                    db.SaveChanges();

                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return true;
        }


        private bool UpdatePassengerTransport(OrdersPassTransportViewModel model)
        {
            var PassTransInfo = db.OrdersPassengerTransport.FirstOrDefault(o => o.OrderId == model.Id);

            if (PassTransInfo == null)
            {
                NewPassengerTransport(model);
            }
            else
            {
                if (db.Organization.Count(o => o.Name == model.OrgFrom) == 0)
                {
                    db.Organization.Add(new Organization()
                    {
                        Name = model.OrgFrom,
                        City = model.CityFrom,
                        Address = model.AdressFrom,
                        CountryId = model.CountryFrom,
                        IsAuto = true
                    });
                    db.SaveChanges();
                }

                if (db.Organization.Count(o => o.Name == model.OrgTo) == 0)
                {
                    db.Organization.Add(new Organization()
                    {
                        Name = model.OrgTo,
                        City = model.CityTo,
                        Address = model.AdressTo,
                        CountryId = model.CountryTo,
                        IsAuto = true
                    });
                    db.SaveChanges();
                }

                PassTransInfo.AdressFrom = model.AdressFrom;
                PassTransInfo.AdressTo = model.AdressTo;

                PassTransInfo.OrgFrom = model.OrgFrom;
                PassTransInfo.OrgTo = model.OrgTo;

                if (model.TripType < 2)
                {
                    PassTransInfo.FromCountry = model.DefaultCountry;
                    PassTransInfo.ToCountry = model.DefaultCountry;
                }
                else
                {
                    PassTransInfo.FromCountry = model.CountryFrom;
                    PassTransInfo.ToCountry = model.CountryTo;
                }

                if (model.TripType < 1)
                {
                    PassTransInfo.FromCity = model.CityFrom;
                    PassTransInfo.ToCity = model.CityFrom;
                }
                else
                {
                    PassTransInfo.FromCity = model.CityFrom;
                    PassTransInfo.ToCity = model.CityTo;
                }

                PassTransInfo.StartDateTimeOfTrip = DateTimeConvertClass.getDateTime(model.StartDateTimeOfTripRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.StartDateTimeExOfTripRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.StartDateTimeExOfTripRaw));


                PassTransInfo.FinishDateTimeOfTrip = DateTimeConvertClass.getDateTime(model.FinishDateTimeOfTripRaw).
                                                       AddHours(DateTimeConvertClass.getHours(model.FinishDateTimeExOfTripRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.FinishDateTimeExOfTripRaw));

                PassTransInfo.NeedReturn = model.NeedReturn;

                if (model.NeedReturn)
                {
                    PassTransInfo.ReturnStartDateTimeOfTrip = DateTimeConvertClass.getDateTime(model.ReturnStartDateTimeOfTripRaw).
                                                           AddHours(DateTimeConvertClass.getHours(model.ReturnStartDateTimeExOfTripRaw)).
                                                           AddMinutes(DateTimeConvertClass.getMinutes(model.ReturnStartDateTimeExOfTripRaw));

                    PassTransInfo.ReturnFinishDateTimeOfTrip = DateTimeConvertClass.getDateTime(model.ReturnFinishDateTimeOfTripRaw).
                                                           AddHours(DateTimeConvertClass.getHours(model.ReturnFinishDateTimeExOfTripRaw)).
                                                           AddMinutes(DateTimeConvertClass.getMinutes(model.ReturnFinishDateTimeExOfTripRaw));

                }

                PassTransInfo.TripDescription = model.TripDescription;
                PassTransInfo.PassInfo = model.PassInfo;
                PassTransInfo.CarDetailInfo = model.CarDetailInfo;
                PassTransInfo.CarDriverFio = model.CarDriverFio;
                PassTransInfo.CarDriverContactInfo = model.CarDriverContactInfo;
                PassTransInfo.TripType = model.TripType;
                //PassTransInfo.OrgFromId = (model.OrgFromId == 0) ? (long?)null : (long?)model.OrgFromId;
                //PassTransInfo.OrgToId = (model.OrgToId == 0) ? (long?)null : (long?)model.OrgToId;
                PassTransInfo.OrgFromId = model.OrgFromId != 0 ? model.OrgFromId : db.Organization.FirstOrDefault(o => o.Name == model.OrgFrom).Id;
                PassTransInfo.OrgToId = model.OrgToId != 0 ? model.OrgToId : db.Organization.FirstOrDefault(o => o.Name == model.OrgTo).Id;
            }

            db.SaveChanges();

            return true;
        }

        public bool DeletePipelineStep(int id)
        {
            var orderPipelineStep = db.OrderPipelineSteps.FirstOrDefault(o => o.Id == id);

            if (orderPipelineStep != null)
            {
                db.OrderPipelineSteps.Remove(orderPipelineStep);
                db.SaveChanges();
            }
            return true;

        }


        public bool DeleteOrder(long id)
        {
            var addPointInfo = db.AdditionalRoutePoints.Where(o => o.OrderId == id);
            if (addPointInfo.Count() > 0)
            {
                db.AdditionalRoutePoints.RemoveRange(addPointInfo);
                db.SaveChanges();
            }

            var orderPassInfo = db.OrdersPassengerTransport.FirstOrDefault(o => o.OrderId == id);

            if (orderPassInfo != null)
            {
                db.OrdersPassengerTransport.Remove(orderPassInfo);
                db.SaveChanges();

            }

            var orderTruckInfo = db.OrderTruckTransport.FirstOrDefault(o => o.OrderId == id);

            if (orderTruckInfo != null)
            {
                db.OrderTruckTransport.Remove(orderTruckInfo);
                db.SaveChanges();
            }

            var OrderBaseSpecification = db.OrderBaseSpecification.Where(o => o.OrderId == id);
            if (OrderBaseSpecification.Count() > 0)
            {
                db.OrderBaseSpecification.RemoveRange(OrderBaseSpecification);
                db.SaveChanges();
            }


            var orderBaseProjects = db.OrderBaseProjects.Where(o => o.OrderId == id);
            if (orderBaseProjects.Count() > 0)
            {
                db.OrderBaseProjects.RemoveRange(orderBaseProjects);
                db.SaveChanges();
            }

            var orderInfo = db.OrdersBase.FirstOrDefault(o => o.Id == id);

            if (orderInfo != null)
            {
                db.OrdersBase.Remove(orderInfo);
                db.SaveChanges();
            }
            return true;

        }

        public bool DeleteClient(long id)
        {
            var orderClientInfo = db.OrderClients.FirstOrDefault(o => o.Id == id);

            if (orderClientInfo != null)
            {
                db.OrderClients.Remove(orderClientInfo);
                db.SaveChanges();
            }
            return true;
        }

        public bool DeleteObserver(long id)
        {
            var orderObserverInfo = db.OrderObservers.FirstOrDefault(o => o.Id == id);

            if (orderObserverInfo != null)
            {
                db.OrderObservers.Remove(orderObserverInfo);
                db.SaveChanges();
            }
            return true;
        }

        public bool DeleteUsedCar(long Id)
        {
            var usedCar = db.OrderUsedCars.FirstOrDefault(o => o.Id == Id);

            if (usedCar != null)
            {
                db.OrderUsedCars.Remove(usedCar);
                db.SaveChanges();
            }
            return true;

        }


        public bool DeleteAttachment(long id)
        {
            var orderInfo = db.OrderAttachments.FirstOrDefault(o => o.Id == id);

            if (orderInfo != null)
            {
                db.OrderAttachments.Remove(orderInfo);
                db.SaveChanges();
            }
            return true;
        }

        public IQueryable<OrderStatusHistoryViewModel> getOrderStatusHistory(string userId, long OrderId)
        {
            return db.OrderStatusHistory
                            .AsNoTracking()
                             .Where(osh => osh.OrderId == OrderId)
                              .Select(Mapper.Map)
                               .OrderByDescending(o => o.EditedDateTime)
                                .AsQueryable();

        }


        public IQueryable<OrderAttachmentViewModel> getOrderAttachments(string userId, long OrderId)
        {
            return db.OrderAttachments
                                        .AsNoTracking()
                                         .Where(osh => osh.OrderId == OrderId)
                                          .Select(Mapper.Map)
                                           .OrderByDescending(o => o.Id)
                                            .AsQueryable();
        }

        public void AddStatus(OrderStatusViewModel model)
        {
            var OrderStatusInfo = new OrderStatuses()
            {
                OrderStatusName = model.StatusName,
                Color = model.StatusColor,
                AllowEditClientData = model.AllowClientData,
                AllowEditExecuterData = model.AllowExecuterData,
                ActionName = model.ActionName,
                IconFile = model.IconFile,
                IconDescription = model.IconDescription,
                ShortName = model.ShortName,
                FontColor = model.FontColor,
                BackgroundColor = model.BackgroundColor
            };

            db.OrderStatuses.Add(OrderStatusInfo);
            db.SaveChanges();
        }


        public int NewProject(OrderProjectViewModel model)
        {
            var dbInfo = new Projects()
            {
                Code = model.Code,
                Description = model.Description,
                ProjectCFOId = model.ProjectCFOId,
                ProjectTypeId = model.ProjectTypeId,
                ConstructionDesc = model.ConstructionDesc,
                PlanCount = model.PlanCount,
                isActive = model.isActive,
                ProjectOrderer = model.ProjectOrderer,
                Comments = model.Comments,
                ManufacturingEnterprise = model.ManufacturingEnterprise,
                NumOrder = model.NumOrder,
                DeliveryBasic = model.DeliveryBasic
            };

            if (!(string.IsNullOrEmpty(model.DateOpenOrderRaw)))
                dbInfo.DateOpenOrder = DateTimeConvertClass.getDateTime(model.DateOpenOrderRaw).
                 AddHours(DateTimeConvertClass.getHours(model.DateOpenOrderRaw)).
                 AddMinutes(DateTimeConvertClass.getMinutes(model.DateOpenOrderRaw));

            if (!(string.IsNullOrEmpty(model.PlanPeriodForMPRaw)))
                dbInfo.PlanPeriodForMP = DateTimeConvertClass.getDateTime(model.PlanPeriodForMPRaw).
             AddHours(DateTimeConvertClass.getHours(model.PlanPeriodForMPRaw)).
             AddMinutes(DateTimeConvertClass.getMinutes(model.PlanPeriodForMPRaw));

            if (!(string.IsNullOrEmpty(model.PlanPeriodForComponentsRaw)))
                dbInfo.PlanPeriodForComponents = DateTimeConvertClass.getDateTime(model.PlanPeriodForComponentsRaw).
             AddHours(DateTimeConvertClass.getHours(model.PlanPeriodForComponentsRaw)).
             AddMinutes(DateTimeConvertClass.getMinutes(model.PlanPeriodForComponentsRaw));

            if (!(string.IsNullOrEmpty(model.PlanPeriodForSGIRaw)))
                dbInfo.PlanPeriodForSGI = DateTimeConvertClass.getDateTime(model.PlanPeriodForSGIRaw).
             AddHours(DateTimeConvertClass.getHours(model.PlanPeriodForSGIRaw)).
             AddMinutes(DateTimeConvertClass.getMinutes(model.PlanPeriodForSGIRaw));

            if (!(string.IsNullOrEmpty(model.PlanPeriodForTransportationRaw)))
                dbInfo.PlanPeriodForTransportation = DateTimeConvertClass.getDateTime(model.PlanPeriodForTransportationRaw).
             AddHours(DateTimeConvertClass.getHours(model.PlanPeriodForTransportationRaw)).
             AddMinutes(DateTimeConvertClass.getMinutes(model.PlanPeriodForTransportationRaw));

            if (!(string.IsNullOrEmpty(model.PlanDeliveryToConsigneeRaw)))
                dbInfo.PlanDeliveryToConsignee = DateTimeConvertClass.getDateTime(model.PlanDeliveryToConsigneeRaw).
             AddHours(DateTimeConvertClass.getHours(model.PlanDeliveryToConsigneeRaw)).
             AddMinutes(DateTimeConvertClass.getMinutes(model.PlanDeliveryToConsigneeRaw));

            if (model.Shipper > 0)
                dbInfo.Shipper = model.Shipper;
            if (model.Consignee > 0)
                dbInfo.Consignee = model.Consignee;

            db.Projects.Add(dbInfo);
            db.SaveChanges();

            return dbInfo.Id;

        }

        public void UpdateProject(OrderProjectViewModel model)
        {
            var dbInfo = db.Projects.FirstOrDefault(x => x.Id == model.Id);

            if (dbInfo != null)
            {
                dbInfo.Code = model.Code;
                dbInfo.Description = model.Description;
                dbInfo.ProjectCFOId = model.ProjectCFOId;
                dbInfo.ProjectTypeId = model.ProjectTypeId;
                dbInfo.ConstructionDesc = model.ConstructionDesc;
                dbInfo.PlanCount = model.PlanCount;
                dbInfo.isActive = model.isActive;
                dbInfo.ProjectOrderer = model.ProjectOrderer;
                dbInfo.Comments = model.Comments;
                dbInfo.ManufacturingEnterprise = model.ManufacturingEnterprise;
                dbInfo.NumOrder = model.NumOrder;

                if (!(string.IsNullOrEmpty(model.DateOpenOrderRaw)))
                    dbInfo.DateOpenOrder = DateTimeConvertClass.getDateTime(model.DateOpenOrderRaw).
                     AddHours(DateTimeConvertClass.getHours(model.DateOpenOrderRaw)).
                     AddMinutes(DateTimeConvertClass.getMinutes(model.DateOpenOrderRaw));

                if (!(string.IsNullOrEmpty(model.PlanPeriodForMPRaw)))
                    dbInfo.PlanPeriodForMP = DateTimeConvertClass.getDateTime(model.PlanPeriodForMPRaw).
                 AddHours(DateTimeConvertClass.getHours(model.PlanPeriodForMPRaw)).
                 AddMinutes(DateTimeConvertClass.getMinutes(model.PlanPeriodForMPRaw));

                if (!(string.IsNullOrEmpty(model.PlanPeriodForComponentsRaw)))
                    dbInfo.PlanPeriodForComponents = DateTimeConvertClass.getDateTime(model.PlanPeriodForComponentsRaw).
                 AddHours(DateTimeConvertClass.getHours(model.PlanPeriodForComponentsRaw)).
                 AddMinutes(DateTimeConvertClass.getMinutes(model.PlanPeriodForComponentsRaw));

                if (!(string.IsNullOrEmpty(model.PlanPeriodForSGIRaw)))
                    dbInfo.PlanPeriodForSGI = DateTimeConvertClass.getDateTime(model.PlanPeriodForSGIRaw).
                 AddHours(DateTimeConvertClass.getHours(model.PlanPeriodForSGIRaw)).
                 AddMinutes(DateTimeConvertClass.getMinutes(model.PlanPeriodForSGIRaw));

                if (!(string.IsNullOrEmpty(model.PlanPeriodForTransportationRaw)))
                    dbInfo.PlanPeriodForTransportation = DateTimeConvertClass.getDateTime(model.PlanPeriodForTransportationRaw).
                 AddHours(DateTimeConvertClass.getHours(model.PlanPeriodForTransportationRaw)).
                 AddMinutes(DateTimeConvertClass.getMinutes(model.PlanPeriodForTransportationRaw));

                if (!(string.IsNullOrEmpty(model.PlanDeliveryToConsigneeRaw)))
                    dbInfo.PlanDeliveryToConsignee = DateTimeConvertClass.getDateTime(model.PlanDeliveryToConsigneeRaw).
                 AddHours(DateTimeConvertClass.getHours(model.PlanDeliveryToConsigneeRaw)).
                 AddMinutes(DateTimeConvertClass.getMinutes(model.PlanDeliveryToConsigneeRaw));

                dbInfo.DeliveryBasic = model.DeliveryBasic;
                if (model.Shipper > 0)
                    dbInfo.Shipper = model.Shipper;
                if (model.Consignee > 0)
                    dbInfo.Consignee = model.Consignee;

                db.SaveChanges();
            }
        }

        public void RemoveProject(int Id)
        {
            var dbInfo = db.Projects.FirstOrDefault(x => x.Id == Id);
            if (dbInfo != null)
            {
                db.Projects.Remove(dbInfo);
                db.SaveChanges();
            }
        }



        public void UpdateStatus(OrderStatusViewModel model)
        {
            var dbInfo = db.OrderStatuses.FirstOrDefault(u => u.Id == model.Id);
            if (dbInfo == null) return;

            dbInfo.OrderStatusName = model.StatusName;
            dbInfo.Color = model.StatusColor;
            dbInfo.AllowEditClientData = model.AllowClientData;
            dbInfo.AllowEditExecuterData = model.AllowExecuterData;
            dbInfo.ActionName = model.ActionName;
            dbInfo.IconFile = model.IconFile;
            dbInfo.IconDescription = model.IconDescription;
            dbInfo.ShortName = model.ShortName;
            dbInfo.FontColor = model.FontColor;
            dbInfo.BackgroundColor = model.BackgroundColor;

            db.SaveChanges();
        }

        public void RemoveStatus(int Id)
        {
            var dbInfo = db.OrderStatuses.FirstOrDefault(u => u.Id == Id);
            if (dbInfo == null) return;

            db.OrderStatuses.Remove(dbInfo);
            db.SaveChanges();
        }


        public List<OrderStatusViewModel> GetStatuses(string searchTerm, int pageSize, int pageNum, bool isTransport)
        {
            return GetStatusesBySearchString(searchTerm, isTransport)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetStatusesCount(string searchTerm, bool isTransport)
        {
            return GetStatusesBySearchString(searchTerm, isTransport).Count();
        }

        public IQueryable<OrderStatusViewModel> GetStatusesBySearchString(string searchTerm, bool isTransport)
        {
            return
            db.OrderStatuses
                  .AsNoTracking()
                     .Where(s => (s.OrderStatusName.Contains(searchTerm) || s.OrderStatusName.StartsWith(searchTerm) || s.OrderStatusName.EndsWith(searchTerm))
                             && (s.OrdersBase.Where(x => x.OrderTypesBase.IsTransportType == isTransport).Count() > 0))
                        .Select(Mapper.Map)
                        .OrderBy(o => o.StatusName)
                         .AsQueryable();
        }

        public IQueryable<OrderProjectViewModel> GetProjectsBySearchString(string searchTerm)
        {
            return db.Projects
                      .AsNoTracking()
                       .Where(s => ((((s.Description.Contains(searchTerm) || s.Description.StartsWith(searchTerm) || s.Description.EndsWith(searchTerm))))
                                 || (((s.Code.Contains(searchTerm) || s.Code.StartsWith(searchTerm) || s.Code.EndsWith(searchTerm)))))
                                 && s.isActive == true)
                        .Select(Mapper.Map)
                         .OrderBy(o => o.Code)
                          .AsQueryable();
        }

        public OrderProjectViewModel GetProjectById(int Id)
        {
            return Mapper.Map(db.Projects.FirstOrDefault(x => x.Id == Id));
        }

        public IQueryable<OrderProjectViewModel> GetProjects()
        {
            return db.Projects
                      .AsNoTracking()
                        .Select(Mapper.Map)
                         .OrderBy(o => o.Code)
                          .AsQueryable();
        }

        public List<OrderProjectViewModel> GetProjects(string searchTerm, int pageSize, int pageNum)
        {
            return GetProjectsBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetProjectsCount(string searchTerm)
        {
            return GetProjectsBySearchString(searchTerm).Count();
        }

        public List<UserViewModel> GetOrderCreators(string searchTerm, int pageSize, int pageNum, bool isTransport)
        {
            return GetOrderCreatorsBySearchString(searchTerm, isTransport)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetOrderCreatorsCount(string searchTerm, bool isTransport)
        {
            return GetOrderCreatorsBySearchString(searchTerm, isTransport).Count();
        }

        public IQueryable<UserViewModel> GetOrderCreatorsBySearchString(string searchTerm, bool isTransport)
        {
            return
            db.AspNetUsers
                  .AsNoTracking()
                     .Where(s => (((s.DisplayName.Contains(searchTerm) || s.DisplayName.StartsWith(searchTerm) || s.DisplayName.EndsWith(searchTerm))
                     ) && (s.OrdersBase.Where(x => x.OrderTypesBase.IsTransportType == isTransport).Count() > 0)))
                        .Select(Mapper.Map)
                        .OrderBy(o => o.displayName)
                         .AsQueryable();
        }


        public List<OrderTypeViewModel> GetOrderTypes(string searchTerm, int pageSize, int pageNum, bool isTransport)
        {
            return GetOrderTypesBySearchString(searchTerm, isTransport)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetOrderTypesCount(string searchTerm, bool isTransport)
        {
            return GetOrderTypesBySearchString(searchTerm, isTransport).Count();
        }

        public IQueryable<OrderTypeViewModel> GetOrderTypesBySearchString(string searchTerm, bool isTransport)
        {
            return
            db.OrderTypesBase
                  .AsNoTracking()
                     .Where(s => ((s.TypeName.Contains(searchTerm) || s.TypeName.StartsWith(searchTerm) || s.TypeName.EndsWith(searchTerm)))
                                 && s.OrdersBase.Where(x => x.OrderTypesBase.IsTransportType == isTransport).Count() > 0
                                 && s.IsTransportType == isTransport)
                        .Select(Mapper.Map)
                        .OrderBy(o => o.TypeName)
                         .AsQueryable();
        }

        public List<OrderClientsViewModel> GetOrderClients(string searchTerm, int pageSize, int pageNum, bool isTransport)
        {
            return GetOrderClientsBySearchString(searchTerm, isTransport)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetOrderClientsCount(string searchTerm, bool isTransport)
        {
            return GetOrderClientsBySearchString(searchTerm, isTransport).Count();
        }

        public IQueryable<OrderClientsViewModel> GetOrderClientsBySearchString(string searchTerm, bool isTransport)
        {
            return
            db.OrderClients
                  .AsNoTracking()
                     .Where(s => ((s.ClientName.Contains(searchTerm) || s.ClientName.StartsWith(searchTerm) || s.ClientName.EndsWith(searchTerm)))
                              && (s.Centers.Center.Contains(searchTerm) || s.Centers.Center.StartsWith(searchTerm) || s.Centers.Center.EndsWith(searchTerm))
                              && s.OrdersBase.Where(x => x.OrderTypesBase.IsTransportType == isTransport).Count() > 0
                              )
                        .Select(Mapper.Map)
                        .OrderBy(o => o.ClientName)
                         .AsQueryable();
        }

        public List<OrderCountriesViewModel> GetOrderCountries(string searchTerm, int pageSize, int pageNum)
        {
            return GetOrderCountriesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetOrderCountriesCount(string searchTerm)
        {
            return GetOrderCountriesBySearchString(searchTerm).Count();
        }

        public IQueryable<OrderCountriesViewModel> GetOrderCountriesBySearchString(string searchTerm)
        {
            return
            db.Countries
                  .AsNoTracking()
                     .Where(s => ((s.Name.Contains(searchTerm) || s.Name.StartsWith(searchTerm) || s.Name.EndsWith(searchTerm))))
                        .Select(Mapper.Map)
                        .OrderBy(o => o.IsDefault).OrderBy(o => o.CountryName)
                         .AsQueryable();
        }

        public List<UserViewModel> GetOrderExecutors(string searchTerm, int pageSize, int pageNum, bool isTransport)
        {
            return GetOrderExecutorsBySearchString(searchTerm, isTransport)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetOrderExecutorsCount(string searchTerm, bool isTransport)
        {
            return GetOrderExecutorsBySearchString(searchTerm, isTransport).Count();
        }

        public List<UserViewModel> GetOrderExecutorsEx(int orderTypeId)
        {
            var result = db.GetOrderExecuters(orderTypeId).ToList();
            return result.Select(Mapper.Map).ToList();
        }

        public IQueryable<UserViewModel> GetOrderExecutorsBySearchString(string searchTerm, bool isTransport)
        {

            return
            db.AspNetUsers
                  .AsNoTracking()
                     .Where(s => (((s.DisplayName.Contains(searchTerm) || s.DisplayName.StartsWith(searchTerm) || s.DisplayName.EndsWith(searchTerm))) && (s.OrdersBase1.Where(x => x.OrderTypesBase.IsTransportType == isTransport).Count() > 0)))
                        .Select(Mapper.Map)
                        .OrderBy(o => o.displayName)
                         .AsQueryable();
        }


        public IQueryable<UserViewModel> GetReceiversBySearchString(string searchTerm, long? Id)
        {
            return (from Us in db.AspNetUsers
                    join Oo in db.OrderObservers on Us.Id equals Oo.userId
                    join Ob in db.OrdersBase on Oo.OrderId equals Ob.Id
                    where Ob.Id == Id
                    select Us)
                           .Select(Mapper.Map)
                           .OrderBy(o => o.displayName)
                           .AsQueryable();
        }

        public List<UserViewModel> GetReceivers(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetReceiversBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetReceiverCount(string searchTerm, long? Id)
        {
            return GetReceiversBySearchString(searchTerm, Id).Count();
        }

        public OrderCountriesViewModel getDefaultCountry()
        {
            return Mapper.Map(db.Countries.AsNoTracking().FirstOrDefault(c => c.IsDefault == true));
        }


        public int getCountryByUserId(string userId)
        {
            var profile = db.UserProfile.FirstOrDefault(c => c.UserId == userId);
            if (profile != null)
            {
                return profile.CountryId ?? 0;
            }
            else return 0;
        }

        public string getCountryNameByUserId(int UserCountryId)
        {
            return db.Countries.FirstOrDefault(c => c.Сode == UserCountryId).Name;
        }

        public UserProfileViewModel getUserProfileByUserId(string userId)
        {
            var UserProfileInfo = Mapper.Map(db.UserProfile.FirstOrDefault(c => c.UserId == userId));
            if (UserProfileInfo == null) return null;

            return UserProfileInfo;
        }

        public OrderCountriesViewModel getCountryById(int Id)
        {
            return Mapper.Map(db.Countries.AsNoTracking().FirstOrDefault(c => c.Сode == Id));
        }


        public string GetFromInfoForExport(long orderId)
        {
            var orderInfo = db.OrdersPassengerTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

            if (orderInfo != null)
            {

                if (orderInfo.TripType == 2)
                {
                    return string.Concat(orderInfo.OrgFrom, ", ", orderInfo.Countries == null ? null : orderInfo.Countries.Name, ", ", orderInfo.FromCity, ", ", orderInfo.AdressFrom);
                }
                else
                { return string.Concat(orderInfo.OrgFrom, ", ", orderInfo.FromCity, ", ", orderInfo.AdressFrom); }

            }

            return string.Empty;
        }

        public string GetToInfoForExport(long orderId)
        {
            var orderInfo = db.OrdersPassengerTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

            if (orderInfo != null)
            {
                //return string.Concat(orderInfo.Countries1 == null ? null : orderInfo.Countries1.Name, ", ", orderInfo.ToCity, ", ", orderInfo.AdressTo, ", ", orderInfo.OrgTo);

                if (orderInfo.TripType == 2)
                {
                    return string.Concat(orderInfo.OrgTo, ", ", orderInfo.Countries1 == null ? null : orderInfo.Countries1.Name, ", ",
                          orderInfo.ToCity, ", ", orderInfo.AdressTo);
                }
                else
                { return string.Concat(orderInfo.OrgTo, ", ", orderInfo.ToCity, ", ", orderInfo.AdressTo); }

            }

            return string.Empty;
        }


        public long NewUsedCar(OrderUsedCarViewModel model)
        {
            var car = new OrderUsedCars()
            {
                OrderId = model.OrderId,
                CarOwnerInfo = model.CarOwnerInfo,
                CarModelInfo = model.CarModelInfo,
                CarRegNum = model.CarRegNum,
                CarCapacity = model.CarCapacity,
                CarDriverInfo = model.CarDriverInfo,
                DriverContactInfo = model.DriverContactInfo,
                CarrierInfo = model.CarrierInfo,
                ContractInfo = model.ContractInfo,
                ContractExpBkId = model.ContractExpBkId,
                ExpeditorId = model.ExpeditorId,
                DriverCardInfo = model.DriverCardInfo,
                Comments = model.Comments,
                /*
                FactShipperDateTime = DateTimeConvertClass.getDateTime(model.FactShipperDate).
                                                           AddHours(DateTimeConvertClass.getHours(model.FactShipperTime)).
                                                           AddMinutes(DateTimeConvertClass.getMinutes(model.FactShipperTime)),

                 FactConsigneeDateTime = DateTimeConvertClass.getDateTime(model.FactConsigneeDate).
                                                        AddHours(DateTimeConvertClass.getHours(model.FactConsigneeTime)).
                                                        AddMinutes(DateTimeConvertClass.getMinutes(model.FactConsigneeTime)),*/
                Summ = 0
            };

            db.OrderUsedCars.Add(car);
            db.SaveChanges();

            return car.Id;

        }

        public bool UpdateUsedCar(OrderUsedCarViewModel model)
        {
            var dbInfo = db.OrderUsedCars.FirstOrDefault(u => u.Id == model.Id);
            if (dbInfo == null) return false;

            dbInfo.OrderId = model.OrderId;
            dbInfo.CarOwnerInfo = model.CarOwnerInfo;
            dbInfo.CarModelInfo = model.CarModelInfo;
            dbInfo.CarRegNum = model.CarRegNum;
            dbInfo.CarCapacity = model.CarCapacity;
            dbInfo.CarDriverInfo = model.CarDriverInfo;
            dbInfo.DriverContactInfo = model.DriverContactInfo;
            dbInfo.CarrierInfo = model.CarrierInfo;
            dbInfo.ContractInfo = model.ContractInfo;
            dbInfo.ContractExpBkId = model.ContractExpBkId;
            dbInfo.ExpeditorId = model.ExpeditorId;
            dbInfo.DriverCardInfo = model.DriverCardInfo;
            dbInfo.Comments = model.Comments;

            /*   dbInfo.FactShipperDateTime = DateTimeConvertClass.getDateTime(model.FactShipperDate).
                                                          AddHours(DateTimeConvertClass.getHours(model.FactShipperTime)).
                                                          AddMinutes(DateTimeConvertClass.getMinutes(model.FactShipperTime));

           dbInfo.FactConsigneeDateTime = DateTimeConvertClass.getDateTime(model.FactConsigneeDate).
                                                       AddHours(DateTimeConvertClass.getHours(model.FactConsigneeTime)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(model.FactConsigneeTime));*/
            dbInfo.Summ = 0;

            db.SaveChanges();

            return true;
        }

        public bool UpdateUsedCarAddInfo(OrderUsedCarViewModel model)
        {
            var dbInfo = db.OrderUsedCars.FirstOrDefault(u => u.Id == model.Id);
            if (dbInfo == null) return false;

            dbInfo.PlanDistance = Convert.ToDecimal(model.PlanDistance.Replace(".", ","));
            dbInfo.PlanTimeWorkDay = model.PlanTimeWorkDay;
            dbInfo.PlanTimeHoliday = model.PlanTimeHoliday;
            dbInfo.BaseRate = Convert.ToDecimal(model.BaseRate.Replace(".", ","));
            dbInfo.BaseRateWorkDay = Convert.ToDecimal(model.BaseRateWorkDay.Replace(".", ","));
            dbInfo.BaseRateHoliday = Convert.ToDecimal(model.BaseRateHoliday.Replace(".", ","));
            dbInfo.DelayDays = model.DelayDays;

            db.SaveChanges();

            return true;
        }

        public IQueryable<OrderFilterSettingsModel> GetFilterSettingsBySearchString(string searchTerm, string userId)
        {
            var OrderFilterSettings2 = db.OrderFilterSettings2
                .AsNoTracking()
                .Where(c => c.IdCurrentUser == userId)
                .Select(Mapper.Map)
                .OrderBy(o => o.NameFilter)
                .ToList();
            // .AsQueryable();

            int ii = 0;
            foreach (var i in OrderFilterSettings2)
            {
                var OrderFilters = db.OrderFilters.Where(x => x.OrderFilterSetId == i.Id);

                string FilterStatusId = "";
                foreach (var Status in OrderFilters)
                {
                    if (Status.StatusId != null)
                    {
                        if (FilterStatusId.Length > 0)
                        {
                            FilterStatusId += ",";
                        }

                        FilterStatusId = string.Concat(FilterStatusId, Status.StatusId);
                    }
                }

                OrderFilterSettings2[ii].StatusId = FilterStatusId;

                string FilterOrderClientId = "";
                foreach (var Client in OrderFilters)
                {
                    if (Client.ClientId != null)
                    {

                        if (FilterOrderClientId.Length > 0)
                        {
                            FilterOrderClientId += ",";
                        }

                        FilterOrderClientId = string.Concat(FilterOrderClientId, Client.ClientId);
                    }
                }

                OrderFilterSettings2[ii].ClientId = FilterOrderClientId;

                string FilterOrderCreatorId = "";
                foreach (var Creator in OrderFilters)
                {
                    if (Creator.CreatorId != null)
                    {
                        if (FilterOrderCreatorId.Length > 0)
                        {
                            FilterOrderCreatorId += ",";
                        }
                        FilterOrderCreatorId = string.Concat(FilterOrderCreatorId, Creator.CreatorId);
                    }
                }

                OrderFilterSettings2[ii].CreatorId = FilterOrderCreatorId;

                string FilterOrderExecuterId = "";
                foreach (var Executer in OrderFilters)
                {
                    if (Executer.ExecuterId != null)
                    {
                        if (FilterOrderExecuterId.Length > 0)
                        {
                            FilterOrderExecuterId += ",";
                        }
                        FilterOrderExecuterId = string.Concat(FilterOrderExecuterId, Executer.ExecuterId);
                    }
                }

                OrderFilterSettings2[ii].ExecuterId = FilterOrderExecuterId;


                string FilterOrderPayerId = "";
                foreach (var Payer in OrderFilters)
                {
                    if (Payer.PayerId != null)
                    {
                        if (FilterOrderPayerId.Length > 0)
                        {
                            FilterOrderPayerId += ",";
                        }
                        FilterOrderPayerId = string.Concat(FilterOrderPayerId, Payer.PayerId);
                    }
                }

                OrderFilterSettings2[ii].PayerId = FilterOrderPayerId;

                string FilterOrderOrgFromId = "";
                foreach (var OrgFrom in OrderFilters)
                {
                    if (OrgFrom.OrgFromId != null)
                    {
                        if (FilterOrderOrgFromId.Length > 0)
                        {
                            FilterOrderOrgFromId += ",";
                        }
                        FilterOrderOrgFromId = string.Concat(FilterOrderOrgFromId, OrgFrom.OrgFromId);
                    }
                }

                OrderFilterSettings2[ii].OrgFromId = FilterOrderOrgFromId;

                string FilterOrderOrgToId = "";
                foreach (var OrgTo in OrderFilters)
                {
                    if (OrgTo.OrgToId != null)
                    {
                        if (FilterOrderOrgToId.Length > 0)
                        {
                            FilterOrderOrgToId += ",";
                        }
                        FilterOrderOrgToId = string.Concat(FilterOrderOrgToId, OrgTo.OrgToId);
                    }
                }

                OrderFilterSettings2[ii].OrgToId = FilterOrderOrgToId;

                string FilterOrderTypeId = "";
                foreach (var Type in OrderFilters)
                {
                    if (Type.TypeId != null)
                    {

                        if (FilterOrderTypeId.Length > 0)
                        {
                            FilterOrderTypeId += ",";
                        }

                        FilterOrderTypeId = string.Concat(FilterOrderTypeId, Type.TypeId);
                    }
                }
                OrderFilterSettings2[ii].TypeId = FilterOrderTypeId;

                ii++;
            }

            return OrderFilterSettings2.AsQueryable();

        }

        public List<OrderFilterSettingsModel> GetFilterSettings(string searchTerm, int pageSize, int pageNum, string userId)
        {
            return GetFilterSettingsBySearchString(searchTerm, userId)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetFilterSettingsCount(string searchTerm, string userId)
        {
            return GetFilterSettingsBySearchString(searchTerm, userId).Count();
        }

        public List<OrderFilterSettingsModel> GetFilterSettingsBtn(int groupSize, int fromNumb, string userId)
        {
            return GetFilterSettingsBySearchString("", userId)
                  .Skip(fromNumb - 1)
                  .Take(groupSize)
                  .ToList();
        }

        public OrderFilterSettingsModel getOrderFilterSettingById(int Id)
        {
            //return Mapper.Map(db.OrderFilterSettings2.AsNoTracking().FirstOrDefault(c => c.Id ==Id));

            var OrderFilterSettings2 = Mapper.Map(db.OrderFilterSettings2.AsNoTracking().FirstOrDefault(c => c.Id == Id));

            var OrderFilters = db.OrderFilters.Where(x => x.OrderFilterSetId == OrderFilterSettings2.Id);

            string FilterStatusId = "";
            foreach (var Status in OrderFilters)
            {
                if (Status.StatusId != null)
                {
                    if (FilterStatusId.Length > 0)
                    {
                        FilterStatusId += ",";
                    }

                    FilterStatusId = string.Concat(FilterStatusId, Status.StatusId);
                }
            }

            OrderFilterSettings2.StatusId = FilterStatusId;


            string FilterOrderClientId = "";
            foreach (var Client in OrderFilters)
            {
                if (Client.ClientId != null)
                {

                    if (FilterOrderClientId.Length > 0)
                    {
                        FilterOrderClientId += ",";
                    }

                    FilterOrderClientId = string.Concat(FilterOrderClientId, Client.ClientId);
                }
            }

            OrderFilterSettings2.ClientId = FilterOrderClientId;

            string FilterOrderCreatorId = "";
            foreach (var Creator in OrderFilters)
            {
                if (Creator.CreatorId != null)
                {
                    if (FilterOrderCreatorId.Length > 0)
                    {
                        FilterOrderCreatorId += ",";
                    }
                    FilterOrderCreatorId = string.Concat(FilterOrderCreatorId, Creator.CreatorId);
                }
            }

            OrderFilterSettings2.CreatorId = FilterOrderCreatorId;

            string FilterOrderExecuterId = "";
            foreach (var Executer in OrderFilters)
            {
                if (Executer.ExecuterId != null)
                {
                    if (FilterOrderExecuterId.Length > 0)
                    {
                        FilterOrderExecuterId += ",";
                    }
                    FilterOrderExecuterId = string.Concat(FilterOrderExecuterId, Executer.ExecuterId);
                }
            }

            OrderFilterSettings2.ExecuterId = FilterOrderExecuterId;

            string FilterOrderTypeId = "";
            foreach (var Type in OrderFilters)
            {
                if (Type.TypeId != null)
                {

                    if (FilterOrderTypeId.Length > 0)
                    {
                        FilterOrderTypeId += ",";
                    }

                    FilterOrderTypeId = string.Concat(FilterOrderTypeId, Type.TypeId);
                }
            }
            OrderFilterSettings2.TypeId = FilterOrderTypeId;

            string FilterOrderPayerId = "";
            foreach (var Payer in OrderFilters)
            {
                if (Payer.PayerId != null)
                {
                    if (FilterOrderPayerId.Length > 0)
                    {
                        FilterOrderPayerId += ",";
                    }

                    FilterOrderPayerId = string.Concat(FilterOrderPayerId, Payer.PayerId);
                }
            }
            OrderFilterSettings2.PayerId = FilterOrderPayerId;

            string FilterOrgFromId = "";
            foreach (var OrgFrom in OrderFilters)
            {
                if (OrgFrom.OrgFromId != null)
                {
                    if (FilterOrgFromId.Length > 0)
                    {
                        FilterOrgFromId += ",";
                    }

                    FilterOrgFromId = string.Concat(FilterOrgFromId, OrgFrom.OrgFromId);
                }
            }
            OrderFilterSettings2.OrgFromId = FilterOrgFromId;

            string FilterOrgToId = "";
            foreach (var OrgTo in OrderFilters)
            {
                if (OrgTo.OrgToId != null)
                {
                    if (FilterOrgToId.Length > 0)
                    {
                        FilterOrgToId += ",";
                    }

                    FilterOrgToId = string.Concat(FilterOrgToId, OrgTo.OrgToId);
                }
            }
            OrderFilterSettings2.OrgToId = FilterOrgToId;

            return OrderFilterSettings2;
        }


        public void AddOrderFilter(OrderFilterSettingsModel model)
        {

            /* var orderFilterInfo = new OrderFilterSettings()
            {
                NameFilter = model.NameFilter,
                //StatusId = model.StatusId,
                CreatorId = model.CreatorId,
                ExecuterId = model.ExecuterId,
                TypeId = model.TypeId,
                ClientId = model.ClientId,
                PriorityType = model.PriorityType,
                DeltaDateBeg = model.DeltaDateBeg,
                DeltaDateEnd = model.DeltaDateEnd,
                DeltaDateBegEx = model.DeltaDateBegEx,
                DeltaDateEndEx = model.DeltaDateEndEx,
                UseStatusFilter = model.UseStatusFilter,
                UseCreatorFilter = model.UseCreatorFilter,
                UseExecuterFilter = model.UseExecuterFilter,
                UseClientFilter = model.UseClientFilter,
                UseTypeFilter = model.UseTypeFilter,
                UsePriorityFilter = model.UsePriorityFilter,
                UseDateFilter = model.UseDateFilter,
                UseExDateFilter = model.UseExDateFilter,
                IdCurrentUser = model.UserCurrentId

        };}*/

            var orderFilterInfo = new OrderFilterSettings2()
            {
                NameFilter = model.NameFilter,
                PriorityType = model.PriorityType,
                DeltaDateBeg = model.DeltaDateBeg,
                DeltaDateEnd = model.DeltaDateEnd,
                DeltaDateBegEx = model.DeltaDateBegEx,
                DeltaDateEndEx = model.DeltaDateEndEx,
                UseStatusFilter = model.UseStatusFilter,
                UseCreatorFilter = model.UseCreatorFilter,
                UseExecuterFilter = model.UseExecuterFilter,
                UseClientFilter = model.UseClientFilter,
                UseTypeFilter = model.UseTypeFilter,
                UsePriorityFilter = model.UsePriorityFilter,
                UseDateFilter = model.UseDateFilter,
                UseExDateFilter = model.UseExDateFilter,
                IdCurrentUser = model.UserCurrentId,
                UseOrderPayerFilter = model.UseOrderPayerFilter,
                UseOrderOrgFromFilter = model.UseOrderOrgFromFilter,
                UseOrderOrgToFilter = model.UseOrderOrgToFilter
            };

            db.OrderFilterSettings2.Add(orderFilterInfo);

            db.SaveChanges();

            //int id = orderFilterInfo.Id;
            if ((model.UseStatusFilter) && (model.StatusId.Length > 0))
            {
                int[] intStatusId = model.StatusId.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                foreach (int i in intStatusId)
                {
                    var orderFilterAddInfo = new OrderFilters()
                    {
                        StatusId = i,
                        CreatorId = null,
                        ExecuterId = null,
                        TypeId = null,
                        ClientId = null,
                        PayerId = null,
                        OrgFromId = null,
                        OrgToId = null,
                        OrderFilterSetId = orderFilterInfo.Id
                    };

                    db.OrderFilters.Add(orderFilterAddInfo);
                    db.SaveChanges();
                }
            }

            if ((model.UseCreatorFilter) && (model.CreatorId.Length > 0))
            {
                string[] intCreatorId = model.CreatorId.Split(new char[] { ',' });

                foreach (string i in intCreatorId)
                {
                    var orderFilterAddInfo = new OrderFilters()
                    {
                        StatusId = null,
                        CreatorId = i,
                        ExecuterId = null,
                        TypeId = null,
                        ClientId = null,
                        PayerId = null,
                        OrgFromId = null,
                        OrgToId = null,
                        OrderFilterSetId = orderFilterInfo.Id
                    };

                    db.OrderFilters.Add(orderFilterAddInfo);
                    db.SaveChanges();
                }
            }

            if ((model.UseExecuterFilter) && (model.ExecuterId.Length > 0))
            {
                string[] intExecuterId = model.ExecuterId.Split(new char[] { ',' });

                foreach (string i in intExecuterId)
                {
                    var orderFilterAddInfo = new OrderFilters()
                    {
                        StatusId = null,
                        CreatorId = null,
                        ExecuterId = i,
                        TypeId = null,
                        ClientId = null,
                        PayerId = null,
                        OrgFromId = null,
                        OrgToId = null,
                        OrderFilterSetId = orderFilterInfo.Id
                    };

                    db.OrderFilters.Add(orderFilterAddInfo);
                    db.SaveChanges();
                }
            }

            if ((model.UseTypeFilter) && (model.TypeId.Length > 0))
            {
                int[] intTypeId = model.TypeId.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                foreach (int i in intTypeId)
                {
                    var orderFilterAddInfo = new OrderFilters()
                    {
                        StatusId = null,
                        CreatorId = null,
                        ExecuterId = null,
                        TypeId = i,
                        ClientId = null,
                        PayerId = null,
                        OrgFromId = null,
                        OrgToId = null,
                        OrderFilterSetId = orderFilterInfo.Id
                    };

                    db.OrderFilters.Add(orderFilterAddInfo);
                    db.SaveChanges();
                }
            }


            if ((model.UseClientFilter) && (model.ClientId.Length > 0))
            {
                int[] intClientId = model.ClientId.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                foreach (int i in intClientId)
                {
                    var orderFilterAddInfo = new OrderFilters()
                    {
                        StatusId = null,
                        CreatorId = null,
                        ExecuterId = null,
                        TypeId = null,
                        ClientId = i,
                        PayerId = null,
                        OrgFromId = null,
                        OrgToId = null,
                        OrderFilterSetId = orderFilterInfo.Id
                    };

                    db.OrderFilters.Add(orderFilterAddInfo);
                    db.SaveChanges();
                }
            }

            if ((model.UseOrderPayerFilter) && (model.PayerId.Length > 0))
            {
                int[] intPayerId = model.PayerId.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                foreach (int i in intPayerId)
                {
                    var orderFilterAddInfo = new OrderFilters()
                    {
                        StatusId = null,
                        CreatorId = null,
                        ExecuterId = null,
                        TypeId = null,
                        ClientId = null,
                        PayerId = i,
                        OrgFromId = null,
                        OrgToId = null,
                        OrderFilterSetId = orderFilterInfo.Id
                    };

                    db.OrderFilters.Add(orderFilterAddInfo);
                    db.SaveChanges();
                }
            }

            if ((model.UseOrderOrgFromFilter) && (model.OrgFromId.Length > 0))
            {
                int[] intOrgFromId = model.OrgFromId.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                foreach (int i in intOrgFromId)
                {
                    var orderFilterAddInfo = new OrderFilters()
                    {
                        StatusId = null,
                        CreatorId = null,
                        ExecuterId = null,
                        TypeId = null,
                        ClientId = null,
                        PayerId = null,
                        OrgFromId = i,
                        OrgToId = null,
                        OrderFilterSetId = orderFilterInfo.Id
                    };

                    db.OrderFilters.Add(orderFilterAddInfo);
                    db.SaveChanges();
                }
            }

            if ((model.UseOrderOrgToFilter) && (model.OrgToId.Length > 0))
            {
                int[] intOrgToId = model.OrgToId.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                foreach (int i in intOrgToId)
                {
                    var orderFilterAddInfo = new OrderFilters()
                    {
                        StatusId = null,
                        CreatorId = null,
                        ExecuterId = null,
                        TypeId = null,
                        ClientId = null,
                        PayerId = null,
                        OrgFromId = null,
                        OrgToId = i,
                        OrderFilterSetId = orderFilterInfo.Id
                    };

                    db.OrderFilters.Add(orderFilterAddInfo);
                    db.SaveChanges();
                }
            }
        }



        public void RemoveOrderFilter(int Id)
        {
            var dbInfo = db.OrderFilterSettings2.FirstOrDefault(u => u.Id == Id);
            if (dbInfo == null) return;

            var FilterInfo = db.OrderFilters.Where(u => u.OrderFilterSetId == Id).ToList();

            foreach (var i in FilterInfo)
            {
                db.OrderFilters.Remove(i);
                db.SaveChanges();
            }

            db.OrderFilterSettings2.Remove(dbInfo);
            db.SaveChanges();
        }



        public List<AvailableRoles> GetRoles(string searchTerm, int pageSize, int pageNum, string userId)
        {

            return GetRolesBySearchString(searchTerm, userId)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetRolesCount(string searchTerm, string userId)
        {
            return GetRolesBySearchString(searchTerm, userId).Count();
        }

        public IQueryable<AvailableRoles> GetRolesBySearchString(string searchTerm, string userId)
        {
            List<AvailableRoles> roles = new List<AvailableRoles>();

            //отбираем группы ролей
            var groupRoleItems = db.RoleGroups.ToList();

            foreach (var groupRoleItem in groupRoleItems)
            {
                AvailableRoles groupRoleInfoItem = new AvailableRoles();
                groupRoleInfoItem.text = groupRoleItem.Name;

                //берем только последнюю группу, которая уже непосредственно привязана к ролям
                if (!(RoleGroupsHasChild(groupRoleItem.Id)))
                {
                    List<ChildrenRoles> GroupRole = new List<ChildrenRoles>();
                    //отбираем роли
                    var GroupRoles = (from R in db.AspNetRoles
                                      join RGR in db.RoleGroupsRole on R.Id equals RGR.RoleId
                                      where RGR.RoleGroupsId == groupRoleItem.Id
                                      select new GroupRoleViewModel()
                                      {
                                          RoleId = R.Id,
                                          RoleName = R.Name,
                                      }).ToList();

                    foreach (var GroupRolesItem in GroupRoles)
                    {
                        ChildrenRoles child = new ChildrenRoles();
                        child.id = GroupRolesItem.RoleId;
                        child.text = GroupRolesItem.RoleName;

                        GroupRole.Add(child);
                    }
                    List<ChildrenRoles> childSearch = new List<ChildrenRoles>();

                    childSearch =
                        GroupRole.Where(
                            u =>
                                (((searchTerm == "") ||
                                  ((searchTerm != "") && ((u.text.ToUpper().StartsWith(searchTerm.ToUpper()))
                                                          || (u.text.ToUpper().Contains(searchTerm.ToUpper()))
                                                          || (u.text.ToUpper().EndsWith(searchTerm.ToUpper())))))
                                    )).ToList();

                    groupRoleInfoItem.children = childSearch;
                    if (groupRoleInfoItem.children.Count > 0)
                        roles.Add(groupRoleInfoItem);
                    else
                    {
                        if (((searchTerm == "") ||
                             ((searchTerm != "") && ((groupRoleInfoItem.text.ToUpper().StartsWith(searchTerm.ToUpper()))
                                                     ||
                                                     (groupRoleInfoItem.text.ToUpper().Contains(searchTerm.ToUpper()))
                                                     ||
                                                     (groupRoleInfoItem.text.ToUpper().EndsWith(searchTerm.ToUpper()))))))

                            roles.Add(groupRoleInfoItem);
                    }
                }
            }

            return roles
                           .OrderBy(o => o.text)
                            .AsQueryable();


        }

        public string getContactName(long OrderId)
        {
            var OrderUsedCarsInfo = db.OrderUsedCars.FirstOrDefault(u => u.OrderId == OrderId);
            if (OrderUsedCarsInfo == null) return "";
            return OrderUsedCarsInfo.ContractInfo;

        }

        public IQueryable<TruckTypeViewModel> GetTruckTypesBySearchString(string searchTerm)
        {
            return db.OrderTruckTypes
                  .AsNoTracking()
                        .Select(Mapper.Map)
                         .OrderBy(o => o.Id)
                         .AsQueryable();
        }
        public List<TruckTypeViewModel> TruckTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetTruckTypesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int TruckTypesCount(string searchTerm)
        {
            return GetTruckTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<VehicleViewModel> GetVehicleTypesBySearchString(string searchTerm)
        {
            return db.OrderVehicleTypes
                 .AsNoTracking()
                       .Select(Mapper.Map)
                        .OrderBy(o => o.Id)
                        .AsQueryable();
        }

        public List<VehicleViewModel> VehicleTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetVehicleTypesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int VehicleTypesCount(string searchTerm)
        {
            return GetVehicleTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<LoadingTypeViewModel> GetLoadingTypesBySearchString(string searchTerm)
        {
            return db.OrderLoadingTypes
                 .AsNoTracking()
                       .Select(Mapper.Map)
                        .OrderBy(o => o.Id)
                        .AsQueryable();
        }

        public List<LoadingTypeViewModel> LoadingTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetLoadingTypesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int LoadingTypesCount(string searchTerm)
        {
            return GetLoadingTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<UnloadingTypeViewModel> GetUnloadingTypesBySearchString(string searchTerm)
        {
            return db.OrderUnloadingTypes
                 .AsNoTracking()
                       .Select(Mapper.Map)
                        .OrderBy(o => o.Id)
                        .AsQueryable();
        }

        public List<UnloadingTypeViewModel> UnloadingTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetUnloadingTypesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int UnloadingTypesCount(string searchTerm)
        {
            return GetUnloadingTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<OrderObserverViewModel> getOrderAssignObservers(long Id, string[] ObserversList)
        {
            var allOrderObservers = db.OrderObservers
                .AsNoTracking()
                .Where(osh => osh.OrderId == Id)
                .Select(Mapper.Map)
                .OrderByDescending(o => o.Id)
                .ToList();

            List<OrderObserverViewModel> orderAssignObservers = new List<OrderObserverViewModel>();
            if (ObserversList != null)
            {
                foreach (var observer in ObserversList)
                {
                    orderAssignObservers.Add(allOrderObservers.FirstOrDefault(x => x.observerId == observer));
                }
            }
            return orderAssignObservers.AsQueryable();
        }


        public IQueryable<OrderClientsViewModel> GetClientsBySearchString(string searchTerm, string userId)
        {
            if (this.getUser(userId).isAdmin)
            {
                return this.getClients(userId, searchTerm);
            }
            else
            {
                return this.getClientsInPipeline(userId, searchTerm);
            }
        }

        public List<OrderClientsViewModel> Clients(string searchTerm, int pageSize, int pageNum, string userId)
        {
            return GetClientsBySearchString(searchTerm, userId)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int ClientsCount(string searchTerm, string userId)
        {
            return GetClientsBySearchString(searchTerm, userId).Count();
        }


        public IQueryable<OrderClientBalanceKeeperViewModel> GetPayersBySearchString(string searchTerm, string userId)
        {
            return db.BalanceKeepers
                           .AsNoTracking()
                           .Where(s => (((s.BalanceKeeper.Contains(searchTerm) || s.BalanceKeeper.StartsWith(searchTerm) || s.BalanceKeeper.EndsWith(searchTerm)))))
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();
        }

        public List<OrderClientBalanceKeeperViewModel> Payers(string searchTerm, int pageSize, int pageNum, string userId)
        {
            return GetPayersBySearchString(searchTerm, userId)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int PayersCount(string searchTerm, string userId)
        {
            return GetPayersBySearchString(searchTerm, userId).Count();
        }

        public List<OrderBaseViewModel> GetOrders(string searchTerm, int pageSize, int pageNum)
        {

            return GetOrdersBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();

        }

        public IQueryable<OrderBaseViewModel> GetOrdersBySearchString(string searchTerm)
        {
            return
            db.OrdersBase
                  .AsNoTracking()
                  .Where(s => (((s.Id.ToString().Contains(searchTerm) || s.Id.ToString().StartsWith(searchTerm) || s.Id.ToString().EndsWith(searchTerm)))))
                        .Select(Mapper.Map)
                         .OrderBy(o => o.Id)
                         .AsQueryable();
        }

        public int GetOrderCount(string searchTerm)
        {
            return GetOrdersBySearchString(searchTerm).Count();
        }

        public IQueryable<CarOwnersAccessViewModel> GetExpeditorNameBySearchString(string searchTerm, long? Id)
        {
            return (from Us in db.CarOwners
                    where Us.IsForwarder == true
                    select Us)
                           .Select(Mapper.Map)
                           .Where(s => (((s.CarrierName.Contains(searchTerm) || s.CarrierName.StartsWith(searchTerm) || s.CarrierName.EndsWith(searchTerm)))))
                           .OrderBy(Us => Us.CarrierName)
                           .AsQueryable();
        }

        public List<CarOwnersAccessViewModel> GetExpeditorName(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetExpeditorNameBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetExpeditorNameCount(string searchTerm, long? Id)
        {
            return GetExpeditorNameBySearchString(searchTerm, Id).Count();
        }




        public IQueryable<ContractsViewModel> GetContractExpCarInfoBySearchString(string searchTerm, int? urlData1, int? urlData2)
        {
            return (from Us in db.Contracts
                    where Us.CarOwnersId == urlData2 &&
                          Us.ExpeditorId == urlData1 &&
                          (Us.IsActive == true || Us.IsActive == null)
                    select Us)
                           .Select(Mapper.Map)
                           .OrderBy(Us => Us.ContractNumber)
                           .AsQueryable();
        }

        public List<ContractsViewModel> GetContractExpCarrierInfo(string searchTerm, int pageSize, int pageNum, int? urlData1, int? urlData2)
        {
            return GetContractExpCarInfoBySearchString(searchTerm, urlData1, urlData2)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetContractExpCarrierInfoCount(string searchTerm, int? urlData1, int? urlData2)
        {
            return GetContractExpCarInfoBySearchString(searchTerm, urlData1, urlData2).Count();
        }

        public IQueryable<ContractsViewModel> GetContractExpBkInfoBySearchString(string searchTerm, int urlData1, int urlData2)
        {
            return (from Us in db.Contracts
                    where Us.CarOwnersId == urlData1 &&
                          Us.BalanceKeeperId == urlData2 &&
                          (Us.IsActive == true || Us.IsActive == null)
                    select Us)
                           .Select(Mapper.Map)
                           .OrderBy(Us => Us.ContractNumber)
                           .AsQueryable();
        }

        public List<ContractsViewModel> GetContractExpBkInfo(string searchTerm, int pageSize, int pageNum, int urlData1, int urlData2)
        {
            return GetContractExpBkInfoBySearchString(searchTerm, urlData1, urlData2)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetContractExpBkInfoPECount(string searchTerm, int urlData1, int urlData2)
        {
            return GetContractExpBkInfoBySearchString(searchTerm, urlData1, urlData2).Count();
        }


        public IQueryable<CarOwnersAccessViewModel> GetCarrierInfoBySearchString(string searchTerm, long? Id, int? urlData)
        {
            return (from Us in db.CarOwners
                    where Us.IsForwarder == false
                    && Us.parentId == urlData
                    select Us)
                           .Select(Mapper.Map)
                           .OrderBy(Us => Us.CarrierName)
                           .AsQueryable();
        }

        public List<CarOwnersAccessViewModel> GetCarrierInfo(string searchTerm, int pageSize, int pageNum, long? Id, int? urlData)
        {
            return GetCarrierInfoBySearchString(searchTerm, Id, urlData)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetCarrierInfoCount(string searchTerm, long? Id, int? urlData)
        {
            return GetCarrierInfoBySearchString(searchTerm, Id, urlData).Count();
        }

        public List<CarsViewModel> GetCarInfo(string searchTerm, int pageSize, int pageNum, long? Id, int? urlData)
        {
            return GetCarInfoBySearchString(searchTerm, Id, urlData)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetCarInfoCount(string searchTerm, long? Id, int? urlData)
        {
            return GetCarInfoBySearchString(searchTerm, Id, urlData).Count();
        }

        public IQueryable<CarsViewModel> GetCarInfoBySearchString(string searchTerm, long? Id, int? urlData)
        {
            return (from Us in db.Cars
                    where Us.CarOwnersId == urlData
                    select Us)
                           .Select(Mapper.Map)
                           .OrderBy(Us => Us.CarModel)
                          .AsQueryable();
        }

        public long NewDirUsedCar(OrderUsedCarViewModel model)
        {
            //var expeditorName = db.CarOwners.AsNoTracking().FirstOrDefault(x => x.Id == model.ExpeditorId).CarrierName;
            var carrierInfo = db.CarOwners.AsNoTracking().FirstOrDefault(x => x.Id == model.CarrierId).CarrierName;
            var cars = db.Cars.AsNoTracking().FirstOrDefault(x => x.Id == model.CarId);
            var contractInfo = db.Contracts.AsNoTracking().FirstOrDefault(x => x.Id == model.ContractId).ContractNumber;
            var contractDate = db.Contracts.AsNoTracking()
                    .FirstOrDefault(x => x.Id == model.ContractId)
                    .ContractDate ?? DateTime.MinValue;
            if (contractDate != DateTime.MinValue)
                contractInfo = contractInfo + " от " +
                                   contractDate.ToString("dd.MM.yyyy");

            var car = new OrderUsedCars()
            {
                OrderId = model.OrderId,
                ContractId = model.ContractId,
                ContractExpBkId = model.ContractExpBkId,
                CarId = model.CarId,
                CarrierInfo = carrierInfo,
                ExpeditorId = model.ExpeditorId,

                CarModelInfo = cars.Model ?? "",
                CarRegNum = cars.Number,
                CarCapacity = model.CarCapacity,
                CarDriverInfo = cars.Driver,
                DriverCardInfo = cars.DriverLicenseSeria + " " + cars.DriverLicenseNumber,


                ContractInfo = contractInfo,
                DriverContactInfo = model.DriverContactInfo,
                Comments = model.Comments,

                /*  FactShipperDateTime = DateTimeConvertClass.getDateTime(model.FactShipperDate).
                                                             AddHours(DateTimeConvertClass.getHours(model.FactShipperTime)).
                                                             AddMinutes(DateTimeConvertClass.getMinutes(model.FactShipperTime)),

              FactConsigneeDateTime = DateTimeConvertClass.getDateTime(model.FactConsigneeDate).
                                                          AddHours(DateTimeConvertClass.getHours(model.FactConsigneeTime)).
                                                          AddMinutes(DateTimeConvertClass.getMinutes(model.FactConsigneeTime)),*/
                Summ = 0
            };

            db.OrderUsedCars.Add(car);
            db.SaveChanges();

            return car.Id;

        }

        public IQueryable<BaseReportViewModel> getBaseReport(string userId,
                                                             bool isAdmin,
                                                             bool UseOrderClientFilter,
                                                             bool UseOrderTypeFilter,
                                                             bool UseTripTypeFilter,
                                                             string FilterOrderClientId,
                                                             string FilterOrderTypeId,
                                                             string FilterTripTypeId,
                                                             DateTime FilterOrderDateBeg,
                                                             DateTime FilterOrderDateEnd,
                                                             DateTime FilterAcceptDateBeg,
                                                             DateTime FilterAcceptDateEnd,
                                                             bool UseOrderDateFilter,
                                                             bool UseAcceptDateFilter,
                                                             bool isPassOrders)
        {

            var _FilterOrderClientId = string.IsNullOrEmpty(FilterOrderClientId) ? "0" : FilterOrderClientId;
            var _FilterOrderTypeId = string.IsNullOrEmpty(FilterOrderTypeId) ? "0" : FilterOrderTypeId;
            var _FilterTripTypeId = string.IsNullOrEmpty(FilterTripTypeId) ? "0" : FilterTripTypeId;

            List<BaseReportViewModel> OrdersInfo = new List<BaseReportViewModel>();

            var OrdersItems = db.GetBaseReport(userId,
                isAdmin,
                UseOrderClientFilter,
                UseOrderTypeFilter,
                UseTripTypeFilter,
                _FilterOrderClientId,
                _FilterOrderTypeId,
                _FilterTripTypeId,
                FilterOrderDateBeg,
                FilterOrderDateEnd,
                FilterAcceptDateBeg,
                FilterAcceptDateEnd,
                UseOrderDateFilter,
                UseAcceptDateFilter,
                isPassOrders).ToList();

            foreach (var o in OrdersItems)
            {
                BaseReportViewModel OrdersInfoItem = new BaseReportViewModel();

                OrdersInfoItem.Id = o.Id;
                OrdersInfoItem.OrderDate = o.OrderDate.ToString("dd.MM.yyyy");
                OrdersInfoItem.OrderDateRaw = DateTimeConvertClass.getString(o.OrderDate);
                OrdersInfoItem.CreatedByUser = o.CreatedByUser;
                OrdersInfoItem.CreatedByUserName = o.CreatorDispalyName;

                var userPost = db.AspNetUsers.FirstOrDefault(u => u.Id == o.CreatedByUser)?.PostName;
                //OrdersInfoItem.OrdersAuthor = string.Concat(OrdersInfoItem.CreatedByUserName, ", ", userPost, ", ", o.CreatorContact);
                OrdersInfoItem.OrdersAuthor = OrdersInfoItem.CreatedByUserName;
                OrdersInfoItem.CreateDatetime = o.CreateDatetime;
                OrdersInfoItem.OrderType = o.OrderType;
                OrdersInfoItem.OrderTypename = o.TypeName;
                OrdersInfoItem.OrderTypeShortName = o.TypeShortName;
                OrdersInfoItem.CurrentOrderStatus = o.CurrentOrderStatus;
                OrdersInfoItem.CurrentOrderStatusColor = o.Color;
                OrdersInfoItem.CurrentOrderStatusName = o.OrderStatusName;
                OrdersInfoItem.CurrentStatusShortName = o.OrderStatusShortName;
                OrdersInfoItem.FontColor = o.FontColor;
                OrdersInfoItem.BackgroundColor = o.BackgroundColor;
                OrdersInfoItem.OrderDescription = o.OrderDescription;
                OrdersInfoItem.ClientId = o.ClientId;
                OrdersInfoItem.ClientName = o.ClientName;
                OrdersInfoItem.ClientCenterName = o.CenterName;
                OrdersInfoItem.CanBeDelete = (o.CurrentOrderStatus == 1) || (o.CurrentOrderStatus == 17);
                OrdersInfoItem.Summ = o.Summ ?? 0;
                OrdersInfoItem.UseNotifications = o.UseNotifications ?? false;
                OrdersInfoItem.CreatorContact = o.CreatorContact;
                OrdersInfoItem.CreatorPosition = o.CreatorPosition;
                OrdersInfoItem.PriorityType = o.PriotityType;
                OrdersInfoItem.OrderServiceDatetime = o.OrderServiceDateTime ?? DateTime.Now;
                OrdersInfoItem.IconFile = o.IconFile;
                OrdersInfoItem.IconDescription = o.IconDescription;
                OrdersInfoItem.OrderExecuter = o.OrderExecuter;
                OrdersInfoItem.OrderExecuterName = o.ExecutorDisplayName;
                OrdersInfoItem.PayerId = o.PayerId ?? 0;
                OrdersInfoItem.PayerName = o.PayerName;
                OrdersInfoItem.ProjectNum = o.ProjectNum;
                OrdersInfoItem.CarNumber = o.CarNumber ?? 0;
                OrdersInfoItem.TotalDistanceDescription = o.DistanceDescription;
                OrdersInfoItem.TotalCost = (o.TotalPrice ?? 0).ToString(CultureInfo.CreateSpecificCulture("uk-UA"));
                OrdersInfoItem.TotalDistanceLenght = Convert.ToInt32(o.TotalDistanceLength ?? 0);
                OrdersInfoItem.IsTransport = o.IsTransportType ?? false;
                OrdersInfoItem.IsPrivateOrder = o.IsPrivateOrder ?? false;
                OrdersInfoItem.IsFinishOfTheProcess = o.isFinishOfTheProcess ?? false;
                OrdersInfoItem.ReportStatusName = (o.isFinishOfTheProcess ?? false) ? "Финальный статус" : o.OrderStatusName;
                OrdersInfoItem.ReportStatusId = (o.isFinishOfTheProcess ?? false) ? 0 : o.CurrentOrderStatus;
                OrdersInfoItem.ReportColor = (o.isFinishOfTheProcess ?? false) ? "#666666" : o.BackgroundColor;

                OrdersInfoItem.FactCarNumber = GetOrdersUsedCars(OrdersInfoItem.Id);

                //  OrdersInfoItem.SumPlanCarNumber = 0;
                // OrdersInfoItem.SumFactCarNumber = 0;
                if ((OrdersInfoItem.OrderType == 4) || (OrdersInfoItem.OrderType == 5) || (OrdersInfoItem.OrderType == 7))
                {

                    OrdersInfoItem.AcceptDate = o.FromShipperDatetime.Value.ToString("dd.MM.yyyy");
                    OrdersInfoItem.TripType = o.TripType ?? 0;

                    OrdersInfoItem.Shipper = o.Shipper;
                    OrdersInfoItem.ShipperCountryId = o.ShipperCountryId ?? 0;
                    var shipperCountryName = db.Countries.FirstOrDefault(u => u.Сode == o.ShipperCountryId)?.Name;
                    OrdersInfoItem.ShipperCountryName = shipperCountryName;
                    OrdersInfoItem.ShipperCity = o.ShipperCity;
                    OrdersInfoItem.ShipperAdress = o.ShipperAdress;

                    if (OrdersInfoItem.TripType == 2)
                        OrdersInfoItem.FromInfo = string.Concat(OrdersInfoItem.ShipperCountryName, ", ",
                        OrdersInfoItem.ShipperCity, ", ", OrdersInfoItem.ShipperAdress);
                    else
                        OrdersInfoItem.FromInfo = string.Concat(OrdersInfoItem.ShipperCity, ", ",
                        OrdersInfoItem.ShipperAdress);

                    OrdersInfoItem.Consignee = o.Consignee;
                    OrdersInfoItem.ConsigneeCountryId = o.ConsigneeCountryId ?? 0;

                    var consigneeCountryName = db.Countries.FirstOrDefault(u => u.Сode == o.ConsigneeCountryId)?.Name;
                    OrdersInfoItem.ConsigneeCountryName = consigneeCountryName;
                    OrdersInfoItem.ConsigneeCity = o.ConsigneeCity;
                    OrdersInfoItem.ConsigneeAdress = o.ConsigneeAdress;

                    if (OrdersInfoItem.TripType == 2)
                        OrdersInfoItem.ToInfo = string.Concat(OrdersInfoItem.ConsigneeCountryName, ", ",
                        OrdersInfoItem.ConsigneeCity, ", ", OrdersInfoItem.ConsigneeAdress);
                    else
                        OrdersInfoItem.ToInfo = string.Concat(OrdersInfoItem.ConsigneeCity, ", ",
                        OrdersInfoItem.ConsigneeAdress);

                }

                if ((OrdersInfoItem.OrderType == 1) || (OrdersInfoItem.OrderType == 3) || (OrdersInfoItem.OrderType == 6))
                {
                    //  OrdersInfoItem.SumPlanCarNumber = OrdersInfoItem.SumPlanCarNumber + 1;
                    //  OrdersInfoItem.SumFactCarNumber = OrdersInfoItem.SumFactCarNumber + OrdersInfoItem.FactCarNumber;

                    OrdersInfoItem.AcceptDate = o.StartDateTimeOfTrip.Value.ToString("dd.MM.yyyy");

                    OrdersInfoItem.Shipper = o.OrgFrom;

                    OrdersInfoItem.ShipperCountryId = o.FromCountry ?? 0;
                    OrdersInfoItem.TripType = o.PassTripType ?? 0;
                    var fromCountryName = db.Countries.FirstOrDefault(u => u.Сode == o.FromCountry)?.Name;
                    OrdersInfoItem.ShipperCountryName = fromCountryName;
                    OrdersInfoItem.ShipperCity = o.FromCity;
                    OrdersInfoItem.ShipperAdress = o.AdressFrom;

                    if (OrdersInfoItem.TripType == 2)
                        OrdersInfoItem.FromInfo = string.Concat(OrdersInfoItem.ShipperCountryName, ", ", OrdersInfoItem.ShipperCity, ", ", OrdersInfoItem.ShipperAdress);
                    else
                        OrdersInfoItem.FromInfo = string.Concat(OrdersInfoItem.ShipperCity, ", ", OrdersInfoItem.ShipperAdress);


                    OrdersInfoItem.Consignee = o.OrgTo;
                    OrdersInfoItem.ConsigneeCountryId = o.ToCountry ?? 0;

                    var consigneeCountryName = db.Countries.FirstOrDefault(u => u.Сode == o.ToCountry)?.Name;
                    OrdersInfoItem.ConsigneeCountryName = consigneeCountryName;
                    OrdersInfoItem.ConsigneeCity = o.ToCity;
                    OrdersInfoItem.ConsigneeAdress = o.AdressTo;

                    if (OrdersInfoItem.TripType == 2)
                        OrdersInfoItem.ToInfo = string.Concat(OrdersInfoItem.ConsigneeCountryName, ", ",
                        OrdersInfoItem.ConsigneeCity, ", ", OrdersInfoItem.ConsigneeAdress);
                    else
                        OrdersInfoItem.ToInfo = string.Concat(OrdersInfoItem.ConsigneeCity, ", ",
                        OrdersInfoItem.ConsigneeAdress);

                    if (o.PassInfo.Length >= 50)
                        OrdersInfoItem.TruckTypeName_Cut = o.PassInfo.Substring(0, 49) + "...";
                    else OrdersInfoItem.TruckTypeName_Cut = o.PassInfo;

                    OrdersInfoItem.TruckTypeName = o.PassInfo;

                    if (o.TripDescription.Length >= 50)
                        OrdersInfoItem.TruckDescription_Cut = o.TripDescription.Substring(0, 49) + "...";
                    else OrdersInfoItem.TruckDescription_Cut = o.TripDescription;

                    OrdersInfoItem.TruckDescription = o.TripDescription;

                    OrdersInfoItem.AvtoPlanFact = "1/" + OrdersInfoItem.FactCarNumber;

                    OrdersInfoItem.CarNumber = 1;
                }


                if ((OrdersInfoItem.OrderType == 4) || (OrdersInfoItem.OrderType == 5) || (OrdersInfoItem.OrderType == 7))
                {
                    //OrdersInfoItem.SumPlanCarNumber = OrdersInfoItem.SumPlanCarNumber + OrdersInfoItem.CarNumber;//
                    //OrdersInfoItem.SumFactCarNumber = OrdersInfoItem.SumFactCarNumber + OrdersInfoItem.FactCarNumber;

                    OrdersInfoItem.FromShipperDate = o.FromShipperDatetime.Value.ToString("dd.MM.yyyy");
                    OrdersInfoItem.FromShipperDateRaw = DateTimeConvertClass.getString(o.FromShipperDatetime.Value);
                    OrdersInfoItem.FromShipperTime = o.FromShipperDatetime.Value.ToString("HH:mm");
                    OrdersInfoItem.FromShipperTimeRaw = DateTimeConvertClass.getString(o.FromShipperDatetime.Value);



                    OrdersInfoItem.ToConsigneeDate = o.ToConsigneeDatetime.Value.ToString("dd.MM.yyyy");
                    OrdersInfoItem.ToConsigneeDateRaw = DateTimeConvertClass.getString(o.ToConsigneeDatetime.Value);
                    OrdersInfoItem.ToConsigneeTime = o.ToConsigneeDatetime.Value.ToString("HH:mm");
                    OrdersInfoItem.ToConsigneeTimeRaw = DateTimeConvertClass.getString(o.ToConsigneeDatetime.Value);

                    OrdersInfoItem.BoxingDescription = o.BoxingDescription;
                    OrdersInfoItem.TruckDescription = o.TruckDescription;
                    OrdersInfoItem.Weight = (o.Weight ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA"));
                    OrdersInfoItem.Volume = Convert.ToDouble(o.Volume ?? 0);
                    OrdersInfoItem.DimenssionL = Convert.ToDouble(o.DimenssionL ?? 0);
                    OrdersInfoItem.DimenssionW = Convert.ToDouble(o.DimenssionW ?? 0);
                    OrdersInfoItem.DimenssionH = Convert.ToDouble(o.DimenssionH ?? 0);
                    OrdersInfoItem.TruckTypeId = o.TruckTypeId ?? 0;

                    var truckTypeName = db.OrderTruckTypes.FirstOrDefault(u => u.Id == o.TruckTypeId)?.TruckTypeName;
                    OrdersInfoItem.TruckTypeName = truckTypeName;
                    OrdersInfoItem.VehicleTypeId = o.VehicleTypeId ?? 0;
                    var vehicleTypeName =
                    db.OrderVehicleTypes.FirstOrDefault(u => u.Id == o.VehicleTypeId)?.VehicleTypeName;
                    OrdersInfoItem.VehicleTypeName = vehicleTypeName;
                    OrdersInfoItem.LoadingTypeId = o.LoadingTypeId ?? 0;
                    var loadingTypeName =
                    db.OrderLoadingTypes.FirstOrDefault(u => u.Id == o.LoadingTypeId)?.LoadingTypeName;
                    OrdersInfoItem.LoadingTypeName = loadingTypeName;
                    OrdersInfoItem.UnloadingTypeId = o.UnloadingTypeId ?? 0;
                    var unloadingTypeName =
                    db.OrderUnloadingTypes.FirstOrDefault(u => u.Id == o.UnloadingTypeId)?.UnloadingTypeName;
                    OrdersInfoItem.UnloadingTypeName = unloadingTypeName;

                    OrdersInfoItem.ShipperContactPerson = o.ShipperContactPerson;
                    OrdersInfoItem.ShipperContactPersonPhone = o.ShipperContactPersonPhone;
                    OrdersInfoItem.ConsigneeContactPerson = o.ConsigneeContactPerson;
                    OrdersInfoItem.ConsigneeContactPersonPhone = o.ConsigneeContactPersonPhone;

                    OrdersInfoItem.AvtoPlanFact = OrdersInfoItem.CarNumber + "/" + OrdersInfoItem.FactCarNumber;
                }


                OrdersInfoItem.ExecuterNotes = o.ExecuterNotes;
                // OrdersInfoItem.AcceptDate = GetAcceptOnlyDate(OrdersInfoItem.Id);
                OrdersInfoItem.FinalComment = GetComment(OrdersInfoItem.Id);


                //var orderStatuses = db.OrderTruckTypes.FirstOrDefault(u => u.Id == o.TruckTypeId)?.TruckTypeName;
                OrdersInfo.Add(OrdersInfoItem);

            }

            return OrdersInfo.OrderByDescending(o=>DateTime.Parse(o.AcceptDate).Ticks).AsQueryable();
            //return result;
        }

        public IQueryable<string> getFinalPipelineSteps(string userId, int OrderTypeId)
        {

            var query = (from pl in db.OrderPipelineSteps
                         join st in db.OrderStatuses on pl.FromStatus equals st.Id
                         join os in db.OrderStatuses on pl.ToStatus equals os.Id
                         where ((pl.OrderTypeId == OrderTypeId)
                         && (pl.FinishOfTheProcess == true))
                         select os.OrderStatusName).Distinct().ToList();

            return query.AsQueryable();

        }

        public List<OrderTypeViewModel> GetOrderTruckTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetOrderTruckTypesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                        .Take(pageSize)
                           .ToList();
        }

        public int GetOrderTruckTypesCount(string searchTerm)
        {
            return GetOrderTruckTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<OrderTypeViewModel> GetOrderTruckTypesBySearchString(string searchTerm)
        {
            return
                       db.OrderTypesBase
                             .AsNoTracking()
                                .Where(s => ((s.TypeName.Contains(searchTerm) || s.TypeName.StartsWith(searchTerm) || s.TypeName.EndsWith(searchTerm)))
                                            && s.OrdersBase.Where(x => x.OrderTypesBase.IsTransportType == true).Count() > 0
                                            && s.IsTransportType == true && ((s.Id == 7) || (s.Id == 4) || (s.Id == 5))
                                            )
                                   .Select(Mapper.Map)
                                   .OrderBy(o => o.TypeName)
                                    .AsQueryable();

        }

        public List<RouteTypesViewModel> GetOrderTruckTripTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetOrderTruckTripTypesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                        .Take(pageSize)
                           .ToList();
        }

        public int GetOrderTruckTripTypesCount(string searchTerm)
        {
            return GetOrderTruckTripTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<RouteTypesViewModel> GetOrderTruckTripTypesBySearchString(string searchTerm)
        {
            return
                       db.RouteTypes
                                    .Where(s => ((s.NameRouteType.Contains(searchTerm) || s.NameRouteType.StartsWith(searchTerm) || s.NameRouteType.EndsWith(searchTerm)))
                                            && s.OrderTruckTransport.Any())
                                   .Select(Mapper.Map)
                                   .OrderBy(o => o.Id)
                                    .AsQueryable();
        }

        public List<RouteTypesViewModel> GetOrderPassTripTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetOrderPassTripTypesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                        .Take(pageSize)
                           .ToList();
        }

        public int GetOrderPassTripTypesCount(string searchTerm)
        {
            return GetOrderPassTripTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<RouteTypesViewModel> GetOrderPassTripTypesBySearchString(string searchTerm)
        {
            return
                       db.RouteTypes
                                .Where(s => ((s.NameRouteType.Contains(searchTerm) || s.NameRouteType.StartsWith(searchTerm) || s.NameRouteType.EndsWith(searchTerm)))
                                     && s.OrdersPassengerTransport.Any())
                                   .Select(Mapper.Map)
                                   .OrderBy(o => o.Id)
                                    .AsQueryable();

        }

        public string GetAcceptOnlyDate(long orderId)
        {
            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderId);

            if (orderInfo != null)
            {
                var stepInfo =
                            db.OrderPipelineSteps.FirstOrDefault(
                            x => x.OrderTypeId == orderInfo.OrderType && x.StartDateForClient == true);

                if (stepInfo != null)
                {
                    var date = db.OrderStatusHistory.FirstOrDefault(x => x.OldStatus == stepInfo.FromStatus &&
                        x.OrderId == orderId) == null
                        ? null
                        : db.OrderStatusHistory.FirstOrDefault(x => x.OldStatus == stepInfo.FromStatus &&
                        x.OrderId == orderId)
                            .ChangeDateTime.ToString("dd.MM.yyyy");

                    return date;
                }
            }
            return string.Empty;
        }


        public string GetComment(long orderId)
        {
            var orderInfo = db.OrdersBase.AsNoTracking().FirstOrDefault(x => x.Id == orderId);

            if (orderInfo != null)
            {
                var stepInfo =
                            db.OrderPipelineSteps.FirstOrDefault(
                            x => x.OrderTypeId == orderInfo.OrderType && x.FinishOfTheProcess == true);
                if (stepInfo != null)
                {
                    var date = db.OrderStatusHistory.FirstOrDefault(x => x.OldStatus == stepInfo.FromStatus &&
                               x.OrderId == orderId) == null
                               ? null
                               : db.OrderStatusHistory.FirstOrDefault(x => x.OldStatus == stepInfo.FromStatus &&
                               x.OrderId == orderId)
                            .StatusChangeComment;

                    return date;
                }
            }
            return string.Empty;
        }

        public IQueryable<StatusReportViewModel> getStatusReport(string userId,
                                                             bool isAdmin,
                                                             bool UseOrderClientFilter,
                                                             bool UseOrderTypeFilter,
                                                             bool UseTripTypeFilter,
                                                             string FilterOrderClientId,
                                                             string FilterOrderTypeId,
                                                             string FilterTripTypeId,
                                                             DateTime FilterOrderDateBeg,
                                                             DateTime FilterOrderDateEnd,
                                                             DateTime FilterAcceptDateBeg,
                                                             DateTime FilterAcceptDateEnd,
                                                             bool UseOrderDateFilter,
                                                             bool UseAcceptDateFilter,
                                                             bool isPassOrders)
        {

            var _FilterOrderClientId = string.IsNullOrEmpty(FilterOrderClientId) ? "0" : FilterOrderClientId;
            var _FilterOrderTypeId = string.IsNullOrEmpty(FilterOrderTypeId) ? "0" : FilterOrderTypeId;
            var _FilterTripTypeId = string.IsNullOrEmpty(FilterTripTypeId) ? "0" : FilterTripTypeId;

            List<StatusReportViewModel> OrdersInfo = new List<StatusReportViewModel>();

            var OrdersItems = db.GetStatusReport(userId,
                isAdmin,
                UseOrderClientFilter,
                UseOrderTypeFilter,
                UseTripTypeFilter,
                _FilterOrderClientId,
                _FilterOrderTypeId,
                _FilterTripTypeId,
                FilterOrderDateBeg,
                FilterOrderDateEnd,
                FilterAcceptDateBeg,
                FilterAcceptDateEnd,
                UseOrderDateFilter,
                UseAcceptDateFilter,
                isPassOrders).ToList();


            int i = 1;
            string initialValue = "";
            foreach (var o in OrdersItems)
            {
                StatusReportViewModel OrdersInfoItem = new StatusReportViewModel();

                //убираем (сжимаем) дублирующиеся значения
                if (o.TruckTypeName == initialValue)
                    OrdersInfoItem.TruckTypeName = string.Empty;
                else
                {
                    initialValue = o.TruckTypeName;
                    OrdersInfoItem.TruckTypeName = o.TruckTypeName;
                }

                //инициализация переменной initialValue
                if (i == 1) initialValue = o.TruckTypeName;

                OrdersInfoItem.PayerName = o.PayerName;

                OrdersInfoItem.CntAll = o.CntAll ?? 0;

                OrdersInfoItem.CntZero = o.CntZero ?? 0;
                double tmpCntZeroPercent = (OrdersInfoItem.CntZero * 100) / OrdersInfoItem.CntAll;
                OrdersInfoItem.CntZeroPercent = Math.Round(tmpCntZeroPercent);
                OrdersInfoItem.CntZeroPercentRaw = OrdersInfoItem.CntZeroPercent.ToString() + "%";

                OrdersInfoItem.CntOne = o.CntOne ?? 0;
                double tmpCntOnePercent = (OrdersInfoItem.CntOne * 100) / OrdersInfoItem.CntAll;
                OrdersInfoItem.CntOnePercent = Math.Round(tmpCntOnePercent);
                //OrdersInfoItem.CntOnePercent = 100 - OrdersInfoItem.CntZeroPercent;
                OrdersInfoItem.CntOnePercentRaw = OrdersInfoItem.CntOnePercent.ToString() + "%";

                OrdersInfo.Add(OrdersInfoItem);
                i++;
            }

            return OrdersInfo.AsQueryable();

        }

        public IQueryable<OrdersReportViewModel> getOrdersReport(string userId,
                                                      bool isAdmin,
                                                      bool UseOrderClientFilter,
                                                      bool UseOrderTypeFilter,
                                                      bool UseTripTypeFilter,
                                                      string FilterOrderClientId,
                                                      string FilterOrderTypeId,
                                                      string FilterTripTypeId,
                                                       DateTime FilterOrderDateBeg,
                                                            DateTime FilterOrderDateEnd,
                                                            DateTime FilterAcceptDateBeg,
                                                            DateTime FilterAcceptDateEnd,
                                                            bool UseOrderDateFilter,
                                                            bool UseAcceptDateFilter,
                                                            bool isPassOrders)
        {

            var _FilterOrderClientId = string.IsNullOrEmpty(FilterOrderClientId) ? "0" : FilterOrderClientId;
            var _FilterOrderTypeId = string.IsNullOrEmpty(FilterOrderTypeId) ? "0" : FilterOrderTypeId;
            var _FilterTripTypeId = string.IsNullOrEmpty(FilterTripTypeId) ? "0" : FilterTripTypeId;

            List<OrdersReportViewModel> OrdersInfo = new List<OrdersReportViewModel>();

            var OrdersItems = db.GetOrdersReport(userId,
                isAdmin,
                UseOrderClientFilter,
                UseOrderTypeFilter,
                UseTripTypeFilter,
                _FilterOrderClientId,
                _FilterOrderTypeId,
                _FilterTripTypeId,
                FilterOrderDateBeg,
                FilterOrderDateEnd,
                FilterAcceptDateBeg,
                FilterAcceptDateEnd,
                UseOrderDateFilter,
                UseAcceptDateFilter,
                isPassOrders).FirstOrDefault();

            var OrdersBKItems = db.GetOrdersBKReport(userId,
                isAdmin,
                UseOrderClientFilter,
                UseOrderTypeFilter,
                UseTripTypeFilter,
                _FilterOrderClientId,
                _FilterOrderTypeId,
                _FilterTripTypeId,
                FilterOrderDateBeg,
                FilterOrderDateEnd,
                FilterAcceptDateBeg,
                FilterAcceptDateEnd,
                UseOrderDateFilter,
                UseAcceptDateFilter,
                isPassOrders).ToList();

            //всего
            OrdersReportViewModel OrdersInfoItem = new OrdersReportViewModel();
            OrdersInfoItem.CntName = "Поступило заявок";
            if (OrdersItems != null)
            {
                OrdersInfoItem.CntOrders = OrdersItems.CntAll ?? 0;
            }
            OrdersInfoItem.BalanceKeepersName = OrdersBKItems.Select(x => x.PayerName).ToList();
            OrdersInfoItem.BalanceKeepers = OrdersBKItems.Select(x => x.CntAll).ToList();
            OrdersInfo.Add(OrdersInfoItem);

            //плановые
            OrdersReportViewModel OrdersInfoItemPlan = new OrdersReportViewModel();
            OrdersInfoItemPlan.CntName = "в т.ч плановых";
            if (OrdersItems != null)
            {
                OrdersInfoItemPlan.CntOrders = OrdersItems.CntZero ?? 0;
            }
            OrdersInfoItemPlan.BalanceKeepersName = OrdersBKItems.Select(x => x.PayerName).ToList();
            OrdersInfoItemPlan.BalanceKeepers = OrdersBKItems.Select(x => x.CntZero).ToList();
            OrdersInfo.Add(OrdersInfoItemPlan);

            //срочные
            OrdersReportViewModel OrdersInfoItemUrgent = new OrdersReportViewModel();
            OrdersInfoItemUrgent.CntName = "срочных";
            if (OrdersItems != null)
            {
                OrdersInfoItemUrgent.CntOrders = OrdersItems.CntOne ?? 0;
            }
            OrdersInfoItemUrgent.BalanceKeepersName = OrdersBKItems.Select(x => x.PayerName).ToList();
            OrdersInfoItemUrgent.BalanceKeepers = OrdersBKItems.Select(x => x.CntOne).ToList();
            OrdersInfo.Add(OrdersInfoItemUrgent);

            return OrdersInfo.AsQueryable();
        }

        public IQueryable<FinalReportViewModel> getFinalReport(string userId,
                                                      bool isAdmin,
                                                      bool UseOrderClientFilter,
                                                      bool UseOrderTypeFilter,
                                                      bool UseTripTypeFilter,
                                                      string FilterOrderClientId,
                                                      string FilterOrderTypeId,
                                                      string FilterTripTypeId,
                                                     DateTime FilterOrderDateBeg,
                                                          DateTime FilterOrderDateEnd,
                                                          DateTime FilterAcceptDateBeg,
                                                          DateTime FilterAcceptDateEnd,
                                                          bool UseOrderDateFilter,
                                                          bool UseAcceptDateFilter,
                                                          bool isPassOrders)
        {

            var _FilterOrderClientId = string.IsNullOrEmpty(FilterOrderClientId) ? "0" : FilterOrderClientId;
            var _FilterOrderTypeId = string.IsNullOrEmpty(FilterOrderTypeId) ? "0" : FilterOrderTypeId;
            var _FilterTripTypeId = string.IsNullOrEmpty(FilterTripTypeId) ? "0" : FilterTripTypeId;

            List<FinalReportViewModel> OrdersInfo = new List<FinalReportViewModel>();

            var OrdersItems = db.GetFinalReport(userId,
                isAdmin,
                UseOrderClientFilter,
                UseOrderTypeFilter,
                UseTripTypeFilter,
                _FilterOrderClientId,
                _FilterOrderTypeId,
                _FilterTripTypeId,
                FilterOrderDateBeg,
                FilterOrderDateEnd,
                FilterAcceptDateBeg,
                FilterAcceptDateEnd,
                UseOrderDateFilter,
                UseAcceptDateFilter,
                isPassOrders).ToList();

            //всего
            FinalReportViewModel OrdersInfoItem = new FinalReportViewModel();

            //OrdersInfoItem.CntAll = OrdersItems.Sum(x => x.CntAll ?? 0);
            OrdersInfoItem.CntAllNotFinal = OrdersItems.Where(x => x.FinishOfTheProcess == false).Sum(x => x.CntAll ?? 0);

            OrdersInfoItem.OrderStatusName = OrdersItems.Where(x => x.FinishOfTheProcess == true).Select(x => x.OrderStatusName).ToList();
            OrdersInfoItem.OrderStatus = OrdersItems.Where(x => x.FinishOfTheProcess == true).Select(x => x.CntAll ?? 0).ToList();

            OrdersInfoItem.CntAll = OrdersInfoItem.CntAllNotFinal + OrdersInfoItem.OrderStatus.Sum(x => x);
            OrdersInfo.Add(OrdersInfoItem);

            return OrdersInfo.AsQueryable();
        }


        public List<OrderTypeViewModel> GetOrderPassTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetOrderPassTypesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                        .Take(pageSize)
                           .ToList();
        }

        public int GetOrderPassTypesCount(string searchTerm)
        {
            return GetOrderPassTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<OrderTypeViewModel> GetOrderPassTypesBySearchString(string searchTerm)
        {
            return
                       db.OrderTypesBase
                             .AsNoTracking()
                                .Where(s => ((s.TypeName.Contains(searchTerm) || s.TypeName.StartsWith(searchTerm) || s.TypeName.EndsWith(searchTerm)))
                                            && s.OrdersBase.Where(x => x.OrderTypesBase.IsTransportType == true).Count() > 0
                                            && s.IsTransportType == true && ((s.Id == 1) || (s.Id == 3) || (s.Id == 6))
                                            )
                                   .Select(Mapper.Map)
                                   .OrderBy(o => o.TypeName)
                                    .AsQueryable();

        }

        public int GetOrdersUsedCars(long orderId)
        {
            return db.OrderUsedCars.Count(x => x.OrderId == orderId);
        }

        public List<RouteTypesViewModel> GetTripTypes(string searchTerm, int pageSize, int pageNum)
        {
            return GetTripTypesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetTripTypesCount(string searchTerm)
        {
            return GetTripTypesBySearchString(searchTerm).Count();
        }

        public IQueryable<RouteTypesViewModel> GetTripTypesBySearchString(string searchTerm)
        {
            return
            db.RouteTypes
                  .AsNoTracking()
                     .Where(s => ((s.NameRouteType.Contains(searchTerm) || s.NameRouteType.StartsWith(searchTerm) || s.NameRouteType.EndsWith(searchTerm))))
                        .Select(Mapper.Map)
                        .OrderBy(o => o.Id)
                         .AsQueryable();
        }

        public IQueryable<OrderBaseProjectsViewModel> getOrderProjects(long OrderId)
        {
            return db.OrderBaseProjects
                    .AsNoTracking()
                    .Where(osh => osh.OrderId == OrderId)
                    .Select(Mapper.Map)
                    .OrderByDescending(o => o.Id)
                    .AsQueryable();

        }


        public IQueryable<SpecificationTypesViewModel> getOrderSpecification(long OrderId)
        {
            //var SpecificationTypes = db.SpecificationTypes.AsNoTracking().Select(Mapper.Map).ToList();

            //var orderSpec = db.OrderBaseSpecification.AsNoTracking().FirstOrDefault(x => x.OrderId == OrderId);

            return (from R in db.SpecificationTypes
                    where (R.OrderBaseSpecification.Any(m => m.OrderId == OrderId))
                    select new SpecificationTypesViewModel()
                    {
                        Id = R.Id,
                        SpecificationType = R.SpecificationType,
                        Assigned = true

                    })
                    .Union
                    (from R in db.SpecificationTypes
                     where (!(R.OrderBaseSpecification.Any(m => m.OrderId == OrderId)))
                     select new SpecificationTypesViewModel()
                     {
                         Id = R.Id,
                         SpecificationType = R.SpecificationType,
                         Assigned = false

                     }).OrderBy(o => o.Id).AsQueryable();
            /* return db.OrderBaseProjects
                     .AsNoTracking()
                     .Where(osh => osh.OrderId == OrderId)
                     .Select(Mapper.Map)
                     .OrderByDescending(o => o.Id)
                     .AsQueryable();*/

        }


        public IQueryable<OrderAdditionalRoutePointModel> getLoadPoints(long OrderId, bool IsLoading)
        {
            return db.AdditionalRoutePoints
                           .AsNoTracking()
                            .Where(x => x.OrderId == OrderId && x.IsLoading == IsLoading)
                             .Select(Mapper.Map)
                              .OrderBy(o => o.NumberPoint)
                               .AsQueryable();
        }

        public long NewRoutePoint(OrderAdditionalRoutePointModel model)
        {
            var point = new AdditionalRoutePoints()
            {
                OrderId = model.OrderId ?? 0,
                IsLoading = model.IsLoading,
                RoutePointId = model.RoutePointId,
                ContactPerson = model.ContactPerson,
                ContactPersonPhone = model.ContactPersonPhone,
                NumberPoint = model.NumberPoint
            };

            db.AdditionalRoutePoints.Add(point);



            var orderInfo = db.OrdersBase.FirstOrDefault(x => x.Id == model.OrderId);
            if (orderInfo != null)
            {
                if (!(orderInfo.IsAdditionalRoutePoints ?? false))
                {
                    orderInfo.IsAdditionalRoutePoints = true;
                }
            }

            db.SaveChanges();
            return point.Id;
        }

        public void UpdateRoutePoint(OrderAdditionalRoutePointModel model)
        {
            var point = db.AdditionalRoutePoints.FirstOrDefault(o => o.Id == model.Id);

            if (point == null) return;

            point.ContactPerson = model.ContactPerson;
            point.ContactPersonPhone = model.ContactPersonPhone;
            point.NumberPoint = model.NumberPoint;

            db.SaveChanges();
        }

        public bool DeleteRoutePoint(long Id)
        {
            var point = db.AdditionalRoutePoints.FirstOrDefault(o => o.Id == Id);

            if (point != null)
            {
                db.AdditionalRoutePoints.Remove(point);
                db.SaveChanges();
            }
            return true;

        }


        public string GetProjectsInfo(long orderId, int orderType)
        {
            var projects = db.OrderBaseProjects.Where(x => x.OrderId == orderId).Select(x => x.Projects.Code).ToList();
            return string.Join(", ", projects);
        }

        public void GetCoordinates(string address, out decimal latitude, out decimal longitude)
        {
            try
            {
                var locationService = new GoogleLocationService();

                var point = locationService.GetLatLongFromAddress(address);
                if (point == null)
                {
                    latitude = 0;
                    longitude = 0;
                    return;
                }
                latitude = (decimal)point.Latitude;
                longitude = (decimal)point.Longitude;
            }
            catch (Exception ex)
            {
                latitude = 0;
                longitude = 0;
            }

        }

        public List<UserViewModel> GetOrderExecuter(string searchTerm, int pageSize, int pageNum, string UserRoleIdForExecuterData)
        {

            return GetOrderExecuterBySearchString(searchTerm, UserRoleIdForExecuterData)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();

        }

        public IQueryable<UserViewModel> GetOrderExecuterBySearchString(string searchTerm, string UserRoleIdForExecuterData)
        {//IsAdmin(currentUser) ? true : (context.UserHasRole(currentUser, OrderTypeFullInfo.UserRoleIdForExecuterData)
            return
            db.AspNetUsers
                  .AsNoTracking()
                  .Where(s => (((s.DisplayName.Contains(searchTerm) || s.DisplayName.StartsWith(searchTerm) || s.DisplayName.EndsWith(searchTerm)))))
                        .Select(Mapper.Map)
                         //.Where(o => o.isAdmin == false)
                         .Where(o => o.Dismissed == false &&
                         (o.isAdmin == true || UserHasRole(o.userId, UserRoleIdForExecuterData)))
                         .OrderBy(o => o.displayName)
                         .AsQueryable();
        }

        public int GetOrderExecuterCount(string searchTerm, string UserRoleIdForExecuterData)
        {
            return GetOrderExecuterBySearchString(searchTerm, UserRoleIdForExecuterData).Count();
        }

        public IQueryable<OrderUsedCarViewModel> getFactCars(FactCarsFilter factCarsFilter)
        {

            var _FilterOrderIdFilter = string.IsNullOrEmpty(factCarsFilter.FilterOrderIdFilter) ? "0" : factCarsFilter.FilterOrderIdFilter;
            var _FilterExpeditorIdFilter = string.IsNullOrEmpty(factCarsFilter.FilterExpeditorIdFilter) ? "0" : factCarsFilter.FilterExpeditorIdFilter;
            var _FilterContractExpBkInfoFilter = string.IsNullOrEmpty(factCarsFilter.FilterContractExpBkInfoFilter) ? "0" : factCarsFilter.FilterContractExpBkInfoFilter;
            var _FilterCarrierInfoFilter = string.IsNullOrEmpty(factCarsFilter.FilterCarrierInfoFilter) ? "0" : factCarsFilter.FilterCarrierInfoFilter;
            var _FilterContractInfoFilter = string.IsNullOrEmpty(factCarsFilter.FilterContractInfoFilter) ? "0" : factCarsFilter.FilterContractInfoFilter;
            var _FilterCarModelInfoFilter = string.IsNullOrEmpty(factCarsFilter.FilterCarModelInfoFilter) ? "0" : factCarsFilter.FilterCarModelInfoFilter;
            var _FilterCarRegNumFilter = string.IsNullOrEmpty(factCarsFilter.FilterCarRegNumFilter) ? "0" : factCarsFilter.FilterCarRegNumFilter;
            var _FilterCarCapacityFilter = string.IsNullOrEmpty(factCarsFilter.FilterCarCapacityFilter) ? "0" : factCarsFilter.FilterCarCapacityFilter;
            var _FilterCarDriverInfoFilter = string.IsNullOrEmpty(factCarsFilter.FilterCarDriverInfoFilter) ? "0" : factCarsFilter.FilterCarDriverInfoFilter;
            var _FilterDriverCardInfoFilter = string.IsNullOrEmpty(factCarsFilter.FilterDriverCardInfoFilter) ? "0" : factCarsFilter.FilterDriverCardInfoFilter;
            var _FilterDriverContactInfoFilter = string.IsNullOrEmpty(factCarsFilter.FilterDriverContactInfoFilter) ? "0" : factCarsFilter.FilterDriverContactInfoFilter;
            var _FilterCommentsFilter = string.IsNullOrEmpty(factCarsFilter.FilterCommentsFilter) ? "0" : factCarsFilter.FilterCommentsFilter;

            var _FilterStatusId = string.IsNullOrEmpty(factCarsFilter.FilterStatusId) ? "0" : factCarsFilter.FilterStatusId;
            var _FilterOrderTypeId = string.IsNullOrEmpty(factCarsFilter.FilterOrderTypeId) ? "0" : factCarsFilter.FilterOrderTypeId;
            var _FilterOrderClientId = string.IsNullOrEmpty(factCarsFilter.FilterOrderClientId) ? "0" : factCarsFilter.FilterOrderClientId;
            var _FilterOrderProjectId = string.IsNullOrEmpty(factCarsFilter.FilterOrderProjectId) ? "0" : factCarsFilter.FilterOrderProjectId;
            var _FilterOrderPayerId = string.IsNullOrEmpty(factCarsFilter.FilterOrderPayerId) ? "0" : factCarsFilter.FilterOrderPayerId;
            var _FilterOrderOrgFromId = string.IsNullOrEmpty(factCarsFilter.FilterOrderOrgFromId) ? "" : factCarsFilter.FilterOrderOrgFromId;
            var _FilterOrderOrgToId = string.IsNullOrEmpty(factCarsFilter.FilterOrderOrgToId) ? "" : factCarsFilter.FilterOrderOrgToId;

            if (!string.IsNullOrEmpty(factCarsFilter.FilterOrderOrgFromId))
            {
                string[] idList = factCarsFilter.FilterOrderOrgFromId.Split(new char[] { ',' });
                string FilterOrderOrgFromName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderOrgFromName.Length > 0)
                    {
                        FilterOrderOrgFromName += ",";
                    }

                    var OrgFromName = GetOrganization(Convert.ToInt32(i));
                    FilterOrderOrgFromName = string.Concat(FilterOrderOrgFromName, string.Concat(OrgFromName?.Name));
                }
                _FilterOrderOrgFromId = FilterOrderOrgFromName;
            }

            if (!string.IsNullOrEmpty(factCarsFilter.FilterOrderOrgToId))
            {
                string[] idList = factCarsFilter.FilterOrderOrgToId.Split(new char[] { ',' });
                string FilterOrderOrgToName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderOrgToName.Length > 0)
                    {
                        FilterOrderOrgToName += ",";
                    }

                    var OrgToName = GetOrganization(Convert.ToInt32(i));
                    FilterOrderOrgToName = string.Concat(FilterOrderOrgToName, string.Concat(OrgToName?.Name));
                }
                _FilterOrderOrgToId = FilterOrderOrgToName;
            }

            /* if (!string.IsNullOrEmpty(factCarsFilter.FilterOrderIdFilter))
             {
                 string[] idList = factCarsFilter.FilterOrderIdFilter.Split(new char[] { ',' });
                 string FilterOrderIdName = "";

                 foreach (string i in idList)
                 {
                     if (FilterOrderIdName.Length > 0)
                     {
                         FilterOrderIdName += ",";
                     }

                     var OrderIdName = i;
                     FilterOrderIdName = string.Concat(FilterOrderIdName, string.Concat(OrderIdName?.Name));
                 }
                 _FilterOrderIdFilter = FilterOrderIdName;
             }*/
            //factCarsFilter.UseOrderIdFilter = true;
            // _FilterOrderIdFilter = "28";

            /*   _FilterExpeditorIdFilter = false;
               _FilterContractExpBkInfoFilter = false;
               _FilterCarrierInfoFilter = false;
               _FilterContractInfoFilter = false;
               _FilterCarModelInfoFilter = false;
               _FilterCarRegNumFilter = false;
               _FilterCarCapacityFilter = false;
               _FilterCarDriverInfoFilter = false;
               _FilterDriverCardInfoFilter = false;
               _FilterDriverContactInfoFilter = false;
              _FilterCommentsFilter = false;*/


            var query = db.GetFactCars(factCarsFilter.userId,
                                       factCarsFilter.isAdmin,
                                       factCarsFilter.UseOrderIdFilter,
                                       factCarsFilter.UseExpeditorIdFilter,
                                       factCarsFilter.UseContractExpBkInfoFilter,
                                       factCarsFilter.UseCarrierInfoFilter,
                                       factCarsFilter.UseContractInfoFilter,
                                       factCarsFilter.UseCarModelInfoFilter,
                                       factCarsFilter.UseCarRegNumFilter,
                                       factCarsFilter.UseCarCapacityFilter,
                                       factCarsFilter.UseCarDriverInfoFilter,
                                       factCarsFilter.UseDriverCardInfoFilter,
                                       factCarsFilter.UseDriverContactInfoFilter,
                                       factCarsFilter.UseCommentsFilter,
                                       factCarsFilter.UseFactShipperFilter,
                                       factCarsFilter.UseFactConsigneeFilter,

                                       factCarsFilter.UseOrderExDateFilter,
                                       factCarsFilter.UseOrderEndDateFilter,

                                       _FilterOrderIdFilter,
                                       _FilterExpeditorIdFilter,
                                       _FilterContractExpBkInfoFilter,
                                       _FilterCarrierInfoFilter,
                                       _FilterContractInfoFilter,
                                       _FilterCarModelInfoFilter,
                                       _FilterCarRegNumFilter,
                                       _FilterCarCapacityFilter,
                                       _FilterCarDriverInfoFilter,
                                       _FilterDriverCardInfoFilter,
                                       _FilterDriverContactInfoFilter,
                                       _FilterCommentsFilter,
                                       factCarsFilter.FilterFactShipperBeg,
                                       factCarsFilter.FilterFactShipperEnd,
                                       factCarsFilter.FilterFactConsigneeBeg,
                                       factCarsFilter.FilterFactConsigneeEnd,

                                        factCarsFilter.FilterOrderExDateBeg,
                                       factCarsFilter.FilterOrderExDateEnd,
                                       factCarsFilter.FilterOrderEndDateBeg,
                                       factCarsFilter.FilterOrderEndDateEnd,

                                       factCarsFilter.UseStatusFilter,
                                             factCarsFilter.UseOrderCreatorFilter,
                                             factCarsFilter.UseOrderExecuterFilter,
                                             factCarsFilter.UseOrderTypeFilter,
                                             factCarsFilter.UseOrderClientFilter,
                                             factCarsFilter.UseOrderPriorityFilter,
                                             factCarsFilter.UseOrderDateFilter,
                                             /* factCarsFilter.UseOrderExDateFilter,
                                              factCarsFilter.UseOrderEndDateFilter,*/
                                             factCarsFilter.UseFinalStatusFilter,
                                             factCarsFilter.UseOrderProjectFilter,
                                             factCarsFilter.UseOrderPayerFilter,
                                             factCarsFilter.UseOrderOrgFromFilter,
                                             factCarsFilter.UseOrderOrgToFilter,
                                             _FilterStatusId,
                                             factCarsFilter.FilterOrderCreatorId,
                                             factCarsFilter.FilterOrderExecuterId,
                                             _FilterOrderTypeId,
                                             _FilterOrderClientId,
                                             factCarsFilter.FilterOrderPriority,
                                             factCarsFilter.FilterOrderDateBeg,
                                             factCarsFilter.FilterOrderDateEnd,
                                             factCarsFilter.FilterFinalStatus,
                                             _FilterOrderProjectId,
                                             _FilterOrderPayerId,
                                             _FilterOrderOrgFromId,
                                             _FilterOrderOrgToId
                                           ).ToList().AsQueryable();

            var result = query.Select(Mapper.Map).OrderByDescending(o => o.OrderId).AsQueryable();

            List<OrderUsedCarViewModel> list = new List<OrderUsedCarViewModel>();
            foreach (var o in result)
            {
                OrderUsedCarViewModel factCarsItem = new OrderUsedCarViewModel();
                factCarsItem.OrderId = o.OrderId;
                factCarsItem.ContractId = o.ContractId;
                factCarsItem.ContractExpBkId = o.ContractExpBkId;
                factCarsItem.ContractInfo = o.ContractInfo;
                factCarsItem.ContractExpBkInfo = o.ContractExpBkInfo;
                factCarsItem.ExpeditorName = o.ExpeditorName;
                factCarsItem.CarOwnerInfo = o.CarOwnerInfo;
                factCarsItem.CarModelInfo = o.CarModelInfo;
                factCarsItem.CarRegNum = o.CarRegNum;
                factCarsItem.CarCapacity = o.CarCapacity;
                factCarsItem.CarDriverInfo = o.CarDriverInfo;
                factCarsItem.DriverContactInfo = o.DriverContactInfo;
                factCarsItem.CarrierInfo = o.CarrierInfo;
                factCarsItem.CarId = o.CarId;
                factCarsItem.Summ = o.Summ;
                factCarsItem.DriverCardInfo = o.DriverCardInfo;
                factCarsItem.Comments = o.Comments;
                factCarsItem.ExpeditorId = o.ExpeditorId;
                factCarsItem.CarrierId = o.CarrierId;
                factCarsItem.PlanDistance = o.PlanDistance;
                factCarsItem.PlanTimeWorkDay = o.PlanTimeWorkDay;
                factCarsItem.PlanTimeHoliday = o.PlanTimeHoliday;
                factCarsItem.BaseRate = o.BaseRate;
                factCarsItem.BaseRateWorkDay = o.BaseRateHoliday;
                factCarsItem.BaseRateHoliday = o.BaseRateHoliday;
                factCarsItem.DelayDays = o.DelayDays;
                factCarsItem.FactShipperDateTime = o.FactShipperDateTime;
                factCarsItem.FactConsigneeDateTime = o.FactConsigneeDateTime;
                factCarsItem.Id = o.Id;
                factCarsItem.OrderTypeShortName = o.OrderTypeShortName;

                list.Add(factCarsItem);
            }

            return list.AsQueryable();
        }


        public List<OrderUsedCarViewModel> GetOrderId(string searchTerm, int pageSize, int pageNum)
        {
            return GetOrderIdBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetOrderIdCount(string searchTerm)
        {
            return GetOrderIdBySearchString(searchTerm).Count();
        }

        public IQueryable<OrderUsedCarViewModel> GetOrderIdBySearchString(string searchTerm)
        {
            return
            db.OrderUsedCars
                  .AsNoTracking()
                     .Where(s => ((s.OrderId.ToString().Contains(searchTerm) || s.OrderId.ToString().StartsWith(searchTerm) || s.OrderId.ToString().EndsWith(searchTerm))))
                     // .Where (s => (s.OrderId == 13243 || s.OrderId == 13241))
                     .Select(Mapper.Map)
                        .Distinct()
                        .OrderBy(o => o.OrderId)
                         .AsQueryable();
        }

        public void UpdateFactCars(OrderUsedCarViewModel model)
        {
            var factCarsInfo = db.OrderUsedCars.FirstOrDefault(u => u.Id == model.Id);
            if (factCarsInfo == null) return;

            if (model.FactShipperDateRaw != null)
            {
                if (model.FactShipperTimeRaw != null)
                    factCarsInfo.FactShipperDateTime = DateTimeConvertClass.getDateTime(model.FactShipperDateRaw).
                        AddHours(DateTimeConvertClass.getHours(model.FactShipperTimeRaw)).
                        AddMinutes(DateTimeConvertClass.getMinutes(model.FactShipperTimeRaw));
                else
                    factCarsInfo.FactShipperDateTime = DateTimeConvertClass.getDateTime(model.FactShipperDateRaw);
            }
            else factCarsInfo.FactShipperDateTime = null;

            if (model.FactConsigneeDateRaw != null)
            {
                if (model.FactConsigneeTimeRaw != null)
                    factCarsInfo.FactConsigneeDateTime = DateTimeConvertClass.getDateTime(model.FactConsigneeDateRaw).
                        AddHours(DateTimeConvertClass.getHours(model.FactConsigneeTimeRaw)).
                        AddMinutes(DateTimeConvertClass.getMinutes(model.FactConsigneeTimeRaw));
                else
                    factCarsInfo.FactConsigneeDateTime = DateTimeConvertClass.getDateTime(model.FactConsigneeDateRaw);
            }
            else factCarsInfo.FactConsigneeDateTime = null;
          

                  if (model.RealFactShipperDateRaw != null)
            {
                if (model.RealFactShipperTimeRaw != null)
                    factCarsInfo.FactShipper = DateTimeConvertClass.getDateTime(model.RealFactShipperDateRaw).
                        AddHours(DateTimeConvertClass.getHours(model.RealFactShipperTimeRaw)).
                        AddMinutes(DateTimeConvertClass.getMinutes(model.RealFactShipperTimeRaw));
                else
                    factCarsInfo.FactShipper = DateTimeConvertClass.getDateTime(model.RealFactShipperDateRaw);
            }
            else factCarsInfo.FactShipper = null;

            if (model.RealFactConsigneeDateRaw != null)
            {
                if (model.RealFactConsigneeTimeRaw != null)
                    factCarsInfo.FactConsignee = DateTimeConvertClass.getDateTime(model.RealFactConsigneeDateRaw).
                        AddHours(DateTimeConvertClass.getHours(model.RealFactConsigneeTimeRaw)).
                        AddMinutes(DateTimeConvertClass.getMinutes(model.RealFactConsigneeTimeRaw));
                else
                    factCarsInfo.FactConsignee = DateTimeConvertClass.getDateTime(model.RealFactConsigneeDateRaw);
            }
            else factCarsInfo.FactConsignee = null;
            db.SaveChanges();
        }


        public IQueryable<CarOwnersAccessViewModel> GetExpeditorFilterBySearchString(string searchTerm, long? Id)
        {
            var query = (from cl in db.OrderUsedCars
                         join ro in db.CarOwners on cl.ExpeditorId equals ro.Id
                         where (ro.IsForwarder == true) && (cl.ExpeditorId != null) && (cl.ExpeditorId > 0)
                         select ro).Distinct();

            return query
                .Where(s => (((s.CarrierName.Contains(searchTerm) || s.CarrierName.StartsWith(searchTerm) || s.CarrierName.EndsWith(searchTerm)))))
                .Select(Mapper.Map)
                .OrderByDescending(o => o.CarrierName)
                .AsQueryable();
        }

        public List<CarOwnersAccessViewModel> GetExpeditorFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetExpeditorFilterBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetExpeditorFilterCount(string searchTerm, long? Id)
        {
            return GetExpeditorFilterBySearchString(searchTerm, Id).Count();
        }

        public IQueryable<OrderUsedCarViewModel> GetCarModelInfoFilterBySearchString(string searchTerm, long? Id)
        {

            return db.GetCarModelInfoFilter(searchTerm).ToList().Select(Mapper.Map).AsQueryable();

            /* db.OrderUsedCars
                 .AsNoTracking()
                    .Where(s => ((s.CarModelInfo.ToString().Contains(searchTerm) || s.CarModelInfo.ToString().StartsWith(searchTerm) || s.CarModelInfo.ToString().EndsWith(searchTerm)))
                      &&  (s.CarModelInfo.Length > 0))
                       .Select(Mapper.Map)
                       .Distinct()
                       .OrderBy(o => o.Id)
                        .AsQueryable();                    */
        }

        public List<OrderUsedCarViewModel> GetCarModelInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetCarModelInfoFilterBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }
        public int GetCarModelInfoFilterCount(string searchTerm, long? Id)
        {
            return GetCarModelInfoFilterBySearchString(searchTerm, Id).Count();
        }
        //CarRegNum
        public IQueryable<OrderUsedCarViewModel> GetCarRegNumFilterBySearchString(string searchTerm, long? Id)
        {
            return db.GetCarRegNumFilter(searchTerm).ToList().Select(Mapper.Map).AsQueryable();
        }

        public List<OrderUsedCarViewModel> GetCarRegNumFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetCarRegNumFilterBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }
        public int GetCarRegNumFilterCount(string searchTerm, long? Id)
        {
            return GetCarRegNumFilterBySearchString(searchTerm, Id).Count();
        }

        //CarCapacity
        public IQueryable<OrderUsedCarViewModel> GetCarCapacityFilterBySearchString(string searchTerm, long? Id)
        {
            return db.GetCarCapacityFilter(searchTerm).ToList().Select(Mapper.Map).AsQueryable();
        }

        public List<OrderUsedCarViewModel> GetCarCapacityFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetCarCapacityFilterBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }
        public int GetCarCapacityFilterCount(string searchTerm, long? Id)
        {
            return GetCarCapacityFilterBySearchString(searchTerm, Id).Count();
        }

        //CarDriverInfo
        public IQueryable<OrderUsedCarViewModel> GetCarDriverInfoFilterBySearchString(string searchTerm, long? Id)
        {
            return db.GetCarDriverInfoFilter(searchTerm).ToList().Select(Mapper.Map).AsQueryable();
        }

        public List<OrderUsedCarViewModel> GetCarDriverInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetCarDriverInfoFilterBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }
        public int GetCarDriverInfoFilterCount(string searchTerm, long? Id)
        {
            return GetCarDriverInfoFilterBySearchString(searchTerm, Id).Count();
        }

        //DriverCardInfoFilter
        public IQueryable<OrderUsedCarViewModel> GetDriverCardInfoFilterBySearchString(string searchTerm, long? Id)
        {
            return db.GetDriverCardInfoFilter(searchTerm).ToList().Select(Mapper.Map).AsQueryable();
        }

        public List<OrderUsedCarViewModel> GetDriverCardInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetDriverCardInfoFilterBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }
        public int GetDriverCardInfoFilterCount(string searchTerm, long? Id)
        {
            return GetDriverCardInfoFilterBySearchString(searchTerm, Id).Count();
        }

        //DriverContactInfo
        public IQueryable<OrderUsedCarViewModel> GetDriverContactInfoFilterBySearchString(string searchTerm, long? Id)
        {

            return db.GetDriverContactInfoFilter(searchTerm).ToList().Select(Mapper.Map).AsQueryable();
        }

        public List<OrderUsedCarViewModel> GetDriverContactInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetDriverContactInfoFilterBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }
        public int GetDriverContactInfoFilterCount(string searchTerm, long? Id)
        {
            return GetDriverContactInfoFilterBySearchString(searchTerm, Id).Count();
        }

        //Comments
        public IQueryable<OrderUsedCarViewModel> GetCommentsFilterBySearchString(string searchTerm, long? Id)
        {
            return db.GetCommentsFilter(searchTerm).ToList().Select(Mapper.Map).AsQueryable();

        }

        public List<OrderUsedCarViewModel> GetCommentsFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetCommentsFilterBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }
        public int GetCommentsFilterCount(string searchTerm, long? Id)
        {
            return GetCommentsFilterBySearchString(searchTerm, Id).Count();
        }

        public CarOwnersAccessViewModel getExpeditors(int ExpeditorId)
        {
            return Mapper.Map(db.CarOwners.AsNoTracking().FirstOrDefault(p => p.Id == ExpeditorId));
        }

        public IQueryable<ContractsViewModel> GetContractExpBySearchString(string searchTerm, long? Id)
        {
            var query = (from cl in db.OrderUsedCars
                         join ro in db.Contracts on cl.ContractExpBkId equals ro.Id
                         where (ro.IsActive == true || ro.IsActive == null)
                         select ro).Distinct();

            return query
               .Where(s => (((s.ContractNumber.Contains(searchTerm) || s.ContractNumber.StartsWith(searchTerm) || s.ContractNumber.EndsWith(searchTerm)))))
               .Select(Mapper.Map)
               .OrderByDescending(o => o.ContractNumber)
               .AsQueryable();
        }

        public List<ContractsViewModel> GetContractExpInfo(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetContractExpBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetContractExpCount(string searchTerm, long? Id)
        {
            return GetContractExpBySearchString(searchTerm, Id).Count();
        }

        public ContractsViewModel getContracts(int ContractsId)
        {
            return Mapper.Map(db.Contracts.AsNoTracking().FirstOrDefault(p => p.Id == ContractsId));
        }


        public IQueryable<OrderUsedCarViewModel> GetCarrierInfoFilterBySearchString(string searchTerm, long? Id)
        {
            return db.GetCarrierInfoFilter(searchTerm).ToList().Select(Mapper.Map).AsQueryable();
        }

        public List<OrderUsedCarViewModel> GetCarrierInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetCarrierInfoFilterBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetCarrierInfoFilterCount(string searchTerm, long? Id)
        {
            return GetCarrierInfoFilterBySearchString(searchTerm, Id).Count();
        }

        public IQueryable<OrderUsedCarViewModel> GetContractExpBkInfoBySearchString2(string searchTerm, long? Id)
        {
            return db.GetContractInfoFilter(searchTerm).ToList().Select(Mapper.Map).AsQueryable();
        }

        public List<OrderUsedCarViewModel> GetContractExpBkInfo2(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetContractExpBkInfoBySearchString2(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetContractExpBkInfoPECount2(string searchTerm, long? Id)
        {
            return GetContractExpBkInfoBySearchString2(searchTerm, Id).Count();
        }

        public IQueryable<OrderProjectViewModel> GetOrderProjects()
        {
            return db.Projects
                      .AsNoTracking()
                        .Select(Mapper.Map)
                         .OrderBy(o => o.Code)
                          .AsQueryable();
        }

        public List<ProjectTypeViewModel> GetOrderProjects(long orderId, string searchTerm, int pageSize, int pageNum)
        {
            return GetOrderProjectsBySearchString(orderId, searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetOrderProjectsCount(long orderId, string searchTerm)
        {
            return GetOrderProjectsBySearchString(orderId, searchTerm).Count();
        }

        private IQueryable<ProjectTypeViewModel> GetOrderProjectsBySearchString(long orderId, string searchTerm)
        {
            return db.GetOrderProjects(orderId, searchTerm).ToList().Select(Mapper.Map).AsQueryable();
        }        

        public IQueryable<TruckViewModel> getTruckReportData(string userId,
            bool isAdmin,
            bool UseOrderTypeFilter,
            string FilterOrderTypeId,
            DateTime FilterOrderDate,
            bool UseOrderDateFilter, 
            String IdTree)
        {

            var _FilterOrderTypeId = string.IsNullOrEmpty(FilterOrderTypeId) ? "0" : FilterOrderTypeId;
            UseOrderDateFilter = true;

            // List<BaseReportViewModel> OrdersInfo = new List<BaseReportViewModel>();

            var OrdersItems = db.GetTruckReport(userId,
                isAdmin,
                UseOrderTypeFilter,
                _FilterOrderTypeId,
                FilterOrderDate,
                UseOrderDateFilter).ToList();

            List<TruckViewModel> TruckInfo = new List<TruckViewModel>();            
           // var OrdersItems = OrdersItems1; //.Where(x => x.ShipperId == 1446);
            bool isShipper = false;
            foreach (var o in OrdersItems)
            {
                if ((o.FromShipperDatetime ?? DateTime.Now).Date == FilterOrderDate)
                    isShipper = true;

                if (isShipper)
                { 
                    TruckViewModel OrdersInfoItem = new TruckViewModel();
                    OrdersInfoItem.IsShipper = true;
                    OrdersInfoItem.IsSystemOrg = o.ShipperIsSystemOrg ?? false;
                    OrdersInfoItem.ShipperCountryName = o.ShipperCountry;
                    OrdersInfoItem.ShipperCity = o.ShipperCity;
                    OrdersInfoItem.ShipperAdress = o.ShipperAdress;
                    OrdersInfoItem.ShipperCountryId = o.ShipperCountryId ?? 0;
                    OrdersInfoItem.ShipperId = o.ShipperId ?? 0;
                    OrdersInfoItem.Shipper = o.Shipper;     
                    OrdersInfoItem.IdTree = IdTree;                                     

                    TruckOrganization(ref OrdersInfoItem, o);
                    TruckInfo.Add(OrdersInfoItem);
                }
                else
                {
                    TruckViewModel OrdersInfoItem2 = new TruckViewModel();
                    OrdersInfoItem2.IsShipper = false;
                    OrdersInfoItem2.IsSystemOrg = o.ConsigneeIsSystemOrg ?? false;
                    OrdersInfoItem2.ShipperCountryName = o.ConsigneeCountry;
                    OrdersInfoItem2.ShipperCity = o.ConsigneeCity;
                    OrdersInfoItem2.ShipperAdress = o.ConsigneeAdress;
                    OrdersInfoItem2.ShipperCountryId = o.ConsigneeCountryId ?? 0;
                    OrdersInfoItem2.ShipperId = o.ShipperId ?? 0;
                    OrdersInfoItem2.Shipper = o.Consignee;    
                    OrdersInfoItem2.IdTree = IdTree;                  

                    TruckOrganization(ref OrdersInfoItem2, o);
                    TruckInfo.Add(OrdersInfoItem2);                    
                }
            }
            return TruckInfo.AsQueryable();
        }

        public IQueryable<TruckViewModel> getTruckReport(string userId,
                                                            bool isAdmin,
                                                            bool UseOrderTypeFilter,
                                                            string FilterOrderTypeId,
                                                            DateTime FilterOrderDate,
                                                            bool UseOrderDateFilter,
                                                            String IdTree,
                                                            ref  List<TruckViewModel> TruckInfo)
        {
            TruckInfo = getTruckTree(userId, isAdmin, UseOrderTypeFilter, FilterOrderTypeId, FilterOrderDate, UseOrderDateFilter, IdTree);
         
            return TruckInfo.Where(x => x.IdGroudId != 6).OrderBy(c => c.IsSystemOrg).ThenBy(c => c.ShipperCountryName).ThenBy(c => c.ShipperCity).ThenBy(c => c.ShipperAdress).AsQueryable();             
        }

        private List<TruckViewModel> getTruckTree(string userId, bool isAdmin, bool UseOrderTypeFilter, string FilterOrderTypeId,
            DateTime FilterOrderDate, bool UseOrderDateFilter, String IdTree)
        {
            var TruckInfo =
                getTruckReportData(userId, isAdmin, UseOrderTypeFilter, FilterOrderTypeId, FilterOrderDate, UseOrderDateFilter, IdTree)
                    .ToList();
            //var Trucks = TruckInfo.Select(x => new {x.ShipperAdress, x.ShipperCity, x.ShipperCountryId, x.Id}).ToList().Distinct();

            //добавляем системная фирма или нет
            var SystemOrg = TruckInfo.Where(x => x.IdGroudId == 6).Select(x => new {x.IsSystemOrg}).ToList().Distinct();
            Dictionary<String, bool> IdSystemOrg = new Dictionary<String, bool>();

            foreach (var org in SystemOrg)
            {
                if (org.IsSystemOrg)
                    IdSystemOrg.Add(AddSystemOrg("ПРЕДПРИЯТИЯ КОРУМ", FilterOrderDate, ref TruckInfo, IdTree), true);
                else if (!org.IsSystemOrg)
                    IdSystemOrg.Add(AddSystemOrg("ПРЕДПРИЯТИЯ не КОРУМ", FilterOrderDate, ref TruckInfo, IdTree), false);
            }

            //добавляем страны
            AddCountries(ref TruckInfo, FilterOrderDate, IdSystemOrg, IdTree);

            //добавляем города
            AddCities(ref TruckInfo, FilterOrderDate, IdTree);

            //добавляем адреса
            AddAddresses(ref TruckInfo, FilterOrderDate, IdTree);

            //добавляем запись с грузоотправителем
            AddShipper(ref TruckInfo, FilterOrderDate, IdTree);
            return TruckInfo;
        }

        private void AddShipper(ref List<TruckViewModel> TruckInfo, DateTime FilterOrderDate, String IdTree)
        {            
            var Shippers = TruckInfo.Where(x => x.IdGroudId == 6).Select(x => new {x.ShipperId, x.Shipper, x.ShipperAdress, x.ShipperCity, x.ShipperCountryId, x.IsSystemOrg}).ToList().Distinct();

            var Addresses = TruckInfo.Where(x => x.IdGroudId == 4).Select(x => new {x.ShipperAdress, x.ShipperCity, x.ShipperCountryId, x.IsSystemOrg, x.Id}).ToList().Distinct();

            List<TruckViewModel> tmp = new List<TruckViewModel>();

                foreach (var address in Addresses)
                {
                    foreach (var shippers in Shippers)
                    {
                        if (shippers.ShipperAdress == null || shippers.ShipperCity == null) continue;
                        if (shippers.ShipperAdress.Equals(address.ShipperAdress) && shippers.ShipperCity.Equals(address.ShipperCity)
                        && shippers.ShipperCountryId == address.ShipperCountryId && shippers.IsSystemOrg == address.IsSystemOrg)

                        {
                            TruckViewModel OrdersInfoItem = new TruckViewModel();
                            OrdersInfoItem.IdParent = address.Id;

                            OrdersInfoItem.Name = ""; //shippers.Shipper;
                            OrdersInfoItem.ShipperId = shippers.ShipperId;
                            OrdersInfoItem.IsLeaf = true;
                            OrdersInfoItem.IdGroudId = 5;
                            OrdersInfoItem.IdTree = IdTree;    

                            OrdersInfoItem.Id = Guid.NewGuid().ToString();
                            OrdersInfoItem.IdDetails = OrdersInfoItem.Id;
                            OrdersInfoItem.ReportsDate = FilterOrderDate;

                            OrdersInfoItem.ShipperCountryId = shippers.ShipperCountryId;
                            OrdersInfoItem.IsSystemOrg = shippers.IsSystemOrg;
                            OrdersInfoItem.ShipperCity = shippers.ShipperCity;
                            OrdersInfoItem.ShipperAdress = shippers.ShipperAdress;
                            OrdersInfoItem.Shipper = shippers.Shipper;
                        
                            tmp.Add(OrdersInfoItem);
                        }
                }
            }

                TruckInfo.AddRange(tmp);
                tmp = null;

        }

        private void AddAddresses(ref List<TruckViewModel> TruckInfo, DateTime FilterOrderDate, String IdTree)//, String[] IdSystemOrg)
        {
             var Addresses = TruckInfo.Where(x => x.IdGroudId == 6).Select(x => new {x.ShipperAdress, x.ShipperCity, x.ShipperCountryId, x.IsSystemOrg}).ToList().Distinct();

             var Cities = TruckInfo.Where(x => x.IdGroudId == 3).Select(x => new {x.ShipperCity, x.ShipperCountryId, x.IsSystemOrg, x.Id}).ToList().Distinct();

            /*var Countries =
                TruckInfo.Where(x => x.IdGroudId == 2)
                    .Select(x => new {x.ShipperCountryId, x.ShipperCountryName, x.Id})
                    .Distinct();*/
                List<TruckViewModel> tmp = new List<TruckViewModel>();

                foreach (var city in Cities)
                {
                    foreach (var address in Addresses)
                    {
                        if (address.ShipperCity == null) continue;
                        if (city.ShipperCountryId == address.ShipperCountryId && address.ShipperCity.Equals(city.ShipperCity)
                        && city.IsSystemOrg == address.IsSystemOrg)
                        {
                            TruckViewModel OrdersInfoItem = new TruckViewModel();
                            OrdersInfoItem.IdParent = city.Id;

                            OrdersInfoItem.Name = address.ShipperAdress;
                            OrdersInfoItem.ShipperAdress = address.ShipperAdress;
                            OrdersInfoItem.IsLeaf = false;
                            OrdersInfoItem.IdGroudId = 4;
                            OrdersInfoItem.IdTree = IdTree;    

                            OrdersInfoItem.Id = Guid.NewGuid().ToString();
                            OrdersInfoItem.IdDetails = OrdersInfoItem.Id;
                            OrdersInfoItem.ReportsDate = FilterOrderDate;

                            OrdersInfoItem.ShipperCountryId = city.ShipperCountryId;
                            OrdersInfoItem.IsSystemOrg = city.IsSystemOrg;
                            OrdersInfoItem.ShipperCity = city.ShipperCity;

                            tmp.Add(OrdersInfoItem);
                        }
                }
            }

                TruckInfo.AddRange(tmp);
                tmp = null;
        }

        private void AddCities(ref List<TruckViewModel> TruckInfo, DateTime FilterOrderDate, String IdTree)//, String[] IdSystemOrg)
        {
            var Cities = TruckInfo.Where(x => x.IdGroudId == 6).Select(x => new {x.ShipperCity, x.ShipperCountryId, x.IsSystemOrg}).ToList().Distinct();

            var Countries =
                TruckInfo.Where(x => x.IdGroudId == 2)
                    .Select(x => new {x.ShipperCountryId, x.ShipperCountryName, x.IsSystemOrg, x.Id})
                    .Distinct();


             List<TruckViewModel> tmp = new List<TruckViewModel>();
             //foreach (String s in IdSystemOrg)
           //  {
                 foreach (var country in Countries)
                 {
                     foreach (var city in Cities)
                     {
                         if (city.ShipperCountryId == country.ShipperCountryId && city.IsSystemOrg == country.IsSystemOrg)
                         {
                             TruckViewModel OrdersInfoItem = new TruckViewModel();
                             OrdersInfoItem.IdParent = country.Id;

                             OrdersInfoItem.Name = city.ShipperCity;
                             OrdersInfoItem.ShipperCity = city.ShipperCity;
                             OrdersInfoItem.IsLeaf = false;
                             OrdersInfoItem.IdGroudId = 3;
                             OrdersInfoItem.IdTree = IdTree;    

                             OrdersInfoItem.Id = Guid.NewGuid().ToString();
                             OrdersInfoItem.IdDetails = OrdersInfoItem.Id;
                             OrdersInfoItem.ReportsDate = FilterOrderDate;
                             OrdersInfoItem.ShipperCountryId = city.ShipperCountryId;
                             OrdersInfoItem.IsSystemOrg = city.IsSystemOrg;
                             tmp.Add(OrdersInfoItem);
                         }
                     }
                // }
             }
            TruckInfo.AddRange(tmp);
            tmp = null;
        }

        private static void AddCountries(ref List<TruckViewModel> TruckInfo, DateTime FilterOrderDate, Dictionary<String, bool> IdSystemOrg, String IdTree)
        {            
           var Countries = TruckInfo.Where(x => x.ShipperCountryId != 0 && x.IdGroudId == 6).Select(x => new {x.ShipperCountryId, x.ShipperCountryName, x.IsSystemOrg}).Distinct();

            List<TruckViewModel> tmp = new List<TruckViewModel>();            
            foreach(KeyValuePair<String, bool> s in IdSystemOrg)
            {
                foreach (var country in Countries)
                {
                    if (s.Value == country.IsSystemOrg)
                    {
                        TruckViewModel OrdersInfoItem = new TruckViewModel();
                        OrdersInfoItem.IdParent = s.Key;

                        OrdersInfoItem.Name = country.ShipperCountryName;
                        OrdersInfoItem.ShipperCountryName = country.ShipperCountryName;
                        OrdersInfoItem.ShipperCountryId = country.ShipperCountryId;
                        OrdersInfoItem.IsLeaf = false;
                       // OrdersInfoItem.IsLeaf = true;
                        OrdersInfoItem.IdGroudId = 2;
                        OrdersInfoItem.IdTree = IdTree;    

                        OrdersInfoItem.Id = Guid.NewGuid().ToString();
                        OrdersInfoItem.IdDetails = OrdersInfoItem.Id;
                        OrdersInfoItem.ReportsDate = FilterOrderDate;
                        OrdersInfoItem.IsSystemOrg = s.Value;
                        tmp.Add(OrdersInfoItem);
                    }
                }
            }

            TruckInfo.AddRange(tmp);
            tmp = null;
        }

        private static String AddSystemOrg(string Name, DateTime FilterOrderDate, ref List<TruckViewModel> TruckInfo, String IdTree)
        {
            var OrdersInfoItem = new TruckViewModel();
            OrdersInfoItem.IdParent = null;
            OrdersInfoItem.Name = Name;
            OrdersInfoItem.Id = Guid.NewGuid().ToString();
            OrdersInfoItem.IsLeaf = false;
            OrdersInfoItem.IdGroudId = 1;
            OrdersInfoItem.IdDetails = OrdersInfoItem.Id;
            OrdersInfoItem.ReportsDate = FilterOrderDate;
            OrdersInfoItem.IdTree = IdTree;    
            TruckInfo.Add(OrdersInfoItem);
            return OrdersInfoItem.Id;
        }

        private static void TruckOrganization(ref TruckViewModel OrdersInfoItem, GetTruckReport_Result o)
        {
            OrdersInfoItem.TruckDescription = o.TruckDescription;
            OrdersInfoItem.ExpeditorName = o.ExpeditorName;
            OrdersInfoItem.ExpeditorId = o.ExpeditorId;
            OrdersInfoItem.CarModelInfo = o.CarModelInfo;
            OrdersInfoItem.CarRegNum = o.CarRegNum;
            OrdersInfoItem.CarDriverInfo = o.CarDriverInfo;
            OrdersInfoItem.PlanDateTime = OrdersInfoItem.isShipper ? o.FromShipperDatetime : o.ToConsigneeDatetime;
            OrdersInfoItem.FactDateTime = OrdersInfoItem.isShipper ? o.FactShipperDateTime : o.FactConsigneeDateTime;
            OrdersInfoItem.PlanTime = OrdersInfoItem.PlanDateTime?.ToString("HH:mm");
            OrdersInfoItem.FactTime = OrdersInfoItem.FactDateTime?.ToString("HH:mm");

            OrdersInfoItem.PlanDate = OrdersInfoItem.PlanDateTime?.ToString("dd.MM.yyyy");
            OrdersInfoItem.FactDate = OrdersInfoItem.FactDateTime?.ToString("dd.MM.yyyy");

            OrdersInfoItem.DateFactConsignee = o.FactConsignee?.ToString("dd.MM.yyyy");
            OrdersInfoItem.TimeFactConsignee = o.FactConsignee?.ToString("HH:mm");
            OrdersInfoItem.BalanceKeeper = o.BalanceKeeper;
            OrdersInfoItem.CreatorByUserName = o.CreatorByUserName;
            OrdersInfoItem.IsShipperString = OrdersInfoItem.isShipper ? "Отгрузка" : "Поступление";
            OrdersInfoItem.CarCapacity = o.CarCapacity; 
            OrdersInfoItem.IsLeaf = true;
            OrdersInfoItem.IdGroudId = 6;
        }

        private static void CountriesSystemOrg(TruckViewModel OrdersInfoItem, ref Dictionary<int, TruckViewModel> CountriesShipper,
           ref Dictionary<int, TruckViewModel> CountriesConsignee)
        {
            if (OrdersInfoItem.IsSystemOrg)
            {
                if (!CountriesShipper.ContainsKey(OrdersInfoItem.ShipperCountryId))
                {
                    TruckViewModel CountryItem = new TruckViewModel();
                    CountryItem.ShipperCountryName = OrdersInfoItem.ShipperCountryName;
                    CountryItem.IsSystemOrg = OrdersInfoItem.IsSystemOrg;

                    CountriesShipper.Add(OrdersInfoItem.ShipperCountryId, CountryItem);
                }
            }
            else
            {
                if (!CountriesConsignee.ContainsKey(OrdersInfoItem.ShipperCountryId))
                {
                    TruckViewModel CountryItem2 = new TruckViewModel();
                    CountryItem2.ShipperCountryName = OrdersInfoItem.ShipperCountryName;
                    CountryItem2.IsSystemOrg = OrdersInfoItem.IsSystemOrg;
                    CountriesConsignee.Add(OrdersInfoItem.ShipperCountryId, CountryItem2);
                }
            }
        }

        public IQueryable<TruckViewModel> getTruckReportDetails2(List<TruckViewModel> TruckInfo,
            int IdGroup, string Id)
        {
            IQueryable<TruckViewModel> res = null;
            var data = TruckInfo.Where(x => x.Id == Id).FirstOrDefault();
            switch (IdGroup)
            {
                case 1:  //системная фирма                                       
                    res = TruckInfo.Where(x => x.IdGroudId == 6 && x.IsSystemOrg == data?.IsSystemOrg).AsQueryable();
                    break;
                case 2: //страна                    
                    res = TruckInfo.Where(x => x.IdGroudId == 6 && 
                                               x.ShipperCountryId == data?.ShipperCountryId &&
                                               x.IsSystemOrg == data.IsSystemOrg).AsQueryable();
                    break;
                case 3: //город                    
                    res = TruckInfo.Where(x => x.IdGroudId == 6 && x.ShipperCity.Equals(data?.ShipperCity) &&
                                               x.ShipperCountryId == data?.ShipperCountryId &&
                                               x.IsSystemOrg == data.IsSystemOrg).AsQueryable();
                    break;
                case 4: //адрес
                      res = TruckInfo.Where(x => x.IdGroudId == 6 && x.ShipperAdress.Equals(data?.ShipperAdress) &&
                                               x.ShipperCity.Equals(data?.ShipperCity) && 
                                               x.ShipperCountryId == data?.ShipperCountryId &&
                                               x.IsSystemOrg == data.IsSystemOrg).AsQueryable();
                    break;
                case 5: //организация                                       
                    res = TruckInfo.Where(x => x.IdGroudId == 6 && x.ShipperId == data.ShipperId &&
                                               x.ShipperAdress.Equals(data?.ShipperAdress) &&
                                               x.ShipperCity.Equals(data?.ShipperCity) && 
                                               x.ShipperCountryId == data?.ShipperCountryId &&
                                               x.IsSystemOrg == data.IsSystemOrg).AsQueryable();
                    break;
                default:
                    break;
                    
            }

            //int IdGroupId1 = 3; //по городу
            // String id1 = "4746d166-cb4f-4bd5-b0b9-5ba59a0650fa";
            return res?.AsQueryable();        
        }

        public String getTruckReportTitle(List<TruckViewModel> TruckInfo,
            int IdGroup, string Id, ref String Address)
        {
            String res = "";
            Address = "";
            var data = TruckInfo.Where(x => x.Id == Id).FirstOrDefault();
            switch (IdGroup)
            {
                case 1:  //системная фирма                                       
                    res = data?.Name;
                    break;
                case 2: //страна                    
                    res = data?.ShipperCountryName;
                    break;
                case 3: //город                    
                    res = data?.ShipperCity;
                    break;
                case 4: //адрес
                      res = data?.ShipperAdress;
                    break;
                case 5: //организация                                       
                    res = data?.Shipper;
                    Address = String.Concat("Адрес:",data?.ShipperAdress);
                        //isShipper ? GetShipperAddress(data.OrderId) : GetConsigneeAddress(data.OrderId);
                    break;
                default:
                    break;                    
            }
            return res;
        }


        public IQueryable<TruckReportViewModel> getTruckReportDetails(string userId,
            bool isAdmin,
            int OrgId,
            DateTime ReportDate, int IdGroudId, string Id)
        {
            //var orgInfo = GetOrganization(OrgId);
            List<GetTruckReportDetails_Result> OrdersItems=null;
            //if (IdGroudId == 5)
            OrdersItems = db.GetTruckReportDetails(userId,
              isAdmin,
              OrgId,
              ReportDate).ToList();


            List<TruckReportViewModel> truckInfo = new List<TruckReportViewModel>();           
            
            foreach (var o in OrdersItems)
            {
                TruckReportViewModel truckInfoItem = new TruckReportViewModel();
             //   truckInfoItem.ShipperName = orgInfo.Name;
                truckInfoItem.OrderId = o.Id;

                truckInfoItem.Shipper = o.Shipper;
                truckInfoItem.Consignee = o.Consignee;

                truckInfoItem.ShipperId = o.ShipperId ??0;

                //как понять где Отгрузка а где Поступление
               // По тому в грузоотправителях или в грузополучателях находится организация
                truckInfoItem.isShipper = o.ShipperId == OrgId;
               // truckInfoItem.ConsigneeId = o.ConsigneeId;

                truckInfoItem.TruckDescription = o.TruckDescription;
                truckInfoItem.ExpeditorName = o.ExpeditorName;
                truckInfoItem.ExpeditorId = o.ExpeditorId;
                truckInfoItem.CarModelInfo = o.CarModelInfo;
                truckInfoItem.CarRegNum = o.CarRegNum;
                truckInfoItem.CarDriverInfo = o.CarDriverInfo;

                truckInfoItem.PlanDateTime = truckInfoItem.isShipper ? o.FromShipperDatetime : o.ToConsigneeDatetime;
                truckInfoItem.FactDateTime = truckInfoItem.isShipper ? o.FactShipperDateTime : o.FactConsigneeDateTime;
                truckInfoItem.PlanTime = truckInfoItem.PlanDateTime?.ToString("HH:mm");
                truckInfoItem.FactTime = truckInfoItem.FactDateTime?.ToString("HH:mm");

                truckInfoItem.PlanDate = truckInfoItem.PlanDateTime?.ToString("dd.MM.yyyy");
                truckInfoItem.FactDate = truckInfoItem.FactDateTime?.ToString("dd.MM.yyyy");               

                truckInfoItem.DateFactConsignee = o.FactConsignee?.ToString("dd.MM.yyyy");
                truckInfoItem.TimeFactConsignee = o.FactConsignee?.ToString("HH:mm");
                truckInfoItem.BalanceKeeper = o.BalanceKeeper;
                truckInfoItem.CreatorByUserName = o.CreatorByUserName;
                truckInfoItem.IsShipperString = truckInfoItem.isShipper ? "Отгрузка" : "Поступление";
                /*truckInfoItem.Address = truckInfoItem.isShipper
                    ? GetShipperAddress(o.Id)
                    : GetConsigneeAddress(o.Id);*/
                truckInfoItem.CarCapacity = o.CarCapacity;
              if (truckInfoItem.PlanDateTime?.ToString("dd.MM.yyyy") == ReportDate.ToString("dd.MM.yyyy"))
                truckInfo.Add(truckInfoItem);
            }
            return truckInfo.AsQueryable();        
        }

        public string GetShipperAddress(long orderId)
        {
              var orderInfo2 = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo2 != null)
                    {
                        return string.Concat(orderInfo2.Countries1 == null ? null : orderInfo2.Countries1.Name, ", ", orderInfo2.ShipperCity, ", ", orderInfo2.ShipperAdress);
                    }
                    
            return string.Empty;
        }

         public string GetConsigneeAddress(long orderId)
        {
              var orderInfo2 = db.OrderTruckTransport.AsNoTracking().FirstOrDefault(x => x.OrderId == orderId);

                    if (orderInfo2 != null)
                    {
                        return string.Concat(orderInfo2.Countries == null ? null : orderInfo2.Countries.Name, ", ", orderInfo2.ConsigneeCity, ", ", orderInfo2.ConsigneeAdress);
                    }
                    
            return string.Empty;
        }
    }
}
