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
    
    public partial class OrderNotifications
    {
        public long Id { get; set; }
        public System.DateTime Datetime { get; set; }
        public int TypeId { get; set; }
        public string CreatedBy { get; set; }
        public string Body { get; set; }
        public long OrderId { get; set; }
        public string Reciever { get; set; }
    
        public virtual OrderNotificationTypes OrderNotificationTypes { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
    }
}
