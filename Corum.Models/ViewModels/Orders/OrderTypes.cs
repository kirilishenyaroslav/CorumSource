using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderTypeViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название типа заявки")]
        [Display(Name = "Название типа заявки")]
        public string TypeName { get; set; }

        [Required(ErrorMessage = "Введите сокращенное наименование типа")]
        [Display(Name = "Сокращенное наименование типа")]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "Выберите роль")]
        [Display(Name = "Роль пользователей для доступа к клиентским данным заявки")]
        public string UserRoleIdForClientData { get; set; }
        
        public string UserRoleIdForClientDataName { get; set; }

        [Required(ErrorMessage = "Выберите роль")]
        [Display(Name = "Роль пользователей для доступа к данным исполнителя")]
        public string UserRoleIdForExecuterData { get; set; }

        public string UserRoleIdForExecuterDataName { get; set; }

        [Required(ErrorMessage = "Выберите исполнителя по умолчанию")]
        [Display(Name = "Пользователь - исполнитель по умолчанию")]
        public string DefaultExecuterId { get; set; }

        public string DefaultExecuterName { get; set; }

        [Required(ErrorMessage = "Выберите пользователя для не персонифицированной формы")]
        [Display(Name = "Пользователь - для не персонифицированной формы")]
        public string UserIdForAnonymousForm { get; set; }

        public string UserIdForAnonymousFormName { get; set; }

        [Display(Name = "Доступен для использования")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Выберите роль")]
        [Display(Name = "Роль пользователей для достпа к типу заявки")]
        public string UserRoleIdForTypeAccess { get; set; }
        public string UserRoleNameTypeAccess { get; set; }

        [Display(Name = "Транспортная заявка?")]
        public bool IsTransportType { get; set; }

        [Required(ErrorMessage = "Выберите роль")]
        [Display(Name = "Роль пользователей для доступа к конкурентным листам")]
        public string UserRoleIdForCompetitiveList { get; set; }

        public string UserRoleIdForCompetitiveListName { get; set; }

        public List<RoleViewModel> AvaliableRoles { get; set; }
        public List<UserViewModel> AvaliableUsers { get; set; }
    }
}
