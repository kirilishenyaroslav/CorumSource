
using System.Linq;
using System.Web.Mvc;
using Corum.Models;
using Corum.Models.ViewModels.Orders;


namespace CorumAdminUI.Controllers
{
    [Authorize]
    public class SettingsController : CorumBaseController
    {
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrderTypes(NavigationInfo navInfo)
        {
            var model = new NavigationResult<OrderTypeViewModel>(navInfo, userId)
            {
                DisplayValues = context.getOrderTypes()

            };
            return View(model);
        }

        [HttpGet]
        public ActionResult NewOrderType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewOrderType(OrderTypeViewModel model)
        {
            context.AddOrderType(model);
            return RedirectToAction("OrderTypes", "Settings");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateOrderType(int Id)
        {
            var OrderTypeInfo = context.getOrderType(Id);
            OrderTypeInfo.AvaliableRoles =  context.getRoles(string.Empty).ToList();
            OrderTypeInfo.AvaliableUsers =  context.getUsers(string.Empty).ToList();

            return View(OrderTypeInfo);
        }

        [HttpPost]
        public ActionResult UpdateOrderType(OrderTypeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
           
            context.UpdateOrderType(model);
            return RedirectToAction("OrderTypes", "Settings");
        }

        [HttpGet]
        public ActionResult RemoveOrderType(int Id)
        {
            context.RemoveOrderType(Id);
            return RedirectToAction("OrderTypes", "Settings");
        }

        public ActionResult TripCalculator(string NameFrom, string NameTo, string FuelPrice)
        {

            string path = Request.Url.Authority;
            ViewBag.path = "http://" + path+"/Content/Site.css";

            if (FuelPrice == null)
            { ViewBag.FuelPrice = "24"; }
            else
            { ViewBag.FuelPrice = FuelPrice; }

            ViewBag.NameFrom = NameFrom;
            ViewBag.NameTo = NameTo;

            return View();
        }        
    }
}