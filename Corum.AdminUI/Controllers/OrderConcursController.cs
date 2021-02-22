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

namespace CorumAdminUI.Controllers
{

    [Authorize]
    public partial class OrderConcursController : CorumBaseController
    {
        static long OrderID;
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrderCompetitiveList(OrderNavigationInfo navInfo)
        {
            OrderID = navInfo.OrderId;
            var model = new OrderNavigationResult<OrderCompetitiveListViewModel>(navInfo, userId)
            {
                DisplayValues = context.getOrderCompetitiveList(userId, navInfo.OrderId),
                CompetitiveListInfo = context.getCompetitiveListInfo(navInfo.OrderId),
                listStatuses = context.getAvialiableStepsForList(navInfo.OrderId),
                orderInfo = context.getOrder(navInfo.OrderId),
                currentStatus = context.getCurrentStatusForList(navInfo.OrderId),
                tenderServices = context.GetTenderServices()
            };

            return View(model);
        }



        [HttpPost]
        public ActionResult SendNotificationTender(string tenderName, string authorId, string companyId, string subCompanyId,
            string services, string depId, string budget, string ture, string typePublication, string dateCreate, string dateClose, string regume,
            string lotName, string nmcName, string unitName, int qty)
        {
            IList listParamsFormTender = new string[] {
                tenderName,
                authorId,
                companyId,
                subCompanyId,
                services,
                depId,
                budget,
                ture,
                typePublication,
                dateCreate,
                dateClose,
                regume,
                lotName,
                nmcName,
                unitName,
                qty.ToString() };
            
            PostRequest(listParamsFormTender);
            return RedirectToAction("OrderCompetitiveList", "OrderConcurs", new { OrderId = OrderID });
        }

        void PostRequest(IList listParamsFormTender)
        {
            Random random = new Random();
            Dictionary<TenderServices, int> tender = new Dictionary<TenderServices, int>();
            var query = context.GetTenderServices().Find(m => m.industryId == int.Parse(listParamsFormTender[4].ToString()));

            TenderForma postValues = new TenderForma();
            postValues.data = new DataTender()
            {
                tenderName = listParamsFormTender[0].ToString(),
                industryId = query.industryId,

                budget = Convert.ToDouble(listParamsFormTender[6]),
                tenderAuthorId = 38,
                companyId = 8,
                subCompanyId = 8,
                depId = 401,
                tenderExternalN = "45-" + random.Next(1, 10000).ToString(),
                mode = 2,
                kind = 2,
                dateStart = listParamsFormTender[9].ToString(),
                dateEnd = listParamsFormTender[10].ToString(),
                lots = new List<Lots>()
                    {
                        {
                            new Lots()
                            {
                                lotName = listParamsFormTender[12].ToString(),

                                items = new List<Items>()
                                {
                                        new Items()
                                        {
                                            nmcId = 783,
                                            itemName = listParamsFormTender[13].ToString(),
                                            qty = Convert.ToDouble(listParamsFormTender[15]),
                                            itemExternalN = "566532-23566",

                                            detailId = 4
                                        }
                                }
                            }
                        }
                    },
                lightMode = 0
            };

            BaseClient clientbase = new BaseClient("https://tender.corum.com/test/rest/rest.dll/tender/append", "supervisor", "4AA4DAEB367ADC060FCFAFECCF7F4506");
            string content = $"\nMessage: { new PostApiTender().GetCallAsync(clientbase, postValues).Result.ResponseMessage}";
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetSpecifications(long OrderId, bool UseTripTypeFilter, string FilterTripTypeId,
            bool UseSpecificationTypeFilter, string FilterSpecificationTypeId,
            bool UseVehicleTypeFilter, string FilterVehicleTypeId, bool UsePayerFilter, string FilterPayerId, bool UseRouteFilter, int AlgorithmId)
        {
            var specificationList = context.GetSpecifications(null, 5, 1, OrderId, UseTripTypeFilter, FilterTripTypeId,
            UseSpecificationTypeFilter, FilterSpecificationTypeId,
            UseVehicleTypeFilter, FilterVehicleTypeId, UsePayerFilter, FilterPayerId, UseRouteFilter, AlgorithmId);
            return Json(specificationList, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SetListStatus(long OrderId, int StepId)
        {
            context.SaveListStatus(new CompetetiveListStepsInfoViewModel()
            {
                StepId = StepId,
                OrderId = OrderId,
                userId = this.userId
            });

            return RedirectToAction("OrderCompetitiveList", "OrderConcurs", new { OrderId = OrderId });
        }


        private static Select2PagedResult OrderSpecVmToSelect2Format(IEnumerable<SpecificationListViewModel> groupItems,
            int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.ExpeditorName

                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult AddSpecificationsInfo(SpecificationListViewModel model)
        {
            var result = context.NewSpecification(model, this.userId);
            return RedirectToAction("OrderCompetitiveList", "OrderConcurs", new { OrderId = model.OrderId });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteConcurs(long Id, long OrderId)
        {
            context.DeleteConcurs(Id);
            return RedirectToAction("OrderCompetitiveList", "OrderConcurs", new { OrderId = OrderId });
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateConcurs(long Id, long OrderId)
        {
            var concursInfo = context.getConcurs(Id);
            concursInfo.CompetitiveListInfo = context.getCompetitiveListInfo(OrderId);

            return View(concursInfo);
        }

        [HttpPost]
        public ActionResult UpdateConcurs(OrderCompetitiveListViewModel model)
        {
            context.UpdateConcurs(model);

            return RedirectToAction("OrderCompetitiveList", "OrderConcurs", new { OrderId = model.OrderId });
        }


        [HttpGet]
        public ActionResult GetSpecificationType(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetSpecificationTypes(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetSpecificationTypesCount(searchTerm);

            var pagedAttendees = SpecificationTypeVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult SpecificationTypeVmToSelect2Format(IEnumerable<SpecificationTypesViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.SpecificationType
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetListTimeLine(OrderNavigationInfo navInfo)
        {
            var model = new OrderNavigationResult<CompetetiveListStepsInfoViewModel>(navInfo, userId)
            {
                DisplayValues = context.getTimeLineForList(navInfo.OrderId),
                CompetitiveListInfo = context.getCompetitiveListInfo(navInfo.OrderId)
            };
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ConcursHistory(/*long Id, */ConcursNavigationInfo navInfo)
        {
            DateTime OrderDate = context.getConcursHistoryHeader(navInfo.OrderId);
            if (navInfo.FilterOrderDateBeg == null)
            {
                navInfo.FilterOrderDateBeg = OrderDate.AddMonths(-3).ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateBegRaw = DateTimeConvertClass.getString(OrderDate.AddMonths(-3));
            }
            if (navInfo.FilterOrderDateEnd == null)
            {
                navInfo.FilterOrderDateEnd = OrderDate.ToString("dd.MM.yyyy");
                navInfo.FilterOrderDateEndRaw = DateTimeConvertClass.getString(OrderDate);
            }


            var model = new OrderNavigationResult<OrderCompetitiveListViewModel>(navInfo, userId)
            {
                DisplayValues = context.getConcursHistory(navInfo.Id, navInfo.ShowAll, navInfo.OrderId,
                string.IsNullOrEmpty(navInfo.FilterOrderDateBegRaw)
                        ? OrderDate.AddMonths(-3)
                        : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateBegRaw),
                string.IsNullOrEmpty(navInfo.FilterOrderDateEndRaw)
                        ? OrderDate
                        : DateTimeConvertClass.getDateTime(navInfo.FilterOrderDateEndRaw)),
                // CompetitiveListInfo = context.getCompetitiveListInfo(navInfo.OrderId)
                //headerInfo = context.getConcursHistoryHeader(navInfo.Id, navInfo.OrderId), 
                OrderId = navInfo.OrderId,
                Id = navInfo.Id,
                ShowAll = navInfo.ShowAll,
                FilterOrderDateBeg = navInfo.FilterOrderDateBeg,
                FilterOrderDateBegRaw = navInfo.FilterOrderDateBegRaw,
                FilterOrderDateEnd = navInfo.FilterOrderDateEnd,
                FilterOrderDateEndRaw = navInfo.FilterOrderDateEndRaw,
            };
            return View(model);
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ConcursDiscountRate(OrderNavigationInfo navInfo)
        {
            var model = new OrderNavigationResult<ConcursDiscountRateModel>(navInfo, userId)
            {
                DisplayValues = context.GetConcursDiscountRate(),
                OrderId = navInfo.OrderId
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult ConcursTenderQuery(OrderNavigationInfo navInfo)
        {
            var model = new OrderNavigationResult<ConcursDiscountRateModel>(navInfo, userId)
            {
                DisplayValues = context.GetConcursDiscountRate(),
                OrderId = navInfo.OrderId
            };
            return View(model);
        }


        public string StatusSendToTenderQuery()
        {
            return "<h2>Заявка отправлена!</h2>";
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateDiscountRate(long Id)
        {
            var discountRateInfo = context.GetConcursDiscountRate(Id);

            return View(discountRateInfo);
        }

        [HttpPost]
        public ActionResult UpdateDiscountRate(ConcursDiscountRateModel model)
        {
            context.UpdateDiscountRate(model);

            return RedirectToAction("ConcursDiscountRate", "OrderConcurs");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteDiscountRate(long Id)
        {
            context.DeleteDiscountRate(Id);
            return RedirectToAction("ConcursDiscountRate", "OrderConcurs");
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewDiscountRate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewDiscountRate(ConcursDiscountRateModel model)
        {
            context.AddDiscountRate(model);
            return RedirectToAction("ConcursDiscountRate", "OrderConcurs");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CloneConcurs(long Id, long OrderId)
        {
            context.CloneConcurs(Id, userId);

            return RedirectToAction("OrderCompetitiveList", "OrderConcurs", new { OrderId = OrderId });

        }

    }
}