using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Corum.Models.ViewModels
{
   public class UserProfileViewModel : BaseViewModel
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }

        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Display(Name = "Страна")]
        public int? CountryId { get; set; }

        public string Country { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Адрес отправления")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля не больше 500 символов")]
        public string AdressFrom { get; set; }

        public string Photo { get; set; }

        public int? LanguageId { get; set; }

        [Display(Name = "Дашборд по умолчанию")]
        public bool isFinishStatuses { get; set; }
    }
}
