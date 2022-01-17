using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Corum.Models.ViewModels.Tender
{
    public class RequestJSONDeserializedToModel
    {
        public bool success { get; set; }
        public Data data { get; set; }
    }
    public class OwnerFile
    {
        public string fileName { get; set; }
        public DateTime dateChange { get; set; }
        public int userChangeId { get; set; }
        public string fileUuid { get; set; }
        public bool suppVisible { get; set; }
        public string fileUrl { get; set; }
    }

    public class CompetitorList
    {
        public string filename { get; set; }
        public string tenderFileUuid { get; set; }
        public string tenderFileUrl { get; set; }
    }

    public class Criterion
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class CriteriaValue
    {
        public int id { get; set; }
        public string name { get; set; }
        public object value { get; set; }
    }

    public class PropValue
    {
        private string GetDate(object value)
        {
            string dataBeforeT = "";
            string val = value.ToString();
            string[] valMassive = val.Contains('T')?val.Split('T'):val.Split(' ');
            string[] valMassiveTwo = valMassive[0].Contains('-')?valMassive[0].Split('-'): valMassive[0].Split('.');
            string[] valMassiveThree = valMassive[1].Split(':');
            foreach (var item in valMassiveTwo)
            {
                foreach (var it in item)
                {
                    if (dataBeforeT.Length < 10)
                    {
                        dataBeforeT += it;
                    }
                }
                if (dataBeforeT.Length < 10)
                {
                    dataBeforeT += '-';
                }
            }
            dataBeforeT += 'T';
            foreach (var item in valMassiveThree)
            {
                foreach (var it in item)
                {
                    if (dataBeforeT.Length < 20)
                    {
                        dataBeforeT += it;
                    }
                }
                if (dataBeforeT.Length < 19)
                {
                    dataBeforeT += ':';
                }
            }
            return dataBeforeT;
        }
        private object dateOnload;
        [JsonProperty("Доп.точка загрузки 1")]
        public string ДопТочкаЗагрузки1 { get; set; }

        [JsonProperty("Доп.точка выгрузки 1")]
        public string ДопТочкаВыгрузки1 { get; set; }

        [JsonProperty("Доп.точка загрузки 2")]
        public string ДопТочкаЗагрузки2 { get; set; }

        [JsonProperty("Доп.точка выгрузки 2")]
        public string ДопТочкаВыгрузки2 { get; set; }

        [JsonProperty("Доп.точка загрузки 3")]
        public string ДопТочкаЗагрузки3 { get; set; }

        [JsonProperty("Доп.точка выгрузки 3")]
        public string ДопТочкаВыгрузки3 { get; set; }

        [JsonProperty("Доп.точка загрузки 4")]
        public string ДопТочкаЗагрузки4 { get; set; }

        [JsonProperty("Доп.точка выгрузки 4")]
        public string ДопТочкаВыгрузки4 { get; set; }

        [JsonProperty("Доп.точка загрузки 5")]
        public string ДопТочкаЗагрузки5 { get; set; }

        [JsonProperty("Доп.точка выгрузки 5")]
        public string ДопТочкаВыгрузки5 { get; set; }

        [JsonProperty("Вес, т")]
        public string ВесТ { get; set; }

        [JsonProperty("Маршрут")]
        public string Маршрут { get; set; }

        [JsonProperty("Наименование груза")]
        public string НаименованиеГруза { get; set; }

        [JsonProperty("Дата загрузки требуемая")]
        public object ДатаЗагрузкиТребуемая
        {
            set
            {
                dateOnload = GetDate(value);
            }
            get
            {
                return dateOnload as DateTime?;
            }
        }

        [JsonProperty("Дата выгрузки требуемая")]
        public DateTime? ДатаВыгрузкиТребуемая { get; set; }

        [JsonProperty("Требуемое кол-во автомобилей")]
        public string ТребуемоеКолВоАвтомобилей { get; set; }

        [JsonProperty("Особые условия")]
        public string ОсобыеУсловия { get; set; }

        [JsonProperty("Адрес загрузки")]
        public string АдресЗагрузки { get; set; }

        [JsonProperty("Адрес выгрузки")]
        public string АдресВыгрузки { get; set; }
    }
    public class Winner
    {
        public int id { get; set; }
        public string edrpou { get; set; }
        public string name { get; set; }
        public string ownershipType { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string addressActual { get; set; }
        public string addressLegal { get; set; }
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
        public List<CriteriaValue> criteriaValues { get; set; }
        public int qty { get; set; }
        public List<PropValue> propValues { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public string tenderItemUuid { get; set; }
        public string offersUrl { get; set; }
        public string offersUrlEx { get; set; }
        public Winner winner { get; set; }
    }
    public class ClSupplier
    {
        public int id { get; set; }
        public string edrpou { get; set; }
        public string name { get; set; }
    }
    public class Lot
    {
        public int lotNumber { get; set; }
        public string lotName { get; set; }
        public List<string> props { get; } = new List<string>();
        public int stageNumber { get; set; }
        public int lotState { get; set; }
        public string lotResultNote { get; set; }
        public string lotStateName { get; set; }
        public string lotReport { get; set; }
        public List<Criterion> criteria { get; set; }
        public List<Item> items { get; } = new List<Item>();
        public List<ClSupplier> clSuppliers { get; set; }
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
        public string tenderCurrency { get; set; }
        public int tenderCurrencyRate { get; set; }
        public double economy { get; set; }
        public double solutionCost { get; set; }
        public bool successful { get; set; }
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
        public List<OwnerFile> ownerFiles { get; set; }
        public CompetitorList competitorList { get; set; }
        public List<Lot> lots { get; } = new List<Lot>();
    }
}
