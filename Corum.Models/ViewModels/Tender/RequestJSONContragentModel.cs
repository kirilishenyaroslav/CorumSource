using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Corum.Models.ViewModels.Tender
{
    public class RequestJSONContragentModel
    {
        [JsonProperty("data")]
        public List<ContragentModel> Data { get; set; }
    }
    public class ContragentModel
    {
        [JsonProperty("supplierId")]
        public int SupplierId { get; set; }

        [JsonProperty("supplier")]
        public string Supplier { get; set; }

        [JsonProperty("edrpou")]
        public string Edrpou { get; set; }

        [JsonProperty("criteriaValues")]
        public List<CriteriaValue> CriteriaValues { get; set; }

        [JsonProperty("isWinner")]
        public bool? IsWinner { get; set; }
    }
    public class CriteriaValues
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
