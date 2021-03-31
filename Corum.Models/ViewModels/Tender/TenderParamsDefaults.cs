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
        protected static List<BalanceKeepers> listBalance;
        protected static TendFormDeserializedJSON FormDeserializedJSON;
        protected static List<SpecificationNames> SpecificationNames;
        protected CompetitiveListViewModel competitiveListViewModel;
        protected List<TenderServices> listTenderServices;
        protected List<BalanceKeepers> listBalanceKeepers;
        protected TendFormDeserializedJSON formDeserializedJSON;
        protected List<SpecificationNames> listSpecificationNames;
        protected TenderParamsDefaults(CompetitiveListViewModel CompetitiveListViewModel, List<TenderServices> listTenderServices, List<BalanceKeepers> listBalanceKeepers)
        {
            competitiveList = CompetitiveListViewModel;
            listTender = listTenderServices;
            listBalance = listBalanceKeepers;
        }

        protected TenderParamsDefaults(CompetitiveListViewModel CompetitiveListViewModel, List<TenderServices> listTenderServices, List<BalanceKeepers> listBalanceKeepers, TendFormDeserializedJSON tendFormDeserializedJSON, List<SpecificationNames> specificationNames)
        {
            competitiveList = CompetitiveListViewModel;
            listTender = listTenderServices;
            listBalance = listBalanceKeepers;
            FormDeserializedJSON = tendFormDeserializedJSON;
            SpecificationNames = specificationNames;
        }
        protected TenderParamsDefaults()
        {
            this.competitiveListViewModel = competitiveList;
            this.listTenderServices = listTender;
            this.listBalanceKeepers = listBalance;
            allAppSettings = ConfigurationManager.AppSettings;
            this.formDeserializedJSON = new TendFormDeserializedJSON();
            this.formDeserializedJSON = FormDeserializedJSON;
            this.listSpecificationNames = SpecificationNames;
        }
    }
}
