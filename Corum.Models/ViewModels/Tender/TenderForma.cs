using Corum.Models.Tender;
using Corum.Models.ViewModels.OrderConcurs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
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
        public DataTender data { get; set; }
        public TenderForma()
        { }
        public TenderForma(CompetitiveListViewModel competitiveListViewModel, List<TenderServices> listTenderServices) : base(competitiveListViewModel, listTenderServices)
        {
            this.competitiveListViewModel = competitiveListViewModel;
            data = new DataTender();
        }
    }


    public class Items : TenderParamsDefaults  //Класс описывающий позицию лота (обязательное поле)
    {
        private string itemNAME, itemNOTE, itemEXTERNALN;
        public long nmcId { get; set; } // код номенклатуры (тип  данных long) 
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
        //public List<PropAliasValues> propAliasValues { get; set; }  // массив значений словарных атрибутов (необязательное поле)

        public long detailId { get; set; }  // код места поставки (тип  данных long) (необязательное поле)
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
            //    //"Наименование груза","Вес, т","Упаковка","Габариты","Дата подачи"
            //    //,
            //    //"Дата выгрузки","Маршрут","Расстояние, км","Требуемое кол-во автомобилей","Тип заявки (плановая/срочная)"
            //};
            propAliases = new string[]
            {
                    //"CARGO_NAME",
                    //"WEIGHT",
                    //"PACKAGE",
                    //"DIMENSIONS",
                    //"APPLICATION_DATE",
                    //"UPLOAD_DATE",
                    //"ROUTE",
                    //"DISTANCE",
                    //"REQUIRED_NUMBER_OF_CARS",
                    //"APPLICATION_TYPE"
            };
            items = new List<Items>()
            {
                new Items()
                {
                    qty = 1,
                    itemExternalN = "656523",
                    nmcId = 349,
                    detailId = 6
                },
                new Items()
                {
                    qty = 2,
                    itemExternalN = "562325",
                    nmcId = 321,
                    detailId = 6
                }
            };
        }
    }




    public class DataTender : TenderParamsDefaults// Тело data из формы тендера
    {
        private string dateEND, tenderEXTERNALN, dateStartDef;
        private long industryID;
        private int modeID, kindID, lightModeID;
        public string[] regums, typeTures, typePublications;
        public Dictionary<int, string> listTenderCategor;
        public SelectList listRegums, listTures, listServices, listPublications;
        DateTime date = DateTime.Now;
        public DataTender()
        {
            lots = new List<Lots>() { new Lots() };
            tenderName = $"({competitiveListViewModel.Id}) Перевозка по маршруту: «‎{competitiveListViewModel.Route}». " +
                                     $"Дата подачи/выгрузки: «‎{competitiveListViewModel.ToDate}/{competitiveListViewModel.ToDateRaw}». " +
                       $"Наименование груза: «‎{competitiveListViewModel.TruckDescription}».";
            tenderAuthorName = "Литовченко С.В.";
            tenderAuthorId = (tenderAuthorName != "Литовченко С.В.") ? 0 : Convert.ToInt64(allAppSettings["tenderAuthorId"]);
            companyName = "ООО «КОРУМ ГРУПП»";
            companyId = (companyName != "ООО «КОРУМ ГРУПП»") ? 0 : Convert.ToInt64(allAppSettings["companyId"]);
            regums = new string[] { "Облегченный" };
            listRegums = new SelectList(regums);

            subCompanyName = competitiveListViewModel.PayerName;
            subCompanyId = (subCompanyName != competitiveListViewModel.PayerName) ? 0 : Convert.ToInt64(allAppSettings["subCompanyId"]);
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
            dateStartDef = date.ToString("yyyy-MM-dd'T'HH:mm");
            dateStart = dateStartDef;

            typePublications = new string[] { "Открытый", "Закрытый" };
            listPublications = new SelectList(typePublications);
            Random random = new Random();
            tenderExternalN = competitiveListViewModel.Id.ToString() + "-" + random.Next(1, 10000).ToString();
        }


        [Display(Name = "Наименование тендера")]
        [Required(ErrorMessage = "Введите название тендера")]
        [StringLength(300, ErrorMessage = "Максимальная длина поля не больше 300 символов")]
        public string tenderName { get; set; }   //наименование тендера (тип данных string, максимальная длина 300 символов)


        [Display(Name = "Категория тендера")]
        public string industryName { get; set; }  // Название категории тендера
        public long industryId
        {
            get
            {
                foreach (var items in listTenderCategor)
                {
                    if (items.Value.Contains(industryName))
                        return items.Key;
                }
                return 0;
            }

            set
            {
                industryID = value;
            }
        }  //Id категории тендера (тип  данных long) 



        [Display(Name = "Бюджет тендера")]
        [Required(ErrorMessage = "Введите целое или вещественное число")]
        public double budget { get; set; }  //бюджет тендера (тип  данных double) (необязательное поле)


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
        public int mode
        {
            get
            {
                return (typeTrure != "Тендер RFx") ? 2 : 1;
            }
            set
            {
                modeID = value;
            }

        } // тип тура (необязательное поле, тип int, 1 - Rfx, 2 - аукцион/редукцион, 3 -регистрация закупки, по умолчанию - 1)



        [Display(Name = "Тип публикации")]
        public string typePublic { get; set; }  // Название типа публикации
        public int kind
        {
            get
            {
                return (typePublic != "Открытый") ? 2 : 1;
            }
            set
            {
                kindID = value;
            }
        } // тип публикации (необязательное поле, тип int, 1 - открытая, 2 - закрытая,по умолчанию - 1)


        [Display(Name = "Дата создания тендера")]
        public string dateStart { get; set; } // дата начала приема предложений (необязательное  поле,  тип данных string, дата в формате ISO 8601)


        [Display(Name = "Дата окончания приёма предложений")]
        [Required(ErrorMessage = "Введите дату в формате дд.мм.гггг --:--")]
        public string dateEnd { get; set; }  // дата конца приема предложений (необязательное поле, тип данных string, дата в формате ISO 8601)



        [Display(Name = "Наименование лота")]
        [Required(ErrorMessage = "Введите наименование лота")]
        [StringLength(100, ErrorMessage = "Максимальная длина поля не больше 100 символов")]
        public string lotName            // наименование лота (тип  данных string, максимальная длина 100 символов) (необязательное поле)
        {
            get
            {
                return this.lots[0].lotName;
            }
            set { this.lots[0].lotName = value; }
        }  


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
