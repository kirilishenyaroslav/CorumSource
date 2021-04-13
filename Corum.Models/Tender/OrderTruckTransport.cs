using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public class OrderTruckTransport
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public int TruckTypeId { get; set; }
        public string TruckDescription { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public string BoxingDescription { get; set; }
        public Nullable<decimal> DimenssionL { get; set; }
        public Nullable<decimal> DimenssionW { get; set; }
        public Nullable<decimal> DimenssionH { get; set; }
        public Nullable<int> VehicleTypeId { get; set; }
        public Nullable<int> LoadingTypeId { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public Nullable<System.DateTime> FromShipperDatetime { get; set; }
        public Nullable<System.DateTime> ToConsigneeDatetime { get; set; }
        public Nullable<int> UnloadingTypeId { get; set; }
        public Nullable<int> ShipperCountryId { get; set; }
        public Nullable<int> ConsigneeCountryId { get; set; }
        public string ShipperCity { get; set; }
        public string ConsigneeCity { get; set; }
        public string ShipperAdress { get; set; }
        public string ConsigneeAdress { get; set; }
        public Nullable<int> TripType { get; set; }
        public string ShipperContactPerson { get; set; }
        public string ShipperContactPersonPhone { get; set; }
        public string ConsigneeContactPerson { get; set; }
        public string ConsigneeContactPersonPhone { get; set; }
        public Nullable<long> ShipperId { get; set; }
        public Nullable<long> ConsigneeId { get; set; }
    }
}
