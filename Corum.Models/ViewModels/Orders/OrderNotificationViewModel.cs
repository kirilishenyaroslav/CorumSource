using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderNotificationViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }

        public DateTime Datetime { get; set; }

        [Required(ErrorMessage = "Введите тип уведомления")]
        [Display(Name = "Тип уведомления")]
        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        [Display(Name = "Сообщение")]
        public string Body { get; set; }

        [Required]
        public long OrderId { get;  set;}

        [Required(ErrorMessage = "Выберите получателя")]
        [Display(Name = "Получатель")]
        public string Receiver { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }

        public List<OrderNotificationTypesViewModel> AvaliableNotifTypes { get; set; } 
        public List<UserViewModel> AvialiableRecievers { get; set; }
    }
}
