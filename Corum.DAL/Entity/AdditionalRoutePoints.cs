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
    
    public partial class AdditionalRoutePoints
    {
        public long Id { get; set; }
        public long RoutePointId { get; set; }
        public Nullable<bool> IsLoading { get; set; }
        public long OrderId { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonPhone { get; set; }
        public Nullable<int> NumberPoint { get; set; }
    
        public virtual OrdersBase OrdersBase { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
