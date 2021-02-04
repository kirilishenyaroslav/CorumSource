using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderPayerViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите плательщика")]
        [Display(Name = "Название плательщика")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля не больше 500 символов")]
        public string PayerName { get; set; }        
    }
}
