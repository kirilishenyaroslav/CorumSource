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
    
    public partial class OrderBaseSpecification
    {
        public int Id { get; set; }
        public Nullable<long> OrderId { get; set; }
        public Nullable<int> SpecificationId { get; set; }
    
        public virtual SpecificationTypes SpecificationTypes { get; set; }
        public virtual OrdersBase OrdersBase { get; set; }
    }
}
