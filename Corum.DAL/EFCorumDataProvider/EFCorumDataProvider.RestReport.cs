using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Corum.DAL.Entity;
using Corum.Models;
using Corum.DAL.Mappings;
using Corum.Models.ViewModels;

namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {
        public IQueryable<BriefViewModel> GetBriefDataByScreenShotId(int screenShotId, GroupItemFilters filters)
        {
            return db.GetBriefDataBySnapshot(screenShotId,
                      filters.FilterStorageId,
                      filters.FilterCenterId,
                      filters.FilterRecieverPlanId,
                      filters.FilterRecieverFactId,
                      filters.FilterKeeperId,
                      filters.FilterProducerId,
                      Convert.ToInt32(filters.UseStorageFilter),
                      Convert.ToInt32(filters.UseCenterFilter),
                      Convert.ToInt32(filters.UseRecieverPlanFilter),
                      Convert.ToInt32(filters.UseRecieverFactFilter),
                      Convert.ToInt32(filters.UseKeeperFilter),
                      Convert.ToInt32(filters.UseProducerFilter))
                        .Select(s => new BriefViewModel
                        {
                            currentDate     = s.currentDate.Value,
                            Quantity_OnDate = s.Quantity_OnDate.Value,
                            Mass_OnDate     = s.Mass_OnDate.Value/1000,
                            PE_OnDate       = s.PE_OnDate.Value,
                            PF_OnDate       = s.PF_OnDate.Value,
                            PCP_OnDate      = s.PCP_OnDate.Value,
                            PCPC_OnDate     = s.PCPC_OnDate.Value,
                            FCP_OnDate      = s.FCP_OnDate.Value,
                            FCPC_OnDate     = s.FCPC_OnDate.Value,
                            BP_OnDate       = s.BP_OnDate.Value,

                            ShipOnDate_Quantity = s.ShipOnDate_Quantity.Value,
                            ShipOnDate_Mass = s.ShipOnDate_Mass.Value/1000,
                            ShipOnDate_PE   = s.ShipOnDate_PE.Value,
                            ShipOnDate_PF   = s.ShipOnDate_PF.Value,
                            ShipOnDate_PCP  = s.ShipOnDate_PCP.Value,
                            ShipOnDate_PCPC = s.ShipOnDate_PCPC.Value,
                            ShipOnDate_FCP  = s.ShipOnDate_FCP.Value,
                            ShipOnDate_FCPC = s.ShipOnDate_FCPC.Value,
                            ShipOnDate_BP   = s.ShipOnDate_BP.Value,

                            ShipForPeriod_Quantity = s.ShipForPeriod_Quantity.Value,
                            ShipForPeriod_Mass = s.ShipForPeriod_Mass.Value/1000,
                            ShipForPeriod_PE   = s.ShipForPeriod_PE.Value,
                            ShipForPeriod_PF   = s.ShipForPeriod_PF.Value,
                            ShipForPeriod_PCP  = s.ShipForPeriod_PCP.Value,
                            ShipForPeriod_PCPC = s.ShipForPeriod_PCPC.Value,
                            ShipForPeriod_FCP  = s.ShipForPeriod_FCP.Value,
                            ShipForPeriod_FCPC = s.ShipForPeriod_FCPC.Value,
                            ShipForPeriod_BP   = s.ShipForPeriod_BP.Value,

                            ProdOnDate_Quantity = s.ProdOnDate_Quantity.Value,
                            ProdOnDate_Mass    = s.ProdOnDate_Mass.Value/1000,
                            ProdOnDate_PE      = s.ProdOnDate_PE.Value,
                            ProdOnDate_PF      = s.ProdOnDate_PF.Value,
                            ProdOnDate_PCP     = s.ProdOnDate_PCP.Value,
                            ProdOnDate_PCPC    = s.ProdOnDate_PCPC.Value,
                            ProdOnDate_FCP     = s.ProdOnDate_FCP.Value,
                            ProdOnDate_FCPC    = s.ProdOnDate_FCPC.Value,
                            ProdOnDate_BP      = s.ProdOnDate_BP.Value,

                            ProdByPeriod_Quantity  = s.ProdByPeriod_Quantity.Value,
                            ProdByPeriod_Mass  = s.ProdByPeriod_Mass.Value/1000,
                            ProdByPeriod_PE    = s.ProdByPeriod_PE.Value,
                            ProdByPeriod_PF    = s.ProdByPeriod_PF.Value,
                            ProdByPeriod_PCP   = s.ProdByPeriod_PCP.Value,
                            ProdByPeriod_PCPC  = s.ProdByPeriod_PCPC.Value,
                            ProdByPeriod_FCP   = s.ProdByPeriod_FCP.Value,
                            ProdByPeriod_FCPC  = s.ProdByPeriod_FCPC.Value,
                            ProdByPeriod_BP    = s.ProdByPeriod_BP.Value
                        }).OrderBy(o => o.currentDate).ToList().AsQueryable();

        }

        public IQueryable<SnapshotInfoViewModel> GetSnapshotLists()
        {
            return db.LogisticSnapshots
                      .AsNoTracking()
                       .Select(Mapper.Map)
                        .AsQueryable();
        }

        public SnapshotInfoViewModel GetMaxScreenShot()
        {
            var max = db.LogisticSnapshots
                        .Where(s=>s.isDefaultForReports==1)
                         .OrderByDescending(o => (o.shapshot_data))
                          .FirstOrDefault();
            if (max!=null) return Mapper.Map(max);

            return new SnapshotInfoViewModel()
            {
                Id = -1,
                IsDocsWereImported = false,
                IsRestsWereImported = false,
                DateOfImport = DateTime.Now,
                ActualDateBeg = DateTime.Now,
                ActualDateEnd = DateTime.Now
            };
        }

        public SnapshotInfoViewModel GetScreenShotById(int Id)
        {
            var max = db.LogisticSnapshots.FirstOrDefault(s => s.id_spanshot == Id);
            if (max != null) return Mapper.Map(max);

            return new SnapshotInfoViewModel()
            {
                Id = -1,
                IsDocsWereImported = false,
                IsRestsWereImported = false,
                DateOfImport = DateTime.Now,
                ActualDateBeg = DateTime.Now,
                ActualDateEnd = DateTime.Now
            };
        }

        public IQueryable<SnapshotInfoViewModel> GetScreenShots(DateTime actualDate)
        {
            return db.LogisticSnapshots
                      .AsNoTracking()
                       .Select(Mapper.Map)
                        .AsQueryable();
        }

        public List<DocViewModel> GetDocsById(string InnerPartyKey, int snapshotId)
        {
            return db.GetDocsDataByInnerPartyKey(snapshotId, InnerPartyKey)
                    .Select(s => new DocViewModel
                    {
                        idrow = s.idrow,
                        id_snapshot = s.id_snapshot,
                        InnerPartyKey = s.InnerPartyKey,
                        Producer = s.Producer,
                        Product = s.Product,
                        Shifr = s.Shifr,
                        Figure = s.Figure,
                        Measure = s.Measure,
                        Weight = s.Weight,
                        pType = s.pType,
                        pGroup = s.pGroup,
                        pRecieverPlan = s.pRecieverPlan,
                        pRecieverFact = s.pRecieverFact,
                        RecieverGroupPlan = s.RecieverGroupPlan,
                        InnerOrderNum = s.InnerOrderNum,
                        OrderedBy = s.OrderedBy,
                        OrderNum = s.OrderNum,
                        Quantity = s.Quantity ?? 0,
                        PE = s.PE ?? 0,
                        PF = s.PF ?? 0,
                        PCP = s.PCP ?? 0,
                        PCPC = s.PCPC ?? 0,
                        FCP = s.FCP ?? 0,
                        FCPC = s.FCPC ?? 0,
                        BP = s.BP ?? 0,
                        Storage = s.Storage,
                        StorageCity = s.StorageCity,
                        StorageCountry = s.StorageCountry,
                        Сenter = s.Сenter,
                        BalanceKeeper = s.BalanceKeeper,
                        ReadyForSaleStatus = s.ReadyForSaleStatus,
                        ReserveStatus = s.ReserveStatus,
                        ProduceDate = s.ProduceDate,
                        ReconcervationDate = s.ReconcervationDate,
                        TermOnStorage = s.TermOnStorage,
                        DocType = s.DocType,
                        DocNum = s.DocNum,
                        DocDate = s.DocDate,
                        BalanceCurrency = s.BalanceCurrency,
                        CurrencyIndexToUAH = s.CurrencyIndexToUAH,
                        ISPrihod = s.ISPrihod,
                        Mass = s.Quantity * s.Weight ?? 0
                    }).OrderBy(o => o.DocDate).ToList();
        }


        public RestViewModel GetRestById(string InnerPartyKey, int snapshotId)
        {
            return (from s in db.GetRestDataByInnerPartyKey(snapshotId, InnerPartyKey)
                       select new RestViewModel
                        {
                           InnerPartyKey = s.InnerPartyKey,
                           Producer = s.Producer,
                           Product = s.Product,
                           Shifr = s.Shifr,
                           Shifr_MDM =  s.Shifr_MDM,
                           BacodeProduct = s.BarcodeProduct,
                           BacodeConsignment = s.BacodeConsignment,
                           BacodesAll = s.BacodesAll,
                           Figure = s.Figure,
                           Measure = s.Measure,
                           Weight = s.Weight,
                           pType = s.pType,
                           pGroup = s.pGroup,
                           ProductFullName = string.Concat(s.Product, " (", s.Shifr, ")"),
                           pRecieverPlan = s.pRecieverPlan,
                           pRecieverFact = s.pRecieverFact,
                           RecieverGroupPlan = s.RecieverGroupPlan,
                           InnerOrderNum = s.InnerOrderNum,
                           OrderedBy = s.OrderedBy,
                           OrderNum = s.OrderNum,
                           QuantityBefore = s.QuantityBefore ?? 0,
                           MassBefore = s.Weight * s.QuantityBefore ?? 0,
                           PE_Before = s.PE_Before ?? 0,
                           PF_Before = s.PF_Before ?? 0,
                           PCPC_Before = s.PCPC_Before ?? 0,
                           FCP_Before = s.FCP_Before ?? 0,
                           FCPC_Before = s.FCPC_Before ?? 0,
                           BP_Before = s.BP_Before ?? 0,
                           QuantityAfter = s.QuantityAfter ?? 0,
                           MassAfter = s.Weight * s.QuantityAfter ?? 0,
                           PE_After = s.PE_After ?? 0,
                           PF_After = s.PF_After ?? 0,
                           PCPC_After = s.PCPC_After ?? 0,
                           FCP_After = s.FCP_After ?? 0,
                           FCPC_After = s.FCPC_After ?? 0,
                           BP_After = s.BP_After ?? 0,
                           Storage = s.Storage,
                           StorageCity = s.StorageCity,
                           StorageCountry = s.StorageCountry,
                           Сenter = s.Сenter,
                           BalanceKeeper = s.BalanceKeeper,
                           ReadyForSaleStatus = s.ReadyForSaleStatus,
                           ReserveStatus = s.ReserveStatus,
                           FullSellStatus = string.Concat(s.ReadyForSaleStatus, ",", s.ReserveStatus),
                           ProduceDate = s.ProduceDate,
                           ReconcervationDate = s.ReconcervationDate,
                           TermOnStorage = s.TermOnStorage,
                           PrihodDocType = s.PrihodDocType,
                           PrihodDocNum = s.PrihodDocNum,
                           PrihodDocDate = s.PrihodDocDate,
                           BalanceCurrency = s.BalanceCurrency,
                           CurrencyIndexToUAH = s.CurrencyIndexToUAH
                       }).FirstOrDefault();
        }

        public IQueryable<RestViewModel> GetRestsByScreenShotId(int screenShotId, GroupItemFilters filters)
        {
            return db.GetRestDataBySnapshot(screenShotId,
                      filters.FilterStorageId,
                      filters.FilterCenterId,
                      filters.FilterRecieverPlanId,
                      filters.FilterRecieverFactId,
                      filters.FilterKeeperId,
                      filters.FilterProducerId,
                      filters.FilterOrderProjectId,
                      filters.FilterProductBarcodeId,
                      Convert.ToInt32(filters.UseStorageFilter),
                      Convert.ToInt32(filters.UseCenterFilter),
                      Convert.ToInt32(filters.UseRecieverPlanFilter),
                      Convert.ToInt32(filters.UseRecieverFactFilter),
                      Convert.ToInt32(filters.UseKeeperFilter),
                      Convert.ToInt32(filters.UseProducerFilter),
                      Convert.ToInt32(filters.UseOrderProjectFilter),
                      Convert.ToInt32(filters.UseProductBarcodeFilter)
                      )
                        .Select(s => new RestViewModel
                        {
                            InnerPartyKey = s.InnerPartyKey,
                            Producer = s.Producer,
                            Product = s.Product,
                            Shifr = s.Shifr,
                            Figure = s.Figure,
                            Shifr_MDM =  s.Shifr_MDM,
                            BacodeProduct = s.BacodeProduct,
                            BacodeConsignment = s.BacodeConsignment,
                            Measure = s.Measure,
                            Weight = s.Weight,
                            pType = s.pType,
                            pGroup = s.pGroup,
                            ProductFullName = string.Concat(s.Product," (", s.Shifr,")"),
                            pRecieverPlan = s.pRecieverPlan,
                            pRecieverFact = s.pRecieverFact,
                            RecieverGroupPlan = s.RecieverGroupPlan,
                            InnerOrderNum = s.InnerOrderNum,
                            OrderedBy = s.OrderedBy,
                            OrderNum = s.OrderNum,
                            QuantityBefore = s.QuantityBefore ?? 0,
                            MassBefore = s.Weight* s.QuantityBefore ?? 0,
                            PE_Before = s.PE_Before ?? 0,
                            PF_Before = s.PF_Before ?? 0,
                            PCPC_Before = s.PCPC_Before ?? 0,
                            FCP_Before = s.FCP_Before ?? 0,
                            FCPC_Before = s.FCPC_Before ?? 0,
                            BP_Before = s.BP_Before ?? 0,
                            QuantityAfter = s.QuantityAfter ?? 0,
                            MassAfter = s.Weight * s.QuantityAfter ?? 0,
                            PE_After = s.PE_After ?? 0,
                            PF_After = s.PF_After ?? 0,
                            PCPC_After = s.PCPC_After ?? 0,
                            FCP_After = s.FCP_After ?? 0,
                            FCPC_After = s.FCPC_After ?? 0,
                            BP_After = s.BP_After ?? 0,
                            Storage = s.Storage,
                            StorageCity = s.StorageCity,
                            StorageCountry = s.StorageCountry,
                            Сenter = s.Сenter,
                            BalanceKeeper = s.BalanceKeeper,
                            ReadyForSaleStatus = s.ReadyForSaleStatus,
                            ReserveStatus = s.ReserveStatus,
                            FullSellStatus=string.Concat(s.ReadyForSaleStatus,",", s.ReserveStatus),
                            ProduceDate = s.ProduceDate,
                            ReconcervationDate = s.ReconcervationDate,
                            TermOnStorage = s.TermOnStorage,
                            PrihodDocType = s.PrihodDocType,
                            PrihodDocNum = s.PrihodDocNum,
                            PrihodDocDate = s.PrihodDocDate,
                            BalanceCurrency = s.BalanceCurrency,
                            CurrencyIndexToUAH = s.CurrencyIndexToUAH
                        }).OrderBy(o => o.idrow).ToList().AsQueryable();
        }

        public Expression<Func<TItem, object>> GroupByExpression<TItem>(string[] propertyNames)
        {
            var properties = propertyNames.Select(name => typeof(TItem).GetProperty(name)).ToArray();
            var propertyTypes = properties.Select(p => p.PropertyType).ToArray();
            var tupleTypeDefinition = typeof(Tuple).Assembly.GetType("System.Tuple`" + properties.Length);
            var tupleType = tupleTypeDefinition.MakeGenericType(propertyTypes);
            var constructor = tupleType.GetConstructor(propertyTypes);
            var param = Expression.Parameter(typeof(TItem), "item");
            var body = Expression.New(constructor, properties.Select(p => Expression.Property(param, p)));
            var expr = Expression.Lambda<Func<TItem, object>>(body, param);
            return expr;
        }

        public Dictionary<string, string> GetRestColumnsForGroupReport(string currentFieldName)
        {
            var fields = new Dictionary<string, string>
            {
                {"Storage", "Склад"},
                {"Сenter",  "ЦФО" },
                {"Product", "Номенклатура"},
                {"pRecieverPlan", "Грузополучатель <br> (плановый)"},
                {"pRecieverFact", "Грузополучатель <br> (фактический)"},
                {"pGroup", "Группа <br> товаров"},
                {"pType", "Тип <br> продукции"},
                {"BalanceKeeper", "Балансодержатель"},
                {"Producer", "Производитель"},
                {"Shifr", "Артикул <br> номенклатуры"},
                {"Figure", "Чертеж"},
                {"PrihodDocType", "Тип <br> документа"},
                {"ReadyForSaleStatus", "Статус <br> готовности <br> к продаже"},
                {"ReserveStatus", "Статус <br> резерва"},
                {"ProduceDate", "Дата <br> производства"},
                {"ReconcervationDate", "Дата <br> переконсервации"},
                {"StorageCity", "Город<br>храненения" },
                {"StorageCountry", "Страна<br>храненения" },
                {"RecieverGroupPlan", "Группа<br>грузополучателя<br>(плановая)"},
                {"OrderNum", "Номер<br>заказа<br>покупателя"},
                {"InnerOrderNum", "Номер<br>заказа<br>в производство"}
            };

            return fields;
        }


        public IQueryable<DocViewModel> GetDocsByScreenShotId(int screenShotId, GroupItemFilters filters)
        {
            return db.GetDocsDataBySnapshot(screenShotId,
                      filters.FilterStorageId,
                      filters.FilterCenterId,
                      filters.FilterRecieverPlanId,
                      filters.FilterRecieverFactId,
                      filters.FilterKeeperId,
                      filters.FilterProducerId,
                      Convert.ToInt32(filters.UseStorageFilter),
                      Convert.ToInt32(filters.UseCenterFilter),
                      Convert.ToInt32(filters.UseRecieverPlanFilter),
                      Convert.ToInt32(filters.UseRecieverFactFilter),
                      Convert.ToInt32(filters.UseKeeperFilter),
                      Convert.ToInt32(filters.UseProducerFilter))
                    .Where(s=>s.ISPrihod==filters.IsPrihodDocs)
                    .Select(s => new DocViewModel
                    {
                        idrow= s.idrow,
                        id_snapshot = s.id_snapshot,
                        InnerPartyKey = s.InnerPartyKey,
                        Producer = s.Producer,
                        Product = s.Product,
                        Shifr = s.Shifr,
                        Figure = s.Figure,
                        Measure = s.Measure,
                        Weight = s.Weight,
                        pType = s.pType,
                        pGroup = s.pGroup,
                        pRecieverPlan = s.pRecieverPlan,
                        pRecieverFact = s.pRecieverFact,
                        RecieverGroupPlan = s.RecieverGroupPlan,
                        InnerOrderNum = s.InnerOrderNum,
                        OrderedBy = s.OrderedBy,
                        OrderNum = s.OrderNum,
                        Quantity = s.Quantity??0,
                        PE = s.PE??0,
                        PF = s.PF??0,
                        PCP = s.PCP??0,
                        PCPC = s.PCPC??0,
                        FCP = s.FCP??0,
                        FCPC = s.FCPC??0,
                        BP = s.BP??0,
                        Storage = s.Storage,
                        StorageCity = s.StorageCity,
                        StorageCountry = s.StorageCountry,
                        Сenter = s.Сenter,
                        BalanceKeeper = s.BalanceKeeper,
                        ReadyForSaleStatus = s.ReadyForSaleStatus,
                        ReserveStatus = s.ReserveStatus,
                        ProduceDate = s.ProduceDate,
                        ReconcervationDate = s.ReconcervationDate,
                        TermOnStorage = s.TermOnStorage,
                        DocType = s.DocType,
                        DocNum = s.DocNum,
                        DocDate = s.DocDate,
                        BalanceCurrency = s.BalanceCurrency,
                        CurrencyIndexToUAH = s.CurrencyIndexToUAH,
                        ISPrihod = s.ISPrihod,
                        Mass = s.Quantity*s.Weight??0
                    }).OrderBy(o => o.DocDate).ToList().AsQueryable();
        }


        public IQueryable<GroupItemRestViewModel> GetGroupRestsBySnapShotId(int screenShotId, string FieldName, GroupItemFilters filters)
        {
            string[] fields = { FieldName };
            var lambda = GroupByExpression<GetSummaryDataBySnapshot_Result>(fields);

            return db.GetSummaryDataBySnapshot(screenShotId,
                      filters.FilterStorageId,
                      filters.FilterCenterId,
                      filters.FilterRecieverPlanId,
                      filters.FilterRecieverFactId,
                      filters.FilterKeeperId,
                      filters.FilterProducerId,
                      filters.FilterOrderProjectId,

                      Convert.ToInt32(filters.UseStorageFilter),
                      Convert.ToInt32(filters.UseCenterFilter),
                      Convert.ToInt32(filters.UseRecieverPlanFilter),
                      Convert.ToInt32(filters.UseRecieverFactFilter),
                      Convert.ToInt32(filters.UseKeeperFilter),
                      Convert.ToInt32(filters.UseProducerFilter),
                      Convert.ToInt32(filters.UseOrderProjectFilter))
                    .GroupBy(lambda.Compile())
                    .Select(g => new GroupItemRestViewModel
                    {
                        Position = 0,
                        groupItem = (string)g.FirstOrDefault().GetType().GetProperty(FieldName).GetValue(g.FirstOrDefault()),
                        QuantityBefore = g.Sum(s => s.QuantityBefore??0),
                        QuantityAfter = g.Sum(s => s.QuantityAfter??0),
                        WeightBefore = g.Sum(s=>(s.QuantityBefore ?? 0)*(s.Weight ?? 0)),
                        WeightAfter = g.Sum(s => (s.QuantityAfter ?? 0) * (s.Weight ?? 0)),
                        PE_Before = g.Sum(s => s.PE_Before??0),
                        PF_Before = g.Sum(s => s.PF_Before??0),
                        PCP_Before = g.Sum(s => s.PCP_Before??0),
                        PCPC_Before = g.Sum(s => s.PCPC_Before??0),
                        FCP_Before = g.Sum(s => s.FCP_Before??0),
                        FCPC_Before = g.Sum(s => s.FCPC_Before??0),
                        BP_Before = g.Sum(s => s.BP_Before??0),
                        PE_After = g.Sum(s => s.PE_After??0),
                        PF_After = g.Sum(s => s.PF_After??0),
                        PCP_After = g.Sum(s => s.PCP_After??0),
                        PCPC_After = g.Sum(s => s.PCPC_After??0),
                        FCP_After = g.Sum(s => s.FCP_After??0),
                        FCPC_After = g.Sum(s => s.FCPC_After??0),
                        BP_After = g.Sum(s => s.BP_After??0),

                        QuantityPrihod = g.Sum(s => s.QuantityPrihod??0),
                        MassPrihod = g.Sum(s => s.MassPrihod??0),
                        PE_Prihod = g.Sum(s => s.PE_Prihod??0),
                        PF_Prihod = g.Sum(s => s.PF_Prihod??0),        
                        PCP_Prihod = g.Sum(s => s.PCP_Prihod??0),       
                        PCPC_Prihod = g.Sum(s => s.PCPC_Prihod??0),
                        FCP_Prihod  = g.Sum(s => s.FCP_Prihod??0),     
                        FCPC_Prihod = g.Sum(s => s.FCPC_Prihod??0),
                        BP_Prihod = g.Sum(s => s.BP_Prihod??0),

                        QuantityRashod = g.Sum(s => s.QuantityRashod??0),
                        MassRashod = g.Sum(s => s.MassRashod ?? 0),
                        PE_Rashod = g.Sum(s => s.PE_Rashod ?? 0),
                        PF_Rashod = g.Sum(s => s.PF_Rashod ?? 0),
                        PCP_Rashod = g.Sum(s => s.PCP_Rashod ?? 0),
                        PCPC_Rashod = g.Sum(s => s.PCPC_Rashod ?? 0),
                        FCP_Rashod = g.Sum(s => s.FCP_Rashod ?? 0),
                        FCPC_Rashod = g.Sum(s => s.FCPC_Rashod ?? 0),
                        BP_Rashod = g.Sum(s => s.BP_Rashod ?? 0)

                    }).OrderBy(o=>o.groupItem).ToList().AsQueryable();
        }

        public IQueryable<RestViewModel> GetGroupRecieverRestsByScreenShotId(int screenShotId, string storage)
        {
            return new List<RestViewModel>().AsQueryable();
        }

        public List<GroupItemViewModel> GetStorages(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            return  GetStoragesBySearchString(snapShot, searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetStoragesCount(int snapShot, string searchTerm)
        {
            return GetStoragesBySearchString(snapShot, searchTerm).Count();
        }

        private IQueryable<GroupItemViewModel> GetStoragesBySearchString(int snapShot, string searchTerm)
        {
            return
            db.RestsSnapshot
                 .AsNoTracking()
                    .Where(s => s.id_snapshot == snapShot && (s.Storage.Contains(searchTerm) || s.Storage.StartsWith(searchTerm) || s.Storage.EndsWith(searchTerm)))
                     .GroupBy(ri => ri.Storage)
                       .Select(s => new GroupItemViewModel
                       {
                           Id = 0,
                           Name = s.FirstOrDefault().Storage
                       })
                       .OrderBy(o => o.Name)
                        .AsQueryable();
        }

        public List<GroupItemViewModel> GetCenters(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            return GetCentersBySearchString(snapShot, searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetCentersCount(int snapShot, string searchTerm)
        {
            return GetCentersBySearchString(snapShot, searchTerm).Count();
        }

        private IQueryable<GroupItemViewModel> GetCentersBySearchString(int snapShot, string searchTerm)
        {
            return
            db.RestsSnapshot
                  .AsNoTracking()
                     .Where(s => s.id_snapshot == snapShot && (s.Сenter.Contains(searchTerm) || s.Сenter.StartsWith(searchTerm) || s.Сenter.EndsWith(searchTerm)))
                      .GroupBy(ri => ri.Сenter)
                        .Select(s => new GroupItemViewModel
                        {
                            Id = 0,
                            Name = s.FirstOrDefault().Сenter
                        })
                        .OrderBy(o => o.Name)
                         .AsQueryable();
        }

        public List<GroupItemViewModel> GetRecievers(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            return GetRecieversBySearchString(snapShot, searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetRecieversCount(int snapShot, string searchTerm)
        {
            return GetRecieversBySearchString(snapShot, searchTerm).Count();
        }

        private IQueryable<GroupItemViewModel> GetRecieversBySearchString(int snapShot, string searchTerm)
        {
            return
                db.RestsSnapshot
                      .AsNoTracking()
                         .Where(s => s.id_snapshot == snapShot && (s.pRecieverPlan.Contains(searchTerm) || s.pRecieverPlan.StartsWith(searchTerm) || s.pRecieverPlan.EndsWith(searchTerm)))
                          .GroupBy(ri => ri.pRecieverPlan)
                            .Select(s => new GroupItemViewModel
                            {
                                Id = 0,
                                Name = s.FirstOrDefault().pRecieverPlan
                            })
                            .OrderBy(o => o.Name)
                             .AsQueryable();
        }

        public List<GroupItemViewModel> GetBalanceKeepers(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            return GetBalanceKeepersBySearchString(snapShot, searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetBalanceKeepersCount(int snapShot, string searchTerm)
        {
            return GetBalanceKeepersBySearchString(snapShot, searchTerm).Count();
        }

        private IQueryable<GroupItemViewModel> GetBalanceKeepersBySearchString(int snapShot, string searchTerm)
        {
            return
                db.RestsSnapshot
                      .AsNoTracking()
                         .Where(s => s.id_snapshot == snapShot && (s.BalanceKeeper.Contains(searchTerm) || s.BalanceKeeper.StartsWith(searchTerm) || s.BalanceKeeper.EndsWith(searchTerm)))
                          .GroupBy(ri => ri.BalanceKeeper)
                            .Select(s => new GroupItemViewModel
                            {
                                Id = 0,
                                Name = s.FirstOrDefault().BalanceKeeper
                            })
                            .OrderBy(o => o.Name)
                             .AsQueryable();
        }

        public List<GroupItemViewModel> GetProducers(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            return GetProducerBySearchString(snapShot, searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetProducersCount(int snapShot, string searchTerm)
        {
            return GetProducerBySearchString(snapShot, searchTerm).Count();
        }

        private IQueryable<GroupItemViewModel> GetProducerBySearchString(int snapShot, string searchTerm)
        {
            return
                db.RestsSnapshot
                      .AsNoTracking()
                         .Where(s => s.id_snapshot == snapShot && (s.Producer.Contains(searchTerm) || s.Producer.StartsWith(searchTerm) || s.Producer.EndsWith(searchTerm)))
                          .GroupBy(ri => ri.Producer)
                            .Select(s => new GroupItemViewModel
                            {
                                Id = 0,
                                Name = s.FirstOrDefault().Producer
                            })
                            .OrderBy(o => o.Name)
                             .AsQueryable();
        }

        public List<RestViewModel> GetProjects(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            return GetProjectsBySearchString(snapShot, searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public List<RestViewModel> GetBarcodes(int snapShot, string searchTerm, int pageSize, int pageNum)
        {
            return GetBarcodesBySearchString(snapShot, searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetBarcodesCount(int snapShot, string searchTerm)
        {
            return GetBarcodesBySearchString(snapShot, searchTerm).Count();
        }

        public int GetProjectsCount(int snapShot, string searchTerm)
        {
            return GetProjectsBySearchString(snapShot, searchTerm).Count();
        }

        private IQueryable<RestViewModel> GetProjectsBySearchString(int snapShot, string searchTerm)
        {
            return db.GetInnerOrderNumFilter(searchTerm,snapShot).ToList().Select(Mapper.Map).AsQueryable();
        }

        private IQueryable<RestViewModel> GetBarcodesBySearchString(int snapShot, string searchTerm)
        {
            return db.GetProductBarcodeFilter(searchTerm, snapShot).ToList().Select(Mapper.Map).AsQueryable();
        }
    }
}
