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
    
    public partial class OrdersPassengerTransport
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public System.DateTime StartDateTimeOfTrip { get; set; }
        public string AdressFrom { get; set; }
        public string AdressTo { get; set; }
        public System.DateTime FinishDateTimeOfTrip { get; set; }
        public string TripDescription { get; set; }
        public string CarDetailInfo { get; set; }
        public string CarDriverFio { get; set; }
        public string CarDriverContactInfo { get; set; }
        public bool NeedReturn { get; set; }
        public Nullable<System.DateTime> ReturnStartDateTimeOfTrip { get; set; }
        public Nullable<System.DateTime> ReturnFinishDateTimeOfTrip { get; set; }
        public Nullable<System.TimeSpan> ReturnWaitingTime { get; set; }
        public string PassInfo { get; set; }
        public string OrgFrom { get; set; }
        public string OrgTo { get; set; }
        public Nullable<int> TripType { get; set; }
        public Nullable<int> FromCountry { get; set; }
        public Nullable<int> ToCountry { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public Nullable<long> OrgFromId { get; set; }
        public Nullable<long> OrgToId { get; set; }
    
        public virtual Countries Countries { get; set; }
        public virtual Countries Countries1 { get; set; }
        public virtual RouteTypes RouteTypes { get; set; }
        public virtual OrdersBase OrdersBase { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Organization Organization1 { get; set; }
    }
}