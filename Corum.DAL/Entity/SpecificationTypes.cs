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
    
    public partial class SpecificationTypes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SpecificationTypes()
        {
            this.OrderBaseSpecification = new HashSet<OrderBaseSpecification>();
            this.ContractSpecifications = new HashSet<ContractSpecifications>();
            this.OrdersBase = new HashSet<OrdersBase>();
        }
    
        public int Id { get; set; }
        public string SpecificationType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderBaseSpecification> OrderBaseSpecification { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractSpecifications> ContractSpecifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersBase> OrdersBase { get; set; }
    }
}
