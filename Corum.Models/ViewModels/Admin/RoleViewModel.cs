using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Barnivann.Models;

namespace Corum.Models.ViewModels
{
    public class RoleViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string roleId { get; set; }

        [Required(ErrorMessage = "Введите роль")]
        [Display(Name = "Название роли")]        
        public string roleName { get; set; }

        [Display(Name = "Описание роли")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля не больше 500 символов")]
        public string roleDescription { get; set; }

        public int roleGroupsId { get; set; }
    }

  
    }
