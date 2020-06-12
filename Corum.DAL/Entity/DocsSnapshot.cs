//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Corum.DAL.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocsSnapshot
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
        public string pType { get; set; }
        public string pGroup { get; set; }
        public string pRecieverPlan { get; set; }
        public string pRecieverFact { get; set; }
        public string RecieverGroupPlan { get; set; }
        public string InnerOrderNum { get; set; }
        public string OrderedBy { get; set; }
        public string OrderNum { get; set; }
        public Nullable<decimal> QuantityPrihod { get; set; }
        public Nullable<decimal> PE_Prihod { get; set; }
        public Nullable<decimal> PF_Prihod { get; set; }
        public Nullable<decimal> PCP_Prihod { get; set; }
        public Nullable<decimal> PCPC_Prihod { get; set; }
        public Nullable<decimal> FCP_Prihod { get; set; }
        public Nullable<decimal> FCPC_Prihod { get; set; }
        public Nullable<decimal> BP_Prihod { get; set; }
        public Nullable<decimal> PE_Rashod { get; set; }
        public Nullable<decimal> PF_Rashod { get; set; }
        public Nullable<decimal> PCP_Rashod { get; set; }
        public Nullable<decimal> PCPC_Rashod { get; set; }
        public Nullable<decimal> FCP_Rashod { get; set; }
        public Nullable<decimal> FCPC_Rashod { get; set; }
        public Nullable<decimal> BP_Rashod { get; set; }
        public Nullable<decimal> QuantityRashod { get; set; }
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
        public Nullable<System.DateTime> PrihodDocDate { get; set; }
        public string RashodDocType { get; set; }
        public string RashodDocNum { get; set; }
        public Nullable<System.DateTime> RashodDocDate { get; set; }
        public string BalanceCurrency { get; set; }
        public Nullable<decimal> CurrencyIndexToUAH { get; set; }
    
        public virtual LogisticSnapshots LogisticSnapshots { get; set; }
    }
}
