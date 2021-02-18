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

namespace CorumAdminUI.Controllers
{

    [Authorize]
    public partial class OrderConcursController : CorumBaseController
    {
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrderCompetitiveList(OrderNavigationInfo navInfo)
        {
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
        public ActionResult SendNotificationTender(string services, int cars)
        {
            Dictionary<TenderServices, int> tender = new Dictionary<TenderServices, int>();
            var query = context.GetTenderServices().Find(m => m.Id == int.Parse(services));

            return PostRequest(query, cars);
        }

        private ContentResult PostRequest(TenderServices services, int cars)
        {
            Random random = new Random();
           
                TenderForma postValues = new TenderForma();
                postValues.data = new DataTender()
                {
                    tenderName = "Услуги перевозки",
                    industryId = services.industryId,

                    budget = 123.89,
                    tenderAuthorId = 38,
                    companyId = 8,
                    subCompanyId = 8,
                    depId = 401,
                    tenderExternalN = "45-" + random.Next(1, 10000).ToString(),
                    //tenderHead = "Петров В.Н.",
                    mode = 2,
                    kind = 2,
                    dateStart = "2021-02-23T20:45:00",
                    dateEnd = "2021-02-28T17:45:00",
                    lots = new List<Lots>()
                    {
                        {
                            new Lots()
                            {
                                lotName = "Лот №1",
                                //props = new List<Props>()
                                //{

                                //        new Props()
                                //        {
                                //        properties = "Цвет"
                                //        },

                                //        new Props()
                                //        {
                                //        properties = "Марка"
                                //        }
                                //},
                                items = new List<Items>()
                                {
                                        new Items()
                                        {
                                            nmcId = 782,
                                            itemName = services.industryName.ToString(),
                                            qty = cars,
                                            itemNote = "Машина с комфортом",
                                            itemExternalN = "566532-23566",
                                            //propValues = new List<PropValues>()
                                            //{
                                            //        new PropValues()
                                            //        {
                                            //             propValItems = "февраль-март"
                                            //        }
                                            //},
                                            detailId = 4
                                        },

                                        new Items()
                                        {
                                            nmcId = 782,
                                            itemName = services.industryName.ToString(),
                                            qty = cars,
                                            itemNote = "Машина с без комфорта",
                                            itemExternalN = "566532-24555",
                                            //propValues = new List<PropValues>()
                                            //{
                                            //        new PropValues()
                                            //        {
                                            //             propValItems = "февраль-март"
                                            //        }
                                            //},
                                            detailId = 3
                                        }
                                }
                            }
                        }
                    },
                    lightMode = 0
                };
            
            BaseClient clientbase = new BaseClient("https://tender.corum.com/test/rest/rest.dll/tender/append", "supervisor", "4AA4DAEB367ADC060FCFAFECCF7F4506");
            return Content($"\nMessage: { new PostApiTender().GetCallAsync(clientbase, postValues).Result.ResponseMessage}");          
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