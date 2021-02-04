using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Barnivann.Models;

namespace Corum.Models.ViewModels
{
    public class ManageGroupRolesAccessViewModel
    {
        public string JSONData { get; set; }
    }


    public class GroupRolesAccessViewModel
    {        
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите группу ролей")]
        [Display(Name = "Название группы ролей")]                 
        public string GroupRolesName { get; set; }      
        [Display(Name = "Родительская группа ролей")]
        //[Required(ErrorMessage = "Выберите родителя")]
        public int? parentId { get; set; }
        public int IdForScript { get; set; }

        public bool is_Leaf { get; set; }

        public bool CanBeDelete { get; set; }

        [Display(Name = "Корневой элемент")]
        public bool isRoot { get; set; }

        public List<GroupRolesAccessViewModel> AvailableRoleGroups { get; set; }        
    }

    public class GroupRolesAllViewModel
    {
        public string JSONData { get; set; }
        public GroupRolesAccessViewModel GroupRolesInfo { get; set; }
        public IEnumerable<GroupRoleViewModel> Roles { get; set; }
    }

    public class GroupRolesViewModel
    {
        public GroupRolesAccessViewModel GroupRolesInfo { get; set; }
        public IEnumerable<GroupRoleViewModel> Roles { get; set; }
    }

    public class RoleGroupViewModel
    {
        public GroupRolesAccessViewModel GroupRolesInfo { get; set; }
        public RoleViewModel Roles { get; set; }
    }

    public class GroupRoleViewModel
    {
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Введите роль")]
        [Display(Name = "Название роли")]
        public string RoleName { get; set; }
        public bool Assigned { get; set; }

        [Display(Name = "Описание роли")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля не больше 500 символов")]
        public string RoleDescription { get; set; }
        public bool CanBeDelete { get; set; }
        public DateTime? RoleGroupsDate { get; set; }
    }

    public class RoleGroupsViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите группу ролей")]
        [Display(Name = "Название группы ролей")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Выберите родителя")]
        [Display(Name = "Родительская группа ролей")]
        public int? parentId { get; set; }
        
        [Display(Name = "Корневой элемент")]
        public bool isRoot { get; set; }
        

        public List<GroupRolesAccessViewModel> AvailableRoleGroups { get; set; }

    }

  
    }
