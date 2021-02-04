using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Corum.Models.ViewModels.Customers
{
    public class OrganizationViewModel : BaseViewModel
    {
        public long Id { set; get; }

        [Required(ErrorMessage = "Введите название организации")]
        [Display(Name = "Название организации")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Введите город")]
        [Display(Name = "Город")]
        public string City { set; get; }

        [Required(ErrorMessage = "Введите адрес")]
        [Display(Name = "Адрес")]
        public string Address { set; get; }

        public long IdFocus { set; get; }

        [Display(Name = "Страна")]
        public int? CountryId { set; get; }

        [Display(Name = "Страна")]
        public string Country { set; get; }

        [Display(Name = "Грузовые перевозки")]
        public bool IsTruck { set; get; }

        [Display(Name = "Широта")]
        public string Latitude { get; set; }

        [Display(Name = "Долгота")]
        public string Longitude { get; set; }

        [Display(Name = "Создано автоматически через заявку")]
        public bool IsAuto { get; set; }

        public string GoogleMapApiKey { get; set; }

        [Display(Name = "Системная фирма")]
        public bool IsSystemOrg { get; set; }

    }
}
