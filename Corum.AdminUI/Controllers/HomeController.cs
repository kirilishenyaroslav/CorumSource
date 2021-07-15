using System;
using System.Web;
using System.Web.Mvc;
using CorumAdminUI.Controllers;
using Corum.Models.ViewModels.Dashboard;
using System.Collections.Generic;

namespace CorumAdminUI.Controllers
{

    [Authorize]
    public class HomeController : CorumBaseController
    {
        public ActionResult Index(bool isFinishStatuses = false)
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            if (Request.Cookies["CorumInit"] == null)
            {
                isFinishStatuses = context.getFinishStatusesByUserId(this.userId);
            }


            HttpCookie myCookie = new HttpCookie("CorumInit");
            myCookie["done"] = "1";
            myCookie.Expires = DateTime.Now.AddDays(1d);
            Response.Cookies.Add(myCookie);
            var model = new DashboardViewModel()
            {
                isFinishStatuses = isFinishStatuses,
                dateStart = startOfMonth,
                dateEnd = DateTime.Now,
                BPInfo = context.getBPInfoByUser(startOfMonth, this.userId, this.isAdmin, isFinishStatuses),
                PermissionToCompetetiveList = isAdmin || (context.UserHasRole(userId, context.GetAspNetRoleId("Доступ к конкурентным листам"))),
                shareTendersfromRegistyTenders = context.ShareTendersFromRegistyTenders()
            };

            return View(model);
        }
    }
}