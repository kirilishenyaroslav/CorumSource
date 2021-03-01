using Corum.Models.Tender;
using Corum.Models.ViewModels.OrderConcurs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Tender
{
    public class TenderParamsDefaults
    {
        protected NameValueCollection allAppSettings;
        protected static CompetitiveListViewModel competitiveList;
        protected static List<TenderServices> listTender;
        protected CompetitiveListViewModel competitiveListViewModel;
        protected List<TenderServices> listTenderServices;
        protected TenderParamsDefaults(CompetitiveListViewModel CompetitiveListViewModel, List<TenderServices> listTenderServices)
        {
            competitiveList = CompetitiveListViewModel;
            listTender = listTenderServices;
        }
        protected TenderParamsDefaults()
        {
            this.competitiveListViewModel = competitiveList;
            this.listTenderServices = listTender;
            allAppSettings = ConfigurationManager.AppSettings;
        }
    }
}
