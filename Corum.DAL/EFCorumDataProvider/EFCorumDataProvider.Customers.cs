using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corum.Models;
using Corum.Common;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Customers;
using Corum.DAL.Mappings;
using Corum.DAL.Entity;
using System.Data.Entity.Validation;

namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {
        public IQueryable<OrganizationViewModel> getOrganizations()
        {
            return db.Organization.AsNoTracking()
                .Select(Mapper.Map)
                .Where(x => x.IsAuto == false)
                .OrderBy(o => o.Country)
                .ThenBy(x => x.City)
                .ThenBy(x => x.Name).AsQueryable();
        }

        public void AddOrganization(OrganizationViewModel model)
        {
            
            var orgInfo = new Organization()
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                CountryId = model.CountryId,
                IsTruck = model.IsTruck,
                Latitude = Convert.ToDecimal((model.Latitude ?? "0").Replace(".", ",")),
                Longitude = Convert.ToDecimal((model.Longitude ?? "0").Replace(".", ",")),
                IsAuto = false,
                IsSystemOrg = false

            };

            db.Organization.Add(orgInfo);
            db.SaveChanges();
        }

        public OrganizationViewModel GetOrganization(long? orgId)
        {
            return Mapper.Map(db.Organization.AsNoTracking().FirstOrDefault(u => u.Id == orgId && u.IsAuto == false));
        }

        public bool DeleteOrganization(long id)
        {
            var orgInfo = db.Organization.FirstOrDefault(o => o.Id == id);

            if (orgInfo != null)
            {
                db.Organization.Remove(orgInfo);
                db.SaveChanges();
            }
            return true;

        }

        public void UpdateOrganization(OrganizationViewModel model)
        {

            var orgInfo = db.Organization.FirstOrDefault(o => o.Id == model.Id);

            if (orgInfo == null) return;

            orgInfo.Name = model.Name;
            orgInfo.City = model.City;
            orgInfo.Address = model.Address;
            orgInfo.CountryId = model.CountryId;
            orgInfo.IsTruck = model.IsTruck;
            orgInfo.Latitude = Convert.ToDecimal((model.Latitude ?? "0").Replace(".", ","));
            orgInfo.Longitude = Convert.ToDecimal((model.Longitude ?? "0").Replace(".", ","));
            orgInfo.IsAuto = model.IsAuto;
            orgInfo.IsSystemOrg = model.IsSystemOrg;

            db.SaveChanges();
        }

        public IQueryable<OrganizationViewModel> GetOrgsBySearchString(string searchTerm)
        {
            return
            db.Organization
                  .AsNoTracking()
                     .Where(s => (s.IsAuto == false) && ((((searchTerm == null) || ((searchTerm != null) && ((s.Name.Contains(searchTerm) || s.Name.StartsWith(searchTerm) || s.Name.EndsWith(searchTerm)))))
                                 || ((searchTerm == null) || ((searchTerm != null) && ((s.Address.Contains(searchTerm) || s.Address.StartsWith(searchTerm) || s.Address.EndsWith(searchTerm))))))))
                        .Select(Mapper.Map)
                         .OrderBy(o => o.Name)
                         .AsQueryable();
        }


        public List<OrganizationViewModel> GetAllOrganizations(string searchTerm, int pageSize, int pageNum)
        {
            return db.Organization
                  .AsNoTracking()
                        .Select(Mapper.Map)
                        .Where(x => x.IsAuto == false)
                         .OrderBy(o => o.Country)
                         .ThenBy(x => x.City)
                         .ThenBy(x => x.Name)
                         .ToList();
        }

        public List<OrganizationViewModel> GetOrganizations(string searchTerm, int pageSize, int pageNum)
        {
            return GetOrgsBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetOrganizationsCount(string searchTerm)
        {
            return GetOrgsBySearchString(searchTerm).Count();
        }

        public long AddNewRoute(RouteViewModel model)
        {
            var routeTimeInt = DateTimeConvertClass.convertHoursToInt(model.RouteTime);//(MinutesInt + HoursInt * 60)*60000; 
            var routeInfo = new Routes()
            {
                OrgFromId = model.OrgFromId,
                OrgToId = model.OrgToId ?? 0,
                RouteDistance = Convert.ToDecimal(model.RouteDistance.Replace(".", ",")),
                RouteTime = routeTimeInt,
                ShortName = model.ShortName                
            };

            db.Routes.Add(routeInfo);
            db.SaveChanges();
            return routeInfo.Id;
        }

        public RouteViewModel getRoute(long routeId)
        {
            return Mapper.Map(db.Routes.AsNoTracking().FirstOrDefault(u => u.Id == routeId));
        }

        public void UpdateRoute(RouteViewModel model)
        {
            var routeTimeInt = DateTimeConvertClass.convertHoursToInt(model.RouteTime);//(MinutesInt + HoursInt * 60) * 60000;

            var routeInfo = db.Routes.FirstOrDefault(o => o.Id == model.Id);

            if (routeInfo == null) return;

            routeInfo.OrgToId = model.OrgToId ?? 0;
            routeInfo.RouteDistance = Convert.ToDecimal(model.RouteDistance.Replace(".", ","));
            routeInfo.RouteTime = routeTimeInt;
            routeInfo.ShortName = model.ShortName;

            db.SaveChanges();
        }

        public bool DeleteRoute(long id)
        {
            var routePointsInfo = db.RoutePoints.Where(o => o.RoutePointId == id);
            if (routePointsInfo.Any())
            {
                db.RoutePoints.RemoveRange(routePointsInfo);
                db.SaveChanges();
            }

            var routeInfo = db.Routes.FirstOrDefault(o => o.Id == id);

            if (routeInfo != null)
            {
                db.Routes.Remove(routeInfo);
                db.SaveChanges();
            }
            return true;

        }


        public IQueryable<RouteViewModel> getRoutes(long? orgId)
        {
            return db.Routes.AsNoTracking().Select(Mapper.Map).Where(u => u.OrgFromId == orgId).AsQueryable();
        }

        public IQueryable<RouteViewModel> getRoutesAll()
        {
            return db.Routes.AsNoTracking().Select(Mapper.Map).AsQueryable();            
        }


        public List<RouteViewModel> GetRoutesByPage(int pageSize, int pageNum)
        {
            return getRoutesAll()
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public int GetRoutesCount()
        {
            return getRoutesAll().Count();
        }


        public IQueryable<RouteViewModel> GetRoutesByFilter(int OrgFromId, int OrgToId)
        {
            return db.Routes
                  .AsNoTracking()
                  .Where(s => s.OrgFromId == OrgFromId &&
                  s.OrgToId == OrgToId &&
                  s.Organization.IsAuto == false && s.Organization1.IsAuto == false
                  )
                 .Select(Mapper.Map)
                  .AsQueryable();
        }

  
        public int GetRoutesByFilterCount(int OrgFromId, int OrgToId)
        {
            return GetRoutesByFilter(OrgFromId, OrgToId).Count();
        }

        public RoutePointTypeViewModel getRouteTypePoints(long RoutePointTypeId)
        {
            return db.RoutePointType
                           .AsNoTracking()
                            .Where(x => x.Id == RoutePointTypeId)
                             .Select(Mapper.Map)
                              .FirstOrDefault();
      }

        public IQueryable<RoutePointsViewModel> getRoutePoints(long RoutePointId)
        {
            var Routes = db.RoutePoints
                           .AsNoTracking()
                            .Where(x => x.RoutePointId == RoutePointId)
                             .Select(Mapper.Map)
                              .OrderBy(o => o.NumberPoint)
                              .ToList();

            List<RoutePointsViewModel> routesAll = new List<RoutePointsViewModel>();
            
            foreach (var route in Routes)
            {
                RoutePointsViewModel r = new RoutePointsViewModel();
                r.Id = route.Id;
                r.RoutePointId = route.RoutePointId;
                r.OrganizationId = route.OrganizationId;
                r.ContactPerson = route.ContactPerson;
                r.ContactPersonPhone = route.ContactPersonPhone;
                r.NamePoint = route.NamePoint;
                r.CountryPoint = route.CountryPoint;
                r.CityPoint = route.CityPoint;
                r.AddressPoint = route.AddressPoint;
                r.CityAddress = route.CityAddress;
                r.IsSaved = route.IsSaved;
                r.Contacts = route.Contacts;
                r.NumberPoint = route.NumberPoint;
                r.RoutePointTypeId = route.RoutePointTypeId;
                r.ShortNamePointType = route.ShortNamePointType ?? "";
                r.FullNamePointType = route.FullNamePointType ?? "";
                r.Latitude = route.Latitude;
                r.Longitude = route.Longitude;

                routesAll.Add(r);
            }

                
            return routesAll.AsQueryable();
        }

        public long NewRoutePointOrg(RoutePointsViewModel model)
        {
            var point = new RoutePoints()
            {
                //OrderId = model.OrderId ?? 0,
                //IsLoading = model.IsLoading,
                RoutePointId = model.RoutePointId,
                OrganizationId = model.OrganizationId,
                ContactPerson = model.ContactPerson,
                ContactPersonPhone = model.ContactPersonPhone,
                NumberPoint = model.NumberPoint,
                RoutePointTypeId = model.RoutePointTypeId
            };

            db.RoutePoints.Add(point);

            db.SaveChanges();
            return point.Id;
        }

        public IQueryable<RoutePointTypeViewModel> GetRoutePointBySearchString(string searchTerm, long? Id)
        {
            return (from Us in db.RoutePointType                   
                    select Us)
                           .Select(Mapper.Map)
                           .Where(s => (((s.FullNamePointType.Contains(searchTerm) || s.FullNamePointType.StartsWith(searchTerm) || s.FullNamePointType.EndsWith(searchTerm)))))
                           .OrderBy(Us => Us.FullNamePointType)
                           .AsQueryable();
        }

        public List<RoutePointTypeViewModel> GetRoutePoint(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetRoutePointBySearchString(searchTerm, Id)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetRoutePointCount(string searchTerm, long? Id)
        {
            return GetRoutePointBySearchString(searchTerm, Id).Count();
        }

        public void UpdateRoutePointOrg(RoutePointsViewModel model)
        {
            var point = db.RoutePoints.FirstOrDefault(o => o.Id == model.Id);

            if (point == null) return;

            point.ContactPerson = model.ContactPerson;
            point.ContactPersonPhone = model.ContactPersonPhone;
            point.NumberPoint = model.NumberPoint;
            point.RoutePointTypeId = model.RoutePointTypeId;

            db.SaveChanges();
        }

        public bool DeleteRoutePointOrg(long Id)
        {
            var point = db.RoutePoints.FirstOrDefault(o => o.Id == Id);

            if (point != null)
            {
                db.RoutePoints.Remove(point);
                db.SaveChanges();
            }
            return true;

        }

          public string GetOrganizationNameById(long? orgId)
          {
           /* if (db.OrderTruckTransport.AsNoTracking().FirstOrDefault(u => u.ShipperId == orgId) != null)
              return db.OrderTruckTransport.AsNoTracking().FirstOrDefault(u => u.ShipperId == orgId).Shipper;
            else
              return db.OrderTruckTransport.AsNoTracking().FirstOrDefault(u => u.ConsigneeId == orgId)?.Consignee ?? "";*/

            return db.Organization.AsNoTracking().FirstOrDefault(u => u.Id == orgId)?.Name ?? "";
            
        }
    }
}
