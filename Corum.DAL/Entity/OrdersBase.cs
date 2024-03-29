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
    
    public partial class OrdersBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdersBase()
        {
            this.AdditionalRoutePoints = new HashSet<AdditionalRoutePoints>();
            this.OrderAttachments = new HashSet<OrderAttachments>();
            this.OrderBaseProjects = new HashSet<OrderBaseProjects>();
            this.OrderBaseSpecification = new HashSet<OrderBaseSpecification>();
            this.OrderCompetitiveList = new HashSet<OrderCompetitiveList>();
            this.OrderConcursListsSteps = new HashSet<OrderConcursListsSteps>();
            this.OrderObservers = new HashSet<OrderObservers>();
            this.OrdersPassengerTransport = new HashSet<OrdersPassengerTransport>();
            this.OrderStatusHistory = new HashSet<OrderStatusHistory>();
            this.OrderTruckTransport = new HashSet<OrderTruckTransport>();
            this.OrderUsedCars = new HashSet<OrderUsedCars>();
        }
    
        public long Id { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string CreatedByUser { get; set; }
        public System.DateTime CreateDatetime { get; set; }
        public int OrderType { get; set; }
        public int CurrentOrderStatus { get; set; }
        public string OrderDescription { get; set; }
        public long ClientId { get; set; }
        public Nullable<long> ClientDogId { get; set; }
        public Nullable<decimal> Summ { get; set; }
        public Nullable<bool> UseNotifications { get; set; }
        public string CreatorPosition { get; set; }
        public string CreatorContact { get; set; }
        public int PriotityType { get; set; }
        public Nullable<System.DateTime> OrderServiceDateTime { get; set; }
        public string OrderExecuter { get; set; }
        public Nullable<int> PayerId { get; set; }
        public string ProjectNum { get; set; }
        public Nullable<int> CarNumber { get; set; }
        public string DistanceDescription { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> TotalDistanceLength { get; set; }
        public Nullable<bool> IsPrivateOrder { get; set; }
        public string ExecuterNotes { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> TypeSpecId { get; set; }
        public Nullable<int> TimeRoute { get; set; }
        public Nullable<int> TimeSpecialVehicles { get; set; }
        public Nullable<bool> IsAdditionalRoutePoints { get; set; }
        public Nullable<long> RouteId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdditionalRoutePoints> AdditionalRoutePoints { get; set; }
        public virtual BalanceKeepers BalanceKeepers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderAttachments> OrderAttachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderBaseProjects> OrderBaseProjects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderBaseSpecification> OrderBaseSpecification { get; set; }
        public virtual OrderClients OrderClients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderCompetitiveList> OrderCompetitiveList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderConcursListsSteps> OrderConcursListsSteps { get; set; }
        public virtual OrderDogs OrderDogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderObservers> OrderObservers { get; set; }
        public virtual Projects Projects { get; set; }
        public virtual Routes Routes { get; set; }
        public virtual OrderStatuses OrderStatuses { get; set; }
        public virtual OrderTypesBase OrderTypesBase { get; set; }
        public virtual SpecificationTypes SpecificationTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersPassengerTransport> OrdersPassengerTransport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderStatusHistory> OrderStatusHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderTruckTransport> OrderTruckTransport { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderUsedCars> OrderUsedCars { get; set; }
    }
}
