using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corum.Models;
using System.ComponentModel.DataAnnotations;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderClientsViewModel: BaseViewModel
    {
        [Display(Name = "Код клиента")]
        public long Id { set; get; }

        [Required(ErrorMessage = "Введите подразделение заказчика")]
        [Display(Name = "Подразделение заказчика")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля не больше 500 символов")]
        public string ClientName { set; get; }
        
        public string ClientBalanceKeeperName { set; get; }

        [Required(ErrorMessage = "Выберите ЦФО")]
        [Display(Name = "ЦФО")]
        public int ClientCFOId { set; get; }

        public string ClientCFOName { set; get; }
                
        [Required(ErrorMessage = "Введите роль пользователей")]
        [Display(Name = "Доступен в личном кабинете для следующей группы пользователей")]
        public string AccessRoleId { set; get; }
        
        public string RoleName { set; get; }

        public List<RoleViewModel> AvailableRoles { get; set; }
        public List<OrderClientBalanceKeeperViewModel> AvailableKeepers { get; set; }
        public List<OrderClientCFOViewModel> AvailableCFOs { get; set; }
    }

    public class ChildrenRoles
    {
        public string id { set; get; }

        public string text { set; get; }
    }

    public class AvailableRoles
    {
        public string text { set; get; }

        public List<ChildrenRoles> children { set; get; }
}
}
