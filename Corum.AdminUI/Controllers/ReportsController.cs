using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Corum.Models;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Bucket;
using CorumAdminUI.Common;



namespace CorumAdminUI.Controllers
{
    [Authorize]
    public class ReportsController : CorumBaseController
    {
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult BriefReport(RestNavigationInfo navInfo)
        {
            SnapshotInfoViewModel maxAvialiableSnapshot = null;

            maxAvialiableSnapshot = navInfo.snapshotId > 0 ?
                context.GetScreenShotById(navInfo.snapshotId) : context.GetMaxScreenShot();

            var model = new RestsNavigationResult<BriefViewModel>(navInfo, userId)
            {
                DisplayValues = context.GetBriefDataByScreenShotId(maxAvialiableSnapshot.Id,
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

            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult RestReport(RestNavigationInfo navInfo)
        {
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
                    FilterOrderProjectId = navInfo.FilterOrderProjectId,
                    FilterProductBarcodeId = navInfo.FilterProductBarcodeId,

                    UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                    UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                    UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                    UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                    UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                    UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter,
                    UseOrderProjectFilter = string.IsNullOrEmpty(navInfo.FilterOrderProjectId) ? false : navInfo.UseOrderProjectFilter,
                    UseProductBarcodeFilter = string.IsNullOrEmpty(navInfo.FilterProductBarcodeId) ? false : navInfo.UseProductBarcodeFilter,
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
                FilterOrderProjectId = navInfo.FilterOrderProjectId,
                FilterProductBarcodeId = navInfo.FilterProductBarcodeId,

                UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter,
                UseOrderProjectFilter = string.IsNullOrEmpty(navInfo.FilterOrderProjectId) ? false : navInfo.UseOrderProjectFilter,
                UseProductBarcodeFilter = string.IsNullOrEmpty(navInfo.FilterProductBarcodeId) ? false : navInfo.UseProductBarcodeFilter,
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


            if (model.DisplayTotalValues == null)
            {
                model.DisplayTotalValues = new RestViewModel()
                {
                    QuantityAfter = 0,
                    Weight = 0,
                    PE_After = 0,
                    PF_After = 0,
                    PCP_After = 0,
                    PCPC_After = 0,
                    FCP_After = 0,
                    FCPC_After = 0,
                    BP_After = 0
                };
            }

            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult BucketDocuments(NavigationInfo navInfo)
        {
            var model = new BucketDocsDocsNavigationResult<BucketDocument>(navInfo, userId)
            {
                DisplayValues = context.GetBucketDocuments()
            };

            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ViewBucketDocument(long Id)
        {
            var doc = context.GetBucketDocument(Id);

            if (doc.Items != null)
            {
                var items = doc.Items.ToList();
                var order = 1;
                foreach(var item in items)
                {
                    item.OrderNum = order++;
                }

                doc.Items = items.AsQueryable();
            }

            return View(doc);
        }



        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GroupRestReport(RestNavigationInfo navInfo)
        {
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
                    FilterOrderProjectId = navInfo.FilterOrderProjectId,

                    UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                    UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                    UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                    UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                    UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                    UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter,
                    UseOrderProjectFilter = string.IsNullOrEmpty(navInfo.FilterOrderProjectId) ? false : navInfo.UseOrderProjectFilter
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
                FilterOrderProjectId = navInfo.FilterOrderProjectId,

                UseStorageFilter = string.IsNullOrEmpty(navInfo.FilterStorageId) ? false : navInfo.UseStorageFilter,
                UseCenterFilter = string.IsNullOrEmpty(navInfo.FilterCenterId) ? false : navInfo.UseCenterFilter,
                UseRecieverPlanFilter = string.IsNullOrEmpty(navInfo.FilterRecieverPlanId) ? false : navInfo.UseRecieverPlanFilter,
                UseRecieverFactFilter = string.IsNullOrEmpty(navInfo.FilterRecieverFactId) ? false : navInfo.UseRecieverFactFilter,
                UseKeeperFilter = string.IsNullOrEmpty(navInfo.FilterKeeperId) ? false : navInfo.UseKeeperFilter,
                UseProducerFilter = string.IsNullOrEmpty(navInfo.FilterProducerId) ? false : navInfo.UseProducerFilter,
                UseOrderProjectFilter = string.IsNullOrEmpty(navInfo.FilterOrderProjectId) ? false : navInfo.UseOrderProjectFilter,

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


            if (model.DisplayTotalValues == null)
            {
                model.DisplayTotalValues = new GroupItemRestViewModel()
                {
                    Position = int.MaxValue,
                    groupItem = "ВСЕГО:",
                    QuantityBefore = 0,
                    QuantityAfter = 0,
                    WeightBefore = 0,
                    WeightAfter = 0,
                    PE_Before = 0,
                    PF_Before = 0,
                    PCP_Before = 0,
                    PCPC_Before = 0,
                    FCP_Before = 0,
                    FCPC_Before = 0,
                    BP_Before = 0,
                    PE_After = 0,
                    PF_After = 0,
                    PCP_After = 0,
                    PCPC_After = 0,
                    FCP_After = 0,
                    FCPC_After = 0,
                    BP_After = 0,

                    QuantityPrihod = 0,
                    MassPrihod = 0,
                    PE_Prihod = 0,
                    PF_Prihod = 0,
                    PCP_Prihod = 0,
                    PCPC_Prihod = 0,
                    FCP_Prihod = 0,
                    FCPC_Prihod = 0,
                    BP_Prihod = 0,

                    QuantityRashod = 0,
                    MassRashod = 0,
                    PE_Rashod = 0,
                    PF_Rashod = 0,
                    PCP_Rashod = 0,
                    PCPC_Rashod = 0,
                    FCP_Rashod = 0,
                    FCPC_Rashod = 0,
                    BP_Rashod = 0
                };
            }




            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DocsReport(RestNavigationInfo navInfo)
        {
            SnapshotInfoViewModel maxAvialiableSnapshot = null;

            maxAvialiableSnapshot = navInfo.snapshotId > 0 ?
                context.GetScreenShotById(navInfo.snapshotId) : context.GetMaxScreenShot();

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


            if (model.DisplayTotalValues == null)
            {
                model.DisplayTotalValues = new DocViewModel()
                {
                    Quantity = 0,
                    Weight = 0,
                    PE = 0,
                    PF = 0,
                    PCP = 0,
                    PCPC = 0,
                    FCP = 0,
                    FCPC = 0,
                    BP = 0
                };
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult GetStorages(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetStorages(snapShot, searchTerm, pageSize, pageNum);
            var storagesCount = context.GetStoragesCount(snapShot, searchTerm);

            var pagedAttendees = StoragesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetCenters(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetCenters(snapShot, searchTerm, pageSize, pageNum);
            var storagesCount = context.GetCentersCount(snapShot, searchTerm);

            var pagedAttendees = StoragesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetRecievers(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetRecievers(snapShot, searchTerm, pageSize, pageNum);
            var storagesCount = context.GetRecieversCount(snapShot, searchTerm);

            var pagedAttendees = StoragesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetKeepers(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetBalanceKeepers(snapShot, searchTerm, pageSize, pageNum);
            var storagesCount = context.GetBalanceKeepersCount(snapShot, searchTerm);

            var pagedAttendees = StoragesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetProducers(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetProducers(snapShot, searchTerm, pageSize, pageNum);
            var storagesCount = context.GetProducersCount(snapShot, searchTerm);

            var pagedAttendees = StoragesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult SaveBucketDocument(IEnumerable<BucketItem> items)
        {
            if (items == null) return Json("Success");
            if (!items.Any()) return Json("Success");

            var id = context.SaveBucketDocument(items, userId);

            if (id > 0)
                return Json(id.ToString());
            else return Json("Error");
        }

        private static Select2PagedResult StoragesVmToSelect2Format(IEnumerable<GroupItemViewModel> groupItems, int totalStorages)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Name,
                    text = groupItem.Name
                });
            }
            jsonGroupItems.Total = totalStorages;
            return jsonGroupItems;
        }

        [HttpGet]
        public ActionResult PartyCard(string InnerPartyKey, int? snapshoId)
        {
            if (!snapshoId.HasValue)
            {
                snapshoId = context.GetDefaultSnapshotId();
            }

            var result = new PartyCardViewModel()
            {
                restRow = context.GetRestById(InnerPartyKey, snapshoId.Value),
                docs = context.GetDocsById(InnerPartyKey, snapshoId.Value)
            };

            return View(result);
        }

        [HttpGet]
        public ActionResult GetProjects(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetProjects(snapShot, searchTerm, pageSize, pageNum);
            var storagesCount = context.GetProjectsCount(snapShot, searchTerm);

            var pagedAttendees = ProjectsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetBarcodes(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetBarcodes(snapShot, searchTerm, pageSize, pageNum);
            var storagesCount = context.GetBarcodesCount(snapShot, searchTerm);

            var pagedAttendees = ProductBarcodesVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult ProductBarcodesVmToSelect2Format(IEnumerable<RestViewModel> groupItems, int totalStorages)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.BacodesAll,
                    text = groupItem.BacodesAll
                });
            }
            jsonGroupItems.Total = totalStorages;
            return jsonGroupItems;
        }

        private static Select2PagedResult ProjectsVmToSelect2Format(IEnumerable<RestViewModel> groupItems, int totalStorages)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.InnerOrderNum,
                    text = groupItem.InnerOrderNum
                });
            }
            jsonGroupItems.Total = totalStorages;
            return jsonGroupItems;
        }
    }
}