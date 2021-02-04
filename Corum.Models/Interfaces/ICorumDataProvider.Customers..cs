using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corum.Models.ViewModels.Customers;

namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        IQueryable<OrganizationViewModel> getOrganizations();

        void AddOrganization(OrganizationViewModel model);
        OrganizationViewModel GetOrganization(long? orgId);
        bool DeleteOrganization(long id);
        void UpdateOrganization(OrganizationViewModel model);
        IQueryable<OrganizationViewModel> GetOrgsBySearchString(string searchTerm);
        List<OrganizationViewModel> GetOrganizations(string searchTerm, int pageSize, int pageNum);
        int GetOrganizationsCount(string searchTerm);
        IQueryable<RouteViewModel> getRoutes(long? orgId);
        long AddNewRoute(RouteViewModel model);
        RouteViewModel getRoute(long routeId);
        void UpdateRoute(RouteViewModel model);
        bool DeleteRoute(long id);
        IQueryable<RouteViewModel> getRoutesAll();
        List<RouteViewModel> GetRoutesByPage(int pageSize, int pageNum);
        int GetRoutesCount();
        IQueryable<RouteViewModel> GetRoutesByFilter(int OrgFromId, int OrgToId);
        int GetRoutesByFilterCount(int OrgFromId, int OrgToId);
        IQueryable<RoutePointsViewModel> getRoutePoints(long RoutePointId);
        long NewRoutePointOrg(RoutePointsViewModel model);
        void UpdateRoutePointOrg(RoutePointsViewModel model);
        bool DeleteRoutePointOrg(long Id);

        IQueryable<RoutePointTypeViewModel> GetRoutePointBySearchString(string searchTerm, long? Id);
        List<RoutePointTypeViewModel> GetRoutePoint(string searchTerm, int pageSize, int pageNum, long? Id);
        int GetRoutePointCount(string searchTerm, long? Id);
        RoutePointTypeViewModel getRouteTypePoints(long RoutePointTypeId);
        string GetOrganizationNameById(long? orgId);
    }
}
