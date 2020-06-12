using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorumAdminUI.Controllers
{
    [AllowAnonymous]
    public class PublicController : CorumBaseController
    {                
        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OperationSuccess(long Id)
        {
            return View(Id);
        }
        
        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Entry()
        {
            var model = context.getAvailableOrderTypes(null, null).Where(x=>x.IsActive==true).ToList();
            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewOrderPublicEntry(int OrderTypeId)
        {
            return RedirectToAction("NewOrder", "Orders", new { OrderTypeId, PublicEntry = true });
        }
    }
}