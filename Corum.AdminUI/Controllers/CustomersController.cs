using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Corum.Common;
using Corum.Models;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Customers;
using CorumAdminUI.Common;
using CorumAdminUI.Helpers;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;
using System.IO;
using System.Net;
using Corum.Models.Toastr;
using System.Configuration;

namespace CorumAdminUI.Controllers
{
    [Authorize]
    public partial class CustomersController : CorumBaseController
    {
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Organizations(NavigationInfo navInfo, long? orgId)
        {
            var model = new OrganizationNavigationResult<OrganizationViewModel>(navInfo, userId)
            {
                DisplayValues = context.getOrganizations(),
                OrgIdFocus = orgId

            };
            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewOrganization()
        {
            var UserCountryId = context.getCountryByUserId(this.userId);
            var DefaultCountry = context.getDefaultCountry();
            var CountryName = "";
            if (UserCountryId > 0)
            {
                CountryName = context.getCountryNameByUserId(UserCountryId);
            }
            else
            {
                UserCountryId = DefaultCountry.Id;
                CountryName = DefaultCountry.CountryName;
            }
            var model = new OrganizationViewModel()
            {
                CountryId = UserCountryId,
                Country = CountryName,
                GoogleMapApiKey = ConfigurationManager.AppSettings["GoogleMapApiKey"]
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewOrganization(OrganizationViewModel model)
        {
            context.AddOrganization(model);
            return RedirectToAction("Organizations", "Customers", null);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateOrganization(long orgId)
        {
            var orgInfo = context.GetOrganization(orgId);
            orgInfo.GoogleMapApiKey = ConfigurationManager.AppSettings["GoogleMapApiKey"];
            return View(orgInfo);
        }

        [HttpPost]
        public ActionResult UpdateOrganization(OrganizationViewModel model)
        {
            context.UpdateOrganization(model);

            return RedirectToAction("Organizations", "Customers", null);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteOrganization(int orgId)
        {
            context.DeleteOrganization(orgId);
            return RedirectToAction("Organizations", "Customers", null);
        }

        public ActionResult Routes(long? orgId)
        {
            RouteOrgViewModel model = null;
            if (orgId != null)
            {
                model = new RouteOrgViewModel()
                {
                    orgInfo = context.GetOrganization(orgId),
                    routes = context.getRoutes(orgId)
                };
            }

            return PartialView(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetOrganization(int Id)
        {
            var orgInfo = context.GetOrganization(Id);

            return Json(orgInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NewRoute(long orgId, string returnUrl)
        {
            var orgFromInfo = context.GetOrganization(orgId);
            var model = new RouteViewModel
            {
                RouteDistance = "0,00",
                RouteTime = "00:00",
                OrgFromId = orgId,
                OrgFromName = orgFromInfo.Name,
                OrgFromCity = orgFromInfo.City,
                OrgToId = null,
                ShortName = "",

            };

            Session["RoutePointLoadListOrg"] = null;
            Session["IsDbDataTakenLoadOrg"] = null;
            Session["pointListDeleteOrg"] = null;
            Session["pointListUpdateOrg"] = null;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult NewRoute(RouteViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {                
                return View(model);
            }
            long Id = context.AddNewRoute(model);

            var pointListLoads = Session["RoutePointLoadListOrg"] as List<RoutePointsViewModel>;

            if (Id > 0)
            {
                if (pointListLoads != null)
                {
                    foreach (var pointListLoad in pointListLoads)
                    {
                        pointListLoad.RoutePointId = Id;
                        context.NewRoutePointOrg(pointListLoad);

                    }
                }
            }
            //  return Redirect(returnUrl);
            return RedirectToAction("Routes", "Customers", new {orgId = model.OrgFromId});
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateRoute(long routeId, string returnUrl)
        {
            Session["RoutePointLoadListOrg"] = null;
            Session["IsDbDataTakenLoadOrg"] = null;
            Session["pointListDeleteOrg"] = null;
            Session["pointListUpdateOrg"] = null;

            var routeInfo = context.getRoute(routeId);
            var orgFromInfo = context.GetOrganization(routeInfo.OrgFromId);
            if (routeInfo != null)
            {
                routeInfo.OrgFromName = orgFromInfo.Name;
                routeInfo.orgInfo = context.getOrganizations().ToList();
            }

            var numbPoint = 1;
            var pointListLoads = context.getRoutePoints(routeId).ToList();
            routeInfo.RoutePointsLoadInfo = pointListLoads;


            return PartialView(routeInfo);
        }

        [HttpPost]
        public ActionResult UpdateRoute(RouteViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            context.UpdateRoute(model);

            var pointListLoads = Session["RoutePointLoadListOrg"] as List<RoutePointsViewModel>;
            var pointListDeletes = Session["pointListDeleteOrg"] as List<RoutePointsViewModel>;
            var pointListUpdates = Session["pointListUpdateOrg"] as List<RoutePointsViewModel>;
            if (pointListLoads != null)
            {
                foreach (var pointListLoad in pointListLoads)
                {
                    if (pointListLoad.IsSaved == false)
                    {
                        context.NewRoutePointOrg(pointListLoad);
                    }
                }
            }

            if (pointListDeletes != null)
            {
                foreach (var pointListDelete in pointListDeletes)
                {
                    context.DeleteRoutePointOrg(pointListDelete.Id);
                }
            }

            if (pointListUpdates != null)
            {
                foreach (var pointListUpdate in pointListUpdates)
                {
                    context.UpdateRoutePointOrg(pointListUpdate);
                }
            }
            //return Redirect(returnUrl);
            return RedirectToAction("Routes", "Customers", new {orgId = model.OrgFromId});
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteRoute(long Id, long OrgId)
        {
            context.DeleteRoute(Id);
            return RedirectToAction("Organizations", "Customers", new {orgId = OrgId});
        }


        [HttpGet]
        public ActionResult GetOrganizationsList(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetOrganizations(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetOrganizationsCount(searchTerm);

            var pagedAttendees = Organization2VmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult Organization2VmToSelect2Format(IEnumerable<OrganizationViewModel> groupItems,
            int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult {Results = new List<Select2Result>()};
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.Name
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetRoutes()
        {
            var routesList = context.getRoutesAll();
            return Json(routesList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetRoutesByFilterCount(int OrgFromId, int OrgToId)
        {

            var routeInfo = context.GetRoutesByFilter(OrgFromId, OrgToId).ToList();

            return Json(routeInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetLoadPointsRouteOrg(long RoutePointId)
        {
            List<RoutePointsViewModel> pointList = null;
            string pointListName;

            pointListName = "RoutePointLoadListOrg";
            bool? IsDbDataTakenLoadOrg = (bool?) Session["IsDbDataTakenLoadOrg"];
            if (IsDbDataTakenLoadOrg == null)
            {
                pointList = context.getRoutePoints(RoutePointId).ToList();
            }
            if (pointList == null || IsDbDataTakenLoadOrg == true)
            {
                pointList = Session[pointListName] as List<RoutePointsViewModel>;
            }
            IsDbDataTakenLoadOrg = true;
            Session["IsDbDataTakenLoadOrg"] = IsDbDataTakenLoadOrg;

            Session[pointListName] = pointList;

            return Json(pointList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveRoutePointOrg(long Id, bool IsSaved)
        {
            string pointListName;

            pointListName = "RoutePointLoadListOrg";

            var pointList = Session[pointListName] as List<RoutePointsViewModel>;
            var itemToRemove = pointList.Single(r => r.Id == Id);
            if (IsSaved == true)
            {
                var pointListDelete = Session["pointListDeleteOrg"] as List<RoutePointsViewModel>;

                if (pointListDelete == null)
                {
                    pointListDelete = new List<RoutePointsViewModel>();
                }

                pointListDelete.Add(itemToRemove);
                Session["pointListDeleteOrg"] = pointListDelete;
            }
            var result = pointList.Remove(itemToRemove);

            return Json(new {DeleteResult = result}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveRoutePointTempOrg(long Id)
        {
            string pointListName;

            pointListName = "RoutePointLoadListOrg";

            var pointList = Session[pointListName] as List<RoutePointsViewModel>;
            var itemToRemove = pointList.Single(r => r.Id == Id);
            var result = pointList.Remove(itemToRemove);
            return Json(new {DeleteResult = result}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateRoutePointOrg(long Id, bool IsSaved, string ContactPerson, string ContactPersonPhone,
            int NumberPoint, int RoutePointTypeId)
        {
            string pointListName;

            pointListName = "RoutePointLoadListOrg";

            var pointList = Session[pointListName] as List<RoutePointsViewModel>;


            var itemToUpdate = pointList.Single(r => r.Id == Id);
            itemToUpdate.ContactPerson = ContactPerson;
            itemToUpdate.ContactPersonPhone = ContactPersonPhone;
            itemToUpdate.NumberPoint = NumberPoint;
            itemToUpdate.Contacts = ContactPerson + Environment.NewLine + ContactPersonPhone;
            itemToUpdate.RoutePointTypeId = RoutePointTypeId;
            RoutePointTypeViewModel types = context.getRouteTypePoints(RoutePointTypeId);
           
            itemToUpdate.ShortNamePointType = types.ShortNamePointType ?? "";
            itemToUpdate.FullNamePointType = types.FullNamePointType ?? "";
            
            if (IsSaved == true)
            {
                var pointListUpdate = Session["pointListUpdateOrg"] as List<RoutePointsViewModel>;

                if (pointListUpdate == null)
                {
                    pointListUpdate = new List<RoutePointsViewModel>();
                }

                pointListUpdate.Add(itemToUpdate);
                Session["pointListUpdateOrg"] = pointListUpdate;
            }
            else
            {
                Session[pointListName] = pointList;
            }

            return Json(new {Id = Id}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NewRoutePointOrg(RoutePointsViewModel model)
        {
            string pointListName;

            pointListName = "RoutePointLoadListOrg";

            var pointList = Session[pointListName] as List<RoutePointsViewModel>;
            int maxId;
            if (pointList == null || pointList.Count == 0)
            {
                pointList = new List<RoutePointsViewModel>();
                maxId = 1;
            }
            else
            {
                maxId = pointList.Max(t => t.Id);
            }

            model.Id = maxId + 1;
            model.CityAddress = model.CityPoint + ", " + model.AddressPoint;                        
            pointList.Add(model);
            Session[pointListName] = pointList;

            return Json(new {Id = model.Id}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetLoadPointsTempOrg()
        {
            string pointListName;
            pointListName = "RoutePointLoadListOrg";

            var pointList = Session[pointListName] as List<RoutePointsViewModel>;

            return Json(pointList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetRoutePoint(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetRoutePoint(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetRoutePointCount(searchTerm, Id);

            var pagedAttendees = GetRoutePointVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult GetRoutePointVmToSelect2Format(
            IEnumerable<RoutePointTypeViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult {Results = new List<Select2Result>()};
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = groupItem.FullNamePointType
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

    }
}