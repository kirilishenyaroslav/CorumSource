using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Tender
{
    public class TenderForma  // Общая форма тендера
    {
        public DataTender data { get; set; }


    }

    public class PropAliasValues  // Класс опивающий значения словарных атрибутов 
    {
        public string propAliasVal { get; set; }  // Значение словарного атрибута
    }

    public class PropValues  // Класс описывающий атрибуты items
    {
        public string propValItems { get; set; }  // Атрибут item
    }


    public class Items  //Класс описывающий позицию лота (обязательное поле)
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


    public class PropAliases  // Класс описывающий алиасы словарных атрибутов (необязательное поле)
    {
        public string aliase { get; set; }
    }


    public class Props  // Класс описывающий атрибуты лота тендера
    {
        public string properties { get; set; }
    }



    public class Lots  // Класс описывающий лот тендера
    {
        private string lotNAME;
        public string lotName  // наименование лота (тип  данных string, максимальная длина 100 символов) (необязательное поле)
        {
            get
            {
                return lotNAME;
            }
            set
            {
                lotNAME = (value.Length > 100) ? value.Substring(0, 99) : value;
            }
        }
        //public List<Props> props { get; set; }  // массив атрибутов (необязательное поле)
        //public List<PropAliases> propAliases { get; set; }  // массив алиасов словарных атрибутов (необязательное поле)
        public List<Items> items { get; set; }  // массив позиций лота (обязательное поле)

    }




    public class DataTender // Тело data из формы тендера
    {
        private string dateSTART, dateEND, tenderNAME, tenderEXTERNALN, tenderHEAD;

        public string tenderName   //наименование тендера (тип данных string, максимальная длина 300 символов)
        {
            get
            {
                return tenderNAME;
            }
            set
            {
                tenderNAME = (value.Length > 300) ? value.Substring(0, 299) : value;
            }
        }
        public long industryId { get; set; }  //Id категории тендера (тип  данных long) 
        public double budget { get; set; }  //бюджет тендера (тип  данных double) (необязательное поле)
        public long tenderAuthorId { get; set; }  //Id автора тендера (тип  данных long) 
        public long companyId { get; set; }  // Id группы компаний (тип  данных long)
        public long subCompanyId { get; set; }  //id компании–заказчика (тип  данных long) 
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
        //public string tenderHead   //глава тендерной комиссии (тип  данных string, максимальная длина 20 символов) (поле обязательное при включенной в настройке'TENDER.USE_TND_HEAD')
        //{
        //    get
        //    {
        //        return tenderHEAD;
        //    }
        //    set
        //    {
        //        tenderHEAD = (value.Length > 20) ? value.Substring(0, 19) : value;
        //    }
        //}
        public int mode { get; set; } // тип тура (необязательное поле, тип int, 1 - Rfx, 2 - аукцион/редукцион, 3 -регистрация закупки, по умолчанию - 1)
        public int kind { get; set; } // тип публикации (необязательное поле, тип int, 1 - открытая, 2 - закрытая,по умолчанию - 1)
        public string dateStart  // дата начала приема предложений (необязательное  поле,  тип данных string, дата в формате ISO 8601)
        {
            get
            {
                return dateSTART;
            }
            set
            {
                DateTime date = DateTime.Parse(value);
                dateSTART = date.ToString("O", CultureInfo.InvariantCulture);
            }
        }
        public string dateEnd  // дата конца приема предложений (необязательное поле, тип данных string, дата в формате ISO 8601)
        {
            get
            {
                return dateEND;
            }
            set
            {
                DateTime date = DateTime.Parse(value);
                dateEND = date.ToString("O", CultureInfo.InvariantCulture);
            }
        }

        public List<Lots> lots { get; set; }  // массив лотов тендера (обязательное поле при необлегченном режиме)

        public int lightMode { get; set; }  // облегченный режим (необязательное поле, тип int, по умолчанию 0)
    }

}
