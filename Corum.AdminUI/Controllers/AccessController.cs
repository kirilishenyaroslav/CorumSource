using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Corum.Models;
using Newtonsoft.Json;
using Corum.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace CorumAdminUI.Controllers
{
    [Authorize]
    public class AccessController : CorumBaseController
    {
        
        [ChildActionOnly]
        //[NoCache]
        //[OutputCache(VaryByParam = "*", Duration = 1, NoStore = true   )]
        public ActionResult MainMenu()
        {
            
          
            var model = new MenuUserViewModel()
            {
                MenuItems = context.MenuUserRole(this.userId),
                context = context,
                currentUserId= this.userId
            }; 

            return PartialView(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult MenuTree()
        {
            var menuItems = context.GetMenuTree();

            var model = new ManageMenuAccessViewModel()
            {
                JSONData = JsonSerializer.Serialize(menuItems)
            };

            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult MenuRoles(int menuId)
        {
            var model = new MenuRolesViewModel()
            {
                MenuInfo = context.GetMenu(menuId),
                Roles = context.GetMenuRoles(menuId)
            };

            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult AssignRoles(int menuId, string[] handledItems)
        {
            context.AssignMenuRoles(menuId, handledItems);
            return RedirectToAction("MenuTree", "Access");
        }

    }
}