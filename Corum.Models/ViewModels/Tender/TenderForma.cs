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
    public class TenderForma : TenderParamsDefaults  // Модель тендера
    {
        public Dictionary<string, string> otherParams;
        public DataTender data { get; set; }
        public TenderForma()
        { }
        public TenderForma(CompetitiveListViewModel competitiveListViewModel, List<TenderServices> listTenderServices, List<BalanceKeepers> listBalanceKeepers, OrderTruckTransport orderTruckTransport) : base(competitiveListViewModel, listTenderServices, listBalanceKeepers, orderTruckTransport)
        {
            this.competitiveListViewModel = competitiveListViewModel;
            this.orderTruckTransport = orderTruckTransport;
            data = new DataTender();
        }

        public TenderForma(CompetitiveListViewModel competitiveListViewModel, List<TenderServices> listTenderServices, List<BalanceKeepers> listBalanceKeepers, TendFormDeserializedJSON formDeserializedJSON, List<SpecificationNames> specificationNames, List<Countries> countries,
            OrderTruckTransport orderTruckTransport, IList<OrderAdditionalRoutePointModel> routePointsLoadInfo, IList<OrderAdditionalRoutePointModel> routePointsUnloadInfo) : base(competitiveListViewModel, listTenderServices, listBalanceKeepers, formDeserializedJSON, specificationNames, countries, orderTruckTransport, routePointsLoadInfo, routePointsUnloadInfo)
        {
            this.otherParams = new Dictionary<string, string>();
            this.competitiveListViewModel = competitiveListViewModel;
            data = new DataTender();
            this.otherParams = data.otherParams;
            this.formDeserializedJSON = formDeserializedJSON;
            this.listSpecificationNames = specificationNames;
            this.listCountriesNames = countries;
            this.orderTruckTransport = orderTruckTransport;
            this.routePointsLoadinfo = routePointsLoadInfo;
            this.routePointsUnloadinfo = routePointsUnloadInfo;
        }
    }

    public class PropValues : TenderParamsDefaults   // Ручная установка значений атрибутов
    {
        public string WEIGHT { get; set; }
        public string ROUTE { get; set; }
        public string CARGO_NAME { get; set; }
        public string DOWNLOADDATEREQUIRED { get; set; }
        public string UNLOADINGDATEREQUIRED { get; set; }
        public string REQUIRED_NUMBER_OF_CARS { get; set; }
        public string SPECIALCONDITIONS { get; set; }
        public string ADDLOADPOINT { get; set; }
        public string ADDUNLOADINGPOINT { get; set; }
        public string DOWNLOAD_ADDRESS { get; set; }
        public string UNLOADING_ADDRESS { get; set; }
    }

    public class PropAliasValues : TenderParamsDefaults  // Установка значений атрибутов через площадку Aps tender
    {
        public string WEIGHT { get; set; }
        public string ROUTE { get; set; }
        public string CARGO_NAME { get; set; }
        public string DOWNLOADDATEREQUIRED { get; set; }
        public string UNLOADINGDATEREQUIRED { get; set; }
        public string REQUIRED_NUMBER_OF_CARS { get; set; }
        public string SPECIALCONDITIONS { get; set; }
        public string ADDLOADPOINT { get; set; }
        public string ADDUNLOADINGPOINT { get; set; }
        public string DOWNLOAD_ADDRESS { get; set; }
        public string UNLOADING_ADDRESS { get; set; }
    }

    public class Items : TenderParamsDefaults  //Класс описывающий позицию лота (обязательное поле)
    {
        private string itemNAME, itemNOTE, itemEXTERNALN;
        private Dictionary<string, string> keyValuePairs { get; set; }
        public long nmcId { get; set; } // код номенклатуры (тип  данных long) 
        public Items()
        {
            //this.propValues = new List<PropValues>()      !!!!! При ручной установке атрибутов необходимо раскомментировать данный блок кода  !!!!!
            //{
            //    new PropValues()
            //    {
            //        WEIGHT = competitiveListViewModel.Weight,
            //        ROUTE = competitiveListViewModel.Route,
            //        CARGO_NAME = competitiveListViewModel.TruckDescription,
            //        DOWNLOADDATEREQUIRED = competitiveListViewModel.FromDateRaw,
            //        UNLOADINGDATEREQUIRED = competitiveListViewModel.ToDateRaw,
            //        REQUIRED_NUMBER_OF_CARS = competitiveListViewModel.CarNumber.ToString(),
            //        SPECIALCONDITIONS = "Основная информация в заявке",
            //        ADDLOADPOINT = "Дополнительная точка загрузки отсутствует",
            //        ADDUNLOADINGPOINT = "Дополнительная точка выгрузки отсутствует"
            //    }
            //};
            var countryShortNameShipper = listCountriesNames.Find(x => x.Code == orderTruckTransport.ShipperCountryId).alpha2;
            var countryShortNameConseegnee = listCountriesNames.Find(x => x.Code == orderTruckTransport.ConsigneeCountryId).alpha2;
            var countryShortNameAddLoadPoint = "";
            var countryShortNameAddUnLoadPoint = "";
            try
            {
                countryShortNameAddLoadPoint = listCountriesNames.Find(x => x.Name == routePointsLoadinfo[0].CountryPoint).alpha2;
                countryShortNameAddUnLoadPoint = listCountriesNames.Find(x => x.Name == routePointsUnloadinfo[0].CountryPoint).alpha2;
            }
            catch
            {
                countryShortNameAddLoadPoint = "";
                countryShortNameAddUnLoadPoint = "";
            }

            string route = $"[{ competitiveListViewModel.ShipperCountryName}]|({ orderTruckTransport.Shipper}) - [‎{competitiveListViewModel.ConsigneeCountryName}]|({orderTruckTransport.Consignee})".Trim();  // Ограничение в количестве символов! Строка не должна быть слишком длинной! Иначе возинкнет ошибка запроса на тендер.
            if (route.Length > 100)
            {
                route = ($"[{ countryShortNameShipper}]|({ orderTruckTransport.Shipper})-[‎{countryShortNameConseegnee}]|({orderTruckTransport.Consignee})".Trim().Length <= 100) ? $"[{ countryShortNameShipper}]|({ orderTruckTransport.Shipper})-[‎{countryShortNameConseegnee}]|({orderTruckTransport.Consignee})".Trim() : $"[{ competitiveListViewModel.ShipperCountryName}] - [‎{competitiveListViewModel.ConsigneeCountryName}]";
            }
            string addLoadPoint = "";
            string addUnLoadPoint = "";
            if (routePointsLoadinfo.Count != 0)
            {
                addLoadPoint = $"[{countryShortNameAddLoadPoint}]|({routePointsLoadinfo[0].NamePoint}-{routePointsLoadinfo[0].CityAddress})".Trim();
                if (addLoadPoint.Length > 100)
                {
                    addLoadPoint = ($"[{countryShortNameAddLoadPoint}]|({routePointsLoadinfo[0].NamePoint}-{routePointsLoadinfo[0].CityPoint})".Trim().Length <= 100) ? $"[{countryShortNameAddLoadPoint}]|({routePointsLoadinfo[0].NamePoint}-{routePointsLoadinfo[0].CityPoint})".Trim() : $"[{countryShortNameAddLoadPoint}]|({routePointsLoadinfo[0].NamePoint})".Trim();
                }
                addUnLoadPoint = $"[{countryShortNameAddUnLoadPoint}]|({routePointsUnloadinfo[0].NamePoint}-{routePointsUnloadinfo[0].CityAddress})".Trim();
                if (addUnLoadPoint.Length > 100)
                {
                    addUnLoadPoint = ($"[{countryShortNameAddUnLoadPoint}]|({routePointsUnloadinfo[0].NamePoint}-{routePointsUnloadinfo[0].CityPoint})".Trim().Length <= 100) ? $"[{countryShortNameAddUnLoadPoint}]|({routePointsUnloadinfo[0].NamePoint}-{routePointsUnloadinfo[0].CityPoint})".Trim() : $"[{countryShortNameAddUnLoadPoint}]|({routePointsUnloadinfo[0].NamePoint})".Trim();
                }
            }
            else
            {
                addLoadPoint = "Отсутствует дополнительная точка загрузки";
                addUnLoadPoint = "Отсутствует дополнительная точка выгрузки";
            }
            string cargoName = (competitiveListViewModel.TruckDescription.Trim().Length <= 100) ? competitiveListViewModel.TruckDescription.Trim() : competitiveListViewModel.TruckDescription.Trim().Remove(100);
            string specCondition = (competitiveListViewModel.VehicleTypeName.Trim().Length <= 100) ? competitiveListViewModel.VehicleTypeName.Trim() : competitiveListViewModel.VehicleTypeName.Trim().Remove(100);
            string downloadAddress = (orderTruckTransport.ShipperAdress.Trim().Length <= 100) ? orderTruckTransport.ShipperAdress.Trim() : orderTruckTransport.ShipperAdress.Trim().Remove(100);
            string unloadAddress = (orderTruckTransport.ConsigneeAdress.Trim().Length <= 100) ? orderTruckTransport.ConsigneeAdress.Trim() : orderTruckTransport.ConsigneeAdress.Trim().Remove(100);

            this.propAliasValues = new List<PropAliasValues>()   // !!!!! При автоматической установке атрибутов необходимо раскомментировать данный блок кода!!!!!
            {
                  new PropAliasValues()
                  {
                      WEIGHT = competitiveListViewModel.Weight+", тн",
                      ROUTE = route,   // Максимальное значение 100 символов
                      CARGO_NAME = cargoName,
                      DOWNLOADDATEREQUIRED = competitiveListViewModel.FromDateRaw,
                      UNLOADINGDATEREQUIRED = competitiveListViewModel.ToDateRaw,
                      REQUIRED_NUMBER_OF_CARS = competitiveListViewModel.CarNumber.ToString(),
                      SPECIALCONDITIONS = specCondition,
                      ADDLOADPOINT = addLoadPoint,
                      ADDUNLOADINGPOINT = addUnLoadPoint,
                      DOWNLOAD_ADDRESS = downloadAddress,
                      UNLOADING_ADDRESS = unloadAddress
                  }
            };
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

        public List<PropAliasValues> propAliasValues { get; set; }  // массив значений словарных атрибутов (необязательное поле)

        //public long detailId { get; set; }  // код места поставки (тип  данных long) (необязательное поле)
    }


    public class Lots : TenderParamsDefaults  // Класс описывающий лот тендера
    {

        public string lotName { get; set; }  // наименование лота (тип  данных string, максимальная длина 100 символов) (необязательное поле)

        //public string[] props { get; set; }  //  Атрибуты лота тендера
        public string[] propAliases { get; set; } // Алиасы словарных атрибутов (необязательное поле)


        //public List<Props> props { get; set; }  // массив атрибутов (необязательное поле)
        //public List<PropAliases> propAliases { get; set; }  // массив алиасов словарных атрибутов (необязательное поле)
        public List<Items> items { get; set; }  // массив позиций лота (обязательное поле)

        public Lots()
        {
            this.lotName = "Лот №1";
            //props = new string[]
            //{
            //    "WEIGHT"
            //    //"Маршрут","Особые условия","Доп.точки выгрузки","Доп.точки загрузки",
            //    //"Наименование груза","Дата загрузки требуемая","Дата выгрузки требуемая","Требуемое кол-во автомобилей", "Адрес загрузки", "Адрес выгрузки"
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




    public class DataTender : TenderParamsDefaults// Тело data из формы тендера
    {
        private string tenderEXTERNALN, dateStartDef, dateEndDef;
        private int lightModeID;
        public string[] regums, typeTures, typePublications;
        public Dictionary<int, string> listTenderCategor;
        public SelectList listRegums, listTures, listServices, listPublications;
        DateTime date = DateTime.Now;
        public Dictionary<string, string> otherParams;
        public DataTender()
        {

            lots = new List<Lots>() { new Lots() };
            switch (competitiveListViewModel.tripTypeName)
            {
                case "Международная":
                    {
                        tenderName = $"({competitiveListViewModel.Id}) {competitiveListViewModel.ShipperCountryName}, {competitiveListViewModel.CityFrom} ({orderTruckTransport.Shipper})" +
                                     $" - ‎{competitiveListViewModel.ConsigneeCountryName}, {competitiveListViewModel.CityTo}  ({orderTruckTransport.Consignee}), " +
                       $"{competitiveListViewModel.Weight}тн., погрузка {competitiveListViewModel.FromDate}";
                        break;
                    }
                default:
                    {
                        tenderName = $"({competitiveListViewModel.Id}) {competitiveListViewModel.CityFrom} ({orderTruckTransport.Shipper})" +
                                     $" - ‎{competitiveListViewModel.CityTo}  ({orderTruckTransport.Consignee}), груз - " +
                       $"{competitiveListViewModel.Weight}тн., дата погрузки {competitiveListViewModel.FromDate}";
                        break;
                    }
            }
            otherParams = new Dictionary<string, string>();
            tenderAuthorName = "Литовченко С.В.";
            tenderAuthorId = (tenderAuthorName != "Литовченко С.В.") ? 0 : Convert.ToInt64(allAppSettings["tenderAuthorId"]);
            companyName = "ООО «КОРУМ ГРУПП»";
            companyId = (companyName != "ООО «КОРУМ ГРУПП»") ? 0 : Convert.ToInt64(allAppSettings["companyId"]);
            regums = new string[] { "Облегченный" };
            listRegums = new SelectList(regums);

            subCompanyName = competitiveListViewModel.PayerName;
            var subCompanyIdn = listBalanceKeepers.Find((x) => x.BalanceKeeper.Contains(subCompanyName)).subCompanyId;
            subCompanyId = (subCompanyIdn == null) ? 10 : Convert.ToInt64(subCompanyIdn);

            typeTures = new string[] { "Тендер RFx", "Аукцион/Редукцион" };
            listTures = new SelectList(typeTures);

            depName = "Департамент по логистике";
            depId = (depName != "Департамент по логистике") ? 0 : Convert.ToInt64(allAppSettings["depId"]);
            listTenderCategor = new Dictionary<int, string>();
            foreach (var item in listTenderServices.ToList())
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
            tenderExternalN = competitiveListViewModel.Id.ToString() + "-" + random.Next(1, 10000).ToString();
            this.otherParams.Add("DOWNLOAD_ADDRESS", $"{competitiveListViewModel.ShipperCountryName}, {competitiveListViewModel.CityFrom} ({orderTruckTransport.Shipper})");
            this.otherParams.Add("UNLOADING_ADDRESS", $"{competitiveListViewModel.ConsigneeCountryName}, {competitiveListViewModel.CityTo}  ({orderTruckTransport.Consignee})");
            this.otherParams.Add("DOWNLOADDATEREQUIRED", $"{competitiveListViewModel.FromDateRaw}");
            this.otherParams.Add("UNLOADINGDATEREQUIRED", $"{competitiveListViewModel.ToDateRaw}");
            this.otherParams.Add("ROUTE", $"[{ competitiveListViewModel.ShipperCountryName}]|{competitiveListViewModel.CityFrom}({ orderTruckTransport.Shipper}) - [‎{competitiveListViewModel.ConsigneeCountryName}]|{competitiveListViewModel.CityTo}({orderTruckTransport.Consignee})".Trim());
            this.otherParams.Add("CARGO_NAME", $"{competitiveListViewModel.TruckDescription.Trim()}");
        }


        public void InitializedAfterDeserialized()
        {
            Random random = new Random();
            typeTrure = this.formDeserializedJSON.TypeTure;
            mode = (typeTures[0].Contains(typeTrure)) ? 1 : 2;

            budget = Convert.ToDouble(this.formDeserializedJSON.Budget);

            typePublic = this.formDeserializedJSON.TypePublic;
            kind = (typePublications[0].Contains(typePublic)) ? 1 : 2;

            industryName = this.formDeserializedJSON.IndustryName;
            industryId = listTenderServices.ToList().Find((x) => x.industryName.Contains(industryName)).industryId;

            dateStart = this.formDeserializedJSON.DateStart;
            dateEnd = this.formDeserializedJSON.DateEnd;

            this.lots[0].lotName = this.formDeserializedJSON.LotName;
            lotName = this.lots[0].lotName;

            lots[0].items = new List<Items>();
            if (this.formDeserializedJSON.JqxGridNmc != null)
            {
                foreach (var item in this.formDeserializedJSON.JqxGridNmc)
                {
                    try
                    {
                        int countCars = Convert.ToInt32(item.Value.qty);
                        do
                        {
                            Items items = new Items();
                            items.qty = 1;
                            items.nmcId = Convert.ToInt64(listSpecificationNames.ToList().Find((x) => x.SpecName.Contains(item.Value.nmcName)).nmcWorkId);
                            items.itemExternalN = listSpecificationNames.ToList().Find((x) => x.SpecName.Contains(item.Value.nmcName)).SpecCode.ToString()+random.Next(10000).ToString();

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





        public List<Lots> lots { get; set; }  // массив лотов тендера (обязательное поле при необлегченном режиме)

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
