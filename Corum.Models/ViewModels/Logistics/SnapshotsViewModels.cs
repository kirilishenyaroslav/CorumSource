using System;
using System.Collections.Generic;

// ReSharper disable All

namespace Corum.Models.ViewModels
{

    public class PartyCardViewModel
    {
        public RestViewModel restRow { get; set; }
        public List<DocViewModel> docs { get; set; }

    }

    public class GroupHistoryInfo
    {
        public int GroupOrder { get; set; }
        public string GroupFieldName { get; set; }
        public string GroupFieldNameDescription { get; set; }
        public string GroupFieldValue { get; set; }
    }

    public class SnapshotInfoViewModel
    {
        public int Id { get; set; }
        public DateTime DateOfImport { get; set; }
        public bool IsRestsWereImported { get; set; }
        public bool IsDocsWereImported { get; set; }
        public System.DateTime timestamp { get; set; }
        public System.DateTime? ActualDateBeg { get; set; }
        public System.DateTime? ActualDateEnd { get; set; }
        public bool DefaultForReports { get; set; }

    }

    public class RestViewModel
    {
        public long idrow { get; set; }
        public int id_snapshot { get; set; }
        public string InnerPartyKey { get; set; }
        public string Producer { get; set; }
        public string Product { get; set; }
        public string Shifr { get; set; }
        public string Figure { get; set; }
        public string Measure { get; set; }
        public string BacodeProduct { get; set; }
        public string BacodeConsignment { get; set; }
        public string Shifr_MDM { get; set; }
        public string BacodesAll { get; set; }
        public string ProductFullName { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string pType { get; set; }
        public string pGroup { get; set; }
        public string pRecieverPlan { get; set; }
        public string pRecieverFact { get; set; }
        public string RecieverGroupPlan { get; set; }
        public string InnerOrderNum { get; set; }
        public string OrderedBy { get; set; }
        public string OrderNum { get; set; }
        public decimal MassBefore { get; set; }
        public decimal QuantityBefore { get; set; }
        public decimal PE_Before { get; set; }
        public decimal PF_Before { get; set; }
        public decimal PCP_Before { get; set; }
        public decimal PCPC_Before { get; set; }
        public decimal FCP_Before { get; set; }
        public decimal FCPC_Before { get; set; }
        public decimal BP_Before { get; set; }
        public decimal PE_After { get; set; }
        public decimal PF_After { get; set; }
        public decimal PCP_After { get; set; }
        public decimal PCPC_After { get; set; }
        public decimal FCP_After { get; set; }
        public decimal FCPC_After { get; set; }
        public decimal BP_After { get; set; }
        public decimal QuantityAfter { get; set; }
        public decimal MassAfter { get; set; }
        public string Storage { get; set; }
        public string StorageCity { get; set; }
        public string StorageCountry { get; set; }
        public string Сenter { get; set; }
        public string BalanceKeeper { get; set; }
        public string ReadyForSaleStatus { get; set; }
        public string ReserveStatus { get; set; }
        public string ProduceDate { get; set; }
        public string ReconcervationDate { get; set; }
        public Nullable<int> TermOnStorage { get; set; }
        public string PrihodDocType { get; set; }
        public string PrihodDocNum { get; set; }
        public string PrihodDocDate { get; set; }
        public string BalanceCurrency { get; set; }
        public Nullable<decimal> CurrencyIndexToUAH { get; set; }
        public string FullSellStatus { get; set; }
    }

    public class GroupItemRestViewModel
    {
        public string groupItem { get; set; }
        public decimal QuantityBefore { get; set; }
        public decimal WeightBefore { get; set; }
        public decimal PE_Before { get; set; }
        public decimal PF_Before { get; set; }
        public decimal PCP_Before { get; set; }
        public decimal PCPC_Before { get; set; }
        public decimal FCP_Before { get; set; }
        public decimal FCPC_Before { get; set; }
        public decimal BP_Before { get; set; }
        public decimal PE_After { get; set; }
        public decimal PF_After { get; set; }
        public decimal PCP_After { get; set; }
        public decimal PCPC_After { get; set; }
        public decimal FCP_After { get; set; }
        public decimal FCPC_After { get; set; }
        public decimal BP_After { get; set; }
        public Nullable<decimal> QuantityAfter { get; set; }
        public decimal WeightAfter { get; set; }


        public decimal QuantityPrihod { get; set; }
        public decimal MassPrihod {get; set;}
        public decimal PE_Prihod {get; set;}
        public decimal PF_Prihod {get; set;}
        public decimal PCP_Prihod {get; set;}
        public decimal PCPC_Prihod {get; set;}
        public decimal FCP_Prihod  {get; set;}
        public decimal FCPC_Prihod {get; set;}
        public decimal BP_Prihod { get; set; }

        public decimal QuantityRashod { get; set; }
        public decimal MassRashod { get; set; }
        public decimal PE_Rashod { get; set; }
        public decimal PF_Rashod { get; set; }
        public decimal PCP_Rashod { get; set; }
        public decimal PCPC_Rashod { get; set; }
        public decimal FCP_Rashod { get; set; }
        public decimal FCPC_Rashod { get; set; }
        public decimal BP_Rashod { get; set; }

        public int Position { get; set; }


    }

    public class DocViewModel
    {
        public long idrow { get; set; }
        public int id_snapshot { get; set; }
        public string InnerPartyKey { get; set; }
        public string Producer { get; set; }
        public string Product { get; set; }
        public string Shifr { get; set; }
        public string Figure { get; set; }
        public string Measure { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public decimal Mass { get; set; }
        public string pType { get; set; }
        public string pGroup { get; set; }
        public string pRecieverPlan { get; set; }
        public string pRecieverFact { get; set; }
        public string RecieverGroupPlan { get; set; }
        public string InnerOrderNum { get; set; }
        public string OrderedBy { get; set; }
        public string OrderNum { get; set; }
        public decimal Quantity { get; set; }
        public decimal PE { get; set; }
        public decimal PF { get; set; }
        public decimal PCP { get; set; }
        public decimal PCPC { get; set; }
        public decimal FCP { get; set; }
        public decimal FCPC { get; set; }
        public decimal BP { get; set; }
        public string Storage { get; set; }
        public string StorageCity { get; set; }
        public string StorageCountry { get; set; }
        public string Сenter { get; set; }
        public string BalanceKeeper { get; set; }
        public string ReadyForSaleStatus { get; set; }
        public string ReserveStatus { get; set; }
        public string ProduceDate { get; set; }
        public string ReconcervationDate { get; set; }
        public Nullable<int> TermOnStorage { get; set; }
        public string DocType { get; set; }
        public string DocNum { get; set; }
        public System.DateTime? DocDate { get; set; }
        public string BalanceCurrency { get; set; }
        public Nullable<decimal> CurrencyIndexToUAH { get; set; }
        public int ISPrihod { get; set; }
    }


    public class BriefViewModel
    {

        public DateTime currentDate { get; set; }

        public decimal Quantity_OnDate { get; set; }
        public decimal Mass_OnDate { get; set; }
        public decimal PE_OnDate { get; set; }
        public decimal PF_OnDate { get; set; }
        public decimal PCP_OnDate { get; set; }
        public decimal PCPC_OnDate { get; set; }
        public decimal FCP_OnDate { get; set; }
        public decimal FCPC_OnDate { get; set; }
        public decimal BP_OnDate { get; set; }

        public decimal ShipOnDate_Quantity { get; set; }
        public decimal ShipOnDate_Mass { get; set; }
        public decimal ShipOnDate_PE { get; set; }
        public decimal ShipOnDate_PF { get; set; }
        public decimal ShipOnDate_PCP { get; set; }
        public decimal ShipOnDate_PCPC { get; set; }
        public decimal ShipOnDate_FCP { get; set; }
        public decimal ShipOnDate_FCPC { get; set; }
        public decimal ShipOnDate_BP { get; set; }

        public decimal ShipForPeriod_Quantity { get; set; }
        public decimal ShipForPeriod_Mass { get; set; }
        public decimal ShipForPeriod_PE { get; set; }
        public decimal ShipForPeriod_PF { get; set; }
        public decimal ShipForPeriod_PCP { get; set; }
        public decimal ShipForPeriod_PCPC { get; set; }
        public decimal ShipForPeriod_FCP { get; set; }
        public decimal ShipForPeriod_FCPC { get; set; }
        public decimal ShipForPeriod_BP { get; set; }

        public decimal ProdOnDate_Quantity { get; set; }
        public decimal ProdOnDate_Mass { get; set; }
        public decimal ProdOnDate_PE { get; set; }
        public decimal ProdOnDate_PF { get; set; }
        public decimal ProdOnDate_PCP { get; set; }
        public decimal ProdOnDate_PCPC { get; set; }
        public decimal ProdOnDate_FCP { get; set; }
        public decimal ProdOnDate_FCPC { get; set; }
        public decimal ProdOnDate_BP { get; set; }

        public decimal ProdByPeriod_Quantity { get; set; }
        public decimal ProdByPeriod_Mass { get; set; }
        public decimal ProdByPeriod_PE { get; set; }
        public decimal ProdByPeriod_PF { get; set; }
        public decimal ProdByPeriod_PCP { get; set; }
        public decimal ProdByPeriod_PCPC { get; set; }
        public decimal ProdByPeriod_FCP { get; set; }
        public decimal ProdByPeriod_FCPC { get; set; }
        public decimal ProdByPeriod_BP { get; set; }

    }   
}
