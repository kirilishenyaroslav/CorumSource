using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderAdditionalRoutePointModel
    {
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }

        public long IdTemp { get; set; }

        public bool IsSaved { get; set; }
        public bool IsSavedToDelete { get; set; }

        [Required(ErrorMessage = "Введите точку выгрузки/загрузки")]
        [Display(Name = "Название  точки выгрузки/загрузки")]
        public long RoutePointId { get; set; }
        
        public string NamePoint { set; get; }

        public string CityPoint { set; get; }

        public string AddressPoint { set; get; }

        public string CountryPoint { set; get; }

        [Display(Name = "Заявка")]
        public long? OrderId { get; set; }

        [Display(Name = "Тип загрузка/выгрузка")]
        public bool? IsLoading { get; set; }

        [Display(Name = "Контактное лицо")]
        public string ContactPerson { set; get; }

        [Display(Name = "Контактный телефон")]
        public string ContactPersonPhone { set; get; }

        public string Contacts{ set; get; }

        public string CityAddress { set; get; }

        [Display(Name = "Номер по порядку доп точки")]
        public int NumberPoint { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
