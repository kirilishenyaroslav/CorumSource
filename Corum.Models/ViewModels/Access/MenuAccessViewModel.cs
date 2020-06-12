using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Corum.Models
{
    public class ManageMenuAccessViewModel
    {
        
        public string JSONData { get; set; }
    }


    public class MenuAccessViewModel
    {
        public int Id { get; set; }
        public string menuName { get; set; }
        public string menuHtmlId { get; set; }
        public int? parentId { get; set; }
        public int menuIdForScript { get; set; }
        public bool isLeaf { get; set; }
    }

    public class MenuRolesViewModel
    {     
        public MenuAccessViewModel MenuInfo { get; set; }
        public IEnumerable<MenuRoleViewModel> Roles { get; set; }
    }

    public class MenuRoleViewModel
    {     
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Assigned { get; set; }
    }
    public class MenuUserViewModel
    {
        public IEnumerable<MenuAccessViewModel> MenuItems { get; set; }
        public ICorumDataProvider context { get; set; }
        public string currentUserId { get; set; }

    }


}
