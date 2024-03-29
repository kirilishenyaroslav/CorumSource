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
    
    public partial class OrderClients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderClients()
        {
            this.OrderDogs = new HashSet<OrderDogs>();
            this.OrderFilterSettings = new HashSet<OrderFilterSettings>();
            this.OrderFilters = new HashSet<OrderFilters>();
            this.OrdersBase = new HashSet<OrdersBase>();
        }
    
        public long Id { get; set; }
        public string ClientName { get; set; }
        public string ClientCity { get; set; }
        public string ClientAddress { get; set; }
        public string AccessRoleId { get; set; }
        public Nullable<int> ClientCFOId { get; set; }
    
        public virtual AspNetRoles AspNetRoles { get; set; }
        public virtual Centers Centers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDogs> OrderDogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderFilterSettings> OrderFilterSettings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderFilters> OrderFilters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersBase> OrdersBase { get; set; }
    }
}
