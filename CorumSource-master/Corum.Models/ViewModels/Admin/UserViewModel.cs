using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

// ReSharper disable All

namespace Corum.Models.ViewModels
{
    public class UserRolesViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public UserViewModel userInfo { get; set; }
        public IEnumerable<UserRoleViewModel> roles { get; set; }
    }

    public class UserRoleViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string roleId { get; set; }
        public string roleName { get; set; }
        public bool assigned { get; set; }
        public string roleDescription { get; set; }
        public string roleGroupName { get; set; }

    }

    public class UserRoleCloneViewModel
    {
        public string userId { get; set; }
        public IEnumerable<UserViewModel> users { get; set; }
    }

    public class UserViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string userId { get; set; }

        [Required(ErrorMessage = "Введите имя пользователя")]
        [Display(Name = "Имя пользователя")]
        [StringLength(256, ErrorMessage = "Максимальная длина поля не больше 255 символов")]
        public string displayName { get; set; }

        [Required(ErrorMessage = "Введите логин(e-mail)")]
        [Display(Name = "Логин")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Вы должны ввести e-mail пользователя")]
        [Remote("checkEmailExist", "Users", HttpMethod = "POST", ErrorMessage = "Пользователь с таким логином (e-mail) уже существует", AdditionalFields = "userId")]
        public string userEmail { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool isNewPassword { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Введите пароль")]
        public string userPassword { get; set; }

        [Display(Name = "Администратор")]
        public bool isAdmin { get; set; }

        [Display(Name = "Двухфазная аутентификация")]
        public bool twoFactorEnabled { get; set; }

        [Display(Name = "Уволен")]
        public bool Dismissed { get; set; }


        [Display(Name = "Должность")]
        public string postName { get; set; }


        [Display(Name = "Контактный телефон")]
        public string contactPhone{ get; set; }

        public IEnumerable<UserRoleViewModel> roles { get; set; }
        

        public UserViewModel() 
        {
            isNewPassword = false;
        }

    }
}
