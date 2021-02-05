
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Corum.DAL.Entity
{

using System;
    using System.Collections.Generic;
    
public partial class ContractSpecifications
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ContractSpecifications()
    {

        this.OrderCompetitiveList = new HashSet<OrderCompetitiveList>();

    }


    public int Id { get; set; }

    public System.DateTime CreateDate { get; set; }

    public Nullable<System.DateTime> DateBeg { get; set; }

    public Nullable<System.DateTime> DateEnd { get; set; }

    public string CreatedByUser { get; set; }

    public string NameSpecification { get; set; }

    public Nullable<int> CarryCapacityId { get; set; }

    public string DeparturePoint { get; set; }

    public string ArrivalPoint { get; set; }

    public Nullable<decimal> RouteLength { get; set; }

    public Nullable<int> MovingType { get; set; }

    public int RouteTypeId { get; set; }

    public Nullable<int> IntervalTypeId { get; set; }

    public Nullable<decimal> RateKm { get; set; }

    public Nullable<decimal> RateHour { get; set; }

    public Nullable<decimal> RateMachineHour { get; set; }

    public Nullable<decimal> RateTotalFreight { get; set; }

    public Nullable<decimal> NDSTax { get; set; }

    public int GroupeSpecId { get; set; }

    public Nullable<bool> IsTruck { get; set; }

    public Nullable<long> RouteId { get; set; }

    public Nullable<bool> IsPriceNegotiated { get; set; }

    public int NameId { get; set; }

    public int TypeVehicleId { get; set; }

    public int TypeSpecId { get; set; }

    public string RouteName { get; set; }



    public virtual CarCarryCapacity CarCarryCapacity { get; set; }

    public virtual ContractGroupesSpecifications ContractGroupesSpecifications { get; set; }

    public virtual SpecificationNames SpecificationNames { get; set; }

    public virtual RouteIntervalType RouteIntervalType { get; set; }

    public virtual RouteTypes RouteTypes { get; set; }

    public virtual SpecificationTypes SpecificationTypes { get; set; }

    public virtual OrderVehicleTypes OrderVehicleTypes { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<OrderCompetitiveList> OrderCompetitiveList { get; set; }

    public virtual Routes Routes { get; set; }

    public virtual AspNetUsers AspNetUsers { get; set; }

}

}
