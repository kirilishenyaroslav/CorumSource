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
    
    public partial class OrderFilterSettings
    {
        public int Id { get; set; }
        public string NameFilter { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string CreatorId { get; set; }
        public string ExecuterId { get; set; }
        public Nullable<int> TypeId { get; set; }
        public Nullable<long> ClientId { get; set; }
        public Nullable<int> PriorityType { get; set; }
        public double DeltaDateBeg { get; set; }
        public double DeltaDateEnd { get; set; }
        public double DeltaDateBegEx { get; set; }
        public double DeltaDateEndEx { get; set; }
        public bool UseStatusFilter { get; set; }
        public bool UseCreatorFilter { get; set; }
        public bool UseExecuterFilter { get; set; }
        public bool UseTypeFilter { get; set; }
        public bool UseClientFilter { get; set; }
        public bool UsePriorityFilter { get; set; }
        public bool UseDateFilter { get; set; }
        public bool UseExDateFilter { get; set; }
        public string IdCurrentUser { get; set; }
    
        public virtual OrderClients OrderClients { get; set; }
        public virtual OrderStatuses OrderStatuses { get; set; }
        public virtual OrderTypesBase OrderTypesBase { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual AspNetUsers AspNetUsers2 { get; set; }
    }
}
