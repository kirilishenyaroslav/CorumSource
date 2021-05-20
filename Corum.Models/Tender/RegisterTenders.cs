using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public class RegisterTenders
    {
        public int Id { get; set; }
        public long OrderId { get; set; }
        public System.Guid TenderUuid { get; set; }
        public int tenderNumber { get; set; }
        public int subCompanyId { get; set; }
        public string subCompanyName { get; set; }
        public int industryId { get; set; }
        public string industryName { get; set; }
        public System.DateTime dateStart { get; set; }
        public System.DateTime dateEnd { get; set; }
        public byte mode { get; set; }
        public string stageMode { get; set; }
        public byte stageNumber { get; set; }
        public byte process { get; set; }
        public string downloadAddress { get; set; }
        public string unloadAddress { get; set; }
        public System.DateTime downloadDataRequired { get; set; }
        public System.DateTime unloadDataRequired { get; set; }
        public string routeOrder { get; set; }
        public string cargoName { get; set; }
        public int lotState { get; set; }
        public string processValue { get; set; }
        public string resultsTender { get; set; }
        public string tenderOwnerPath { get; set; }
    }
}
