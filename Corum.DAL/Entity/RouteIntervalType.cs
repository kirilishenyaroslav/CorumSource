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
    
    public partial class RouteIntervalType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RouteIntervalType()
        {
            this.ContractSpecifications = new HashSet<ContractSpecifications>();
        }
    
        public int Id { get; set; }
        public string NameIntervalType { get; set; }
        public Nullable<int> MaxInterval { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractSpecifications> ContractSpecifications { get; set; }
    }
}
