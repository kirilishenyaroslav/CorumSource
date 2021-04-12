using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Tender
{
    public class TendFormDeserializedJSON
    {
        public string TenderName { get; set; }
        public string TenderAuthorName { get; set; }
        public string CompanyName { get; set; }
        public string SubCompanyName { get; set; }
        public string DepName { get; set; }
        public string TypeTure { get; set; }
        public string Budget { get; set; }
        public string TypePublic { get; set; }
        public string IndustryName { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string LotName { get; set; }

        public Dictionary<string, JqxGrid> JqxGridNmc { get; set; }
    }

    public class JqxGrid
    {
        public string nmcName { get; set; }
        //public string unitName { get; set; }
        public string qty { get; set; }
    }
}
