using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Tender
{
    public class RequestJSONDeserializedToModel
    {
        public bool success { get; set; }
        public Data data { get; set; }
    }
    public class Item
    {
        public string itemName { get; set; }
        public string itemNote { get; set; }
        public string itemExternalN { get; set; }
        public string unitName { get; set; }
        public int nmcId { get; set; }
        public string nmcUuid { get; set; }
        public string nmcName { get; set; }
        public int qty { get; set; }
        public string tenderItemUuid { get; set; }
        public string offersUrl { get; set; }
        public string offersUrlEx { get; set; }
    }

    public class Lot
    {
        public int lotNumber { get; set; }
        public string lotName { get; set; }
        public List<string> props { get; } = new List<string>();
        public int stageNumber { get; set; }
        public int lotState { get; set; }
        public string lotStateName { get; set; }
        public List<Item> items { get; } = new List<Item>();
    }

    public class Data
    {
        public string tenderNumber { get; set; }
        public string tenderName { get; set; }
        public int tenderAuthorId { get; set; }
        public string tenderAuthorName { get; set; }
        public string tenderAuthorEMail { get; set; }
        public int companyId { get; set; }
        public string companyName { get; set; }
        public int subCompanyId { get; set; }
        public string subCompanyName { get; set; }
        public int industryId { get; set; }
        public string industryName { get; set; }
        public int budget { get; set; }
        public int depId { get; set; }
        public string depName { get; set; }
        public DateTime dateCreate { get; set; }
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public string tenderExternalN { get; set; }
        public int mode { get; set; }
        public string stageMode { get; set; }
        public int kind { get; set; }
        public string stageKind { get; set; }
        public int stageNumber { get; set; }
        public string tenderUuid { get; set; }
        public string tenderOwnerPath { get; set; }
        public string process { get; set; }
        public string generalTerms { get; set; }
        public string processValue { get; set; }
        public string remainingTime { get; set; }
        public DateTime dateUpdateStatus { get; set; }
        public List<Lot> lots { get; } = new List<Lot>();
    }
}
