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
    
    public partial class Cars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cars()
        {
            this.OrderUsedCars = new HashSet<OrderUsedCars>();
        }
    
        public int Id { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }
        public string Driver { get; set; }
        public string DriverLicenseSeria { get; set; }
        public Nullable<int> FuelTypeId { get; set; }
        public Nullable<int> ConsumptionCity { get; set; }
        public Nullable<int> ConsumptionHighway { get; set; }
        public Nullable<int> PassNumber { get; set; }
        public Nullable<int> DriverLicenseNumber { get; set; }
        public Nullable<int> CarOwnersId { get; set; }
    
        public virtual CarOwners CarOwners { get; set; }
        public virtual CarsFuelType CarsFuelType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderUsedCars> OrderUsedCars { get; set; }
    }
}