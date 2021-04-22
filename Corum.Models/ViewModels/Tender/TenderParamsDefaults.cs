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
        protected static List<Countries> listCountries;
        protected static TendFormDeserializedJSON FormDeserializedJSON;
        protected static List<SpecificationNames> SpecificationNames;
        protected static OrderTruckTransport OrderTruckTransport;

        protected CompetitiveListViewModel competitiveListViewModel;
        protected List<TenderServices> listTenderServices;
        protected List<BalanceKeepers> listBalanceKeepers;
        protected TendFormDeserializedJSON formDeserializedJSON;
        protected List<SpecificationNames> listSpecificationNames;
        protected OrderTruckTransport orderTruckTransport;
        protected List<Countries> listCountriesNames;
        protected TenderParamsDefaults(CompetitiveListViewModel CompetitiveListViewModel, List<TenderServices> listTenderServices, List<BalanceKeepers> listBalanceKeepers, OrderTruckTransport orderTruckTransport)
        {
            competitiveList = CompetitiveListViewModel;
            listTender = listTenderServices;
            listBalance = listBalanceKeepers;
            OrderTruckTransport = orderTruckTransport;
        }

        protected TenderParamsDefaults(CompetitiveListViewModel CompetitiveListViewModel, List<TenderServices> listTenderServices, List<BalanceKeepers> listBalanceKeepers, TendFormDeserializedJSON tendFormDeserializedJSON, List<SpecificationNames> specificationNames, List<Countries> countries)
        {
            competitiveList = CompetitiveListViewModel;
            listTender = listTenderServices;
            listBalance = listBalanceKeepers;
            FormDeserializedJSON = tendFormDeserializedJSON;
            SpecificationNames = specificationNames;
            listCountries = countries;
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
            this.orderTruckTransport = OrderTruckTransport;
            this.listCountriesNames = listCountries;
        }
    }
}
