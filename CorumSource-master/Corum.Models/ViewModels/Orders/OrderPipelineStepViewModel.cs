using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderPipelineStepViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Выберите начальный статус из списка")]
        [Display(Name = "Начальный статус")]
        public int FromStatus { get; set; }
        public string FromStatusName { get; set; }

        [Required(ErrorMessage = "Выберите конечный статус из списка")]
        [Display(Name = "Конечный статус")]
        [NotEqualTo("FromStatus", ErrorMessage = "Начальный и конечный статус не должны совпадать")]
        public int ToStatus { get; set; }
        public string ToStatusName { get; set; }

        [Required(ErrorMessage = "Выберите группу пользователей для осуществления шага")]
        [Display(Name = "Группу пользователей для осуществления шага")]
        public string AccessRoleId { get; set; }
        public string AccessRoleName { get; set; }

        [Display(Name = "Дата перехода - трактуется как дата утверждения клиентом")]
        public bool StartDateForClientLayer { get; set; }

        [Display(Name = "Дата перехода - трактуется как дата принятия в работу исполнителем")]
        public bool StartDateForExecuterLayer { get; set; }

        [Display(Name = "Цвет выделения")]
        public string FromStatusColor { get; set; }

        [Display(Name = "Цвет выделения")]
        public string ToStatusColor { get; set; }

        [Required(ErrorMessage = "Выберите тип заявки")]
        [Display(Name = "Тип заявки")]
        public int OrderTypeId { get; set; }

        public string OrderTypeName { get; set; }


        [Display(Name = "Финальный статус для процесса")]
        public bool FinishStatusForBP { get; set; }

        public List<OrderStatusViewModel> AvailiabeFromStatuses { get; set; }
        public List<OrderStatusViewModel> AvailiabeToStatuses { get; set; }
        public List<RoleViewModel> AvailiabeRoles { get; set; }
       
    }
}
