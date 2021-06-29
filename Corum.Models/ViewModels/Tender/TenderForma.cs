using Corum.Models.Tender;
using Corum.Models.ViewModels.OrderConcurs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using Corum.Models.ViewModels.Orders;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Tender
{
    public static class InitializeData
    {
        public static CompetitiveListViewModel competitiveListViewModel;
        public static OrderTruckTransport orderTruckTransport;
        public static TendFormDeserializedJSON formDeserializedJSON;
        public static List<SpecificationNames> listSpecificationNames;
        public static List<Countries> listCountriesNames;
        public static List<RegisterTenders> listRegisterTenders;
        public static IList<OrderAdditionalRoutePointModel> routePointsLoadinfo;
        public static IList<OrderAdditionalRoutePointModel> routePointsUnloadinfo;
        public static List<TenderServices> listTenderServices;
        public static List<BalanceKeepers> listBalanceKeepers;
        public static NameValueCollection allAppSettings;
    }
    public class TenderForma<T> where T : new()  // Модель тендера
    {
        protected Corum.Models.ICorumDataProvider context;
        public Dictionary<string, string> otherParams;
        public DataTender<T> data { get; set; }
        public CompetitiveListViewModel competitiveListViewModel;
        public OrderTruckTransport orderTruckTransport;
        public TendFormDeserializedJSON formDeserializedJSON;
        public List<SpecificationNames> listSpecificationNames;
        public List<Countries> listCountriesNames;
        public IList<OrderAdditionalRoutePointModel> routePointsLoadinfo;
        public IList<OrderAdditionalRoutePointModel> routePointsUnloadinfo;
        public List<TenderServices> listTenderServices;
        public List<BalanceKeepers> listBalanceKeepers;
        public List<RegisterTenders> listRegisterTenders;
        NameValueCollection allAppSettings;

        public TenderForma()
        { }
        public TenderForma(CompetitiveListViewModel competitiveListViewModel, List<TenderServices> listTenderServices, List<BalanceKeepers> listBalanceKeepers, OrderTruckTransport orderTruckTransport, List<RegisterTenders> listRegisterTenders)
        {
            this.context = DependencyResolver.Current.GetService<ICorumDataProvider>();
            this.competitiveListViewModel = competitiveListViewModel;
            this.listTenderServices = listTenderServices;
            this.listBalanceKeepers = listBalanceKeepers;
            this.orderTruckTransport = orderTruckTransport;
            this.listRegisterTenders = listRegisterTenders;
            this.allAppSettings = ConfigurationManager.AppSettings;
            InitializeData.competitiveListViewModel = this.competitiveListViewModel;
            InitializeData.listBalanceKeepers = this.listBalanceKeepers;
            InitializeData.listTenderServices = this.listTenderServices;
            InitializeData.orderTruckTransport = this.orderTruckTransport;
            InitializeData.allAppSettings = this.allAppSettings;
            InitializeData.listRegisterTenders = this.listRegisterTenders;
            data = new DataTender<T>();
        }

        public TenderForma(CompetitiveListViewModel competitiveListViewModel, List<TenderServices> listTenderServices, List<BalanceKeepers> listBalanceKeepers, TendFormDeserializedJSON formDeserializedJSON, List<SpecificationNames> specificationNames, List<Countries> countries,
            OrderTruckTransport orderTruckTransport, IList<OrderAdditionalRoutePointModel> routePointsLoadInfo, IList<OrderAdditionalRoutePointModel> routePointsUnloadInfo)
        {
            this.otherParams = new Dictionary<string, string>();
            this.allAppSettings = ConfigurationManager.AppSettings;
            this.competitiveListViewModel = competitiveListViewModel;
            this.listTenderServices = listTenderServices;
            this.listBalanceKeepers = listBalanceKeepers;
            this.formDeserializedJSON = formDeserializedJSON;
            this.listSpecificationNames = specificationNames;
            this.listCountriesNames = countries;
            this.orderTruckTransport = orderTruckTransport;
            this.routePointsLoadinfo = routePointsLoadInfo;
            this.routePointsUnloadinfo = routePointsUnloadInfo;
            InitializeData.competitiveListViewModel = this.competitiveListViewModel;
            InitializeData.listBalanceKeepers = this.listBalanceKeepers;
            InitializeData.listTenderServices = this.listTenderServices;
            InitializeData.orderTruckTransport = this.orderTruckTransport;
            InitializeData.formDeserializedJSON = this.formDeserializedJSON;
            InitializeData.listCountriesNames = this.listCountriesNames;
            InitializeData.listSpecificationNames = this.listSpecificationNames;
            InitializeData.routePointsLoadinfo = this.routePointsLoadinfo;
            InitializeData.routePointsUnloadinfo = this.routePointsUnloadinfo;
            InitializeData.allAppSettings = this.allAppSettings;
            InitializeData.listRegisterTenders = this.listRegisterTenders;
            data = new DataTender<T>();
            this.otherParams = data.otherParams;
        }
    }

    //public class PropValues : TenderParamsDefaults   // Ручная установка значений атрибутов
    //{
    //    public string ДопТочкаЗагрузки1 { get; set; }
    //    public string ДопТочкаВыгрузки1 { get; set; }
    //    public string ДопТочкаЗагрузки2 { get; set; }
    //    public string ДопТочкаВыгрузки2 { get; set; }
    //    public string ДопТочкаЗагрузки6 { get; set; }
    //    public string ДопТочкаВыгрузки6 { get; set; }
    //}
    //public class PropValuesOne : PropValues  // Ручная установка значений атрибутов
    //{
    //    public string ДопТочкаЗагрузки3 { get; set; }
    //    public string ДопТочкаВыгрузки3 { get; set; }
    //    public string ДопТочкаЗагрузки4 { get; set; }
    //    public string ДопТочкаВыгрузки4 { get; set; }
    //    public string ДопТочкаЗагрузки5 { get; set; }
    //    public string ДопТочкаВыгрузки5 { get; set; }
    //}

    public class PropAliasValues  // Установка значений атрибутов через площадку Aps tender
    {
        public string WEIGHT { get; set; }
        public string ROUTE { get; set; }
        public string CARGO_NAME { get; set; }
        public string DOWNLOADDATEREQUIRED { get; set; }
        public string UNLOADINGDATEREQUIRED { get; set; }
        public string REQUIRED_NUMBER_OF_CARS { get; set; }
        public string SPECIALCONDITIONS { get; set; }
        public string DOWNLOAD_ADDRESS { get; set; }
        public string UNLOADING_ADDRESS { get; set; }
    }

    public class PropAliasValuesOne : PropAliasValues  // Установка значений атрибутов через площадку Aps tender
    {
        public string ADDLOADPOINT1 { get; set; }
        public string ADDUNLOADINGPOINT1 { get; set; }
    }
    public class PropAliasValuesTwo : PropAliasValuesOne // Установка значений атрибутов через площадку Aps tender
    {
        public string ADDLOADPOINT2 { get; set; }
        public string ADDUNLOADINGPOINT2 { get; set; }
    }
    public class PropAliasValuesThree : PropAliasValuesTwo // Установка значений атрибутов через площадку Aps tender
    {
        public string ADDLOADPOINT3 { get; set; }
        public string ADDUNLOADINGPOINT3 { get; set; }
    }
    public class PropAliasValuesFour : PropAliasValuesThree // Установка значений атрибутов через площадку Aps tender
    {
        public string ADDLOADPOINT4 { get; set; }
        public string ADDUNLOADINGPOINT4 { get; set; }
    }
    public class PropAliasValuesFive : PropAliasValuesFour // Установка значений атрибутов через площадку Aps tender
    {
        public string ADDLOADPOINT5 { get; set; }
        public string ADDUNLOADINGPOINT5 { get; set; }
    }
    public class Items<T> where T : new()  //Класс описывающий позицию лота (обязательное поле)
    {
        private string itemNAME, itemNOTE, itemEXTERNALN;
        private Dictionary<string, string> keyValuePairs { get; set; }
        public long nmcId { get; set; } // код номенклатуры (тип  данных long) 

        public Items()
        {
            //this.propValues = new List<PropValues>()   //!!!!! При ручной установке атрибутов необходимо раскомментировать данный блок кода!!!!!
            //{
            //    new PropValuesOne()
            //    {
            //        ДопТочкаЗагрузки2 = "Вторая доп точка загрузки",
            //        ДопТочкаВыгрузки2 = "Вторая доп точка выгрузки",
            //        ДопТочкаЗагрузки4 = "Четвертая доп точка загрузки",
            //        ДопТочкаВыгрузки4 = "Четвертая доп точка выгрузки",
            //         ДопТочкаЗагрузки1 = "Первая доп точка загрузки",
            //        ДопТочкаВыгрузки1 = "Первая доп точка выгрузки",
            //        ДопТочкаЗагрузки3 = "Третья доп точка загрузки",
            //        ДопТочкаВыгрузки3 = "Третья доп точка выгрузки"
            //    }
            //};
            var countryShortNameShipper = InitializeData.listCountriesNames.Find(x => x.Code == InitializeData.orderTruckTransport.ShipperCountryId).alpha2;
            var countryShortNameConseegnee = InitializeData.listCountriesNames.Find(x => x.Code == InitializeData.orderTruckTransport.ConsigneeCountryId).alpha2;
            var countryShortNameAddLoadPoint = "";
            var countryShortNameAddUnLoadPoint = "";
            try
            {
                countryShortNameAddLoadPoint = InitializeData.listCountriesNames.Find(x => x.Name == InitializeData.routePointsLoadinfo[0].CountryPoint).alpha2;
                countryShortNameAddUnLoadPoint = InitializeData.listCountriesNames.Find(x => x.Name == InitializeData.routePointsUnloadinfo[0].CountryPoint).alpha2;
            }
            catch
            {
                countryShortNameAddLoadPoint = "";
                countryShortNameAddUnLoadPoint = "";
            }

            string route = $"[{ InitializeData.competitiveListViewModel.ShipperCountryName}]|({ InitializeData.orderTruckTransport.Shipper}) - [‎{InitializeData.competitiveListViewModel.ConsigneeCountryName}]|({InitializeData.orderTruckTransport.Consignee})".Trim();  // Ограничение в количестве символов! Строка не должна быть слишком длинной! Иначе возинкнет ошибка запроса на тендер.
            if (route.Length > 100)
            {
                route = ($"[{ countryShortNameShipper}]|({ InitializeData.orderTruckTransport.Shipper})-[‎{countryShortNameConseegnee}]|({InitializeData.orderTruckTransport.Consignee})".Trim().Length <= 100) ? $"[{ countryShortNameShipper}]|({ InitializeData.orderTruckTransport.Shipper})-[‎{countryShortNameConseegnee}]|({InitializeData.orderTruckTransport.Consignee})".Trim() : $"[{ InitializeData.competitiveListViewModel.ShipperCountryName}] - [‎{InitializeData.competitiveListViewModel.ConsigneeCountryName}]";
            }
            string addLoadPoint = "";
            string addUnLoadPoint = "";

            if (InitializeData.routePointsLoadinfo.Count != 0 && InitializeData.routePointsUnloadinfo.Count != 0)
            {
                try
                {
                    addLoadPoint = $"[{countryShortNameAddLoadPoint}]|({InitializeData.routePointsLoadinfo[0].NamePoint}-{InitializeData.routePointsLoadinfo[0].CityAddress})".Trim();
                    if (addLoadPoint.Length > 100)
                    {
                        addLoadPoint = ($"[{countryShortNameAddLoadPoint}]|({InitializeData.routePointsLoadinfo[0].NamePoint}-{InitializeData.routePointsLoadinfo[0].CityPoint})".Trim().Length <= 100) ? $"[{countryShortNameAddLoadPoint}]|({InitializeData.routePointsLoadinfo[0].NamePoint}-{InitializeData.routePointsLoadinfo[0].CityPoint})".Trim() : $"[{countryShortNameAddLoadPoint}]|({InitializeData.routePointsLoadinfo[0].NamePoint})".Trim();
                    }
                }
                catch { addLoadPoint = "Отсутствует дополнительная точка загрузки"; }
                try
                {
                    addUnLoadPoint = $"[{countryShortNameAddUnLoadPoint}]|({InitializeData.routePointsUnloadinfo[0].NamePoint}-{InitializeData.routePointsUnloadinfo[0].CityAddress})".Trim();
                    if (addUnLoadPoint.Length > 100)
                    {
                        addUnLoadPoint = ($"[{countryShortNameAddUnLoadPoint}]|({InitializeData.routePointsUnloadinfo[0].NamePoint}-{InitializeData.routePointsUnloadinfo[0].CityPoint})".Trim().Length <= 100) ? $"[{countryShortNameAddUnLoadPoint}]|({InitializeData.routePointsUnloadinfo[0].NamePoint}-{InitializeData.routePointsUnloadinfo[0].CityPoint})".Trim() : $"[{countryShortNameAddUnLoadPoint}]|({InitializeData.routePointsUnloadinfo[0].NamePoint})".Trim();
                    }
                }
                catch { addUnLoadPoint = "Отсутствует дополнительная точка выгрузки"; }
            }
            else
            {
                addLoadPoint = "Отсутствует дополнительная точка загрузки";
                addUnLoadPoint = "Отсутствует дополнительная точка выгрузки";
            }
            string cargoName = (InitializeData.competitiveListViewModel.TruckDescription.Trim().Length <= 100) ? InitializeData.competitiveListViewModel.TruckDescription.Trim() : InitializeData.competitiveListViewModel.TruckDescription.Trim().Remove(100);
            string specCondition = (InitializeData.competitiveListViewModel.VehicleTypeName.Trim().Length <= 100) ? InitializeData.competitiveListViewModel.VehicleTypeName.Trim() : InitializeData.competitiveListViewModel.VehicleTypeName.Trim().Remove(100);
            string downloadAddress = (InitializeData.orderTruckTransport.ShipperAdress.Trim().Length <= 100) ? InitializeData.orderTruckTransport.ShipperAdress.Trim() : InitializeData.orderTruckTransport.ShipperAdress.Trim().Remove(100);
            string unloadAddress = (InitializeData.orderTruckTransport.ConsigneeAdress.Trim().Length <= 100) ? InitializeData.orderTruckTransport.ConsigneeAdress.Trim() : InitializeData.orderTruckTransport.ConsigneeAdress.Trim().Remove(100);
            int num = (InitializeData.routePointsLoadinfo.Count < InitializeData.routePointsUnloadinfo.Count) ? InitializeData.routePointsUnloadinfo.Count : InitializeData.routePointsLoadinfo.Count;
            switch (num)
            {
                case 0:
                    this.propAliasValues = new List<T>() { new T() };
                    (this.propAliasValues[0] as PropAliasValuesOne).WEIGHT = InitializeData.competitiveListViewModel.Weight + ", тн"; (this.propAliasValues[0] as PropAliasValuesOne).ROUTE = route; (this.propAliasValues[0] as PropAliasValuesOne).CARGO_NAME = cargoName; (this.propAliasValues[0] as PropAliasValuesOne).DOWNLOADDATEREQUIRED = InitializeData.competitiveListViewModel.FromDateRaw;
                    (this.propAliasValues[0] as PropAliasValuesOne).UNLOADINGDATEREQUIRED = InitializeData.competitiveListViewModel.ToDateRaw; (this.propAliasValues[0] as PropAliasValuesOne).REQUIRED_NUMBER_OF_CARS = "1"; (this.propAliasValues[0] as PropAliasValuesOne).SPECIALCONDITIONS = specCondition; (this.propAliasValues[0] as PropAliasValuesOne).ADDLOADPOINT1 = AddLoadPoint(1);
                    (this.propAliasValues[0] as PropAliasValuesOne).ADDUNLOADINGPOINT1 = AddUnLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesOne).DOWNLOAD_ADDRESS = downloadAddress; (this.propAliasValues[0] as PropAliasValuesOne).UNLOADING_ADDRESS = unloadAddress; break;
                case 1:
                    this.propAliasValues = new List<T>() { new T() };
                    (this.propAliasValues[0] as PropAliasValuesOne).WEIGHT = InitializeData.competitiveListViewModel.Weight + ", тн"; (this.propAliasValues[0] as PropAliasValuesOne).ROUTE = route; (this.propAliasValues[0] as PropAliasValuesOne).CARGO_NAME = cargoName; (this.propAliasValues[0] as PropAliasValuesOne).DOWNLOADDATEREQUIRED = InitializeData.competitiveListViewModel.FromDateRaw;
                    (this.propAliasValues[0] as PropAliasValuesOne).UNLOADINGDATEREQUIRED = InitializeData.competitiveListViewModel.ToDateRaw; (this.propAliasValues[0] as PropAliasValuesOne).REQUIRED_NUMBER_OF_CARS = "1"; (this.propAliasValues[0] as PropAliasValuesOne).SPECIALCONDITIONS = specCondition; (this.propAliasValues[0] as PropAliasValuesOne).ADDLOADPOINT1 = AddLoadPoint(1);
                    (this.propAliasValues[0] as PropAliasValuesOne).ADDUNLOADINGPOINT1 = AddUnLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesOne).DOWNLOAD_ADDRESS = downloadAddress; (this.propAliasValues[0] as PropAliasValuesOne).UNLOADING_ADDRESS = unloadAddress; break;
                case 2:
                    this.propAliasValues = new List<T>() { new T() };
                    (this.propAliasValues[0] as PropAliasValuesTwo).WEIGHT = InitializeData.competitiveListViewModel.Weight + ", тн"; (this.propAliasValues[0] as PropAliasValuesTwo).ROUTE = route; (this.propAliasValues[0] as PropAliasValuesTwo).CARGO_NAME = cargoName; (this.propAliasValues[0] as PropAliasValuesTwo).DOWNLOADDATEREQUIRED = InitializeData.competitiveListViewModel.FromDateRaw;
                    (this.propAliasValues[0] as PropAliasValuesTwo).UNLOADINGDATEREQUIRED = InitializeData.competitiveListViewModel.ToDateRaw; (this.propAliasValues[0] as PropAliasValuesTwo).REQUIRED_NUMBER_OF_CARS = "1"; (this.propAliasValues[0] as PropAliasValuesTwo).SPECIALCONDITIONS = specCondition; (this.propAliasValues[0] as PropAliasValuesTwo).ADDLOADPOINT1 = AddLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesTwo).ADDLOADPOINT2 = AddLoadPoint(2);
                    (this.propAliasValues[0] as PropAliasValuesTwo).ADDUNLOADINGPOINT1 = AddUnLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesTwo).ADDUNLOADINGPOINT2 = AddUnLoadPoint(2); (this.propAliasValues[0] as PropAliasValuesTwo).DOWNLOAD_ADDRESS = downloadAddress; (this.propAliasValues[0] as PropAliasValuesTwo).UNLOADING_ADDRESS = unloadAddress; break;
                case 3:
                    this.propAliasValues = new List<T>() { new T() };
                    (this.propAliasValues[0] as PropAliasValuesThree).WEIGHT = InitializeData.competitiveListViewModel.Weight + ", тн"; (this.propAliasValues[0] as PropAliasValuesThree).ROUTE = route; (this.propAliasValues[0] as PropAliasValuesThree).CARGO_NAME = cargoName; (this.propAliasValues[0] as PropAliasValuesThree).DOWNLOADDATEREQUIRED = InitializeData.competitiveListViewModel.FromDateRaw;
                    (this.propAliasValues[0] as PropAliasValuesThree).UNLOADINGDATEREQUIRED = InitializeData.competitiveListViewModel.ToDateRaw; (this.propAliasValues[0] as PropAliasValuesThree).REQUIRED_NUMBER_OF_CARS = "1"; (this.propAliasValues[0] as PropAliasValuesThree).SPECIALCONDITIONS = specCondition; (this.propAliasValues[0] as PropAliasValuesThree).ADDLOADPOINT1 = AddLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesThree).ADDLOADPOINT2 = AddLoadPoint(2);
                    (this.propAliasValues[0] as PropAliasValuesThree).ADDUNLOADINGPOINT1 = AddUnLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesThree).ADDUNLOADINGPOINT2 = AddUnLoadPoint(2); (this.propAliasValues[0] as PropAliasValuesThree).DOWNLOAD_ADDRESS = downloadAddress; (this.propAliasValues[0] as PropAliasValuesThree).UNLOADING_ADDRESS = unloadAddress; (this.propAliasValues[0] as PropAliasValuesThree).ADDLOADPOINT3 = AddLoadPoint(3);
                    (this.propAliasValues[0] as PropAliasValuesThree).ADDUNLOADINGPOINT3 = AddUnLoadPoint(3); break;
                case 4:
                    this.propAliasValues = new List<T>() { new T() };
                    (this.propAliasValues[0] as PropAliasValuesFour).WEIGHT = InitializeData.competitiveListViewModel.Weight + ", тн"; (this.propAliasValues[0] as PropAliasValuesFour).ROUTE = route; (this.propAliasValues[0] as PropAliasValuesFour).CARGO_NAME = cargoName; (this.propAliasValues[0] as PropAliasValuesFour).DOWNLOADDATEREQUIRED = InitializeData.competitiveListViewModel.FromDateRaw;
                    (this.propAliasValues[0] as PropAliasValuesFour).UNLOADINGDATEREQUIRED = InitializeData.competitiveListViewModel.ToDateRaw; (this.propAliasValues[0] as PropAliasValuesFour).REQUIRED_NUMBER_OF_CARS = "1"; (this.propAliasValues[0] as PropAliasValuesFour).SPECIALCONDITIONS = specCondition; (this.propAliasValues[0] as PropAliasValuesFour).ADDLOADPOINT1 = AddLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesFour).ADDLOADPOINT2 = AddLoadPoint(2);
                    (this.propAliasValues[0] as PropAliasValuesFour).ADDUNLOADINGPOINT1 = AddUnLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesFour).ADDUNLOADINGPOINT2 = AddUnLoadPoint(2); (this.propAliasValues[0] as PropAliasValuesFour).DOWNLOAD_ADDRESS = downloadAddress; (this.propAliasValues[0] as PropAliasValuesFour).UNLOADING_ADDRESS = unloadAddress; (this.propAliasValues[0] as PropAliasValuesFour).ADDLOADPOINT3 = AddLoadPoint(3);
                    (this.propAliasValues[0] as PropAliasValuesFour).ADDUNLOADINGPOINT3 = AddUnLoadPoint(3); (this.propAliasValues[0] as PropAliasValuesFour).ADDUNLOADINGPOINT4 = AddUnLoadPoint(4); (this.propAliasValues[0] as PropAliasValuesFour).ADDLOADPOINT4 = AddLoadPoint(4); break;
                case 5:
                    this.propAliasValues = new List<T>() { new T() };
                    (this.propAliasValues[0] as PropAliasValuesFive).WEIGHT = InitializeData.competitiveListViewModel.Weight + ", тн"; (this.propAliasValues[0] as PropAliasValuesFive).ROUTE = route; (this.propAliasValues[0] as PropAliasValuesFive).CARGO_NAME = cargoName; (this.propAliasValues[0] as PropAliasValuesFive).DOWNLOADDATEREQUIRED = InitializeData.competitiveListViewModel.FromDateRaw;
                    (this.propAliasValues[0] as PropAliasValuesFive).UNLOADINGDATEREQUIRED = InitializeData.competitiveListViewModel.ToDateRaw; (this.propAliasValues[0] as PropAliasValuesFive).REQUIRED_NUMBER_OF_CARS = "1"; (this.propAliasValues[0] as PropAliasValuesFive).SPECIALCONDITIONS = specCondition; (this.propAliasValues[0] as PropAliasValuesFive).ADDLOADPOINT1 = AddLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesFive).ADDLOADPOINT2 = AddLoadPoint(2);
                    (this.propAliasValues[0] as PropAliasValuesFive).ADDUNLOADINGPOINT1 = AddUnLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesFive).ADDUNLOADINGPOINT2 = AddUnLoadPoint(2); (this.propAliasValues[0] as PropAliasValuesFive).DOWNLOAD_ADDRESS = downloadAddress; (this.propAliasValues[0] as PropAliasValuesFive).UNLOADING_ADDRESS = unloadAddress; (this.propAliasValues[0] as PropAliasValuesFive).ADDLOADPOINT3 = AddLoadPoint(3);
                    (this.propAliasValues[0] as PropAliasValuesFive).ADDUNLOADINGPOINT3 = AddUnLoadPoint(3); (this.propAliasValues[0] as PropAliasValuesFive).ADDUNLOADINGPOINT4 = AddUnLoadPoint(4); (this.propAliasValues[0] as PropAliasValuesFive).ADDLOADPOINT4 = AddLoadPoint(4); (this.propAliasValues[0] as PropAliasValuesFive).ADDUNLOADINGPOINT5 = AddUnLoadPoint(5); (this.propAliasValues[0] as PropAliasValuesFive).ADDLOADPOINT5 = AddLoadPoint(5); break;
                default:
                    this.propAliasValues = new List<T>() { new T() };
                    (this.propAliasValues[0] as PropAliasValuesOne).WEIGHT = InitializeData.competitiveListViewModel.Weight + ", тн"; (this.propAliasValues[0] as PropAliasValuesOne).ROUTE = route; (this.propAliasValues[0] as PropAliasValuesOne).CARGO_NAME = cargoName; (this.propAliasValues[0] as PropAliasValuesOne).DOWNLOADDATEREQUIRED = InitializeData.competitiveListViewModel.FromDateRaw;
                    (this.propAliasValues[0] as PropAliasValuesOne).UNLOADINGDATEREQUIRED = InitializeData.competitiveListViewModel.ToDateRaw; (this.propAliasValues[0] as PropAliasValuesOne).REQUIRED_NUMBER_OF_CARS = "1"; (this.propAliasValues[0] as PropAliasValuesOne).SPECIALCONDITIONS = specCondition; (this.propAliasValues[0] as PropAliasValuesOne).ADDLOADPOINT1 = AddLoadPoint(1);
                    (this.propAliasValues[0] as PropAliasValuesOne).ADDUNLOADINGPOINT1 = AddUnLoadPoint(1); (this.propAliasValues[0] as PropAliasValuesOne).DOWNLOAD_ADDRESS = downloadAddress; (this.propAliasValues[0] as PropAliasValuesOne).UNLOADING_ADDRESS = unloadAddress; break;
            }
        }
        public string AddLoadPoint(int number)
        {
            var countryShortNameShipper = InitializeData.listCountriesNames.Find(x => x.Code == InitializeData.orderTruckTransport.ShipperCountryId).alpha2;
            var countryShortNameAddLoadPoint = "";
            try
            {
                countryShortNameAddLoadPoint = InitializeData.listCountriesNames.Find(x => x.Name == InitializeData.routePointsLoadinfo[number - 1].CountryPoint).alpha2;
            }
            catch
            {
                countryShortNameAddLoadPoint = "";
            }
            string addLoadPoint = "";
            try
            {
                addLoadPoint = $"[{countryShortNameAddLoadPoint}]|({InitializeData.routePointsLoadinfo[number - 1].NamePoint}-{InitializeData.routePointsLoadinfo[number - 1].CityAddress})".Trim();
                if (addLoadPoint.Length > 100)
                {
                    addLoadPoint = ($"[{countryShortNameAddLoadPoint}]|({InitializeData.routePointsLoadinfo[number - 1].NamePoint}-{InitializeData.routePointsLoadinfo[number - 1].CityPoint})".Trim().Length <= 100) ? $"[{countryShortNameAddLoadPoint}]|({InitializeData.routePointsLoadinfo[number - 1].NamePoint}-{InitializeData.routePointsLoadinfo[number - 1].CityPoint})".Trim() : $"[{countryShortNameAddLoadPoint}]|({InitializeData.routePointsLoadinfo[number - 1].NamePoint})".Trim();
                }
            }
            catch
            {
                addLoadPoint = "Отсутствует дополнительная точка загрузки";
            }
            return addLoadPoint;
        }

        public string AddUnLoadPoint(int number)
        {
            var countryShortNameConseegnee = InitializeData.listCountriesNames.Find(x => x.Code == InitializeData.orderTruckTransport.ConsigneeCountryId).alpha2;
            var countryShortNameAddUnLoadPoint = "";
            try
            {
                countryShortNameAddUnLoadPoint = InitializeData.listCountriesNames.Find(x => x.Name == InitializeData.routePointsUnloadinfo[number - 1].CountryPoint).alpha2;
            }
            catch
            {
                countryShortNameAddUnLoadPoint = "";
            }
            string addUnLoadPoint = "";
            try
            {
                addUnLoadPoint = $"[{countryShortNameAddUnLoadPoint}]|({InitializeData.routePointsUnloadinfo[number - 1].NamePoint}-{InitializeData.routePointsUnloadinfo[number - 1].CityAddress})".Trim();
                if (addUnLoadPoint.Length > 100)
                {
                    addUnLoadPoint = ($"[{countryShortNameAddUnLoadPoint}]|({InitializeData.routePointsUnloadinfo[number - 1].NamePoint}-{InitializeData.routePointsUnloadinfo[number - 1].CityPoint})".Trim().Length <= 100) ? $"[{countryShortNameAddUnLoadPoint}]|({InitializeData.routePointsUnloadinfo[number - 1].NamePoint}-{InitializeData.routePointsUnloadinfo[number - 1].CityPoint})".Trim() : $"[{countryShortNameAddUnLoadPoint}]|({InitializeData.routePointsUnloadinfo[number - 1].NamePoint})".Trim();
                }
            }
            catch
            {
                addUnLoadPoint = "Отсутствует дополнительная точка выгрузки";
            }
            return addUnLoadPoint;
        }
        public string itemName  // уточнение для текущего тура (тип  данных string, максимальнаядлина 300 символов) (необязательное поле)
        {
            get
            {
                return itemNAME;
            }
            set
            {
                itemNAME = (value.Length > 300) ? value.Substring(0, 299) : value;
            }
        }
        public double qty { get; set; }  // количество  (тип  данных double)
        public string itemNote  // описание позиции (тип  данных string, максимальная длина 300 символов) (необязательное поле)
        {
            get
            {
                return itemNOTE;
            }
            set
            {
                itemNOTE = (value.Length > 300) ? value.Substring(0, 299) : value;
            }
        }
        public string itemExternalN  // код строки из внешней учетной системы (тип  данных string, максимальная длина 20 символов)
        {
            get
            {
                return itemEXTERNALN;
            }
            set
            {
                itemEXTERNALN = (value.Length > 20) ? value.Substring(0, 19) : value;
            }
        }
        //public List<PropValues> propValues { get; set; }  // массив значений атрибутов item (необязательное поле)

        public List<T> propAliasValues { get; set; }  // массив значений словарных атрибутов (необязательное поле)


        //public long detailId { get; set; }  // код места поставки (тип  данных long) (необязательное поле)
    }


    public class Lots<T> where T : new()  // Класс описывающий лот тендера
    {

        public string lotName { get; set; }  // наименование лота (тип  данных string, максимальная длина 100 символов) (необязательное поле)

        //public string[] props { get; set; }  //  Атрибуты лота тендера
        public string[] propAliases { get; set; } // Алиасы словарных атрибутов (необязательное поле)


        //public List<Props> props { get; set; }  // массив атрибутов (необязательное поле)
        //public List<PropAliases> propAliases { get; set; }  // массив алиасов словарных атрибутов (необязательное поле)
        public List<Items<T>> items { get; set; }  // массив позиций лота (обязательное поле)

        public Lots()
        {
            this.lotName = "Лот №1";
            //props = new string[]
            //{
            //    "ДопТочкаЗагрузки2", "ДопТочкаВыгрузки2", "ДопТочкаЗагрузки3", "ДопТочкаВыгрузки3", "ДопТочкаЗагрузки4", "ДопТочкаВыгрузки4",
            //    //"ДопТочкаЗагрузки1", "ДопТочкаВыгрузки1", "ДопТочкаЗагрузки5", "ДопТочкаВыгрузки5", "ДопТочкаЗагрузки6", "ДопТочкаВыгрузки6"
            //};
            propAliases = new string[]
            {
                    //"WEIGHT"
                    //"ROUTE",
                    //"SPECIALCONDITIONS",
                    //"ADDUNLOADINGPOINT",
                    //"ADDLOADPOINT",
                    //"CARGO_NAME",
                    //"DOWNLOADDATEREQUIRED",
                    //"UNLOADINGDATEREQUIRED",
                    //"REQUIRED_NUMBER_OF_CARS",
                    //"DOWNLOAD_ADDRESS",
                    //"UNLOADING_ADDRESS"
            };

        }
    }




    public class DataTender<T> where T : new()// Тело data из формы тендера
    {
        private string tenderEXTERNALN, dateStartDef, dateEndDef;
        private int lightModeID;
        public string[] regums, typeTures, typePublications;
        public List<string> listTenders;
        public Dictionary<int, string> listTenderCategor;
        public SelectList listRegums, listTures, listServices, listPublications, listTendersToOrder;
        DateTime date = DateTime.Now;
        public Dictionary<string, string> otherParams;

        public DataTender()
        {
            Lots<T> lot = new Lots<T>();
            lots = new List<Lots<T>>();
            lots.Add(lot);
            switch (InitializeData.competitiveListViewModel.tripTypeName)
            {
                case "Международная":
                    {
                        tenderName = $"({InitializeData.competitiveListViewModel.Id}) {InitializeData.competitiveListViewModel.ShipperCountryName}, {InitializeData.competitiveListViewModel.CityFrom} ({InitializeData.orderTruckTransport.Shipper})" +
                                     $" - ‎{InitializeData.competitiveListViewModel.ConsigneeCountryName}, {InitializeData.competitiveListViewModel.CityTo}  ({InitializeData.orderTruckTransport.Consignee}), " +
                       $"{InitializeData.competitiveListViewModel.Weight}тн., погрузка {InitializeData.competitiveListViewModel.FromDate}";
                        break;
                    }
                default:
                    {
                        tenderName = $"({InitializeData.competitiveListViewModel.Id}) {InitializeData.competitiveListViewModel.CityFrom} ({InitializeData.orderTruckTransport.Shipper})" +
                                     $" - ‎{InitializeData.competitiveListViewModel.CityTo}  ({InitializeData.orderTruckTransport.Consignee}), груз - " +
                       $"{InitializeData.competitiveListViewModel.Weight}тн., дата погрузки {InitializeData.competitiveListViewModel.FromDate}";
                        break;
                    }
            }
            otherParams = new Dictionary<string, string>();
            tenderAuthorName = "Литовченко С.В.";
            tenderAuthorId = (tenderAuthorName != "Литовченко С.В.") ? 0 : Convert.ToInt64(InitializeData.allAppSettings["tenderAuthorId"]);
            companyName = "ООО «КОРУМ ГРУПП»";
            companyId = (companyName != "ООО «КОРУМ ГРУПП»") ? 0 : Convert.ToInt64(InitializeData.allAppSettings["companyId"]);
            regums = new string[] { "Облегченный" };
            int numTend = 0;
            listRegums = new SelectList(regums);
            do
            {
                if (numTend == 0)
                {
                    listTenders = new List<string> { "Новый тендер" };
                    if (InitializeData.listRegisterTenders == null)
                    { break; }
                }
                else
                {
                    listTenders.Add(InitializeData.listRegisterTenders[numTend - 1].tenderNumber.ToString());
                }
            }
            while (numTend++ < InitializeData.listRegisterTenders.Count);
            listTendersToOrder = new SelectList(listTenders);
            subCompanyName = InitializeData.competitiveListViewModel.PayerName;
            var subCompanyIdn = InitializeData.listBalanceKeepers.Find((x) => x.BalanceKeeper.Contains(subCompanyName)).subCompanyId;
            subCompanyId = (subCompanyIdn == null) ? 10 : Convert.ToInt64(subCompanyIdn);

            typeTures = new string[] { "Тендер RFx", "Аукцион/Редукцион" };
            listTures = new SelectList(typeTures);

            depName = "Департамент по логистике";
            depId = (depName != "Департамент по логистике") ? 0 : Convert.ToInt64(InitializeData.allAppSettings["depId"]);
            listTenderCategor = new Dictionary<int, string>();
            foreach (var item in InitializeData.listTenderServices.ToList())
            {
                listTenderCategor[item.industryId] = item.industryName;
            }
            listServices = new SelectList(listTenderCategor.Values);
            dateStartDef = date.AddHours(2).ToString("yyyy-MM-dd'T'HH:mm");
            dateEndDef = date.AddDays(10).ToString("yyyy-MM-dd'T'HH:mm");
            dateStart = dateStartDef;
            dateEnd = dateEndDef;

            typePublications = new string[] { "Открытый", "Закрытый" };
            listPublications = new SelectList(typePublications);
            Random random = new Random();
            tenderExternalN = InitializeData.competitiveListViewModel.Id.ToString() + "-" + random.Next(1, 10000).ToString();
            this.otherParams.Add("DOWNLOAD_ADDRESS", $"{InitializeData.competitiveListViewModel.ShipperCountryName}, {InitializeData.competitiveListViewModel.CityFrom} ({InitializeData.orderTruckTransport.Shipper})");
            this.otherParams.Add("UNLOADING_ADDRESS", $"{InitializeData.competitiveListViewModel.ConsigneeCountryName}, {InitializeData.competitiveListViewModel.CityTo}  ({InitializeData.orderTruckTransport.Consignee})");
            this.otherParams.Add("DOWNLOADDATEREQUIRED", $"{InitializeData.competitiveListViewModel.FromDateRaw}");
            this.otherParams.Add("UNLOADINGDATEREQUIRED", $"{InitializeData.competitiveListViewModel.ToDateRaw}");
            this.otherParams.Add("ROUTE", $"[{ InitializeData.competitiveListViewModel.ShipperCountryName}]|{InitializeData.competitiveListViewModel.CityFrom}({ InitializeData.orderTruckTransport.Shipper}) - [‎{InitializeData.competitiveListViewModel.ConsigneeCountryName}]|{InitializeData.competitiveListViewModel.CityTo}({InitializeData.orderTruckTransport.Consignee})".Trim());
            this.otherParams.Add("CARGO_NAME", $"{InitializeData.competitiveListViewModel.TruckDescription.Trim()}");
        }


        public void InitializedAfterDeserialized()
        {
            Random random = new Random();
            typeTrure = InitializeData.formDeserializedJSON.TypeTure;
            mode = (typeTures[0].Contains(typeTrure)) ? 1 : 2;

            budget = Convert.ToDouble(InitializeData.formDeserializedJSON.Budget);

            typePublic = InitializeData.formDeserializedJSON.TypePublic;
            kind = (typePublications[0].Contains(typePublic)) ? 1 : 2;

            industryName = InitializeData.formDeserializedJSON.IndustryName;
            industryId = InitializeData.listTenderServices.ToList().Find((x) => x.industryName.Contains(industryName)).industryId;

            dateStart = InitializeData.formDeserializedJSON.DateStart;
            dateEnd = InitializeData.formDeserializedJSON.DateEnd;

            this.lots[0].lotName = InitializeData.formDeserializedJSON.LotName;
            lotName = this.lots[0].lotName;

            lots[0].items = new List<Items<T>>();

            if (InitializeData.formDeserializedJSON.JqxGridNmc != null)
            {
                foreach (var item in InitializeData.formDeserializedJSON.JqxGridNmc)
                {
                    try
                    {
                        int countCars = Convert.ToInt32(item.Value.qty);
                        do
                        {
                            Items<T> items = new Items<T>();
                            items.qty = 1;
                            items.nmcId = Convert.ToInt64(InitializeData.listSpecificationNames.ToList().Find((x) => x.SpecName.Contains(item.Value.nmcName)).nmcWorkId);
                            items.itemExternalN = InitializeData.listSpecificationNames.ToList().Find((x) => x.SpecName.Contains(item.Value.nmcName)).SpecCode.ToString() + random.Next(10000).ToString();

                            lots[0].items.Add(items);
                        }
                        while (countCars-- > 1);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        [Display(Name = "Наименование тендера")]
        public string tenderName { get; set; }   //наименование тендера (тип данных string, максимальная длина 300 символов)


        [Display(Name = "Введите наименование тендера")]
        public string errorMessageTenderName { get; set; }   //ошибка при наименовании тендера

        [Display(Name = "Максимальная длина поля не больше 300 символов")]
        public string errorMessageTenderNameStringLength { get; set; }   //ошибка при превышении количества литералов в поле "Наименование тендера" более 300 символов



        [Display(Name = "Категория тендера")]
        public string industryName { get; set; }  // Название категории тендера
        public long industryId { get; set; }  //Id категории тендера (тип  данных long) 



        [Display(Name = "Бюджет тендера")]
        public double budget { get; set; }  //бюджет тендера (тип  данных double) (необязательное поле)

        [Display(Name = "Введите целое или вещественное число")]
        public string errorMessageTenderBudget { get; set; }   //ошибка ввода данных в поле "Бюджет тендера"




        [Display(Name = "Автор тендера")]
        public string tenderAuthorName { get; set; } // Имя автора тендера
        public long tenderAuthorId { get; set; }  //Id автора тендера (тип  данных long) 


        [Display(Name = "Группа компаний")]
        public string companyName { get; set; }  // Название группы компаний
        public long companyId { get; set; }  // Id группы компаний (тип  данных long)


        [Display(Name = "Компания заказчик")]
        [Required(ErrorMessage = "Введите название компании заказчика")]
        public string subCompanyName { get; set; }  // Название компании заказчика
        public long subCompanyId { get; set; }  //id компании–заказчика (тип  данных long) 



        [Display(Name = "Подразделение компании")]
        public string depName { get; set; }  // Название подразделения компании
        public long depId { get; set; }  // id подразделения (тип  данных long) (обязательное поле при включенной маршрутизации по подразделениям)



        public string tenderExternalN   // id тендера во внешней учетной системе (тип  данных string, максимальная длина 20 символов)
        {
            get
            {
                return tenderEXTERNALN;
            }
            set
            {
                tenderEXTERNALN = (value.Length > 20) ? value.Substring(0, 19) : value;
            }
        }


        [Display(Name = "Тип тура")]
        public string typeTrure { get; set; }  // Название типа тура
        public int mode { get; set; }   // тип тура (необязательное поле, тип int, 1 - Rfx, 2 - аукцион/редукцион, 3 -регистрация закупки, по умолчанию - 1)



        [Display(Name = "Тип публикации")]
        public string typePublic { get; set; }  // Название типа публикации
        public int kind { get; set; } // тип публикации (необязательное поле, тип int, 1 - открытая, 2 - закрытая,по умолчанию - 1)


        [Display(Name = "Дата начала приема предложений ")]
        public string dateStart { get; set; } // дата начала приема предложений (необязательное  поле,  тип данных string, дата в формате ISO 8601)
        [Display(Name = "Введите дату в формате дд.мм.гггг --:--")]
        public string errorMessageTenderDataStart { get; set; }   //ошибка ввода даты создания в поле "Дата создания тендера"
        [Display(Name = "Некорректная дата")]
        public string UncorrectMessageTenderDataStart { get; set; }   //ошибка ввода даты окончания тендера в поле "Дата окончания приема предложений"



        [Display(Name = "Дата окончания приёма предложений")]
        public string dateEnd { get; set; }  // дата конца приема предложений (необязательное поле, тип данных string, дата в формате ISO 8601)
        [Display(Name = "Введите дату в формате дд.мм.гггг --:--")]
        public string errorMessageTenderDataEnd { get; set; }   //ошибка ввода даты окончания тендера в поле "Дата окончания приема предложений"
        [Display(Name = "Некорректная дата")]
        public string UncorrectMessageTenderDataEnd { get; set; }   //ошибка ввода даты окончания тендера в поле "Дата окончания приема предложений"


        [Display(Name = "Наименование лота")]
        //[Required(ErrorMessage = "Введите наименование лота")]
        //[StringLength(100, ErrorMessage = "Максимальная длина поля не больше 100 символов")]
        public string lotName
        {
            get
            {
                return this.lots[0].lotName;
            }
            set
            {
                this.lots[0].lotName = value;
            }

        }  // наименование лота (тип  данных string, максимальная длина 100 символов) (необязательное поле)

        [Display(Name = "Введите наименование лота")]
        public string errorMessagelotName { get; set; }   //ошибка при наименовании лота

        [Display(Name = "Максимальная длина поля не больше 100 символов")]
        public string errorMessagelotNameStringLength { get; set; }   //ошибка при превышении количества литералов в поле "Наименование лота" более 100 символов





        public List<Lots<T>> lots { get; set; }  // массив лотов тендера (обязательное поле при необлегченном режиме)

        [Display(Name = "Режим подачи тендера")]
        public string regume { get; set; }  // Название режима подачи тендера
        public int lightMode
        {
            get
            {
                return (regume != "Облегченный") ? 0 : 1;
            }
            set
            {
                lightModeID = value;
            }
        }  // облегченный режим (необязательное поле, тип int, по умолчанию 0)
    }

}
