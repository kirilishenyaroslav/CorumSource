using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderStatusViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название статуса заявки")]
        [Display(Name = "Название статуса заявки")]
        public string StatusName { get; set; }

        [Required(ErrorMessage = "Введите цвет выделения")]
        [Display(Name = "Цвет выделения")]
        public string StatusColor { get; set; }

        public bool AllowRegData { get; set; }

        [Display(Name = "Редактирование данных о клиенте")]
        public bool AllowClientData { get; set; }

        [Display(Name = "Редактирование данных о исполнителе")]
        public bool AllowExecuterData { get; set; }

        [Required(ErrorMessage = "Введите наименование действия")]
        [Display(Name = "Наименование действия")]
        public string ActionName { get; set; }

        [Display(Name = "Иконка")]
        public string IconFile { get; set; }

        [Display(Name = "Описание статуса для подсказки")]
        public string IconDescription { get; set; }

        [Display(Name = "Сокращенное наименование")]
        [StringLength(25, ErrorMessage = "Максимальная длина поля не больше 25 символов")]
        public string ShortName { get; set; }

        [Display(Name = "Цвет шрифта")]
        [StringLength(25, ErrorMessage = "Максимальная длина поля не больше 25 символов")]
        public string FontColor { get; set; }

        [Display(Name = "Цвет фона")]
        [StringLength(25, ErrorMessage = "Максимальная длина поля не больше 25 символов")]
        public string BackgroundColor { get; set; }
    }
}
