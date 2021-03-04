using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Corum.Models;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Orders;
using CorumAdminUI.Helpers;
using Corum.Models.Toastr;
using Corum.Common;
using CorumAdminUI.Common;
using Corum.Models.ViewModels.Cars;
using Corum.Models.ViewModels.Customers;
using System.Configuration;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace CorumAdminUI.Controllers
{
    [Authorize]
    public partial class OrdersController : CorumBaseController
    {
        public OrdersController()
        {
            var force = typeof(OrdersPassTransportViewModel).Assembly;
        }

        public ActionResult PipelineSteps(PipelinesNavigationInfo navInfo)
        {
            var orderTypes = context.getAvailableOrderTypes(null, null).ToList();
            var orderTypeId = (navInfo.OrderTypeId != null) ? navInfo.OrderTypeId.Value : orderTypes.FirstOrDefault().Id;
            var displayValues = context.getPipelineSteps(userId, orderTypeId).ToList();
            var model = new PipelinesNavigationResult<OrderPipelineStepViewModel>(navInfo, userId)
            {
                DisplayValues = displayValues.AsQueryable(),
                AvailiableTypes = orderTypes,
                OrderTypeId = orderTypeId
            };
            return View(model);
        }

        public ActionResult Statuses(NavigationInfo navInfo)
        {
            var model = new NavigationResult<OrderStatusViewModel>(navInfo, userId)
            {
                DisplayValues = context.getAvailableOrderStatuses(userId)
            };
            return View(model);
        }

        public ActionResult OrdersClients(NavigationInfo navInfo)
        {
            var model = new NavigationResult<OrderClientsViewModel>(navInfo, userId)
            {
                DisplayValues = isAdmin ? context.getClients(userId) : context.getClientsInPipeline(userId)
            };
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Orders(OrdersNavigationInfo navInfo)
        {
            if (string.IsNullOrEmpty(navInfo.FilterStatusId)) { navInfo.UseStatusFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderCreatorId)) { navInfo.UseOrderCreatorFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId)) { navInfo.UseOrderTypeFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderClientId)) { navInfo.UseOrderClientFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderExecuterId)) { navInfo.UseOrderExecuterFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderProjectId)) { navInfo.UseOrderProjectFilter = false; }

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


            var model = new OrdersNavigationResult<OrderBaseViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getOrders(true,
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
                                                    navInfo.UseOrderOrgToFilter),

                AvailiableTypes =
                                   context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                                   .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,
                DriftDate = navInfo.DriftDate,
                AcceptDate = navInfo.AcceptDate,
                ExecuteDate = navInfo.ExecuteDate,

                FilterStatusId = navInfo.FilterStatusId,
                UseStatusFilter = navInfo.UseStatusFilter,

                FilterOrderCreatorId = navInfo.FilterOrderCreatorId,
                UseOrderCreatorFilter = navInfo.UseOrderCreatorFilter,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,

                FilterOrderPriority = navInfo.FilterOrderPriority,
                UseOrderPriorityFilter = navInfo.UseOrderPriorityFilter,

                FilterOrderExecuterId = navInfo.FilterOrderExecuterId,
                UseOrderExecuterFilter = navInfo.UseOrderExecuterFilter,

                UseFinalStatusFilter = navInfo.UseFinalStatusFilter,
                FilterFinalStatus = navInfo.FilterFinalStatus,

                UseOrderProjectFilter = navInfo.UseOrderProjectFilter,
                FilterOrderProjectId = navInfo.FilterOrderProjectId,

                UseOrderPayerFilter = navInfo.UseOrderPayerFilter,
                FilterOrderPayerId = navInfo.FilterOrderPayerId,

                UseOrderOrgFromFilter = navInfo.UseOrderOrgFromFilter,
                FilterOrderOrgFromId = navInfo.FilterOrderOrgFromId,

                UseOrderOrgToFilter = navInfo.UseOrderOrgToFilter,
                FilterOrderOrgToId = navInfo.FilterOrderOrgToId,

            };

            model.isChrome = Request.Browser.Type.Contains("Chrome");
            model.UseOrderDateFilter = navInfo.UseOrderDateFilter;
            model.FilterOrderDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateBeg;
            model.FilterOrderDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderDateBegRaw;
            model.FilterOrderDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateEnd;
            model.FilterOrderDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderDateEndRaw;

            model.UseOrderExDateFilter = navInfo.UseOrderExDateFilter;
            model.FilterOrderExDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderExDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderExDateBeg;
            model.FilterOrderExDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderExDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderExDateBegRaw;
            model.FilterOrderExDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderExDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderExDateEnd;
            model.FilterOrderExDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderExDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderExDateEndRaw;

            model.UseOrderEndDateFilter = navInfo.UseOrderEndDateFilter;
            model.FilterOrderEndDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderEndDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderEndDateBeg;
            model.FilterOrderEndDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderEndDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderEndDateBegRaw;
            model.FilterOrderEndDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderEndDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderEndDateEnd;
            model.FilterOrderEndDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderEndDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderEndDateEndRaw;

            if ((!model.AcceptDate) && (!model.ExecuteDate))
            {
                model.DriftDate = true;
                model.AcceptDate = true;
                model.ExecuteDate = true;

            }

            if (!string.IsNullOrEmpty(navInfo.FilterStatusId))
            {
                string[] idList = navInfo.FilterStatusId.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterStatusName = "";
                    foreach (string i in idList)
                    {
                        if (FilterStatusName.Length > 0)
                        {
                            FilterStatusName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                            FilterStatusName = string.Concat(FilterStatusName, context.getStatus(Convert.ToInt32(i))?.StatusName);
                        else
                        {
                            model.UseStatusFilter = false;
                            break;
                        }
                    }
                    model.FilterStatusName = FilterStatusName;
                }
                else
                {
                    model.UseStatusFilter = false;
                }
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderCreatorId))
            {
                string[] idList = navInfo.FilterOrderCreatorId.Split(new char[] { ',' });
                string FilterOrderCreatorName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderCreatorName.Length > 0)
                    {
                        FilterOrderCreatorName += ",";
                    }
                    FilterOrderCreatorName = string.Concat(FilterOrderCreatorName, context.getUser(i)?.displayName);
                }
                model.FilterOrderCreatorName = FilterOrderCreatorName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderProjectId))
            {
                string[] idList = navInfo.FilterOrderProjectId.Split(new char[] { ',' });
                string FilterOrderProjectCode = "";

                foreach (string i in idList)
                {
                    if (FilterOrderProjectCode.Length > 0)
                    {
                        FilterOrderProjectCode += ",";
                    }
                    FilterOrderProjectCode = string.Concat(FilterOrderProjectCode, context.GetProjectById(Convert.ToInt32(i))?.Code);
                }
                model.FilterOrderProjectCode = FilterOrderProjectCode;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderExecuterId))
            {
                string[] idList = navInfo.FilterOrderExecuterId.Split(new char[] { ',' });
                string FilterOrderExecuterName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderExecuterName.Length > 0)
                    {
                        FilterOrderExecuterName += ",";
                    }

                    FilterOrderExecuterName = string.Concat(FilterOrderExecuterName, context.getUser(i)?.displayName);
                }
                model.FilterOrderExecuterName = FilterOrderExecuterName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                string[] idList = navInfo.FilterOrderTypeId.Split(new char[] { ',' });
                string FilterOrderTypeName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderTypeName.Length > 0)
                    {
                        FilterOrderTypeName += ",";
                    }

                    FilterOrderTypeName = string.Concat(FilterOrderTypeName, context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }
                model.FilterOrderTypeName = FilterOrderTypeName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderClientId))
            {
                string[] idList = navInfo.FilterOrderClientId.Split(new char[] { ',' });
                string FilterOrderClientName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderClientName.Length > 0)
                    {
                        FilterOrderClientName += ",";
                    }

                    var client = context.getClient(Convert.ToInt32(i));
                    FilterOrderClientName = string.Concat(FilterOrderClientName, string.Concat(client?.ClientBalanceKeeperName, "/", client?.ClientName));
                }
                model.FilterOrderClientName = FilterOrderClientName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderPayerId))
            {
                string[] idList = navInfo.FilterOrderPayerId.Split(new char[] { ',' });
                string FilterOrderPayerName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderPayerName.Length > 0)
                    {
                        FilterOrderPayerName += ",";
                    }

                    var PayerName = context.getPayer(Convert.ToInt32(i));
                    FilterOrderPayerName = string.Concat(FilterOrderPayerName, string.Concat(PayerName?.BalanceKeeperName));
                }
                model.FilterOrderPayerName = FilterOrderPayerName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderOrgFromId))
            {
                string[] idList = navInfo.FilterOrderOrgFromId.Split(new char[] { ',' });
                string FilterOrderOrgFromName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderOrgFromName.Length > 0)
                    {
                        FilterOrderOrgFromName += ",";
                    }

                    var OrgFromName = context.GetOrganization(Convert.ToInt32(i));
                    FilterOrderOrgFromName = string.Concat(FilterOrderOrgFromName, string.Concat(OrgFromName?.Name));
                }
                model.FilterOrderOrgFromName = FilterOrderOrgFromName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderOrgToId))
            {
                string[] idList = navInfo.FilterOrderOrgToId.Split(new char[] { ',' });
                string FilterOrderOrgToName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderOrgToName.Length > 0)
                    {
                        FilterOrderOrgToName += ",";
                    }

                    var OrgToName = context.GetOrganization(Convert.ToInt32(i));
                    FilterOrderOrgToName = string.Concat(FilterOrderOrgToName, string.Concat(OrgToName?.Name));
                }
                model.FilterOrderOrgToName = FilterOrderOrgToName;
            }
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrdersBase(OrdersNavigationInfo navInfo)
        {
            if (string.IsNullOrEmpty(navInfo.FilterStatusId)) { navInfo.UseStatusFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderCreatorId)) { navInfo.UseOrderCreatorFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId)) { navInfo.UseOrderTypeFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderClientId)) { navInfo.UseOrderClientFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderExecuterId)) { navInfo.UseOrderExecuterFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderProjectId)) { navInfo.UseOrderProjectFilter = false; }

            if ((!navInfo.UseStatusFilter)
                 && (!navInfo.UseOrderCreatorFilter)
                 && (!navInfo.UseOrderTypeFilter)
                 && (!navInfo.UseOrderClientFilter)
                 && (!navInfo.UseOrderExecuterFilter)
                 && (!navInfo.UseOrderPriorityFilter)
                 && (!navInfo.UseOrderDateFilter)
                 && (!navInfo.UseOrderExDateFilter)
                 && (!navInfo.UseOrderEndDateFilter)
                 && (!navInfo.UseOrderProjectFilter)
                 && (!navInfo.UseFinalStatusFilter))
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

            var model = new OrdersNavigationResult<OrderBaseViewModel>(navInfo, userId)
            {
                isTransport = false,
                DisplayValues = context.getOrders(false,
                                                    userId,
                                                    isAdmin,
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
                                                    navInfo.UseOrderOrgToFilter),

                AvailiableTypes =
                                   context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, false)
                                   .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,
                DriftDate = navInfo.DriftDate,
                AcceptDate = navInfo.AcceptDate,
                ExecuteDate = navInfo.ExecuteDate,

                FilterStatusId = navInfo.FilterStatusId,
                UseStatusFilter = navInfo.UseStatusFilter,

                FilterOrderCreatorId = navInfo.FilterOrderCreatorId,
                UseOrderCreatorFilter = navInfo.UseOrderCreatorFilter,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,

                FilterOrderPriority = navInfo.FilterOrderPriority,
                UseOrderPriorityFilter = navInfo.UseOrderPriorityFilter,

                UseOrderProjectFilter = navInfo.UseOrderProjectFilter,
                FilterOrderProjectId = navInfo.FilterOrderProjectId,

                FilterOrderExecuterId = navInfo.FilterOrderExecuterId,
                UseOrderExecuterFilter = navInfo.UseOrderExecuterFilter,

                UseFinalStatusFilter = navInfo.UseFinalStatusFilter,
                FilterFinalStatus = navInfo.FilterFinalStatus

            };

            model.UseOrderDateFilter = navInfo.UseOrderDateFilter;
            model.FilterOrderDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateBeg;
            model.FilterOrderDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderDateBegRaw;
            model.FilterOrderDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateEnd;
            model.FilterOrderDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderDateEndRaw;

            model.UseOrderExDateFilter = navInfo.UseOrderExDateFilter;
            model.FilterOrderExDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderExDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderExDateBeg;
            model.FilterOrderExDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderExDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderExDateBegRaw;
            model.FilterOrderExDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderExDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderExDateEnd;
            model.FilterOrderExDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderExDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderExDateEndRaw;

            model.UseOrderEndDateFilter = navInfo.UseOrderEndDateFilter;
            model.FilterOrderEndDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderEndDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderEndDateBeg;
            model.FilterOrderEndDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderEndDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderEndDateBegRaw;
            model.FilterOrderEndDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderEndDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderEndDateEnd;
            model.FilterOrderEndDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderEndDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderEndDateEndRaw;

            if ((!model.AcceptDate) && (!model.ExecuteDate))
            {
                model.DriftDate = true;
                model.AcceptDate = true;
                model.ExecuteDate = true;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterStatusId))
            {
                string[] idList = navInfo.FilterStatusId.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterStatusName = "";
                    foreach (string i in idList)
                    {
                        if (FilterStatusName.Length > 0)
                        {
                            FilterStatusName += ",";
                        }

                        if (Convert.ToInt32(i) > 0)
                            FilterStatusName = string.Concat(FilterStatusName, context.getStatus(Convert.ToInt32(i))?.StatusName);
                        else
                        {
                            model.UseStatusFilter = false;
                            break;
                        }
                    }
                    model.FilterStatusName = FilterStatusName;
                }
                else
                {
                    model.UseStatusFilter = false;
                }
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderCreatorId))
            {
                string[] idList = navInfo.FilterOrderCreatorId.Split(new char[] { ',' });
                string FilterOrderCreatorName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderCreatorName.Length > 0)
                    {
                        FilterOrderCreatorName += ",";
                    }

                    FilterOrderCreatorName = string.Concat(FilterOrderCreatorName, context.getUser(i)?.displayName);
                }

                model.FilterOrderCreatorName = FilterOrderCreatorName;

            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderExecuterId))
            {
                string[] idList = navInfo.FilterOrderExecuterId.Split(new char[] { ',' });
                string FilterOrderExecuterName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderExecuterName.Length > 0)
                    {
                        FilterOrderExecuterName += ",";
                    }

                    FilterOrderExecuterName = string.Concat(FilterOrderExecuterName, context.getUser(i)?.displayName);
                }

                model.FilterOrderExecuterName = FilterOrderExecuterName;

            }

              if (!string.IsNullOrEmpty(navInfo.FilterOrderProjectId))
            {
                string[] idList = navInfo.FilterOrderProjectId.Split(new char[] { ',' });
                string FilterOrderProjectCode = "";

                foreach (string i in idList)
                {
                    if (FilterOrderProjectCode.Length > 0)
                    {
                        FilterOrderProjectCode += ",";
                    }
                    FilterOrderProjectCode = string.Concat(FilterOrderProjectCode, context.GetProjectById(Convert.ToInt32(i))?.Code);
                }
                model.FilterOrderProjectCode = FilterOrderProjectCode;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                string[] idList = navInfo.FilterOrderTypeId.Split(new char[] { ',' });
                string FilterOrderTypeName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderTypeName.Length > 0)
                    {
                        FilterOrderTypeName += ",";
                    }

                    FilterOrderTypeName = string.Concat(FilterOrderTypeName, context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }

                model.FilterOrderTypeName = FilterOrderTypeName;

            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderClientId))
            {
                string[] idList = navInfo.FilterOrderClientId.Split(new char[] { ',' });
                string FilterOrderClientName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderClientName.Length > 0)
                    {
                        FilterOrderClientName += ",";
                    }

                    var client = context.getClient(Convert.ToInt32(i));
                    FilterOrderClientName = string.Concat(FilterOrderClientName, string.Concat(client?.ClientBalanceKeeperName, "/", client?.ClientName));
                }

                model.FilterOrderClientName = FilterOrderClientName;
            }

            return View(model);
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrderNotifications(OrderNavigationInfo navInfo)
        {
            var model = new OrderNavigationResult<OrderNotificationViewModel>(navInfo, userId)
            {
                DisplayValues = context.getNotifications(navInfo.OrderId),
                orderInfo = context.getOrder(navInfo.OrderId)
            };
            return View(model);
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrderStatuses(OrderNavigationInfo navInfo)
        {
            var model = new OrderNavigationResult<OrderStatusHistoryViewModel>(navInfo, userId)
            {
                DisplayValues = context.getOrderStatusHistory(userId, navInfo.OrderId),
                orderInfo = context.getOrder(navInfo.OrderId)
            };
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrderAttachments(OrderNavigationInfo navInfo)
        {
            var model = new OrderNavigationResult<OrderAttachmentViewModel>(navInfo, userId)
            {
                DisplayValues = context.getOrderAttachments(userId, navInfo.OrderId),
                orderInfo = context.getOrder(navInfo.OrderId)
            };
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrderObservers(OrderNavigationInfo navInfo)
        {
            var model = new OrderNavigationResult<OrderObserverViewModel>(navInfo, userId)
            {
                DisplayValues = context.getOrderObservers(navInfo.OrderId),

                orderInfo = context.getOrder(navInfo.OrderId)
            };
            return View(model);
        }



        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewPipelineStep(int OrderTypeId)
        {
            var model = new OrderPipelineStepViewModel()
            {
                AvailiabeFromStatuses = context.getAvailableOrderStatuses(userId).ToList(),
                AvailiabeToStatuses = context.getAvailableOrderStatuses(userId).ToList(),
                AvailiabeRoles = context.getRoles(string.Empty).ToList()
            };

            var types = context.getAvailableOrderTypes(null, null).ToList();
            model.OrderTypeId = OrderTypeId;
            model.OrderTypeName = types.FirstOrDefault(t => t.Id == OrderTypeId).TypeName;

            return View(model);
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdatePipelineStep(int Id)
        {
            var model = context.getStep(Id);

            if (model != null)
            {
                model.AvailiabeFromStatuses = context.getAvailableOrderStatuses(userId).ToList();
                model.AvailiabeToStatuses = context.getAvailableOrderStatuses(userId).ToList();
                model.AvailiabeRoles = context.getRoles(string.Empty).ToList();
            }

            return View(model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewPipelineStep(OrderPipelineStepViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailiabeFromStatuses = context.getAvailableOrderStatuses(userId).ToList();
                model.AvailiabeToStatuses = context.getAvailableOrderStatuses(userId).ToList();
                model.AvailiabeRoles = context.getRoles(string.Empty).ToList();

                return View(model);
            }

            if (context.NewPipelineStep(model))
            {
                return RedirectToAction("PipelineSteps", "Orders", new { OrderTypeId = model.OrderTypeId });
            }

            model.AvailiabeFromStatuses = context.getAvailableOrderStatuses(userId).ToList();
            model.AvailiabeToStatuses = context.getAvailableOrderStatuses(userId).ToList();
            model.AvailiabeRoles = context.getRoles(string.Empty).ToList();

            var types = context.getAvailableOrderTypes(null, null).ToList();
            model.OrderTypeId = model.OrderTypeId;
            model.OrderTypeName = types.FirstOrDefault(t => t.Id == model.OrderTypeId).TypeName;

            AddToastMessage("Внимание", "Возможно подобный шаг уже сконфигурирован или введенные данные не корректны", toastType: ToastType.Error);

            return View(model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdatePipelineStep(OrderPipelineStepViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailiabeFromStatuses = context.getAvailableOrderStatuses(userId).ToList();
                model.AvailiabeToStatuses = context.getAvailableOrderStatuses(userId).ToList();
                model.AvailiabeRoles = context.getRoles(string.Empty).ToList();

                return View(model);
            }

            if (context.UpdatePipelineStep(model))
            {
                return RedirectToAction("PipelineSteps", "Orders", new { OrderTypeId = model.OrderTypeId });
            }

            model.AvailiabeFromStatuses = context.getAvailableOrderStatuses(userId).ToList();
            model.AvailiabeToStatuses = context.getAvailableOrderStatuses(userId).ToList();
            model.AvailiabeRoles = context.getRoles(string.Empty).ToList();


            AddToastMessage("Внимание", "Введенные данные не корректны", toastType: ToastType.Error);

            return View(model);
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewAttachment(long OrderId)
        {
            var types = context.getAvailabbleDocTypes(userId).Select(type =>
            new SelectListItem()
            {
                Value = type.Id.ToString(),
                Text = type.DocTypeName
            }).ToList();

            ViewBag.DocTypes = types;

            return View(new OrderAttachmentViewModel()
            {
                OrderId = OrderId,
                DocType = 1
            });
        }

        [HttpPost]
        public ActionResult NewAttachment(OrderAttachmentViewModel model, HttpPostedFileBase DocumentFile)
        {
            if (ModelState.IsValid)
            {
                if (DocumentFile != null)
                {
                    using (var inputStream = DocumentFile.InputStream)
                    {
                        var memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        model.DocBody = memoryStream.ToArray();
                        model.RealFileName = DocumentFile.FileName;
                    }
                }

                model.AddedByUser = userId;
                model.AddedDateTime = DateTime.Now;

                context.NewAttachment(model);
            }
            else return View(model);

            return RedirectToAction("OrderAttachments", "Orders", new { OrderId = model.OrderId });
        }



        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewObserver(long OrderId)
        {
            var orderObserverInfo = new OrderObserverViewModel()
            {
                AvailiableObserver = context.getUsers(string.Empty).ToList(),
                OrderId = OrderId
            };

            return View(orderObserverInfo);
        }

        [HttpPost]
        public ActionResult NewObserver(OrderObserverViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            context.NewObserver(model);

            return RedirectToAction("OrderObservers", "Orders", new { OrderId = model.OrderId });
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteOrder(long Id)
        {
            var orderInfo = context.getOrder(Id);
            context.DeleteOrder(Id);

            if (orderInfo.IsTransport)
                return RedirectToAction("Orders", "Orders");

            return RedirectToAction("OrdersBase", "Orders");
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeletePipelineStep(int Id)
        {
            context.DeletePipelineStep(Id);
            return RedirectToAction("PipelineSteps", "Orders");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteObserver(long Id, long OrderId)
        {
            context.DeleteObserver(Id);
            return RedirectToAction("OrderObservers", "Orders", new { OrderId = OrderId });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteAttachment(long Id, long OrderId)
        {
            context.DeleteAttachment(Id);
            return RedirectToAction("OrderAttachments", "Orders", new { OrderId = OrderId });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public EmptyResult DownloadAttachment(int Id)
        {
            var orderAttachment = context.getAttachment(Id);
            if (orderAttachment == null) return new EmptyResult();

            if (orderAttachment.DocBody == null) return new EmptyResult();

            Response.Clear();
            var ms = new MemoryStream(orderAttachment.DocBody);
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("content-disposition", string.Concat("attachment;filename=", orderAttachment.RealFileName));
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
            return new EmptyResult();
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewClient()
        {
            var orderClientInfo = new OrderClientsViewModel()
            {
                AvailableRoles = isAdmin ? context.getRoles(string.Empty).ToList() : context.getRolesInPipeline(userId).ToList(),
                AvailableKeepers = context.getBalanceKeepers(userId).ToList(),
                AvailableCFOs = context.getCenters(userId).ToList()
            };

            return View(orderClientInfo);
        }


        [HttpPost]
        public ActionResult NewClient(OrderClientsViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            context.NewClient(model);

            return RedirectToAction("OrdersClients", "Orders");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateClient(long Id)
        {
            var orderClientInfo = context.getClient(Id);
            if (orderClientInfo != null)
            {
                orderClientInfo.AvailableRoles = isAdmin ? context.getRoles(string.Empty).ToList() : context.getRolesInPipeline(userId).ToList();
                orderClientInfo.AvailableKeepers = context.getBalanceKeepers(userId).ToList();
                orderClientInfo.AvailableCFOs = context.getCenters(userId).ToList();
            };
            return View(orderClientInfo);
        }

        [HttpPost]
        public ActionResult UpdateClient(OrderClientsViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            context.UpdateClient(model);
            return RedirectToAction("OrdersClients", "Orders");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteClient(long Id)
        {
            context.DeleteClient(Id);
            return RedirectToAction("OrdersClients", "Orders");
        }

        [HttpGet]
        [AllowAnonymous]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewOrder(int OrderTypeId, bool PublicEntry = false)
        {
            OrderBaseViewModel OrderTypeModel = null;
            OrderCountriesViewModel DefaultCounty = null;
            UserProfileViewModel UserProfileInfo = context.getUserProfileByUserId(this.userId);

            switch (OrderTypeId)
            {
                case 1:
                case 3:
                case 6:
                    OrderTypeModel = new OrdersPassTransportViewModel();
                    DefaultCounty = context.getDefaultCountry();

                    if (UserProfileInfo != null)
                    {
                        var UserCountryId = context.getCountryByUserId(this.userId);

                        if (UserCountryId > 0)
                        {
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFrom = UserCountryId;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFromName = context.getCountryNameByUserId(UserCountryId);
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryTo = UserCountryId;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryToName = context.getCountryNameByUserId(UserCountryId);
                        }
                        else
                        {
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFrom = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFromName = DefaultCounty.CountryName;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryTo = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryToName = DefaultCounty.CountryName;
                        }

                        ((OrdersPassTransportViewModel)OrderTypeModel).CityFrom = UserProfileInfo.City;
                        ((OrdersPassTransportViewModel)OrderTypeModel).CityTo = UserProfileInfo.City;
                        ((OrdersPassTransportViewModel)OrderTypeModel).AdressFrom = UserProfileInfo.AdressFrom;
                    }
                    else
                    {
                        ((OrdersPassTransportViewModel)OrderTypeModel).CountryFrom = DefaultCounty.Id;
                        ((OrdersPassTransportViewModel)OrderTypeModel).CountryFromName = DefaultCounty.CountryName;
                        ((OrdersPassTransportViewModel)OrderTypeModel).CountryTo = DefaultCounty.Id;
                        ((OrdersPassTransportViewModel)OrderTypeModel).CountryToName = DefaultCounty.CountryName;
                    }

                    ((OrdersPassTransportViewModel)OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                    ((OrdersPassTransportViewModel)OrderTypeModel).DefaultCountryName = DefaultCounty.CountryName;

                    ((OrdersPassTransportViewModel)OrderTypeModel).TripType = 0;

                    break;

                case 4:
                case 5:
                case 7:

                    OrderTypeModel = new OrdersTruckTransportViewModel();
                    DefaultCounty = context.getDefaultCountry();

                    UserProfileInfo = context.getUserProfileByUserId(this.userId);

                    if (UserProfileInfo != null)
                    {
                        var UserCountryId = context.getCountryByUserId(this.userId);

                        if (UserCountryId > 0)
                        {
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryId = UserCountryId;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryName = context.getCountryNameByUserId(UserCountryId);
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryId = UserCountryId;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryName = context.getCountryNameByUserId(UserCountryId);
                        }
                        else
                        {
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryId = DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryName = DefaultCounty.CountryName;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryId = DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryName = DefaultCounty.CountryName;
                        }

                        ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCity = UserProfileInfo.City;
                        ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCity = UserProfileInfo.City;
                        ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperAdress = UserProfileInfo.AdressFrom;
                    }
                    else
                    {
                        ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryId = DefaultCounty.Id;
                        ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryName = DefaultCounty.CountryName;
                        ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryId = DefaultCounty.Id;
                        ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryName = DefaultCounty.CountryName;
                    }

                    ((OrdersTruckTransportViewModel)OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                    ((OrdersTruckTransportViewModel)OrderTypeModel).DefaultCountryName = DefaultCounty.CountryName;

                    ((OrdersTruckTransportViewModel)OrderTypeModel).TripType = 0;


                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    OrderTypeModel = new OrderBaseViewModel();
                    break;
                default:
                    throw new NotImplementedException();
            }

            var OrderTypeFullInfo = context.getOrderType(OrderTypeId);
            var OrderStatusFullInfo = context.getStatus(1);

            var currentUser = PublicEntry ? OrderTypeFullInfo.UserIdForAnonymousForm : this.userId;


            OrderTypeModel.AvaliableExecuters = context.GetOrderExecutorsEx(OrderTypeId);

            OrderTypeModel.IsTransport = OrderTypeFullInfo.IsTransportType;
            OrderTypeModel.OrderExecuter = OrderTypeFullInfo.DefaultExecuterId;
            OrderTypeModel.OrderExecuterName = OrderTypeFullInfo.DefaultExecuterName;
            OrderTypeModel.CurrentOrderStatus = 1;
            OrderTypeModel.PriorityType = 0;
            OrderTypeModel.ClientId = -1;
            OrderTypeModel.Summ = 0;
            OrderTypeModel.UseNotifications = true;
            OrderTypeModel.CreatedByUserName = this.displayUserName;
            OrderTypeModel.CreatedByUser = currentUser;
            OrderTypeModel.OrderDate = DateTime.Now.ToString("dd.MM.yyyy");
            OrderTypeModel.OrderDateRaw = DateTimeConvertClass.getString(DateTime.Now);
            OrderTypeModel.OrderType = OrderTypeId;
            OrderTypeModel.OrderTypename = OrderTypeFullInfo.TypeName;

            OrderTypeModel.AllowClientData = IsAdmin(currentUser) ? true : (context.UserHasRole(currentUser, OrderTypeFullInfo.UserRoleIdForClientData) && OrderStatusFullInfo.AllowClientData);
            OrderTypeModel.AllowExecuterData = IsAdmin(currentUser) ? true : (context.UserHasRole(currentUser, OrderTypeFullInfo.UserRoleIdForExecuterData) && OrderStatusFullInfo.AllowExecuterData);

            OrderTypeModel.CurrentStatusActionName = OrderStatusFullInfo.ActionName;
            OrderTypeModel.PublicEntry = PublicEntry;
            OrderTypeModel.CreatedByUser = currentUser;

            OrderTypeModel.RouteInfoStr = "Необходимо привязать маршрут";

            OrderTypeModel.RoutePointsLoadInfo = null;
            OrderTypeModel.RoutePointsUnloadInfo = null;

            OrderTypeModel.TimeRoute = "00:00";
            OrderTypeModel.TimeSpecialVehicles = "00:00";
            var userfullInfo = context.getUser(this.userId);

            if (userfullInfo != null)
            {
                OrderTypeModel.CreatorId = this.userId;
                OrderTypeModel.CreatorContact = userfullInfo?.contactPhone;
                OrderTypeModel.CreatorPosition = string.Concat(userfullInfo?.displayName, " ", userfullInfo?.postName);
            }

            OrderTypeModel.SpecTypeInfo = context.getSpecificationTypes().ToList();

            Session["RoutePointLoadList"] = null;
            Session["RoutePointUnLoadList"] = null;
            Session["IsDbDataTakenLoad"] = null;
            Session["IsDbDataTakenUnLoad"] = null;
            Session["pointListDelete"] = null;
            Session["pointListUpdate"] = null;
            return View(OrderTypeModel);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult NewOrder(OrderBaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var OrderStatusFullInfo = context.getStatus(model.CurrentOrderStatus);
                var OrderTypeFullInfo = context.getAvailableOrderTypes(null, null).FirstOrDefault(t => t.Id == model.OrderType);

                var currentUser = model.PublicEntry ? OrderTypeFullInfo.UserIdForAnonymousForm : this.userId;

                model.OrderExecuter = OrderTypeFullInfo.DefaultExecuterId;
                model.OrderExecuterName = OrderTypeFullInfo.DefaultExecuterName;
                model.AvaliableExecuters = context.GetOrderExecutorsEx(model.OrderType);

                model.CurrentOrderStatus = 1;
                model.ClientId = -1;
                model.PriorityType = model.PriorityType;
                model.Summ = 0;
                model.UseNotifications = true;

                model.CreatedByUserName = context.getUser(currentUser).displayName;
                model.CreatedByUser = currentUser;

                model.OrderDate = DateTime.Now.ToString("dd.MM.yyyy");
                model.OrderDateRaw = DateTimeConvertClass.getString(DateTime.Now);
                model.OrderTypename = OrderTypeFullInfo?.TypeName;

                model.AllowClientData = IsAdmin(currentUser) ? true : (context.UserHasRole(currentUser, OrderTypeFullInfo.UserRoleIdForClientData) && OrderStatusFullInfo.AllowClientData);
                model.AllowExecuterData = IsAdmin(currentUser) ? true : (context.UserHasRole(currentUser, OrderTypeFullInfo.UserRoleIdForExecuterData) && OrderStatusFullInfo.AllowExecuterData);

                model.CurrentStatusActionName = context.getStatus(1)?.ActionName;



                model.CreatedByUser = currentUser;

                return View(model);
            }

            model.CreateDatetime = DateTime.Now;

            var pointListLoads = Session["RoutePointLoadList"] as List<OrderAdditionalRoutePointModel>;
            var pointListUnLoads = Session["RoutePointUnLoadList"] as List<OrderAdditionalRoutePointModel>;
            var Id = context.NewOrder(model);

            if (Id > 0)
            {
                if (pointListLoads != null)
                {
                    foreach (var pointListLoad in pointListLoads)
                    {
                        pointListLoad.OrderId = Id;
                        context.NewRoutePoint(pointListLoad);

                    }
                }
                if (pointListUnLoads != null)
                {
                    foreach (var pointListUnLoad in pointListUnLoads)
                    {
                        pointListUnLoad.OrderId = Id;
                        context.NewRoutePoint(pointListUnLoad);

                    }
                }
            }

            if (Id > 0)
            {
                context.AddDefaultObservers(Id, model.OrderType);

            }

            if (Id > 0)
            {
                if (model.PublicEntry)
                {
                    return RedirectToAction("OperationSuccess", "Public", new { Id = Id });
                }

                AddToastMessage("Инфо", "Информация о новой заявке успешно сохранена!", toastType: ToastType.Success);
                return RedirectToAction("UpdateOrder", "Orders", new { Id = Id });
            }
            else
            {
                var OrderStatusFullInfo = context.getStatus(model.CurrentOrderStatus);
                var OrderTypeFullInfo = context.getAvailableOrderTypes(null, null).FirstOrDefault(t => t.Id == model.OrderType);
                var currentUser = model.PublicEntry ? OrderTypeFullInfo.UserIdForAnonymousForm : this.userId;

                model.OrderExecuter = OrderTypeFullInfo.DefaultExecuterId;
                model.OrderExecuterName = OrderTypeFullInfo.DefaultExecuterName;
                model.AvaliableExecuters = context.GetOrderExecutorsEx(model.OrderType);

                model.CurrentOrderStatus = 1;
                model.ClientId = -1;
                model.PriorityType = model.PriorityType;
                model.Summ = 0;
                model.UseNotifications = true;
                model.CreatedByUserName = context.getUser(currentUser).displayName;
                model.CreatedByUser = currentUser;
                model.OrderDate = DateTime.Now.ToString("dd.MM.yyyy");
                model.OrderDateRaw = DateTimeConvertClass.getString(DateTime.Now);
                model.OrderTypename = OrderTypeFullInfo?.TypeName;

                model.AllowClientData = IsAdmin(currentUser) ? true : (context.UserHasRole(currentUser, OrderTypeFullInfo.UserRoleIdForClientData) && OrderStatusFullInfo.AllowClientData);
                model.AllowExecuterData = IsAdmin(currentUser) ? true : (context.UserHasRole(currentUser, OrderTypeFullInfo.UserRoleIdForExecuterData) && OrderStatusFullInfo.AllowExecuterData);

                model.CurrentStatusActionName = context.getStatus(1)?.ActionName;

                model.CreatedByUser = currentUser;

                return View(model);
            }
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CloneOrder(long Id)
        {
            OrderBaseViewModel OrderTypeModel = null;
            var DefaultCounty = context.getDefaultCountry();
            var orderInfo = context.getOrder(Id);
            var OrderTypeFullInfo = context.getAvailableOrderTypes(null, null).FirstOrDefault(t => t.Id == orderInfo.OrderType);

            if (orderInfo != null)
            {
                switch (orderInfo.OrderType)
                {
                    case 1:
                    case 3:
                    case 6:

                        OrderTypeModel = orderInfo.ConvertTo<OrdersPassTransportViewModel>();
                        var extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
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

                        break;

                    case 4:
                    case 5:
                    case 7:
                        OrderTypeModel = orderInfo.ConvertTo<OrdersTruckTransportViewModel>();
                        var extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
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
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        OrderTypeModel = orderInfo;
                        break;

                    default:
                        throw new NotImplementedException();
                }

            }
            //OrderTypeModel.RoutePointsInfo = context.getRoutePoints((long)OrderTypeModel.RouteId).ToList();

            OrderTypeModel.CurrentOrderStatus = 1;
            OrderTypeModel.CreatedByUser = userId;
            OrderTypeModel.CreateDatetime = DateTime.Now;
            OrderTypeModel.TotalCost = "0,00";
            OrderTypeModel.TimeRoute = "00:00";
            OrderTypeModel.TimeSpecialVehicles = "00:00";
            OrderTypeModel.ExecuterNotes = "";
            OrderTypeModel.TotalDistanceDescription = "";

            OrderTypeModel.OrderExecuter = OrderTypeFullInfo.DefaultExecuterId ?? "0";
            OrderTypeModel.OrderExecuterName = OrderTypeFullInfo.DefaultExecuterName;

            OrderTypeModel.RouteId = 0; 
            OrderTypeModel.TotalDistanceLenght = "0,00";
                       

            var userfullInfo = context.getUser(this.userId);

            if (userfullInfo != null)
            {
                OrderTypeModel.CreatorContact = userfullInfo?.contactPhone;
                OrderTypeModel.CreatorPosition = string.Concat(userfullInfo?.displayName, " ", userfullInfo?.postName);
            }

            context.NewOrder(OrderTypeModel);

            if (orderInfo.IsTransport)
                return RedirectToAction("Orders", "Orders");


            return RedirectToAction("OrdersBase", "Orders");

        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateOrder(long Id)
        {
            OrderBaseViewModel OrderTypeModel = null;
            //Session.Clear();
            Session["RoutePointLoadList"] = null;
            Session["RoutePointUnLoadList"] = null;
            Session["IsDbDataTakenLoad"] = null;
            Session["IsDbDataTakenUnLoad"] = null;
            Session["pointListDelete"] = null;
            Session["pointListUpdate"] = null;

            var DefaultCounty = context.getDefaultCountry();
            var orderInfo = context.getOrder(Id);

            orderInfo.OpenedByCreator = orderInfo.CreatedByUser == this.userId;
            orderInfo.OpenedByLPR = context.IsUserOrderLPRPerson(this.userId, orderInfo.OrderType);

            if (orderInfo != null)
            {
                switch (orderInfo.OrderType)
                {
                    case 1:
                    case 3:
                    case 6:
                        OrderTypeModel = orderInfo.ConvertTo<OrdersPassTransportViewModel>();
                        OrderTypeModel.GoogleMapApiKey = ConfigurationManager.AppSettings["GoogleMapApiKey"];
                        OrderTypeModel.IsLatLngAbsent = false;
                        var extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
                        context.getPassTrasportOrderData(ref extOrderTypeModel1);

                        if ((((OrdersPassTransportViewModel)OrderTypeModel).CountryFrom == 0) || (((OrdersPassTransportViewModel)OrderTypeModel).TripType < 2))
                        {
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFrom = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFromName = DefaultCounty.CountryName;
                        }
                        if ((((OrdersPassTransportViewModel)OrderTypeModel).CountryTo == 0) || (((OrdersPassTransportViewModel)OrderTypeModel).TripType < 2))
                        {
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryTo = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryToName = DefaultCounty.CountryName;
                        }

                        ((OrdersPassTransportViewModel)OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                        ((OrdersPassTransportViewModel)OrderTypeModel).DefaultCountryName = DefaultCounty.CountryName;
                        OrderTypeModel.CartUrl = string.Concat("https://google.com/maps/dir/",
                                                                ((OrdersPassTransportViewModel)OrderTypeModel).CityFrom, ",+",
                                                                ((OrdersPassTransportViewModel)OrderTypeModel).CountryFromName, "/",
                                                                ((OrdersPassTransportViewModel)OrderTypeModel).CityTo, ",+",
                                                                ((OrdersPassTransportViewModel)OrderTypeModel).CountryToName);

                        IList<OrdersMapPointsViewModel> pointMapListPass = new List<OrdersMapPointsViewModel>();

                        //если координаты не заданы, то пытаемся получить их по адресу
                        if ((((OrdersPassTransportViewModel)OrderTypeModel).LatitudeOrgFrom == 0) &&
                            (((OrdersPassTransportViewModel)OrderTypeModel).LongitudeOrgFrom == 0))
                        {
                            decimal latitude, longitude;
                            string address = string.Concat(((OrdersPassTransportViewModel)OrderTypeModel).CountryFromName, ", ", ((OrdersPassTransportViewModel)OrderTypeModel).CityFrom, ", ", ((OrdersPassTransportViewModel)OrderTypeModel).AdressFrom);
                            context.GetCoordinates(address, out latitude, out longitude);
                            ((OrdersPassTransportViewModel)OrderTypeModel).LatitudeOrgFrom = latitude;
                            ((OrdersPassTransportViewModel)OrderTypeModel).LongitudeOrgFrom = longitude;

                        }

                        if ((((OrdersPassTransportViewModel)OrderTypeModel).LatitudeOrgFrom != 0) || (((OrdersPassTransportViewModel)OrderTypeModel).LongitudeOrgFrom != 0))
                        {
                            pointMapListPass.Add(new OrdersMapPointsViewModel(((OrdersPassTransportViewModel)OrderTypeModel).OrgFrom, ((OrdersPassTransportViewModel)OrderTypeModel).LatitudeOrgFrom, ((OrdersPassTransportViewModel)OrderTypeModel).LongitudeOrgFrom, 1));
                        }
                        else
                        {
                            OrderTypeModel.IsLatLngAbsent = true;
                        }

                        //если координаты не заданы, то пытаемся получить их по адресу
                        if ((((OrdersPassTransportViewModel)OrderTypeModel).LatitudeOrgTo == 0) &&
                            (((OrdersPassTransportViewModel)OrderTypeModel).LongitudeOrgTo == 0))
                        {
                            decimal latitude, longitude;
                            string address = string.Concat(((OrdersPassTransportViewModel)OrderTypeModel).CountryToName, ", ", ((OrdersPassTransportViewModel)OrderTypeModel).CityTo, ", ", ((OrdersPassTransportViewModel)OrderTypeModel).AdressTo);
                            context.GetCoordinates(address, out latitude, out longitude);
                            ((OrdersPassTransportViewModel)OrderTypeModel).LatitudeOrgTo = latitude;
                            ((OrdersPassTransportViewModel)OrderTypeModel).LongitudeOrgTo = longitude;

                        }

                        if ((((OrdersPassTransportViewModel)OrderTypeModel).LatitudeOrgTo != 0) || (((OrdersPassTransportViewModel)OrderTypeModel).LongitudeOrgTo != 0))
                        {
                            pointMapListPass.Add(new OrdersMapPointsViewModel(((OrdersPassTransportViewModel)OrderTypeModel).OrgTo, ((OrdersPassTransportViewModel)OrderTypeModel).LatitudeOrgTo, ((OrdersPassTransportViewModel)OrderTypeModel).LongitudeOrgTo, 2));
                        }
                        else
                        {
                            OrderTypeModel.IsLatLngAbsent = true;
                        }

                        OrderTypeModel.MapPoints = pointMapListPass;
                        break;

                    case 4:
                    case 5:
                    case 7:
                        OrderTypeModel = orderInfo.ConvertTo<OrdersTruckTransportViewModel>();
                        OrderTypeModel.GoogleMapApiKey = ConfigurationManager.AppSettings["GoogleMapApiKey"];
                        var extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
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
                        OrderTypeModel.CartUrl = string.Concat("https://google.com/maps/dir/",
                                                               ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCity, ",+",
                                                               ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryName, "/",
                                                               ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCity, ",+",
                                                               ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryName);

                        var pointListLoads = context.getLoadPoints(OrderTypeModel.Id, true).ToList();
                        var pointListUnLoads = context.getLoadPoints(OrderTypeModel.Id, false).ToList();
                        var pointLoads = context.getRoutePoints((long)OrderTypeModel.RouteId).ToList();
                        var numbPoint = 1;

                        IList<OrdersMapPointsViewModel> pointMapListTruck = new List<OrdersMapPointsViewModel>();
                        //если координаты не заданы, то пытаемся получить их по адресу
                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).LatitudeShipper == 0) &&
                            (((OrdersTruckTransportViewModel)OrderTypeModel).LongitudeShipper == 0))
                        {
                            decimal latitude, longitude;
                            string address = string.Concat(((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryName, ", ",
                                ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCity, ", ", ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperAdress);
                            context.GetCoordinates(address, out latitude, out longitude);
                            ((OrdersTruckTransportViewModel)OrderTypeModel).LatitudeShipper = latitude;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).LongitudeShipper = longitude;
                        }

                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).LatitudeShipper != 0) || (((OrdersTruckTransportViewModel)OrderTypeModel).LongitudeShipper != 0))
                        {
                            pointMapListTruck.Add(new OrdersMapPointsViewModel(((OrdersTruckTransportViewModel)OrderTypeModel).Shipper, ((OrdersTruckTransportViewModel)OrderTypeModel).LatitudeShipper, ((OrdersTruckTransportViewModel)OrderTypeModel).LongitudeShipper, numbPoint));
                        }
                        else
                        {
                            OrderTypeModel.IsLatLngAbsent = true;
                        }
                        if (pointLoads != null)
                        {
                            foreach (var pointListLoad in pointLoads)
                            {
                                //если координаты не заданы, то пытаемся получить их по адресу
                                if ((pointListLoad.Latitude == 0) &&
                                    (pointListLoad.Longitude == 0))
                                {
                                    decimal latitude, longitude;
                                    string address = string.Concat(pointListLoad.CountryPoint, ", ",
                                        pointListLoad.CityPoint, ", ", pointListLoad.AddressPoint);
                                    context.GetCoordinates(address, out latitude, out longitude);
                                    pointListLoad.Latitude = latitude;
                                    pointListLoad.Longitude = longitude;
                                }


                                if ((pointListLoad.Latitude != 0) || (pointListLoad.Longitude != 0))
                                {
                                    numbPoint = numbPoint + 1;
                                    pointMapListTruck.Add(new OrdersMapPointsViewModel(pointListLoad.NamePoint, pointListLoad.Latitude, pointListLoad.Longitude, numbPoint));
                                }
                                else
                                {
                                    OrderTypeModel.IsLatLngAbsent = true;
                                }
                            }
                        }
                        numbPoint = numbPoint + 1;

                        //если координаты не заданы, то пытаемся получить их по адресу
                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).LatitudeConsignee == 0) &&
                            (((OrdersTruckTransportViewModel)OrderTypeModel).LongitudeConsignee == 0))
                        {
                            decimal latitude, longitude;
                            string address = string.Concat(((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryName, ", ",
                                ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCity, ", ", ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeAdress);
                            context.GetCoordinates(address, out latitude, out longitude);
                            ((OrdersTruckTransportViewModel)OrderTypeModel).LatitudeConsignee = latitude;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).LongitudeConsignee = longitude;
                        }

                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).LatitudeConsignee != 0) || (((OrdersTruckTransportViewModel)OrderTypeModel).LongitudeConsignee != 0))
                        {
                            pointMapListTruck.Add(new OrdersMapPointsViewModel(((OrdersTruckTransportViewModel)OrderTypeModel).Consignee, ((OrdersTruckTransportViewModel)OrderTypeModel).LatitudeConsignee, ((OrdersTruckTransportViewModel)OrderTypeModel).LongitudeConsignee, numbPoint));
                        }
                        else
                        {
                            OrderTypeModel.IsLatLngAbsent = true;
                        }

                        OrderTypeModel.MapPoints = pointMapListTruck;

                        break;
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        OrderTypeModel = orderInfo;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                var OrderTypeFullInfo = context.getOrderType(orderInfo.OrderType);
                var OrderStatusFullInfo = context.getStatus(OrderTypeModel.CurrentOrderStatus);

                if (OrderTypeModel.OrderExecuter == string.Empty)
                {
                    OrderTypeModel.OrderExecuter = OrderTypeFullInfo.DefaultExecuterId;
                    OrderTypeModel.OrderExecuterName = OrderTypeFullInfo.DefaultExecuterName;
                }
                OrderTypeModel.AllowData = isAdmin || (context.UserHasRole(userId, OrderTypeFullInfo.UserRoleIdForCompetitiveList));
                OrderTypeModel.AllowClientData = isAdmin ? true : (context.UserHasRole(userId, OrderTypeFullInfo.UserRoleIdForClientData) && OrderStatusFullInfo.AllowClientData) && (orderInfo.OpenedByCreator || orderInfo.OpenedByLPR);
                OrderTypeModel.AllowExecuterData = isAdmin ? true : (context.UserHasRole(userId, OrderTypeFullInfo.UserRoleIdForExecuterData) && OrderStatusFullInfo.AllowExecuterData);

                OrderTypeModel.AvaliableExecuters = context.GetOrderExecutorsEx(orderInfo.OrderType);
                OrderTypeModel.nextAvialiableStatuses = context.getAvailableOrderStatusesInPipeline(userId, isAdmin, orderInfo.CurrentOrderStatus, orderInfo.OrderType).ToList();
                OrderTypeModel.previousStatuses = context.getPreviousOrderStatusesInPipeline(userId, isAdmin, orderInfo.CurrentOrderStatus, orderInfo.OrderType, Id).ToList();

                OrderTypeModel.observers = context.getOrderObservers(Id).ToList();
                OrderTypeModel.attachments = context.getOrderAttachments(userId, Id).ToList();
                OrderTypeModel.SpecTypeInfo = context.getOrderSpecification(Id).ToList();

                if (OrderTypeModel.RouteId != 0)
                {
                    OrderTypeModel.RouteInfo = context.getRoute((long)OrderTypeModel.RouteId);
                }
                if (OrderTypeModel.RouteInfo != null)
                {
                    OrderTypeModel.RouteInfoStr = OrderTypeModel.RouteInfo.OrgFromCountry + ", " + OrderTypeModel.RouteInfo.OrgFromCity + "/ " + OrderTypeModel.RouteInfo.OrgToCountry + ", " + OrderTypeModel.RouteInfo.OrgToCity + " расстояние " + OrderTypeModel.RouteInfo.RouteDistance + ", время " + OrderTypeModel.RouteInfo.RouteTime;
                }
                else
                {
                    OrderTypeModel.RouteInfoStr = "Необходимо привязать маршрут";
                }
                OrderTypeModel.RoutePointsLoadInfo = context.getLoadPoints(Id, true).ToList();
                OrderTypeModel.RoutePointsInfo = context.getRoutePoints((long)OrderTypeModel.RouteId).ToList();
                OrderTypeModel.RoutePointsUnloadInfo = context.getLoadPoints(Id, false).ToList();
                //getRoutePoints(long RoutePointId)

                var ProjectList = context.getOrderProjects(Id).ToList();
                OrderTypeModel.ProjectsCnt = ProjectList.Count();

                if (ProjectList.Count() > 0)
                {
                     string ProjectNum = "";
                     string MultiProjectId = "";

                        foreach (var i in ProjectList)
                        {
                            if (ProjectNum.Length > 0)
                            {
                                ProjectNum += ",";
                            }
                            if (MultiProjectId.Length > 0)
                            {
                                MultiProjectId += ",";
                            }

                            if (i.ProjectId > 0)
                            {
                                ProjectNum = string.Concat(ProjectNum, context.GetProjectById(i.ProjectId).Code);
                                MultiProjectId = string.Concat(MultiProjectId, i.ProjectId.ToString());

                            }
                            else
                            {
                                break;
                            }
                        }
                        OrderTypeModel.ProjectNum = ProjectNum;
                        OrderTypeModel.MultiProjectId = MultiProjectId;
                    }
            }

            return View(OrderTypeModel);
        }

        [HttpPost]
        public ActionResult SendNotification(string ReceiverId, long OrderId, string MessageText)
        {


            OrderBaseViewModel OrderTypeModel = null;
            var orderInfo = context.getOrder(OrderId);
            OrderBaseViewModel extOrderTypeModel = null;
            string emailTemplatePath = string.Empty;

            if (orderInfo != null)
            {

                //Поднимаем специфичную информацию о завяке по ее типу
                switch (orderInfo.OrderType)
                {
                    case 1:
                    case 3:
                    case 6:
                        emailTemplatePath = Server.MapPath($"/Templates/RequestForApprovePass.html");
                        OrderTypeModel = orderInfo.ConvertTo<OrdersPassTransportViewModel>();
                        var extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
                        context.getPassTrasportOrderData(ref extOrderTypeModel1);
                        extOrderTypeModel = extOrderTypeModel1;
                        break;
                    case 4:
                    case 5:
                    case 7:
                        emailTemplatePath = Server.MapPath($"/Templates/RequestForApproveTruck.html");
                        OrderTypeModel = orderInfo.ConvertTo<OrdersTruckTransportViewModel>();
                        var extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
                        context.getTruckTrasportOrderData(ref extOrderTypeModel2);
                        extOrderTypeModel = extOrderTypeModel2;
                        break;

                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        emailTemplatePath = Server.MapPath($"/Templates/RequestForApproveBase.html");
                        OrderTypeModel = orderInfo;
                        extOrderTypeModel = orderInfo;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }


            var service = new CorumEmailService();
            var RecieverFullInfo = context.getUser(ReceiverId);

            if ((service != null) && (RecieverFullInfo != null) && (orderInfo != null))
            {
                var message = new OrderNotificationsMessage();

                if (System.IO.File.Exists(emailTemplatePath))
                {

                    using (StreamReader reader = new StreamReader(emailTemplatePath))
                    {
                        message.Body = reader.ReadToEnd();
                        message.Subject = "Запрос на согласование заявки";
                    }

                    var receivers = new List<UserViewModel>();
                    receivers.Add(RecieverFullInfo);

                    Dictionary<string, string> EmailParams = null;

                    string Cars = string.Empty;

                    var c = context.getOrderCarsInfo(orderInfo.Id);
                    if (c != null)
                        Cars = string.Join(";<br>", c.Select(x =>
                        string.Concat("Экспедитор: ", x.ExpeditorName, " ",
                                      "(", x.CarrierInfo, ") ",
                                      "договор: ", x.ContractInfo, " ",
                                      "Автомобиль - ", x.CarModelInfo, " ",
                                      "гос номер: ", x.CarRegNum,
                                      "водитель: ", x.CarDriverInfo, "/", x.DriverContactInfo,
                                      "(", x.DriverCardInfo, ") "
                                      )).ToArray());

                    switch (orderInfo.OrderType)
                    {
                        case 1:
                        case 3:
                        case 6:
                            EmailParams = new Dictionary<string, string>()
                           {
                           {"{{OrderId}}",           orderInfo.Id.ToString()},
                           {"{{MessageText}}",       MessageText},
                           {"{{OrderTypeName}}",     orderInfo.OrderTypename},
                           {"{{CurrentStatusName}}", string.Concat(orderInfo.CurrentOrderStatusName, " присвоен:",orderInfo.OrderDate)},
                           {"{{PriorityStatus}}",    (orderInfo.PriorityType==0)? "Плановая":"Срочная"},
                           {"{{PointFrom}}",         string.Concat(((OrdersPassTransportViewModel)extOrderTypeModel).CountryFromName,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).CityFrom,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).AdressFrom,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).OrgFrom)
                           },
                           {"{{PointTo}}",           string.Concat(((OrdersPassTransportViewModel)extOrderTypeModel).CountryToName,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).CityTo,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).AdressTo,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).OrgTo)},
                           {"{{PassList}}",          ((OrdersPassTransportViewModel)extOrderTypeModel).PassInfo},
                           {"{{CFO}}",               orderInfo.ClientCenterName},
                           {"{{ClientName}}",        orderInfo.ClientName  },
                           {"{{Payer}}",             orderInfo.PayerName },
                           {"{{Creator}}",           orderInfo.CreatedByUserName },
                           {"{{Executer}}",          orderInfo.OrderExecuterName },
                           {"{{Cars}}",              Cars },
                           #if DEBUG
                               { "{{PreviewLink}}",       $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #else
                               { "{{PreviewLink}}",       $"https://corumsource.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #endif
                           
                          };
                            break;
                        case 4:
                        case 5:
                        case 7:
                            EmailParams = new Dictionary<string, string>()
                           {
                           {"{{OrderId}}",           orderInfo.Id.ToString()},
                           {"{{MessageText}}",       MessageText},
                           {"{{OrderTypeName}}",     orderInfo.OrderTypename},
                           {"{{ContactPerson}}",     string.Concat(orderInfo.CreatorPosition," ",orderInfo.CreatorContact) },

                           {"{{CurrentStatusName}}", string.Concat(orderInfo.CurrentOrderStatusName, " присвоен:",orderInfo.OrderDate)},
                           {"{{PriorityStatus}}",    (orderInfo.PriorityType==0)? "Плановая":"Срочная"},
                           {"{{PointFrom}}",         string.Concat(((OrdersTruckTransportViewModel)extOrderTypeModel).ShipperCountryName,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).ShipperCity,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).ShipperAdress,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).Shipper)
                           },
                           {"{{PointTo}}",           string.Concat(((OrdersTruckTransportViewModel)extOrderTypeModel).ConsigneeCountryName,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).ConsigneeCity,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).ConsigneeAdress,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).Consignee)},
                           {"{{PassList}}",          ((OrdersTruckTransportViewModel)extOrderTypeModel).TruckDescription},
                           {"{{CFO}}",               orderInfo.ClientCenterName},
                           {"{{ClientName}}",        orderInfo.ClientName  },
                           {"{{Payer}}",             orderInfo.PayerName },
                           {"{{Creator}}",           orderInfo.CreatedByUserName },
                           {"{{Executer}}",          orderInfo.OrderExecuterName },
                           {"{{Cars}}",              Cars },
                           #if DEBUG
                               { "{{PreviewLink}}",       $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #else
                               { "{{PreviewLink}}",       $"https://corumsource.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #endif
                           
                          };
                            break;

                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                            EmailParams = new Dictionary<string, string>()
                           {
                           {"{{OrderId}}",           orderInfo.Id.ToString()},
                           {"{{MessageText}}",       MessageText},
                           {"{{OrderTypeName}}",     orderInfo.OrderTypename},
                           {"{{ContactPerson}}",     string.Concat(orderInfo.CreatorPosition," ",orderInfo.CreatorContact) },

                           {"{{CurrentStatusName}}", string.Concat(orderInfo.CurrentOrderStatusName, " присвоен:",orderInfo.OrderDate)},
                           {"{{PriorityStatus}}",    (orderInfo.PriorityType==0)? "Плановая":"Срочная"},
                           {"{{PassList}}",          orderInfo.OrderDescription},
                           {"{{CFO}}",               orderInfo.ClientCenterName},
                           {"{{ClientName}}",        orderInfo.ClientName  },
                           {"{{Payer}}",             orderInfo.PayerName },
                           {"{{Creator}}",           orderInfo.CreatedByUserName },
                           {"{{Executer}}",          orderInfo.OrderExecuterName },
                           {"{{Cars}}",              Cars },
                           #if DEBUG
                               { "{{PreviewLink}}",       $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #else
                               { "{{PreviewLink}}",       $"https://corumsource.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #endif
                           
                          };
                            break;

                        default:
                            throw new NotImplementedException();
                    }





                    foreach (KeyValuePair<string, string> pair in EmailParams)
                    {
                        message.Body = message.Body.Replace(pair.Key, pair.Value);
                    }

                    service.SendRequestToEmailAsync(message, receivers);

                    context.SaveNotificationHistory(new OrderNotificationViewModel()
                    {
                        CreatedBy = this.userId,
                        Body = message.Body,
                        OrderId = OrderTypeModel.Id,
                        Receiver = ReceiverId
                    });
                }
            }


            return new JsonpResult
            {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        [HttpPost]
        public ActionResult UpdateOrder(OrderBaseViewModel model)
        {

            if (!ModelState.IsValid)
            {
                var OrderTypeFullInfo = context.getAvailableOrderTypes(null, null).FirstOrDefault(t => t.Id == model.OrderType);
                var OrderStatusFullInfo = context.getStatus(model.CurrentOrderStatus);

                model.AllowClientData = isAdmin ? true : (context.UserHasRole(userId, OrderTypeFullInfo.UserRoleIdForClientData) && OrderStatusFullInfo.AllowClientData);
                model.AllowExecuterData = isAdmin ? true : (context.UserHasRole(userId, OrderTypeFullInfo.UserRoleIdForExecuterData) && OrderStatusFullInfo.AllowExecuterData);

                model.AvaliableExecuters = context.getUsers(string.Empty).ToList();

                model.CreatedByUser = userId;
                model.CreatedByUserName = context.getUser(userId).displayName;

                model.nextAvialiableStatuses = context.getAvailableOrderStatusesInPipeline(userId, isAdmin, model.CurrentOrderStatus, model.OrderType).ToList();

                return View(model);
            }

            model.CreatedByUser = userId;
            model.CreateDatetime = System.DateTime.Now;

            if (context.UpdateOrder(model))
            {
                var pointListLoads = Session["RoutePointLoadList"] as List<OrderAdditionalRoutePointModel>;
                var pointListUnLoads = Session["RoutePointUnLoadList"] as List<OrderAdditionalRoutePointModel>;
                var pointListDeletes = Session["pointListDelete"] as List<OrderAdditionalRoutePointModel>;
                var pointListUpdates = Session["pointListUpdate"] as List<OrderAdditionalRoutePointModel>;
                if (pointListLoads != null)
                {
                    foreach (var pointListLoad in pointListLoads)
                    {
                        if (pointListLoad.IsSaved == false)
                        {
                            context.NewRoutePoint(pointListLoad);
                        }
                    }
                }
                if (pointListUnLoads != null)
                {
                    foreach (var pointListUnLoad in pointListUnLoads)
                    {
                        if (pointListUnLoad.IsSaved == false)
                        {
                            context.NewRoutePoint(pointListUnLoad);
                        }
                    }
                }
                if (pointListDeletes != null)
                {
                    foreach (var pointListDelete in pointListDeletes)
                    {
                        context.DeleteRoutePoint(pointListDelete.Id);
                    }
                }

                if (pointListUpdates != null)
                {
                    foreach (var pointListUpdate in pointListUpdates)
                    {
                        context.UpdateRoutePoint(pointListUpdate);
                    }
                }

                var orderInfo = context.getOrder(model.Id);
                OrderBaseViewModel OrderTypeModel = null;
                OrderBaseViewModel extOrderTypeModel = null;
                string emailTemplatePath = string.Empty;
                Dictionary<string, string> EmailParams = null;

                if (model.UseNotifications)
                {
                    var DefaultCounty = context.getDefaultCountry();

                    switch (orderInfo.OrderType)
                    {
                        case 1:
                        case 3:
                        case 6:
                            emailTemplatePath = Server.MapPath($"/Templates/RequestForApprovePass.html");
                            OrderTypeModel = orderInfo.ConvertTo<OrdersPassTransportViewModel>();
                            var extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
                            context.getPassTrasportOrderData(ref extOrderTypeModel1);
                            extOrderTypeModel = extOrderTypeModel1;
                            break;
                        case 4:
                        case 5:
                        case 7:
                            emailTemplatePath = Server.MapPath($"/Templates/RequestForApproveTruck.html");
                            OrderTypeModel = orderInfo.ConvertTo<OrdersTruckTransportViewModel>();
                            var extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
                            context.getTruckTrasportOrderData(ref extOrderTypeModel2);
                            extOrderTypeModel = extOrderTypeModel2;
                            break;

                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                            emailTemplatePath = Server.MapPath($"/Templates/RequestForApproveBase.html");
                            OrderTypeModel = orderInfo;
                            extOrderTypeModel = orderInfo;
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }



                var service = new CorumEmailService();
                var receivers = new List<OrderObserverViewModel>();


                if ((service != null) && (orderInfo != null))
                {
                    var message = new OrderNotificationsMessage();
                    var MessageText = "Статус заявки успешно изменен!!!";

                    if (System.IO.File.Exists(emailTemplatePath))
                    {

                        using (StreamReader reader = new StreamReader(emailTemplatePath))
                        {
                            message.Body = reader.ReadToEnd();
                            message.Subject = "Cтатус заявки " + orderInfo.Id.ToString() + " изменен на " + orderInfo.CurrentOrderStatusName;
                        }


                        var emailReciversRaw = context.getOrderObservers(model.Id).ToList();

                        if (model.ObserversForNotification != null)
                        {
                            var observersForNotification = model.ObserversForNotification.Split(',').ToList();
                            var emailRecivers = emailReciversRaw.Where(x => observersForNotification.Contains(x.observerId)).ToList();
                            receivers.AddRange(emailRecivers);
                        }


                        string Cars = string.Empty;

                        var c = context.getOrderCarsInfo(orderInfo.Id);
                        if (c != null)
                            Cars = string.Join(";<br>", c.Select(x =>
                            string.Concat("Экспедитор: ", x.ExpeditorName, " ",
                                          "(", x.CarrierInfo, ") ",
                                          "договор: ", x.ContractInfo, " ",
                                          "Автомобиль - ", x.CarModelInfo, " ",
                                          "гос номер: ", x.CarRegNum,
                                          "водитель: ", x.CarDriverInfo, "/", x.DriverContactInfo,
                                          "(", x.DriverCardInfo, ") "
                                          )).ToArray());

                        switch (orderInfo.OrderType)
                        {
                            case 1:
                            case 3:
                            case 6:
                                EmailParams = new Dictionary<string, string>()
                           {
                           {"{{OrderId}}",           orderInfo.Id.ToString()},
                           {"{{MessageText}}",       MessageText},
                           {"{{OrderTypeName}}",     orderInfo.OrderTypename},
                           {"{{CurrentStatusName}}", string.Concat(orderInfo.CurrentOrderStatusName, " присвоен:",orderInfo.OrderDate)},
                           {"{{PriorityStatus}}",    (orderInfo.PriorityType==0)? "Плановая":"Срочная"},
                           {"{{PointFrom}}",         string.Concat(((OrdersPassTransportViewModel)extOrderTypeModel).CountryFromName,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).CityFrom,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).AdressFrom,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).OrgFrom)
                           },
                           {"{{PointTo}}",           string.Concat(((OrdersPassTransportViewModel)extOrderTypeModel).CountryToName,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).CityTo,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).AdressTo,
                                                     " ",
                                                     ((OrdersPassTransportViewModel)extOrderTypeModel).OrgTo)},
                           {"{{PassList}}",          ((OrdersPassTransportViewModel)extOrderTypeModel).PassInfo},
                           {"{{CFO}}",               orderInfo.ClientCenterName},
                           {"{{ClientName}}",        orderInfo.ClientName  },
                           {"{{Payer}}",             orderInfo.PayerName },
                           {"{{Creator}}",           orderInfo.CreatedByUserName },
                           {"{{Executer}}",          orderInfo.OrderExecuterName },
                           {"{{Cars}}",              Cars },
                           #if DEBUG
                               { "{{PreviewLink}}",       $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #else
                               { "{{PreviewLink}}",       $"https://corumsource.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #endif
                           
                          };
                                break;

                            case 4:
                            case 5:
                            case 7:
                                EmailParams = new Dictionary<string, string>()
                           {
                           {"{{OrderId}}",           orderInfo.Id.ToString()},
                           {"{{MessageText}}",       MessageText},
                           {"{{OrderTypeName}}",     orderInfo.OrderTypename},
                           {"{{ContactPerson}}",     string.Concat(orderInfo.CreatorPosition," ",orderInfo.CreatorContact) },

                           {"{{CurrentStatusName}}", string.Concat(orderInfo.CurrentOrderStatusName, " присвоен:",orderInfo.OrderDate)},
                           {"{{PriorityStatus}}",    (orderInfo.PriorityType==0)? "Плановая":"Срочная"},
                           {"{{PointFrom}}",         string.Concat(((OrdersTruckTransportViewModel)extOrderTypeModel).ShipperCountryName,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).ShipperCity,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).ShipperAdress,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).Shipper)
                           },
                           {"{{PointTo}}",           string.Concat(((OrdersTruckTransportViewModel)extOrderTypeModel).ConsigneeCountryName,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).ConsigneeCity,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).ConsigneeAdress,
                                                     " ",
                                                     ((OrdersTruckTransportViewModel)extOrderTypeModel).Consignee)},
                           {"{{PassList}}",          ((OrdersTruckTransportViewModel)extOrderTypeModel).TruckDescription},
                           {"{{CFO}}",               orderInfo.ClientCenterName},
                           {"{{ClientName}}",        orderInfo.ClientName  },
                           {"{{Payer}}",             orderInfo.PayerName },
                           {"{{Creator}}",           orderInfo.CreatedByUserName },
                           {"{{Executer}}",          orderInfo.OrderExecuterName },
                           {"{{Cars}}",              Cars },
                           #if DEBUG
                               { "{{PreviewLink}}",       $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #else
                               { "{{PreviewLink}}",       $"https://corumsource.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #endif
                           
                          };
                                break;

                            case 8:
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                                EmailParams = new Dictionary<string, string>()
                           {
                           {"{{OrderId}}",           orderInfo.Id.ToString()},
                           {"{{MessageText}}",       MessageText},
                           {"{{OrderTypeName}}",     orderInfo.OrderTypename},
                           {"{{ContactPerson}}",     string.Concat(orderInfo.CreatorPosition," ",orderInfo.CreatorContact) },

                           {"{{CurrentStatusName}}", string.Concat(orderInfo.CurrentOrderStatusName, " присвоен:",orderInfo.OrderDate)},
                           {"{{PriorityStatus}}",    (orderInfo.PriorityType==0)? "Плановая":"Срочная"},
                           {"{{PassList}}",          orderInfo.OrderDescription},
                           {"{{CFO}}",               orderInfo.ClientCenterName},
                           {"{{ClientName}}",        orderInfo.ClientName  },
                           {"{{Payer}}",             orderInfo.PayerName },
                           {"{{Creator}}",           orderInfo.CreatedByUserName },
                           {"{{Executer}}",          orderInfo.OrderExecuterName },
                           {"{{Cars}}",              Cars },
                           #if DEBUG
                               { "{{PreviewLink}}",       $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #else
                               { "{{PreviewLink}}",       $"https://corumsource.com/Orders/UpdateOrder/{orderInfo.Id}" },
                           #endif
                           
                          };
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    }

                    foreach (KeyValuePair<string, string> pair in EmailParams)
                    {
                        message.Body = message.Body.Replace(pair.Key, pair.Value);
                    }

                    service.SendRequestToEmailAsync(message, receivers);

                }
            }




            AddToastMessage("Инфо", "Информация о заявке успешно сохранена!", toastType: ToastType.Success);
            return RedirectToAction("UpdateOrder", "Orders", new { Id = model.Id });
        }

        [HttpGet]
        public ActionResult NewStatus()
        {
            return View(new OrderStatusViewModel()
            {
                IconFile = "Icon1"
            });
        }

        [HttpPost]
        public ActionResult NewStatus(OrderStatusViewModel model)
        {
            context.AddStatus(model);
            return RedirectToAction("Statuses", "Orders");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateStatus(int Id)
        {
            var statusInfo = context.getStatus(Id);
            return View(statusInfo);
        }

        [HttpPost]
        public ActionResult UpdateStatus(OrderStatusViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            context.UpdateStatus(model);
            return RedirectToAction("Statuses", "Orders");
        }

        [HttpGet]
        public ActionResult RemoveStatus(int Id)
        {
            context.RemoveStatus(Id);
            return RedirectToAction("Statuses", "Orders");
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetStatuses(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetStatuses(searchTerm, pageSize, pageNum, true);
            var storagesCount = context.GetStatusesCount(searchTerm, true);

            var pagedAttendees = StatusesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetStatusesBase(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetStatuses(searchTerm, pageSize, pageNum, false);
            var storagesCount = context.GetStatusesCount(searchTerm, false);

            var pagedAttendees = StatusesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        public ActionResult GetPayers(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.Payers(searchTerm, pageSize, pageNum, this.userId);
            var storagesCount = context.PayersCount(searchTerm, this.userId);

            var pagedAttendees = OrderPayers2VmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        public ActionResult GetProjects(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetProjects(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetProjectsCount(searchTerm);

            var pagedAttendees = OrderPayers2VmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        public ActionResult GetClients(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.Clients(searchTerm, pageSize, pageNum, this.userId);
            var storagesCount = context.ClientsCount(searchTerm, this.userId);

            var pagedAttendees = OrderClients2VmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetReceivers(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetReceivers(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetReceiverCount(searchTerm, Id);

            var pagedAttendees = OrderCreatorsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrderCreators(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderCreators(searchTerm, pageSize, pageNum, true);
            var storagesCount = context.GetOrderCreatorsCount(searchTerm, true);

            var pagedAttendees = OrderCreatorsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrderCreatorsBase(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderCreators(searchTerm, pageSize, pageNum, false);
            var storagesCount = context.GetOrderCreatorsCount(searchTerm, false);

            var pagedAttendees = OrderCreatorsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrderTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderTypes(searchTerm, pageSize, pageNum, true);
            var storagesCount = context.GetOrderTypesCount(searchTerm, true);

            var pagedAttendees = OrderTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrderTypesBase(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderTypes(searchTerm, pageSize, pageNum, false);
            var storagesCount = context.GetOrderTypesCount(searchTerm, false);

            var pagedAttendees = OrderTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetCountries(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderCountries(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrderCountriesCount(searchTerm);

            var pagedAttendees = OrderCountriesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetOrderClients(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderClients(searchTerm, pageSize, pageNum, true);
            var storagesCount = context.GetOrderClientsCount(searchTerm, true);

            var pagedAttendees = OrderClientsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetOrderClientsBase(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderClients(searchTerm, pageSize, pageNum, false);
            var storagesCount = context.GetOrderClientsCount(searchTerm, false);

            var pagedAttendees = OrderClientsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetOrderExecuters(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderExecutors(searchTerm, pageSize, pageNum, true);
            var storagesCount = context.GetOrderExecutorsCount(searchTerm, true);

            var pagedAttendees = OrderExecutorsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetOrderExecutersBase(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderExecutors(searchTerm, pageSize, pageNum, false);
            var storagesCount = context.GetOrderExecutorsCount(searchTerm, false);

            var pagedAttendees = OrderExecutorsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrganizations()
        {
            var orgList = context.GetAllOrganizations(null, 10, 1);
            return Json(orgList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrganizationsList(string searchTerm, int pageSize, int pageNum)
        {
            var userId = this.userId;
            var storages = context.GetOrganizations(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrganizationsCount(searchTerm);

            var pagedAttendees = OrderOrgVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult OrderOrgVmToSelect2Format(IEnumerable<OrganizationViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.Name
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetUsedCars(long OrderId)
        {
            var carList = context.getOrderCarsInfo(OrderId);
            return Json(carList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetUsedCarInfo(int Id)
        {
            var carList = context.getUsedCarInfo(Id);
            return Json(carList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NewUsedCar(OrderUsedCarViewModel model)
        {
            if (!ModelState.IsValid) new HttpException((int)HttpStatusCode.BadRequest, "Bad car model", new Exception());
            var newId = context.NewUsedCar(model);
            return Json(new { Id = newId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateUsedCar(OrderUsedCarViewModel model)
        {
            if (!ModelState.IsValid) new HttpException((int)HttpStatusCode.BadRequest, "Bad car model", new Exception());
            var result = context.UpdateUsedCar(model);
            return Json(new { UpdateResult = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateUsedCarAddInfo(OrderUsedCarViewModel model)
        {
            var result = context.UpdateUsedCarAddInfo(model);
            return Json(new { UpdateResult = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveUsedCar(long Id)
        {
            var result = context.DeleteUsedCar(Id);
            return Json(new { DeleteResult = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetTruckTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.TruckTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.TruckTypesCount(searchTerm);

            var pagedAttendees = TruckTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetVehicleTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.VehicleTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.VehicleTypesCount(searchTerm);

            var pagedAttendees = VehicleTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetLoadingTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.LoadingTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.LoadingTypesCount(searchTerm);

            var pagedAttendees = LoadingTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetUnloadingTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.UnloadingTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.UnloadingTypesCount(searchTerm);

            var pagedAttendees = UnloadingTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult UnloadingTypesVmToSelect2Format(IEnumerable<UnloadingTypeViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.UnloadingTypeName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


        private static Select2PagedResult LoadingTypesVmToSelect2Format(IEnumerable<LoadingTypeViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.LoadingTypeName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


        private static Select2PagedResult VehicleTypesVmToSelect2Format(IEnumerable<VehicleViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.VehicleTypeName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        private static Select2PagedResult TruckTypesVmToSelect2Format(IEnumerable<TruckTypeViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.TruckTypeName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


        private static Select2PagedResult StatusesVmToSelect2Format(IEnumerable<OrderStatusViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.StatusName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        private static Select2PagedResult OrderCreatorsVmToSelect2Format(IEnumerable<UserViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.userId,
                    text = groupItem.displayName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        private static Select2PagedResult OrderTypesVmToSelect2Format(IEnumerable<OrderTypeViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.TypeName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        private static Select2PagedResult OrderClientsVmToSelect2Format(IEnumerable<OrderClientsViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = string.Concat(groupItem.ClientCFOName, "/", groupItem.ClientName)
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


        private static Select2PagedResult OrderClients2VmToSelect2Format(IEnumerable<OrderClientsViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = string.Concat(groupItem.ClientCFOName, "/", groupItem.ClientName)
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        private static Select2PagedResult OrderPayers2VmToSelect2Format(IEnumerable<OrderClientBalanceKeeperViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = string.Concat(groupItem.BalanceKeeperName)
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


        private static Select2PagedResult OrderPayers2VmToSelect2Format(IEnumerable<OrderProjectViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = string.Concat(groupItem.Code)
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        private static Select2PagedResult OrderCountriesVmToSelect2Format(IEnumerable<OrderCountriesViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = string.Concat(groupItem.CountryName)
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        private static Select2PagedResult OrderExecutorsVmToSelect2Format(IEnumerable<UserViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.userId,
                    text = string.Concat(groupItem.displayName)
                });
            }

            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


        private static Select2PagedResult OrderFiltersVmToSelect2Format(IEnumerable<OrderFilterSettingsModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.NameFilter
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpPost]
        public ActionResult NewOrderFilter(OrderFilterSettingsModel model)
        {
            model.UserCurrentId = this.userId;
            context.AddOrderFilter(model);
            if (model != null)
            {
                return Json("Success");
            }
            else
            {
                return Json("An Error Has occoured");
            }
        }

        [HttpGet]
        public ActionResult RemoveOrderFilter(int Id)
        {
            context.RemoveOrderFilter(Id);

            return Json("Success");
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetFilterSettings(string searchTerm, int pageSize, int pageNum)
        {
            var userId = this.userId;
            var storages = context.GetFilterSettings(searchTerm, pageSize, pageNum, userId);
            var storagesCount = context.GetFilterSettingsCount(searchTerm, userId);

            var pagedAttendees = OrderFiltersVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetFilterSettingsBtn(int groupSize, int fromNumb)
        {
            var userId = this.userId;
            var storages = context.GetFilterSettingsBtn(groupSize, fromNumb, userId);
            var storagesCount = context.GetFilterSettingsCount("", userId);
            var pagedAttendees = OrderFiltersVmToSelect2Format(storages, storagesCount);
            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult getOrderFilterSettingById(int Id)
        {
            var OrderFilterSettings = context.getOrderFilterSettingById(Id);

            if ((OrderFilterSettings.UseExecuterFilter) && (!string.IsNullOrEmpty(OrderFilterSettings.ExecuterId)))
            {
                string[] idList = OrderFilterSettings.ExecuterId.Split(new char[] { ',' });
                string FilterOrderExecuterName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderExecuterName.Length > 0)
                    {
                        FilterOrderExecuterName += ",";
                    }

                    FilterOrderExecuterName = string.Concat(FilterOrderExecuterName, context.getUser(i).displayName);
                }

                OrderFilterSettings.FilterOrderExecuterName = FilterOrderExecuterName;
            }
            else OrderFilterSettings.UseExecuterFilter = false;

            if ((OrderFilterSettings.UseStatusFilter) && (OrderFilterSettings.StatusId.Length > 0))
            {

                string[] idList = OrderFilterSettings.StatusId.Split(new char[] { ',' });
                string FilterStatusName = "";

                foreach (string i in idList)
                {
                    if (FilterStatusName.Length > 0)
                    {
                        FilterStatusName += ",";
                    }

                    FilterStatusName = string.Concat(FilterStatusName, context.getStatus(Convert.ToInt32(i))?.StatusName);
                }
            }
            else OrderFilterSettings.UseStatusFilter = false;

            if ((OrderFilterSettings.UseCreatorFilter) && (!string.IsNullOrEmpty(OrderFilterSettings.CreatorId)))
            {
                string[] idList = OrderFilterSettings.CreatorId.Split(new char[] { ',' });
                string FilterOrderCreatorName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderCreatorName.Length > 0)
                    {
                        FilterOrderCreatorName += ",";
                    }

                    FilterOrderCreatorName = string.Concat(FilterOrderCreatorName, context.getUser(i).displayName);
                }

                OrderFilterSettings.FilterOrderCreatorName = FilterOrderCreatorName;

            }
            else OrderFilterSettings.UseCreatorFilter = false;

            if ((OrderFilterSettings.UseTypeFilter) && (OrderFilterSettings.TypeId.Length > 0))
            {
                string[] idList = OrderFilterSettings.TypeId.Split(new char[] { ',' });
                string FilterOrderTypeName = "";
                foreach (string i in idList)
                {
                    if (FilterOrderTypeName.Length > 0)
                    {
                        FilterOrderTypeName += ",";
                    }

                    FilterOrderTypeName = string.Concat(FilterOrderTypeName, context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }

                OrderFilterSettings.FilterOrderTypeName = FilterOrderTypeName;

            }
            else OrderFilterSettings.UseTypeFilter = false;

            if ((OrderFilterSettings.UseClientFilter) && (OrderFilterSettings.ClientId.Length > 0))
            {
                string[] idList = OrderFilterSettings.ClientId.Split(new char[] { ',' });
                string FilterOrderClientName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderClientName.Length > 0)
                    {
                        FilterOrderClientName += ",";
                    }
                    FilterOrderClientName = string.Concat(FilterOrderClientName, context.getClient(Convert.ToInt32(i)).ClientName);
                }
                OrderFilterSettings.FilterOrderClientName = FilterOrderClientName;
            }
            else OrderFilterSettings.UseClientFilter = false;

            if ((OrderFilterSettings.UseOrderPayerFilter) && (OrderFilterSettings.PayerId.Length > 0))
            {

                string[] idList = OrderFilterSettings.PayerId.Split(new char[] { ',' });
                string FilterPayerName = "";

                foreach (string i in idList)
                {
                    if (FilterPayerName.Length > 0)
                    {
                        FilterPayerName += ",";
                    }

                    FilterPayerName = string.Concat(FilterPayerName, context.getPayer(Convert.ToInt32(i))?.BalanceKeeperName);
                }
                OrderFilterSettings.FilterOrderPayerName = FilterPayerName;
            }
            else OrderFilterSettings.UseOrderPayerFilter = false;


            if ((OrderFilterSettings.UseOrderOrgFromFilter) && (OrderFilterSettings.OrgFromId.Length > 0))
            {

                string[] idList = OrderFilterSettings.OrgFromId.Split(new char[] { ',' });
                string FilterOrgFromName = "";

                foreach (string i in idList)
                {
                    if (FilterOrgFromName.Length > 0)
                    {
                        FilterOrgFromName += ",";
                    }
                    var OrgFromName = context.GetOrganization(Convert.ToInt32(i));
                    FilterOrgFromName = string.Concat(FilterOrgFromName, OrgFromName?.Name);
                }
                OrderFilterSettings.FilterOrderOrgFromName = FilterOrgFromName;
            }
            else OrderFilterSettings.UseOrderOrgFromFilter = false;

            if ((OrderFilterSettings.UseOrderOrgToFilter) && (OrderFilterSettings.OrgToId.Length > 0))
            {

                string[] idList = OrderFilterSettings.OrgToId.Split(new char[] { ',' });
                string FilterOrgToName = "";

                foreach (string i in idList)
                {
                    if (FilterOrgToName.Length > 0)
                    {
                        FilterOrgToName += ",";
                    }
                    var OrgToName = context.GetOrganization(Convert.ToInt32(i));
                    FilterOrgToName = string.Concat(FilterOrgToName, OrgToName?.Name);
                }
                OrderFilterSettings.FilterOrderOrgToName = FilterOrgToName;
            }
            else OrderFilterSettings.UseOrderOrgToFilter = false;


            OrderFilterSettings.FilterOrderDateEnd = DateTime.Now.AddDays(OrderFilterSettings.DeltaDateEnd).ToString("dd.MM.yyyy");
            OrderFilterSettings.FilterOrderDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(OrderFilterSettings.DeltaDateEnd));
            OrderFilterSettings.FilterOrderDateBeg = DateTime.Now.AddDays(OrderFilterSettings.DeltaDateBeg).ToString("dd.MM.yyyy");
            OrderFilterSettings.FilterOrderDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(OrderFilterSettings.DeltaDateBeg));

            OrderFilterSettings.FilterOrderExDateBeg = DateTime.Now.AddDays(OrderFilterSettings.DeltaDateBegEx).ToString("dd.MM.yyyy");
            OrderFilterSettings.FilterOrderExDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(OrderFilterSettings.DeltaDateBegEx));
            OrderFilterSettings.FilterOrderExDateEnd = DateTime.Now.AddDays(OrderFilterSettings.DeltaDateEndEx).ToString("dd.MM.yyyy");
            OrderFilterSettings.FilterOrderExDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(OrderFilterSettings.DeltaDateEndEx));

            return Json(OrderFilterSettings, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRoles(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetRoles(searchTerm, pageSize, pageNum, userId);
            var storagesCount = context.GetRolesCount(searchTerm, userId);

            var pagedAttendees = OrderRolesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResultGroup OrderRolesVmToSelect2Format(IEnumerable<AvailableRoles> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResultGroup { Results = new List<Select2ResultGroup>() };

            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2ResultGroup
                {
                    text = groupItem.text,
                    children = groupItem.children
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        public ActionResult GetOrders(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrders(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrderCount(searchTerm);

            var pagedAttendees = OrdersVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult OrdersVmToSelect2Format(IEnumerable<OrderBaseViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.Id.ToString()
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpPost]
        public ActionResult NewDirUsedCar(OrderUsedCarViewModel model)
        {
            if (!ModelState.IsValid) new HttpException((int)HttpStatusCode.BadRequest, "Bad car model", new Exception());
            var newId = context.NewDirUsedCar(model);
            return Json(new
            {
                Id = newId
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetExpeditorName(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetExpeditorName(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetExpeditorNameCount(searchTerm, Id);

            var pagedAttendees = GetExpeditorNameVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetExpeditorNameVmToSelect2Format(IEnumerable<CarOwnersAccessViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.CarrierName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetContractExpCarrierInfo(string searchTerm, int pageSize, int pageNum, int? urlData1, int? urlData2)
        {
            var storages = context.GetContractExpCarrierInfo(searchTerm, pageSize, pageNum, urlData1, urlData2);
            var storagesCount = context.GetContractExpCarrierInfoCount(searchTerm, urlData1, urlData2);

            var pagedAttendees = GetContractInfoVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetContractExpBkInfo(string searchTerm, int pageSize, int pageNum, int urlData1, int urlData2)
        {
            var storages = context.GetContractExpBkInfo(searchTerm, pageSize, pageNum, urlData1, urlData2);
            var storagesCount = context.GetContractExpBkInfoPECount(searchTerm, urlData1, urlData2);

            var pagedAttendees = GetContractInfoVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        private static Select2PagedResult GetContractInfoVmToSelect2Format(IEnumerable<ContractsViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.ContractNumber + " от " + groupItem.ContractDate + " (с " + groupItem.DateBeg + " по " + groupItem.DateEnd + " ) "
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetCarrierInfo(string searchTerm, int pageSize, int pageNum, long? Id, int? urlData)
        {
            var storages = context.GetCarrierInfo(searchTerm, pageSize, pageNum, Id, urlData);
            var storagesCount = context.GetCarrierInfoCount(searchTerm, Id, urlData);

            var pagedAttendees = GetCarrierInfoVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetCarrierInfoVmToSelect2Format(IEnumerable<CarOwnersAccessViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.CarrierName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetCarInfo(string searchTerm, int pageSize, int pageNum, long? Id, int? urlData)
        {
            var storages = context.GetCarInfo(searchTerm, pageSize, pageNum, Id, urlData);
            var storagesCount = context.GetCarInfoCount(searchTerm, Id, urlData);

            var pagedAttendees = GetCarInfoVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetCarInfoVmToSelect2Format(IEnumerable<CarsViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.CarId.ToString(),
                    text = "Марка и номер: " + groupItem.CarModel + " " + groupItem.Number + " Ф.И.О. водителя: " + groupItem.Driver + " Серия и номер прав: " + groupItem.DriverLicenseSeria + " " + groupItem.DriverLicenseNumber
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult BaseReportPass(OrdersNavigationInfo navInfo)
        {
            //обрабатываем данные фильтров
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

            navInfo.UseFinalStatusFilter = true;
            navInfo.FilterFinalStatus = false;
            //отслеживаем страницу
            if (navInfo.PageNumber == 0) { navInfo.PageNumber = 1; }

            string FilterOrderClientNames = "";

            if (!string.IsNullOrEmpty(navInfo.FilterOrderClientId))
            {
                string[] idList = navInfo.FilterOrderClientId.Split(new char[] { ',' });


                foreach (string i in idList)
                {
                    if (FilterOrderClientNames.Length > 0)
                    {
                        FilterOrderClientNames += ",";
                    }
                    FilterOrderClientNames = context.getClient(Convert.ToInt32(i)).ClientName;
                }
            }

            string FilterOrderTypeNames = "";
            if (!string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                string[] idList = navInfo.FilterOrderTypeId.Split(new char[] { ',' });
                foreach (string i in idList)
                {
                    if (FilterOrderTypeNames.Length > 0)
                    {
                        FilterOrderTypeNames += ",";
                    }

                    FilterOrderTypeNames = string.Concat(FilterOrderTypeNames,
                    context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }
            }

            string FilterTripTypeNames = "";
            if (!string.IsNullOrEmpty(navInfo.FilterTripTypeId))
            {
                string[] idList = navInfo.FilterTripTypeId.Split(new char[] { ',' });
                foreach (string i in idList)
                {
                    if (FilterTripTypeNames.Length > 0)
                    {
                        FilterTripTypeNames += ",";
                    }

                    FilterTripTypeNames = string.Concat(FilterTripTypeNames,
                    context.getTripType(Convert.ToInt32(i))?.RouteTypeName);
                }
            }

            var modelBaseReport = new OrdersNavigationResult<BaseReportViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getBaseReport(
                    userId,
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
                    true),

                AvailiableTypes =
                    context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                        .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,
                FilterOrderClientName = FilterOrderClientNames,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                FilterOrderTypeName = FilterOrderTypeNames,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

                FilterTripTypeId = navInfo.FilterTripTypeId,
                FilterTripTypeName = FilterTripTypeNames,
                UseTripTypeFilter = navInfo.UseTripTypeFilter,

            };


            var modelStatusReport = new OrdersNavigationResult<StatusReportViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getStatusReport(
                                             userId,
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
                                             true),

                AvailiableTypes =
                    context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                        .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,
                FilterOrderClientName = FilterOrderClientNames,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                FilterOrderTypeName = FilterOrderTypeNames,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

                FilterTripTypeId = navInfo.FilterTripTypeId,
                FilterTripTypeName = FilterTripTypeNames,
                UseTripTypeFilter = navInfo.UseTripTypeFilter,

            };


            var modelOrdersReport = new OrdersNavigationResult<OrdersReportViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getOrdersReport(
                                             userId,
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
                                             true),

                AvailiableTypes =
                    context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                        .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,
                FilterOrderClientName = FilterOrderClientNames,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                FilterOrderTypeName = FilterOrderTypeNames,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

                FilterTripTypeId = navInfo.FilterTripTypeId,
                FilterTripTypeName = FilterTripTypeNames,
                UseTripTypeFilter = navInfo.UseTripTypeFilter,

            };

            var modelFinalReport = new OrdersNavigationResult<FinalReportViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getFinalReport(
                                             userId,
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
                                             true),

                AvailiableTypes =
                    context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                        .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,
                FilterOrderClientName = FilterOrderClientNames,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                FilterOrderTypeName = FilterOrderTypeNames,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

                FilterTripTypeId = navInfo.FilterTripTypeId,
                FilterTripTypeName = FilterTripTypeNames,
                UseTripTypeFilter = navInfo.UseTripTypeFilter,

            };

            var model = new OrdersReportsNavigationResult
            {
                BaseReport = modelBaseReport,
                StatusReport = modelStatusReport,
                OrdersReport = modelOrdersReport,
                FinalReport = modelFinalReport
            };

            model.PageNumber = navInfo.PageNumber;

            //var orderTypes1 = model.OrdersReport.DisplayValues.SelectMany(o => o.BalanceKeepersName);
            model.BaseReport.SumPlanCarNumber = model.BaseReport.DisplayValues.Sum(item => item.CarNumber);
            model.BaseReport.SumFactCarNumber = model.BaseReport.DisplayValues.Sum(item => item.FactCarNumber);

            //получаем типы заявок
            var orderTypes = model.BaseReport.DisplayValues.Select(o => o.OrderType).Distinct().ToList();

            List<string> orderFinalStatuses = new List<string>();
            foreach (var OrderType in orderTypes)
            {
                var OrderStatuses = context.getFinalPipelineSteps(userId, OrderType);

                orderFinalStatuses.AddRange(OrderStatuses);
            }

            //только различные
            model.BaseReport.FinalStatuses = orderFinalStatuses.Distinct().ToList();
            //отсортировали
            model.BaseReport.FinalStatuses = model.BaseReport.FinalStatuses.OrderBy(q => q).ToList();

            if (model.BaseReport.DisplayValues.Where(ri => ri.CurrentOrderStatusName == "Проимпортирована").Count() > 0)
            {
                if (orderFinalStatuses.Where(ri => ri == "Проимпортирована").Count() == 0)
                    orderFinalStatuses.Add("Проимпортирована");
                if (model.BaseReport.FinalStatuses.Where(ri => ri == "Проимпортирована").Count() == 0)
                    model.BaseReport.FinalStatuses.Add("Проимпортирована");
            }

            //Есть список фин. статусов, теперь нужно пройти и собрать по ним суммы
            Dictionary<string, int> orderFinalStatusesDict = new Dictionary<string, int>();
            int StatusSumm = 0;
            foreach (var FinalStatus in model.BaseReport.FinalStatuses)
            {
                // StatusSumm = 0;
                var StatusList = model.BaseReport.DisplayValues.Where(ri => ri.CurrentOrderStatusName == FinalStatus).ToList();
                StatusSumm = StatusList.Count;
                if (orderFinalStatusesDict.Where(ri => ri.Key == FinalStatus).Count() == 0)
                    orderFinalStatusesDict.Add(FinalStatus, StatusSumm);

            }

            model.BaseReport.orderFinalStatusesDict = orderFinalStatusesDict;

            //собираем итоги для отчета по плановым и срочным заявкам            
            List<string> statusOrderSumm = new List<string>();
            if (model.StatusReport.DisplayValues.Count() > 0)
            {
                statusOrderSumm.Add(model.StatusReport.DisplayValues.Sum(item => item.CntAll).ToString());
                statusOrderSumm.Add(model.StatusReport.DisplayValues.Sum(item => item.CntZero).ToString());

                statusOrderSumm.Add(
                    ((int)model.StatusReport.DisplayValues.Average(item => item.CntZeroPercent)).ToString() + "%");
                //double tmpCntZeroPercentResult = (model.StatusReport.DisplayValues.Sum(item => item.CntZero) * 100) / model.StatusReport.DisplayValues.Sum(item => item.CntAll);
                //double CntZeroPercentResult = Math.Round(tmpCntZeroPercentResult);            
                //statusOrderSumm.Add(CntZeroPercentResult.ToString() + "%");

                statusOrderSumm.Add(model.StatusReport.DisplayValues.Sum(item => item.CntOne).ToString());
                statusOrderSumm.Add(
                    ((int)model.StatusReport.DisplayValues.Average(item => item.CntOnePercent)).ToString() + "%");
                //double CntOnePercentResult = 100 - CntZeroPercentResult;
                //statusOrderSumm.Add(CntOnePercentResult.ToString() + "%");
            }
            else
            {
                statusOrderSumm.Add("0");
                statusOrderSumm.Add("0");
                statusOrderSumm.Add("0");
                statusOrderSumm.Add("0");
            }
            model.StatusReport.FinalStatuses = statusOrderSumm;

            model.FilterOrderDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateBeg;
            model.FilterOrderDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderDateBegRaw;
            model.FilterOrderDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateEnd;
            model.FilterOrderDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderDateEndRaw;

            model.FilterAcceptDateBeg = string.IsNullOrEmpty(navInfo.FilterAcceptDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterAcceptDateBeg;
            model.FilterAcceptDateBegRaw = string.IsNullOrEmpty(navInfo.FilterAcceptDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterAcceptDateBegRaw;
            model.FilterAcceptDateEnd = string.IsNullOrEmpty(navInfo.FilterAcceptDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterAcceptDateEnd;
            model.FilterAcceptDateEndRaw = string.IsNullOrEmpty(navInfo.FilterAcceptDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterAcceptDateEndRaw;

            model.DateType = navInfo.DateType ?? 0;

            model.FilterOrderClientId = navInfo.FilterOrderClientId;
            model.UseOrderClientFilter = navInfo.UseOrderClientFilter;
            model.FilterOrderClientName = FilterOrderClientNames;

            model.FilterOrderTypeId = navInfo.FilterOrderTypeId;
            model.FilterOrderTypeName = FilterOrderTypeNames;
            model.UseOrderTypeFilter = navInfo.UseOrderTypeFilter;

            model.FilterTripTypeId = navInfo.FilterTripTypeId;
            model.FilterTripTypeName = FilterTripTypeNames;
            model.UseTripTypeFilter = navInfo.UseTripTypeFilter;

            if (!string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                string[] idList = navInfo.FilterOrderTypeId.Split(new char[] { ',' });
                string FilterOrderTypeName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderTypeName.Length > 0)
                    {
                        FilterOrderTypeName += ",";
                    }

                    FilterOrderTypeName = string.Concat(FilterOrderTypeName, context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }

                model.FilterOrderTypeName = FilterOrderTypeName;
            }
            model.UseOrderTypeFilter = navInfo.UseOrderTypeFilter;

            if (!string.IsNullOrEmpty(navInfo.FilterTripTypeId))
            {
                string[] idList = navInfo.FilterTripTypeId.Split(new char[] { ',' });
                string FilterTripTypeName = "";

                foreach (string i in idList)
                {
                    if (FilterTripTypeName.Length > 0)
                    {
                        FilterTripTypeName += ",";
                    }

                    FilterTripTypeName = string.Concat(FilterTripTypeName, context.getTripType(Convert.ToInt32(i))?.RouteTypeName);
                }

                model.FilterTripTypeName = FilterTripTypeName;
            }
            model.UseTripTypeFilter = navInfo.UseTripTypeFilter;

            if (!string.IsNullOrEmpty(navInfo.FilterOrderClientId))
            {
                string[] idList = navInfo.FilterOrderClientId.Split(new char[] { ',' });
                string FilterOrderClientName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderClientName.Length > 0)
                    {
                        FilterOrderClientName += ",";
                    }

                    var client = context.getClient(Convert.ToInt32(i));
                    FilterOrderClientName = string.Concat(FilterOrderClientName, string.Concat(client?.ClientBalanceKeeperName, "/", client?.ClientName));
                }

                model.FilterOrderClientName = FilterOrderClientName;
            }
            model.UseOrderClientFilter = navInfo.UseOrderClientFilter;

            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult BaseReportTruck(OrdersNavigationInfo navInfo)
        {
            //обрабатываем данные фильтров
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

            navInfo.UseFinalStatusFilter = true;
            navInfo.FilterFinalStatus = false;
            //отслеживаем страницу
            if (navInfo.PageNumber == 0) { navInfo.PageNumber = 1; }

            string FilterOrderClientNames = "";

            if (!string.IsNullOrEmpty(navInfo.FilterOrderClientId))
            {
                string[] idList = navInfo.FilterOrderClientId.Split(new char[] { ',' });


                foreach (string i in idList)
                {
                    if (FilterOrderClientNames.Length > 0)
                    {
                        FilterOrderClientNames += ",";
                    }
                    FilterOrderClientNames = context.getClient(Convert.ToInt32(i)).ClientName;
                }
            }

            string FilterOrderTypeNames = "";
            if (!string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                string[] idList = navInfo.FilterOrderTypeId.Split(new char[] { ',' });
                foreach (string i in idList)
                {
                    if (FilterOrderTypeNames.Length > 0)
                    {
                        FilterOrderTypeNames += ",";
                    }

                    FilterOrderTypeNames = string.Concat(FilterOrderTypeNames,
                    context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }
            }

            string FilterTripTypeNames = "";
            if (!string.IsNullOrEmpty(navInfo.FilterTripTypeId))
            {
                string[] idList = navInfo.FilterTripTypeId.Split(new char[] { ',' });
                foreach (string i in idList)
                {
                    if (FilterTripTypeNames.Length > 0)
                    {
                        FilterTripTypeNames += ",";
                    }

                    FilterTripTypeNames = string.Concat(FilterTripTypeNames,
                    context.getTripType(Convert.ToInt32(i))?.RouteTypeName);
                }
            }

            var modelBaseReport = new OrdersNavigationResult<BaseReportViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getBaseReport(
                    userId,
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
                    false),

                AvailiableTypes =
                    context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                        .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,
                FilterOrderClientName = FilterOrderClientNames,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                FilterOrderTypeName = FilterOrderTypeNames,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

            };


            var modelStatusReport = new OrdersNavigationResult<StatusReportViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getStatusReport(
                                             userId,
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

                                             false),

                AvailiableTypes =
                    context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                        .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,
                FilterOrderClientName = FilterOrderClientNames,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                FilterOrderTypeName = FilterOrderTypeNames,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

            };


            var modelOrdersReport = new OrdersNavigationResult<OrdersReportViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getOrdersReport(
                                             userId,
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
                                             false),

                AvailiableTypes =
                    context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                        .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,
                FilterOrderClientName = FilterOrderClientNames,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                FilterOrderTypeName = FilterOrderTypeNames,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

            };

            var modelFinalReport = new OrdersNavigationResult<FinalReportViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getFinalReport(
                                             userId,
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
                                             false),

                AvailiableTypes =
                    context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                        .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,

                FilterOrderClientId = navInfo.FilterOrderClientId,
                UseOrderClientFilter = navInfo.UseOrderClientFilter,
                FilterOrderClientName = FilterOrderClientNames,

                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                FilterOrderTypeName = FilterOrderTypeNames,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

            };

            var model = new OrdersReportsNavigationResult
            {
                BaseReport = modelBaseReport,
                StatusReport = modelStatusReport,
                OrdersReport = modelOrdersReport,
                FinalReport = modelFinalReport
            };

            model.PageNumber = navInfo.PageNumber;

            //var orderTypes1 = model.OrdersReport.DisplayValues.SelectMany(o => o.BalanceKeepersName);
            model.BaseReport.SumPlanCarNumber = model.BaseReport.DisplayValues.Sum(item => item.CarNumber);
            model.BaseReport.SumFactCarNumber = model.BaseReport.DisplayValues.Sum(item => item.FactCarNumber);

            //получаем типы заявок
            var orderTypes = model.BaseReport.DisplayValues.Select(o => o.OrderType).Distinct().ToList();

            List<string> orderFinalStatuses = new List<string>();
            foreach (var OrderType in orderTypes)
            {
                var OrderStatuses = context.getFinalPipelineSteps(userId, OrderType);

                orderFinalStatuses.AddRange(OrderStatuses);
            }

            //только различные
            model.BaseReport.FinalStatuses = orderFinalStatuses.Distinct().ToList();
            //отсортировали
            model.BaseReport.FinalStatuses = model.BaseReport.FinalStatuses.OrderBy(q => q).ToList();

            if (model.BaseReport.DisplayValues.Where(ri => ri.CurrentOrderStatusName == "Проимпортирована").Count() > 0)
            {
                if (orderFinalStatuses.Where(ri => ri == "Проимпортирована").Count() == 0)
                    orderFinalStatuses.Add("Проимпортирована");
                if (model.BaseReport.FinalStatuses.Where(ri => ri == "Проимпортирована").Count() == 0)
                    model.BaseReport.FinalStatuses.Add("Проимпортирована");
            }

            //Есть список фин. статусов, теперь нужно пройти и собрать по ним суммы
            Dictionary<string, int> orderFinalStatusesDict = new Dictionary<string, int>();
            int StatusSumm = 0;
            foreach (var FinalStatus in model.BaseReport.FinalStatuses)
            {
                // StatusSumm = 0;
                var StatusList = model.BaseReport.DisplayValues.Where(ri => ri.CurrentOrderStatusName == FinalStatus).ToList();
                StatusSumm = StatusList.Count;
                if (orderFinalStatusesDict.Where(ri => ri.Key == FinalStatus).Count() == 0)
                    orderFinalStatusesDict.Add(FinalStatus, StatusSumm);

            }

            model.BaseReport.orderFinalStatusesDict = orderFinalStatusesDict;

            //собираем итоги для отчета по плановым и срочным заявкам            
            List<string> statusOrderSumm = new List<string>();
            if (model.StatusReport.DisplayValues.Count() > 0)
            {
                statusOrderSumm.Add(model.StatusReport.DisplayValues.Sum(item => item.CntAll).ToString());
                statusOrderSumm.Add(model.StatusReport.DisplayValues.Sum(item => item.CntZero).ToString());

                statusOrderSumm.Add(
                    ((int)model.StatusReport.DisplayValues.Average(item => item.CntZeroPercent)).ToString() + "%");
                //double tmpCntZeroPercentResult = (model.StatusReport.DisplayValues.Sum(item => item.CntZero) * 100) / model.StatusReport.DisplayValues.Sum(item => item.CntAll);
                //double CntZeroPercentResult = Math.Round(tmpCntZeroPercentResult);            
                //statusOrderSumm.Add(CntZeroPercentResult.ToString() + "%");

                statusOrderSumm.Add(model.StatusReport.DisplayValues.Sum(item => item.CntOne).ToString());
                statusOrderSumm.Add(
                    ((int)model.StatusReport.DisplayValues.Average(item => item.CntOnePercent)).ToString() + "%");
                //double CntOnePercentResult = 100 - CntZeroPercentResult;
                //statusOrderSumm.Add(CntOnePercentResult.ToString() + "%");
            }
            else
            {
                statusOrderSumm.Add("0");
                statusOrderSumm.Add("0");
                statusOrderSumm.Add("0");
                statusOrderSumm.Add("0");
            }
            model.StatusReport.FinalStatuses = statusOrderSumm;

            model.FilterOrderDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateBeg;
            model.FilterOrderDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderDateBegRaw;
            model.FilterOrderDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateEnd;
            model.FilterOrderDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderDateEndRaw;

            model.FilterAcceptDateBeg = string.IsNullOrEmpty(navInfo.FilterAcceptDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterAcceptDateBeg;
            model.FilterAcceptDateBegRaw = string.IsNullOrEmpty(navInfo.FilterAcceptDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterAcceptDateBegRaw;
            model.FilterAcceptDateEnd = string.IsNullOrEmpty(navInfo.FilterAcceptDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterAcceptDateEnd;
            model.FilterAcceptDateEndRaw = string.IsNullOrEmpty(navInfo.FilterAcceptDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterAcceptDateEndRaw;

            model.UseOrderDateFilter = navInfo.UseOrderDateFilter;
            model.UseAcceptDateFilter = navInfo.UseAcceptDateFilter;


            model.FilterOrderClientId = navInfo.FilterOrderClientId;
            model.UseOrderClientFilter = navInfo.UseOrderClientFilter;
            model.FilterOrderClientName = FilterOrderClientNames;

            model.FilterOrderTypeId = navInfo.FilterOrderTypeId;
            model.FilterOrderTypeName = FilterOrderTypeNames;
            model.UseOrderTypeFilter = navInfo.UseOrderTypeFilter;

            model.FilterTripTypeId = navInfo.FilterTripTypeId;
            model.FilterTripTypeName = FilterTripTypeNames;
            model.UseTripTypeFilter = navInfo.UseTripTypeFilter;


            model.DateType = navInfo.DateType ?? 0;

            if (!string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                string[] idList = navInfo.FilterOrderTypeId.Split(new char[] { ',' });
                string FilterOrderTypeName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderTypeName.Length > 0)
                    {
                        FilterOrderTypeName += ",";
                    }

                    FilterOrderTypeName = string.Concat(FilterOrderTypeName, context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }

                model.FilterOrderTypeName = FilterOrderTypeName;
            }
            model.UseOrderTypeFilter = navInfo.UseOrderTypeFilter;


            if (!string.IsNullOrEmpty(navInfo.FilterTripTypeId))
            {
                string[] idList = navInfo.FilterTripTypeId.Split(new char[] { ',' });
                string FilterTripTypeName = "";

                foreach (string i in idList)
                {
                    if (FilterTripTypeName.Length > 0)
                    {
                        FilterTripTypeName += ",";
                    }

                    FilterTripTypeName = string.Concat(FilterTripTypeName, context.getTripType(Convert.ToInt32(i))?.RouteTypeName);
                }

                model.FilterTripTypeName = FilterTripTypeName;
            }
            model.UseTripTypeFilter = navInfo.UseTripTypeFilter;

            if (!string.IsNullOrEmpty(navInfo.FilterOrderClientId))
            {
                string[] idList = navInfo.FilterOrderClientId.Split(new char[] { ',' });
                string FilterOrderClientName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderClientName.Length > 0)
                    {
                        FilterOrderClientName += ",";
                    }

                    var client = context.getClient(Convert.ToInt32(i));
                    FilterOrderClientName = string.Concat(FilterOrderClientName, string.Concat(client?.ClientBalanceKeeperName, "/", client?.ClientName));
                }
                model.FilterOrderClientName = FilterOrderClientName;
            }
            model.UseOrderClientFilter = navInfo.UseOrderClientFilter;

            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrderTruckTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderTruckTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrderTruckTypesCount(searchTerm);

            var pagedAttendees = OrderTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrderPassTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderPassTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrderPassTypesCount(searchTerm);

            var pagedAttendees = OrderTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrderTruckTripTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderTruckTripTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrderTruckTripTypesCount(searchTerm);

            var pagedAttendees = RouteTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrderPassTripTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderPassTripTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrderPassTripTypesCount(searchTerm);

            var pagedAttendees = RouteTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult RouteTypesVmToSelect2Format(IEnumerable<RouteTypesViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.RouteTypeName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        public ActionResult GetTripTypes(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetTripTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetTripTypesCount(searchTerm);

            var pagedAttendees = RouteTypesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetLoadPoints(long OrderId, bool IsLoading)
        {
            List<OrderAdditionalRoutePointModel> pointList = null;
            string pointListName;

            if (IsLoading == true)
            {
                pointListName = "RoutePointLoadList";
                bool? IsDbDataTakenLoad = (bool?)Session["IsDbDataTakenLoad"];
                if (IsDbDataTakenLoad == null)
                {
                    pointList = context.getLoadPoints(OrderId, IsLoading).ToList();
                }
                if (pointList == null || IsDbDataTakenLoad == true)
                {
                    pointList = Session[pointListName] as List<OrderAdditionalRoutePointModel>;
                }
                IsDbDataTakenLoad = true;
                Session["IsDbDataTakenLoad"] = IsDbDataTakenLoad;
            }
            else
            {
                pointListName = "RoutePointUnLoadList";
                bool? IsDbDataTakenUnLoad = (bool?)Session["IsDbDataTakenUnLoad"];
                if (IsDbDataTakenUnLoad == null)
                {
                    pointList = context.getLoadPoints(OrderId, IsLoading).ToList();
                }
                if (pointList == null || IsDbDataTakenUnLoad == true)
                {
                    pointList = Session[pointListName] as List<OrderAdditionalRoutePointModel>;
                }
                IsDbDataTakenUnLoad = true;
                Session["IsDbDataTakenUnLoad"] = IsDbDataTakenUnLoad;
            }

            Session[pointListName] = pointList;

            return Json(pointList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NewRoutePoint(OrderAdditionalRoutePointModel model)
        {
            string pointListName;
            if (model.IsLoading == true)
            {
                pointListName = "RoutePointLoadList";
            }
            else
            {
                pointListName = "RoutePointUnLoadList";
            }
            var pointList = Session[pointListName] as List<OrderAdditionalRoutePointModel>;
            long maxId;
            if (pointList == null || pointList.Count == 0)
            {
                pointList = new List<OrderAdditionalRoutePointModel>();
                maxId = 1;
            }
            else
            {
                maxId = pointList.Max(t => t.Id);
            }

            model.Id = maxId + 1;
            model.CityAddress = model.CityPoint + ", " + model.AddressPoint;
            pointList.Add(model);
            Session[pointListName] = pointList;

            return Json(new { Id = model.Id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateRoutePoint(long Id, bool IsLoading, bool IsSaved, string ContactPerson, string ContactPersonPhone, int NumberPoint)
        {
            string pointListName;
            if (IsLoading == true)
            {
                pointListName = "RoutePointLoadList";
            }
            else
            {
                pointListName = "RoutePointUnLoadList";
            }
            var pointList = Session[pointListName] as List<OrderAdditionalRoutePointModel>;


            var itemToUpdate = pointList.Single(r => r.Id == Id);
            itemToUpdate.ContactPerson = ContactPerson;
            itemToUpdate.ContactPersonPhone = ContactPersonPhone;
            itemToUpdate.NumberPoint = NumberPoint;
            itemToUpdate.Contacts = ContactPerson + Environment.NewLine + ContactPersonPhone;
            if (IsSaved == true)
            {
                var pointListUpdate = Session["pointListUpdate"] as List<OrderAdditionalRoutePointModel>;

                if (pointListUpdate == null)
                {
                    pointListUpdate = new List<OrderAdditionalRoutePointModel>();
                }

                pointListUpdate.Add(itemToUpdate);
                Session["pointListUpdate"] = pointListUpdate;
            }
            else
            {
                Session[pointListName] = pointList;
            }

            return Json(new { Id = Id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveRoutePoint(long Id, bool IsLoading, bool IsSaved)
        {
            string pointListName;
            if (IsLoading == true)
            {
                pointListName = "RoutePointLoadList";
            }
            else
            {
                pointListName = "RoutePointUnLoadList";
            }
            var pointList = Session[pointListName] as List<OrderAdditionalRoutePointModel>;
            var itemToRemove = pointList.Single(r => r.Id == Id);
            if (IsSaved == true)
            {
                var pointListDelete = Session["pointListDelete"] as List<OrderAdditionalRoutePointModel>;

                if (pointListDelete == null)
                {
                    pointListDelete = new List<OrderAdditionalRoutePointModel>();
                }

                pointListDelete.Add(itemToRemove);
                Session["pointListDelete"] = pointListDelete;
            }
            var result = pointList.Remove(itemToRemove);

            return Json(new { DeleteResult = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveRoutePointTemp(long Id, bool IsLoading)
        {
            string pointListName;
            if (IsLoading == true)
            {
                pointListName = "RoutePointLoadList";
            }
            else
            {
                pointListName = "RoutePointUnLoadList";
            }
            var pointList = Session[pointListName] as List<OrderAdditionalRoutePointModel>;
            var itemToRemove = pointList.Single(r => r.Id == Id);
            var result = pointList.Remove(itemToRemove);
            return Json(new { DeleteResult = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetLoadPointsTemp(bool IsLoading)
        {
            string pointListName;
            if (IsLoading == true)
            {
                pointListName = "RoutePointLoadList";
            }
            else
            {
                pointListName = "RoutePointUnLoadList";
            }
            var pointList = Session[pointListName] as List<OrderAdditionalRoutePointModel>;

            return Json(pointList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOrderExecuter(string searchTerm, int pageSize, int pageNum, int OrderTypeId)
        {
            var OrderTypeFullInfo = context.getOrderType(OrderTypeId);

            var storages = context.GetOrderExecuter(searchTerm, pageSize, pageNum, OrderTypeFullInfo.UserRoleIdForExecuterData);
            var storagesCount = context.GetOrderExecuterCount(searchTerm, OrderTypeFullInfo.UserRoleIdForExecuterData);

            var pagedAttendees = OrderExecuterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult OrderExecuterVmToSelect2Format(IEnumerable<UserViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.userId,
                    text = groupItem.displayName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }
                

        //[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult FactCars(OrderCarsNavigationInfo navInfo)
        {
            if (string.IsNullOrEmpty(navInfo.FilterOrderIdFilter)) { navInfo.UseOrderIdFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterExpeditorIdFilter)) { navInfo.UseExpeditorIdFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterContractExpBkInfoFilter)) { navInfo.UseContractExpBkInfoFilter = false; }

            if (string.IsNullOrEmpty(navInfo.FilterCarrierInfoFilter)) { navInfo.UseCarrierInfoFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterContractInfoFilter)) { navInfo.UseContractInfoFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterCarModelInfoFilter)) { navInfo.UseCarModelInfoFilter = false; }

            if (string.IsNullOrEmpty(navInfo.FilterCarRegNumFilter)) { navInfo.UseCarRegNumFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterCarCapacityFilter)) { navInfo.UseCarCapacityFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterCarDriverInfoFilter)) { navInfo.UseCarDriverInfoFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterDriverCardInfoFilter)) { navInfo.UseDriverCardInfoFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterDriverContactInfoFilter)) { navInfo.UseDriverContactInfoFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterCommentsFilter)) { navInfo.UseCommentsFilter = false; }

            //фильтры из заявки
            if (string.IsNullOrEmpty(navInfo.FilterStatusId)) { navInfo.UseStatusFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderCreatorId)) { navInfo.UseOrderCreatorFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId)) { navInfo.UseOrderTypeFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderClientId)) { navInfo.UseOrderClientFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderExecuterId)) { navInfo.UseOrderExecuterFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderProjectId)) { navInfo.UseOrderProjectFilter = false; }

            if (string.IsNullOrEmpty(navInfo.FilterOrderPayerId)) { navInfo.UseOrderPayerFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderOrgFromId)) { navInfo.UseOrderOrgFromFilter = false; }
            if (string.IsNullOrEmpty(navInfo.FilterOrderOrgToId)) { navInfo.UseOrderOrgToFilter = false; }

            if (//(!navInfo.UseOrderIdFilter)
             /* &&*/ (!navInfo.UseExpeditorIdFilter)
              && (!navInfo.UseContractExpBkInfoFilter)
              && (!navInfo.UseCarrierInfoFilter)
              && (!navInfo.UseContractInfoFilter)
              && (!navInfo.UseCarModelInfoFilter)
              && (!navInfo.UseCarRegNumFilter)
              && (!navInfo.UseCarCapacityFilter)
              && (!navInfo.UseCarDriverInfoFilter)
              && (!navInfo.UseDriverCardInfoFilter)
              && (!navInfo.UseDriverContactInfoFilter)
              && (!navInfo.UseCommentsFilter)
              
              && (!navInfo.UseStatusFilter)
              && (!navInfo.UseOrderCreatorFilter)
              && (!navInfo.UseOrderTypeFilter)
              && (!navInfo.UseOrderClientFilter)
              && (!navInfo.UseOrderExecuterFilter)
              && (!navInfo.UseOrderPriorityFilter)
              && (!navInfo.UseOrderDateFilter)
              && (!navInfo.UseOrderExDateFilter)
              && (!navInfo.UseOrderEndDateFilter)
              && (!navInfo.UseFinalStatusFilter)
              && (!navInfo.UseOrderProjectFilter))
            {
                navInfo.UseOrderExDateFilter = true;

                navInfo.FilterOrderExDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterOrderExDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
                navInfo.FilterOrderExDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterOrderExDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));

                navInfo.UseOrderEndDateFilter = true;

                navInfo.FilterOrderEndDateBeg = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy");
                navInfo.FilterOrderEndDateBegRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(-7));
                navInfo.FilterOrderEndDateEnd = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
                navInfo.FilterOrderEndDateEndRaw = DateTimeConvertClass.getString(DateTime.Now.AddDays(1));

              //  navInfo.UseFinalStatusFilter = true;
               // navInfo.FilterFinalStatus = false;

              //  navInfo.UseFactShipperFilter = true;
               // navInfo.UseFactConsigneeFilter = true;
            }

            bool _UseCarModelInfoFilter = navInfo.UseCarModelInfoFilter;
            string _FilterCarModelInfoName = "";
             if (!string.IsNullOrEmpty(navInfo.FilterCarModelInfoFilter))
            {
                string[] idList = navInfo.FilterCarModelInfoFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterCarModelInfoName = "";
                    foreach (string i in idList)
                    {
                        if (FilterCarModelInfoName.Length > 0)
                        {
                            FilterCarModelInfoName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           
                             FilterCarModelInfoName = string.Concat(FilterCarModelInfoName, context.getUsedCarInfo(Convert.ToInt32(i))?.CarModelInfo);
                        }
                        else
                        {
                            _UseCarModelInfoFilter = false;
                            break;
                        }
                    }
                    _FilterCarModelInfoName = FilterCarModelInfoName;
                }
                else
                {
                    _UseCarModelInfoFilter = false;
                }
            }

            bool _UseCarRegNumFilter = navInfo.UseCarRegNumFilter;
            string _FilterCarRegNumName = "";
             if (!string.IsNullOrEmpty(navInfo.FilterCarRegNumFilter))
            {
                string[] idList = navInfo.FilterCarRegNumFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterCarRegNumName = "";
                    foreach (string i in idList)
                    {
                        if (FilterCarRegNumName.Length > 0)
                        {
                            FilterCarRegNumName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           
                             FilterCarRegNumName = string.Concat(FilterCarRegNumName, context.getUsedCarInfo(Convert.ToInt32(i))?.CarRegNum);
                        }
                        else
                        {
                            _UseCarRegNumFilter = false;
                            break;
                        }
                    }
                    _FilterCarRegNumName = FilterCarRegNumName;
                }
                else
                {
                    _UseCarModelInfoFilter = false;
                }
            }
             
            bool _UseCarCapacityFilter = navInfo.UseCarCapacityFilter;
            string _FilterCarCapacityName = "";
             if (!string.IsNullOrEmpty(navInfo.FilterCarCapacityFilter))
            {
                string[] idList = navInfo.FilterCarCapacityFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterCarCapacityName = "";
                    foreach (string i in idList)
                    {
                        if (FilterCarCapacityName.Length > 0)
                        {
                            FilterCarCapacityName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           
                             FilterCarCapacityName = string.Concat(FilterCarCapacityName, context.getUsedCarInfo(Convert.ToInt32(i))?.CarCapacity);
                        }
                        else
                        {
                            _UseCarCapacityFilter = false;
                            break;
                        }
                    }
                    _FilterCarCapacityName = FilterCarCapacityName;
                }
                else
                {
                    _UseCarCapacityFilter = false;
                }
            }

             
            bool _UseCarDriverInfoFilter = navInfo.UseCarDriverInfoFilter;
            string _FilterCarDriverInfoName = "";
             if (!string.IsNullOrEmpty(navInfo.FilterCarDriverInfoFilter))
            {
                string[] idList = navInfo.FilterCarDriverInfoFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterCarDriverInfoName = "";
                    foreach (string i in idList)
                    {
                        if (FilterCarDriverInfoName.Length > 0)
                        {
                            FilterCarDriverInfoName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           
                             FilterCarDriverInfoName = string.Concat(FilterCarDriverInfoName, context.getUsedCarInfo(Convert.ToInt32(i))?.CarDriverInfo);
                        }
                        else
                        {
                            _UseCarDriverInfoFilter = false;
                            break;
                        }
                    }
                    _FilterCarDriverInfoName = FilterCarDriverInfoName;
                }
                else
                {
                    _UseCarDriverInfoFilter = false;
                }
            }


            bool _UseDriverContactInfoFilter = navInfo.UseDriverContactInfoFilter;
            string _FilterDriverContactInfoName = "";
             if (!string.IsNullOrEmpty(navInfo.FilterDriverContactInfoFilter))
            {
                string[] idList = navInfo.FilterDriverContactInfoFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterDriverContactInfoName = "";
                    foreach (string i in idList)
                    {
                        if (FilterDriverContactInfoName.Length > 0)
                        {
                            FilterDriverContactInfoName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           
                             FilterDriverContactInfoName = string.Concat(FilterDriverContactInfoName, context.getUsedCarInfo(Convert.ToInt32(i))?.DriverContactInfo);
                        }
                        else
                        {
                            _UseDriverContactInfoFilter = false;
                            break;
                        }
                    }
                    _FilterDriverContactInfoName = FilterDriverContactInfoName;
                }
                else
                {
                    _UseDriverContactInfoFilter = false;
                }
            }

                bool _UseDriverCardInfoFilter = navInfo.UseDriverCardInfoFilter;
            string _FilterDriverCardInfoName = "";
             if (!string.IsNullOrEmpty(navInfo.FilterDriverCardInfoFilter))
            {
                string[] idList = navInfo.FilterDriverCardInfoFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterDriverCardInfoName = "";
                    foreach (string i in idList)
                    {
                        if (FilterDriverCardInfoName.Length > 0)
                        {
                            FilterDriverCardInfoName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           
                             FilterDriverCardInfoName = string.Concat(FilterDriverCardInfoName, context.getUsedCarInfo(Convert.ToInt32(i))?.DriverCardInfo);
                        }
                        else
                        {
                            _UseDriverCardInfoFilter = false;
                            break;
                        }
                    }
                    _FilterDriverCardInfoName = FilterDriverCardInfoName;
                }
                else
                {
                    _UseDriverCardInfoFilter = false;
                }
            }

            bool _UseCommentsFilter = navInfo.UseCommentsFilter;
            string _FilterCommentsName = "";
            if (!string.IsNullOrEmpty(navInfo.FilterCommentsFilter))
            {
                string[] idList = navInfo.FilterCommentsFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterCommentsName = "";
                    foreach (string i in idList)
                    {
                        if (FilterCommentsName.Length > 0)
                        {
                            FilterCommentsName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {                           
                             FilterCommentsName = string.Concat(FilterCommentsName, context.getUsedCarInfo(Convert.ToInt32(i))?.Comments);
                        }
                        else
                        {
                            _UseCommentsFilter = false;
                            break;
                        }
                    }
                    _FilterCommentsName = FilterCommentsName;
                }
                else
                {
                    _UseCommentsFilter = false;
                }
            }

            bool _UseContractInfoFilter = navInfo.UseContractInfoFilter;
            string _FilterContractInfoName = "";
             if (!string.IsNullOrEmpty(navInfo.FilterContractInfoFilter))
            {
                string[] idList = navInfo.FilterContractInfoFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterContractInfoName = "";
                    foreach (string i in idList)
                    {
                        if (FilterContractInfoName.Length > 0)
                        {
                            FilterContractInfoName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           
                             FilterContractInfoName = string.Concat(FilterContractInfoName, context.getUsedCarInfo(Convert.ToInt32(i))?.ContractInfo);
                        }
                        else
                        {
                            _UseContractInfoFilter = false;
                            break;
                        }
                    }
                    _FilterContractInfoName = FilterContractInfoName;
                }
                else
                {
                    _UseContractInfoFilter = false;
                }
            }

                 bool _UseCarrierInfoFilter = navInfo.UseCarrierInfoFilter;
            string _FilterCarrierInfoName = "";
             if (!string.IsNullOrEmpty(navInfo.FilterCarrierInfoFilter))
            {
                string[] idList = navInfo.FilterCarrierInfoFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterCarrierInfoName = "";
                    foreach (string i in idList)
                    {
                        if (FilterCarrierInfoName.Length > 0)
                        {
                            FilterCarrierInfoName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           
                             FilterCarrierInfoName = string.Concat(FilterCarrierInfoName, context.getUsedCarInfo(Convert.ToInt32(i))?.CarrierInfo);
                        }
                        else
                        {
                            _UseCarrierInfoFilter = false;
                            break;
                        }
                    }
                    _FilterCarrierInfoName = FilterCarrierInfoName;
                }
                else
                {
                    _UseCarrierInfoFilter = false;
                }
            }

              if (!string.IsNullOrEmpty(navInfo.FilterStatusId))
            {
                string[] idList = navInfo.FilterStatusId.Split(new char[] { ',' });
                if ((idList.Length == 1) && (Convert.ToInt32(idList[0]) == 0))
                {
                    navInfo.UseStatusFilter = false;
                }
            }

            FactCarsFilter factCarsFilter = new FactCarsFilter();
            factCarsFilter.isAdmin = this.isAdmin;
            factCarsFilter.userId = userId;
            factCarsFilter.UseOrderIdFilter = navInfo.UseOrderIdFilter;
            factCarsFilter.UseExpeditorIdFilter = navInfo.UseExpeditorIdFilter;
            factCarsFilter.UseContractExpBkInfoFilter = navInfo.UseContractExpBkInfoFilter;
            factCarsFilter.UseCarrierInfoFilter = navInfo.UseCarrierInfoFilter;
            factCarsFilter.UseContractInfoFilter = navInfo.UseContractInfoFilter;
            factCarsFilter.UseCarModelInfoFilter = navInfo.UseCarModelInfoFilter;
            factCarsFilter.UseCarRegNumFilter = navInfo.UseCarRegNumFilter;
            factCarsFilter.UseCarCapacityFilter = navInfo.UseCarCapacityFilter;
            factCarsFilter.UseCarDriverInfoFilter = navInfo.UseCarDriverInfoFilter;
            factCarsFilter.UseDriverCardInfoFilter = navInfo.UseDriverCardInfoFilter;
            factCarsFilter.UseDriverContactInfoFilter = navInfo.UseDriverContactInfoFilter;
            factCarsFilter.UseCommentsFilter = navInfo.UseCommentsFilter;
            factCarsFilter.UseFactShipperFilter = navInfo.UseFactShipperFilter;
            factCarsFilter.UseFactConsigneeFilter = navInfo.UseFactConsigneeFilter;

            factCarsFilter.UseOrderExDateFilter = navInfo.UseOrderExDateFilter;
            factCarsFilter.UseOrderEndDateFilter = navInfo.UseOrderEndDateFilter;
            
            factCarsFilter.FilterFactShipperBeg = string.IsNullOrEmpty(navInfo.FilterFactShipperBegRaw)
                ? DateTime.Now.AddDays(-7)
                : DateTimeConvertClass.getDateTime(navInfo.FilterFactShipperBegRaw);

            factCarsFilter.FilterFactShipperEnd = string.IsNullOrEmpty(navInfo.FilterFactShipperEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterFactShipperEndRaw);

            factCarsFilter.FilterFactConsigneeBeg = string.IsNullOrEmpty(navInfo.FilterFactConsigneeBegRaw)
                ? DateTime.Now.AddDays(-7)
                : DateTimeConvertClass.getDateTime(navInfo.FilterFactConsigneeBegRaw);

            factCarsFilter.FilterFactConsigneeEnd = string.IsNullOrEmpty(navInfo.FilterFactConsigneeEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterFactConsigneeEndRaw);

            factCarsFilter.FilterOrderExDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderExDateBegRaw)
                ? DateTime.Now.AddDays(-7)
                : DateTimeConvertClass.getDateTime(navInfo.FilterOrderExDateBegRaw);

            factCarsFilter.FilterOrderExDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderExDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterOrderExDateEndRaw);

            factCarsFilter.FilterOrderEndDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderEndDateBegRaw)
               ? DateTime.Now.AddDays(-7)
               : DateTimeConvertClass.getDateTime(navInfo.FilterOrderEndDateBegRaw);

            factCarsFilter.FilterOrderEndDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderEndDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterOrderEndDateEndRaw);

            factCarsFilter.FilterOrderIdFilter = navInfo.FilterOrderIdFilter;

            factCarsFilter.FilterOrderIdFilter = navInfo.FilterOrderIdFilter;
            factCarsFilter.FilterExpeditorIdFilter = navInfo.FilterExpeditorIdFilter;
            factCarsFilter.FilterContractExpBkInfoFilter = navInfo.FilterContractExpBkInfoFilter;
            factCarsFilter.FilterCarrierInfoFilter = _FilterCarrierInfoName;
            factCarsFilter.FilterContractInfoFilter = _FilterContractInfoName;
            factCarsFilter.FilterCarModelInfoFilter = _FilterCarModelInfoName;
                
            factCarsFilter.FilterCarRegNumFilter = _FilterCarRegNumName;
            factCarsFilter.FilterCarCapacityFilter = _FilterCarCapacityName;
            factCarsFilter.FilterCarDriverInfoFilter = _FilterCarDriverInfoName;
            factCarsFilter.FilterDriverCardInfoFilter = _FilterDriverCardInfoName;
            factCarsFilter.FilterDriverContactInfoFilter = _FilterDriverContactInfoName;
            factCarsFilter.FilterCommentsFilter = _FilterCommentsName;
            //
            factCarsFilter.FilterOrderExecuterId = navInfo.FilterOrderExecuterId;
            factCarsFilter.UseOrderExecuterFilter = navInfo.UseOrderExecuterFilter;
            factCarsFilter.FilterStatusId = navInfo.FilterStatusId;
            factCarsFilter.UseStatusFilter = navInfo.UseStatusFilter;
            factCarsFilter.FilterOrderCreatorId = navInfo.FilterOrderCreatorId;
            factCarsFilter.UseOrderCreatorFilter = navInfo.UseOrderCreatorFilter;
            factCarsFilter.FilterOrderTypeId = navInfo.FilterOrderTypeId;
            factCarsFilter.UseOrderTypeFilter = navInfo.UseOrderTypeFilter;

            factCarsFilter.FilterTripTypeId = navInfo.FilterTripTypeId;
            factCarsFilter.UseTripTypeFilter = navInfo.UseTripTypeFilter;
            factCarsFilter.FilterOrderClientId = navInfo.FilterOrderClientId;
            factCarsFilter.UseOrderClientFilter = navInfo.UseOrderClientFilter;

            factCarsFilter.FilterOrderPriority = navInfo.FilterOrderPriority;
            factCarsFilter.UseOrderPriorityFilter = navInfo.UseOrderPriorityFilter;

            factCarsFilter.UseOrderDateFilter = navInfo.UseOrderDateFilter;
            factCarsFilter.FilterOrderDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw) ? DateTime.Now.AddDays(-7) : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw);
            factCarsFilter.FilterOrderDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw)
                ? DateTime.Now
                : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw);

            factCarsFilter.UseFinalStatusFilter = navInfo.UseFinalStatusFilter;
            factCarsFilter.FilterFinalStatus = navInfo.FilterFinalStatus;
            factCarsFilter.UseOrderProjectFilter = navInfo.UseOrderProjectFilter;
            factCarsFilter.FilterOrderProjectId = navInfo.FilterOrderProjectId;

            factCarsFilter.FilterOrderPayerId = navInfo.FilterOrderPayerId;
            factCarsFilter.UseOrderPayerFilter = navInfo.UseOrderPayerFilter;
            factCarsFilter.FilterOrderOrgFromId = navInfo.FilterOrderOrgFromId;
            factCarsFilter.UseOrderOrgFromFilter = navInfo.UseOrderOrgFromFilter;
            factCarsFilter.FilterOrderOrgToId = navInfo.FilterOrderOrgToId;
            factCarsFilter.UseOrderOrgToFilter = navInfo.UseOrderOrgToFilter;

            var model = new OrderCarsNavigationResult<OrderUsedCarViewModel>(navInfo, userId)
            {
                DisplayValues = context.getFactCars(factCarsFilter),

                FilterOrderIdFilter = navInfo.FilterOrderIdFilter,
                UseOrderIdFilter = navInfo.UseOrderIdFilter,

                FilterExpeditorIdFilter = navInfo.FilterExpeditorIdFilter,
                UseExpeditorIdFilter = navInfo.UseExpeditorIdFilter,
                
                FilterContractExpBkInfoFilter = navInfo.FilterContractExpBkInfoFilter,
                UseContractExpBkInfoFilter = navInfo.UseContractExpBkInfoFilter,

                FilterCarrierInfoFilter = navInfo.FilterCarrierInfoFilter,
                UseCarrierInfoFilter = navInfo.UseCarrierInfoFilter,

                FilterContractInfoFilter = navInfo.FilterContractInfoFilter,
                UseContractInfoFilter = navInfo.UseContractInfoFilter,

                FilterCarModelInfoFilter = navInfo.FilterCarModelInfoFilter,
                UseCarModelInfoFilter = navInfo.UseCarModelInfoFilter,
                
                FilterCarRegNumFilter = navInfo.FilterCarRegNumFilter,
                UseCarRegNumFilter = navInfo.UseCarRegNumFilter,

                FilterCarCapacityFilter = navInfo.FilterCarCapacityFilter,
                UseCarCapacityFilter = navInfo.UseCarCapacityFilter,
            
                FilterCarDriverInfoFilter = navInfo.FilterCarDriverInfoFilter,
                UseCarDriverInfoFilter = navInfo.UseCarDriverInfoFilter,

                FilterDriverCardInfoFilter = navInfo.FilterDriverCardInfoFilter,
                UseDriverCardInfoFilter = navInfo.UseDriverCardInfoFilter,
                
                FilterDriverContactInfoFilter = navInfo.FilterDriverContactInfoFilter,
                UseDriverContactInfoFilter = navInfo.UseDriverContactInfoFilter,

                FilterCommentsFilter = navInfo.FilterCommentsFilter,
                UseCommentsFilter = navInfo.UseCommentsFilter,
                ////
                FilterOrderExecuterId = navInfo.FilterOrderExecuterId,
            UseOrderExecuterFilter = navInfo.UseOrderExecuterFilter,
            FilterStatusId = navInfo.FilterStatusId,
            UseStatusFilter = navInfo.UseStatusFilter,
            FilterOrderCreatorId = navInfo.FilterOrderCreatorId,
            UseOrderCreatorFilter = navInfo.UseOrderCreatorFilter,
            FilterOrderTypeId = navInfo.FilterOrderTypeId,
            UseOrderTypeFilter = navInfo.UseOrderTypeFilter,

            FilterTripTypeId = navInfo.FilterTripTypeId,
            UseTripTypeFilter = navInfo.UseTripTypeFilter,
            FilterOrderClientId = navInfo.FilterOrderClientId,
            UseOrderClientFilter = navInfo.UseOrderClientFilter,

            FilterOrderPriority = navInfo.FilterOrderPriority,
            UseOrderPriorityFilter = navInfo.UseOrderPriorityFilter,

            UseFinalStatusFilter = navInfo.UseFinalStatusFilter,
            FilterFinalStatus = navInfo.FilterFinalStatus,
            UseOrderProjectFilter = navInfo.UseOrderProjectFilter,
            FilterOrderProjectId = navInfo.FilterOrderProjectId,

            FilterOrderPayerId = navInfo.FilterOrderPayerId,
            UseOrderPayerFilter = navInfo.UseOrderPayerFilter,
            FilterOrderOrgFromId = navInfo.FilterOrderOrgFromId,
            UseOrderOrgFromFilter = navInfo.UseOrderOrgFromFilter,
            FilterOrderOrgToId = navInfo.FilterOrderOrgToId,
            UseOrderOrgToFilter = navInfo.UseOrderOrgToFilter                                         
            };
            // var carList = context.getFactCars(Id);

            model.UseFactShipperFilter = navInfo.UseFactShipperFilter;
            model.FilterFactShipperBeg = string.IsNullOrEmpty(navInfo.FilterFactShipperBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterFactShipperBeg;
            model.FilterFactShipperBegRaw = string.IsNullOrEmpty(navInfo.FilterFactShipperBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterFactShipperBegRaw;
            model.FilterFactShipperEnd = string.IsNullOrEmpty(navInfo.FilterFactShipperEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterFactShipperEnd;
            model.FilterFactShipperEndRaw = string.IsNullOrEmpty(navInfo.FilterFactShipperEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterFactShipperEndRaw;

            model.UseFactConsigneeFilter = navInfo.UseFactConsigneeFilter;
            model.FilterFactConsigneeBeg = string.IsNullOrEmpty(navInfo.FilterFactConsigneeBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterFactConsigneeBeg;
            model.FilterFactConsigneeBegRaw = string.IsNullOrEmpty(navInfo.FilterFactConsigneeBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterFactConsigneeBegRaw;
            model.FilterFactConsigneeEnd = string.IsNullOrEmpty(navInfo.FilterFactConsigneeEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterFactConsigneeEnd;
            model.FilterFactConsigneeEndRaw = string.IsNullOrEmpty(navInfo.FilterFactConsigneeEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterFactConsigneeEndRaw;

            model.UseOrderExDateFilter = navInfo.UseOrderExDateFilter;
            model.FilterOrderExDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderExDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderExDateBeg;
            model.FilterOrderExDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderExDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderExDateBegRaw;
            model.FilterOrderExDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderExDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderExDateEnd;
            model.FilterOrderExDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderExDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderExDateEndRaw;

            model.UseOrderEndDateFilter = navInfo.UseOrderEndDateFilter;
            model.FilterOrderEndDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderEndDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderEndDateBeg;
            model.FilterOrderEndDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderEndDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderEndDateBegRaw;
            model.FilterOrderEndDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderEndDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderEndDateEnd;
            model.FilterOrderEndDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderEndDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderEndDateEndRaw;



            model.UseOrderDateFilter = navInfo.UseOrderDateFilter;
            model.FilterOrderDateBeg = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateBeg;
            model.FilterOrderDateBegRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateBeg) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(-7)) : navInfo.FilterOrderDateBegRaw;
            model.FilterOrderDateEnd = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTime.Now.AddDays(1).ToString("dd.MM.yyyy") : navInfo.FilterOrderDateEnd;
            model.FilterOrderDateEndRaw = string.IsNullOrEmpty(navInfo.FilterOrderDateEnd) ? DateTimeConvertClass.getString(DateTime.Now.AddDays(1)) : navInfo.FilterOrderDateEndRaw;
            
            if (!string.IsNullOrEmpty(navInfo.FilterOrderIdFilter))
            {
                string[] idList = navInfo.FilterOrderIdFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterOrderIdName = "";
                    foreach (string i in idList)
                    {
                        if (FilterOrderIdName.Length > 0)
                        {
                            FilterOrderIdName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                            FilterOrderIdName = string.Concat(FilterOrderIdName, i);
                        else
                        {
                            model.UseOrderIdFilter = false;
                            break;
                        }
                    }
                    model.FilterOrderIdName = FilterOrderIdName;
                }
                else
                {
                    model.UseOrderIdFilter = false;
                }
            }

                 if (!string.IsNullOrEmpty(navInfo.FilterExpeditorIdFilter))
            {
                string[] idList = navInfo.FilterExpeditorIdFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterOrderExpeditorName = "";
                    foreach (string i in idList)
                    {
                        if (FilterOrderExpeditorName.Length > 0)
                        {
                            FilterOrderExpeditorName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)                            
                        FilterOrderExpeditorName = string.Concat(FilterOrderExpeditorName, context.getExpeditors(Convert.ToInt32(i))?.CarrierName);
                        else
                        {
                            model.UseExpeditorIdFilter = false;
                            break;
                        }
                    }
                    model.FilterOrderExpeditorName = FilterOrderExpeditorName;
                }
                else
                {
                    model.UseExpeditorIdFilter = false;
                }
            }

                         if (!string.IsNullOrEmpty(navInfo.FilterContractExpBkInfoFilter))
            {
                string[] idList = navInfo.FilterContractExpBkInfoFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterContractExpBkInfoName = "";
                    foreach (string i in idList)
                    {
                        if (FilterContractExpBkInfoName.Length > 0)
                        {
                            FilterContractExpBkInfoName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           var text =  context.getContracts(Convert.ToInt32(i))?.ContractNumber + " от " +  context.getContracts(Convert.ToInt32(i))?.ContractDate + " (с " +
                                    context.getContracts(Convert.ToInt32(i))?.DateBeg + " по " +  context.getContracts(Convert.ToInt32(i))?.DateEnd + " ) ";
                            FilterContractExpBkInfoName = string.Concat(FilterContractExpBkInfoName, text);
                        }
                        else
                        {
                            model.UseContractExpBkInfoFilter = false;
                            break;
                        }
                    }
                    model.FilterContractExpBkInfoName = FilterContractExpBkInfoName;
                }
                else
                {
                    model.UseContractExpBkInfoFilter = false;
                }
            }

           if (!string.IsNullOrEmpty(navInfo.FilterCarrierInfoFilter))
            {
                string[] idList = navInfo.FilterCarrierInfoFilter.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterCarrierInfoName = "";
                    foreach (string i in idList)
                    {
                        if (FilterCarrierInfoName.Length > 0)
                        {
                            FilterCarrierInfoName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                        {
                           
                             FilterCarrierInfoName = string.Concat(FilterCarrierInfoName, context.getUsedCarInfo(Convert.ToInt32(i))?.CarrierInfo);
                        }
                        else
                        {
                            model.UseCarrierInfoFilter = false;
                            break;
                        }
                    }
                    model.FilterCarrierInfoName = FilterCarrierInfoName;
                }
                else
                {
                    model.UseCarrierInfoFilter = false;
                }
            }

            model.FilterCarModelInfoName = _FilterCarModelInfoName;
            model.UseCarModelInfoFilter = _UseCarModelInfoFilter;

            model.FilterCarRegNumName = _FilterCarRegNumName;
            model.UseCarRegNumFilter = _UseCarRegNumFilter;

            model.FilterCarCapacityName = _FilterCarCapacityName;
            model.UseCarCapacityFilter = _UseCarCapacityFilter;
                                               
            model.FilterCarDriverInfoName = _FilterCarDriverInfoName;
            model.UseCarDriverInfoFilter = _UseCarDriverInfoFilter;

            model.FilterDriverContactInfoName = _FilterDriverContactInfoName;
            model.UseDriverContactInfoFilter = _UseDriverContactInfoFilter;

            model.FilterDriverCardInfoName = _FilterDriverCardInfoName;
            model.UseDriverCardInfoFilter = _UseDriverCardInfoFilter;

            model.FilterCommentsName = _FilterCommentsName;
            model.UseCommentsFilter = _UseCommentsFilter; 

            model.FilterContractInfoName = _FilterContractInfoName;
            model.UseContractInfoFilter = _UseContractInfoFilter; 

              model.FilterCarrierInfoName = _FilterCarrierInfoName;
            model.UseCarrierInfoFilter = _UseCarrierInfoFilter; 
            

                if ((!model.AcceptDate) && (!model.ExecuteDate))
            {
                model.DriftDate = true;
                model.AcceptDate = true;
                model.ExecuteDate = true;

            }
                 if (!string.IsNullOrEmpty(navInfo.FilterStatusId))
            {
                string[] idList = navInfo.FilterStatusId.Split(new char[] { ',' });
                if (idList.Length > 0)
                {
                    string FilterStatusName = "";
                    foreach (string i in idList)
                    {
                        if (FilterStatusName.Length > 0)
                        {
                            FilterStatusName += ",";
                        }
                        if (Convert.ToInt32(i) > 0)
                            FilterStatusName = string.Concat(FilterStatusName, context.getStatus(Convert.ToInt32(i))?.StatusName);
                        else
                        {
                            model.UseStatusFilter = false;
                            break;
                        }
                    }
                    model.FilterStatusName = FilterStatusName;
                }
                else
                {
                    model.UseStatusFilter = false;
                }
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderCreatorId))
            {
                string[] idList = navInfo.FilterOrderCreatorId.Split(new char[] { ',' });
                string FilterOrderCreatorName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderCreatorName.Length > 0)
                    {
                        FilterOrderCreatorName += ",";
                    }
                    FilterOrderCreatorName = string.Concat(FilterOrderCreatorName, context.getUser(i)?.displayName);
                }
                model.FilterOrderCreatorName = FilterOrderCreatorName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderProjectId))
            {
                string[] idList = navInfo.FilterOrderProjectId.Split(new char[] { ',' });
                string FilterOrderProjectCode = "";

                foreach (string i in idList)
                {
                    if (FilterOrderProjectCode.Length > 0)
                    {
                        FilterOrderProjectCode += ",";
                    }
                    FilterOrderProjectCode = string.Concat(FilterOrderProjectCode, context.GetProjectById(Convert.ToInt32(i))?.Code);
                }
                model.FilterOrderProjectCode = FilterOrderProjectCode;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderExecuterId))
            {
                string[] idList = navInfo.FilterOrderExecuterId.Split(new char[] { ',' });
                string FilterOrderExecuterName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderExecuterName.Length > 0)
                    {
                        FilterOrderExecuterName += ",";
                    }

                    FilterOrderExecuterName = string.Concat(FilterOrderExecuterName, context.getUser(i)?.displayName);
                }
                model.FilterOrderExecuterName = FilterOrderExecuterName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                string[] idList = navInfo.FilterOrderTypeId.Split(new char[] { ',' });
                string FilterOrderTypeName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderTypeName.Length > 0)
                    {
                        FilterOrderTypeName += ",";
                    }

                    FilterOrderTypeName = string.Concat(FilterOrderTypeName, context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }
                model.FilterOrderTypeName = FilterOrderTypeName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderClientId))
            {
                string[] idList = navInfo.FilterOrderClientId.Split(new char[] { ',' });
                string FilterOrderClientName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderClientName.Length > 0)
                    {
                        FilterOrderClientName += ",";
                    }

                    var client = context.getClient(Convert.ToInt32(i));
                    FilterOrderClientName = string.Concat(FilterOrderClientName, string.Concat(client?.ClientBalanceKeeperName, "/", client?.ClientName));
                }
                model.FilterOrderClientName = FilterOrderClientName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderPayerId))
            {
                string[] idList = navInfo.FilterOrderPayerId.Split(new char[] { ',' });
                string FilterOrderPayerName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderPayerName.Length > 0)
                    {
                        FilterOrderPayerName += ",";
                    }

                    var PayerName = context.getPayer(Convert.ToInt32(i));
                    FilterOrderPayerName = string.Concat(FilterOrderPayerName, string.Concat(PayerName?.BalanceKeeperName));
                }
                model.FilterOrderPayerName = FilterOrderPayerName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderOrgFromId))
            {
                string[] idList = navInfo.FilterOrderOrgFromId.Split(new char[] { ',' });
                string FilterOrderOrgFromName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderOrgFromName.Length > 0)
                    {
                        FilterOrderOrgFromName += ",";
                    }

                    var OrgFromName = context.GetOrganization(Convert.ToInt32(i));
                    FilterOrderOrgFromName = string.Concat(FilterOrderOrgFromName, string.Concat(OrgFromName?.Name));
                }
                model.FilterOrderOrgFromName = FilterOrderOrgFromName;
            }

            if (!string.IsNullOrEmpty(navInfo.FilterOrderOrgToId))
            {
                string[] idList = navInfo.FilterOrderOrgToId.Split(new char[] { ',' });
                string FilterOrderOrgToName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderOrgToName.Length > 0)
                    {
                        FilterOrderOrgToName += ",";
                    }

                    var OrgToName = context.GetOrganization(Convert.ToInt32(i));
                    FilterOrderOrgToName = string.Concat(FilterOrderOrgToName, string.Concat(OrgToName?.Name));
                }
                model.FilterOrderOrgToName = FilterOrderOrgToName;
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult GetOrderId(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderId(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrderIdCount(searchTerm);

            var pagedAttendees = OrderIdVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult OrderIdVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.OrderId.ToString(),
                    text = string.Concat(groupItem.OrderId.ToString())
                });
            }

            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateFactCars(int factCarsId)
        {
            var carList = context.getUsedCarInfo(factCarsId);
            carList.OrderListInfo = context.getCompetitiveListInfo(carList.OrderId);

            carList.PlanShipperDate = context.GetStartDate(carList.OrderListInfo.Id, carList.OrderListInfo.OrderType).ToString("dd.MM.yyyy");
            carList.PlanShipperTime = context.GetStartDate(carList.OrderListInfo.Id, carList.OrderListInfo.OrderType).ToString("HH:mm");
            carList.PlanConsigneeDate = context.GetFinishDate(carList.OrderListInfo.Id, carList.OrderListInfo.OrderType).ToString("dd.MM.yyyy");
            carList.PlanConsigneeTime = context.GetFinishDate(carList.OrderListInfo.Id, carList.OrderListInfo.OrderType).ToString("HH:mm");            

            return View(carList);
        }

        [HttpPost]
        public ActionResult UpdateFactCars(OrderUsedCarViewModel model)
        {
            context.UpdateFactCars(model);

            return RedirectToAction("FactCars", "Orders");
        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetExpeditorFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetExpeditorFilter(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetExpeditorFilterCount(searchTerm, Id);

            var pagedAttendees = GetExpeditorFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetExpeditorFilterVmToSelect2Format(IEnumerable<CarOwnersAccessViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.CarrierName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetCarModelInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetCarModelInfoFilter(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetCarModelInfoFilterCount(searchTerm, Id);

            var pagedAttendees = GetCarModelInfoFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetCarModelInfoFilterVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.CarModelInfo
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetCarRegNumFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetCarRegNumFilter(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetCarRegNumFilterCount(searchTerm, Id);

            var pagedAttendees = GetCarRegNumFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetCarRegNumFilterVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.CarRegNum
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetCarCapacityFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetCarCapacityFilter(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetCarCapacityFilterCount(searchTerm, Id);

            var pagedAttendees = GetCarCapacityFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetCarCapacityFilterVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.CarCapacity.ToString()
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetCarDriverInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetCarDriverInfoFilter(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetCarDriverInfoFilterCount(searchTerm, Id);

            var pagedAttendees = GetCarDriverInfoFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetCarDriverInfoFilterVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.CarDriverInfo
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetDriverCardInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetDriverCardInfoFilter(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetDriverCardInfoFilterCount(searchTerm, Id);

            var pagedAttendees = GetDriverCardInfoFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetDriverCardInfoFilterVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.DriverCardInfo
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


         [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetDriverContactInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetDriverContactInfoFilter(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetDriverContactInfoFilterCount(searchTerm, Id);

            var pagedAttendees = GetDriverContactInfoFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetDriverContactInfoFilterVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.DriverContactInfo
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetCommentsFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetCommentsFilter(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetCommentsFilterCount(searchTerm, Id);

            var pagedAttendees = GetCommentsFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetCommentsFilterVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.Comments
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


         [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetContractExpInfo(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetContractExpInfo(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetContractExpCount(searchTerm, Id);

            var pagedAttendees = GetContractInfoVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
           [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetCarrierInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetCarrierInfoFilter(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetCarrierInfoFilterCount(searchTerm, Id);

            var pagedAttendees = GetCarrierFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

            private static Select2PagedResult GetCarrierFilterVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.CarrierInfo
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

         [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetContractExpBkInfo2(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetContractExpBkInfo2(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetContractExpBkInfoPECount2(searchTerm, Id);

            var pagedAttendees = GetContractsFilterVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
         

        private static Select2PagedResult GetContractsFilterVmToSelect2Format(IEnumerable<OrderUsedCarViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.ContractInfo
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }
        
        [HttpGet]
        public ActionResult GetOrderProjects(long orderId, string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrderProjects(orderId, searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrderProjectsCount(orderId, searchTerm);

            var pagedAttendees = OrderProjectsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

         private static Select2PagedResult OrderProjectsVmToSelect2Format(IEnumerable<ProjectTypeViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Name,//Id.ToString(),
                    text = string.Concat(groupItem.Name)
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult TruckReport(OrdersNavigationInfo navInfo)
        {
               //обрабатываем данные фильтров
            if (string.IsNullOrEmpty(navInfo.FilterOrderTypeId)) { navInfo.UseOrderTypeFilter = false; }          
           
            if (navInfo.FilterOrderDate == null)
            {
                navInfo.FilterOrderDate = DateTime.Now.ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateRaw = DateTimeConvertClass.getString(DateTime.Now);
            }                     
          

            string FilterOrderTypeNames = "";
            if (!string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                string[] idList = navInfo.FilterOrderTypeId.Split(new char[] { ',' });
                foreach (string i in idList)
                {
                    if (FilterOrderTypeNames.Length > 0)
                    {
                        FilterOrderTypeNames += ",";
                    }

                    FilterOrderTypeNames = string.Concat(FilterOrderTypeNames,
                    context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }
            }

            String IdTree = Guid.NewGuid().ToString();  

            List<TruckViewModel> TruckInfo = null;
            var TruckTree = context.getTruckReport(
                userId,
                this.isAdmin,
                navInfo.UseOrderTypeFilter,
                navInfo.FilterOrderTypeId,
                string.IsNullOrEmpty(navInfo.FilterOrderDateRaw)
                    ? DateTime.Now
                    : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateRaw),
                navInfo.UseOrderDateFilter, 
                IdTree,
                ref TruckInfo);

            var modelTruckReport = new OrdersNavigationResult<TruckViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = TruckTree, // только дерево
                DataDisplayValues = TruckInfo.AsQueryable(), //все данные
                AvailiableTypes =
                    context.getAvailableOrderTypes(this.isAdmin ? null : this.userId, true)
                        .Where(x => x.IsActive == true).OrderBy(o => o.Id).ToList(),
                context = context,
              
                FilterOrderTypeId = navInfo.FilterOrderTypeId,
                FilterOrderTypeName = FilterOrderTypeNames,
                UseOrderTypeFilter = navInfo.UseOrderTypeFilter,  
                IdTree = IdTree                   
            };
                     
            var model = new OrdersReportsNavigationResult
            {
                JSONData = JsonConvert.SerializeObject(modelTruckReport.DisplayValues),
                TruckReport = modelTruckReport              
            };                       

            Session[IdTree] = modelTruckReport;

            //получаем типы заявок
           // var orderTypes = model.BaseReport.DisplayValues.Select(o => o.OrderType).Distinct().ToList();                    
          
            model.FilterOrderDate = string.IsNullOrEmpty(navInfo.FilterOrderDate) ? DateTime.Now.ToString("dd.MM.yyyy") : navInfo.FilterOrderDate;
            model.FilterOrderDateRaw = string.IsNullOrEmpty(navInfo.FilterOrderDate) ? DateTimeConvertClass.getString(DateTime.Now) : navInfo.FilterOrderDateRaw;
           
            model.FilterOrderTypeId = navInfo.FilterOrderTypeId;
            model.FilterOrderTypeName = FilterOrderTypeNames;
            model.UseOrderTypeFilter = navInfo.UseOrderTypeFilter;
            
            if (!string.IsNullOrEmpty(navInfo.FilterOrderTypeId))
            {
                string[] idList = navInfo.FilterOrderTypeId.Split(new char[] { ',' });
                string FilterOrderTypeName = "";

                foreach (string i in idList)
                {
                    if (FilterOrderTypeName.Length > 0)
                    {
                        FilterOrderTypeName += ",";
                    }

                    FilterOrderTypeName = string.Concat(FilterOrderTypeName, context.getOrderType(Convert.ToInt32(i))?.TypeName);
                }

                model.FilterOrderTypeName = FilterOrderTypeName;
            }
            model.UseOrderTypeFilter = navInfo.UseOrderTypeFilter;

            return View(model);
           
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult TruckReportDetails(/*int? OrgId,*/ DateTime? ReportDate, bool UseOrderTypeFilter, int? FilterOrderTypeId, string IdTree, int? IdGroup, string Id)
        {            
            OrdersNavigationInfo navInfo = new OrdersNavigationInfo();
            String Address = "", OrgName = "";
            var modelTruckReport2 = (Session[IdTree] as OrdersNavigationResult<TruckViewModel>);
            var modelTruckReport3 = new OrdersNavigationResult<TruckViewModel>(navInfo, userId);
            if (modelTruckReport2 != null)
            {
                var TruckInfo = modelTruckReport2.DataDisplayValues.ToList();
                modelTruckReport3 = new OrdersNavigationResult<TruckViewModel>(navInfo, userId)
                {                   
                    DisplayValues = context.getTruckReportDetails2(TruckInfo,
                    IdGroup ?? 0,
                    Id)
                };
                OrgName = context.getTruckReportTitle(TruckInfo,
               IdGroup ?? 0, Id, ref Address);

                 var model = new OrdersReportsNavigationResult
            {
                TruckReportDetail = modelTruckReport3,
                OrgName = OrgName,
                OrgId = 0,
                ReportDate = ReportDate ?? DateTime.Now,
                Address = Address,
                IdGroup = IdGroup ?? 0,
                Id = Id,
                IdTree = IdTree
            };
            return View(model);
            }
            else
            {
                navInfo.UseOrderTypeFilter = UseOrderTypeFilter;
                navInfo.FilterOrderTypeId = FilterOrderTypeId.ToString();
                navInfo.FilterOrderDateRaw = ReportDate.Value.ToString("dd.MM.yyyy");

                navInfo.UseOrderDateFilter = true;
                return RedirectToAction( "TruckReport", new RouteValueDictionary( 
    new { controller = "Orders", action = "TruckReport", UseOrderTypeFilter =  navInfo.UseOrderTypeFilter,
        FilterOrderTypeId =  navInfo.FilterOrderTypeId, FilterOrderDateRaw =  navInfo.FilterOrderDateRaw,
        UseOrderDateFilter = navInfo.UseOrderDateFilter} ) );
               // return RedirectToAction("TruckReport", "Orders", new { navInfo = navInfo });
            }

           


         /*    var modelTruckReport = new OrdersNavigationResult<TruckReportViewModel>(navInfo, userId)
            {
                isTransport = true,
                DisplayValues = context.getTruckReportDetails(
                    userId,
                    this.isAdmin, 
                    OrgId ?? 0,
                    ReportDate ??  DateTime.Now,
                    IdGroup ?? 0, 
                    Id), 
                             
            };
            var Address = "";
            var TruckReport = modelTruckReport.DisplayValues.FirstOrDefault();        
            if (TruckReport != null)  
             Address = TruckReport.isShipper
                    ? context.GetShipperAddress(TruckReport.OrderId)
                    : context.GetConsigneeAddress(TruckReport.OrderId);
            
    */


             //carList);
        }
    }
}