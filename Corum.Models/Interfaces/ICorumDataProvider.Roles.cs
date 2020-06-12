using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels;
using System;

namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        IQueryable<RoleViewModel> getRoles(string Search);
        IQueryable<RoleViewModel> getRolesInPipeline(string userId);

        RoleViewModel getRole(string roleId);
        void UpdateRole(RoleViewModel model);
        void AddNewRole(RoleViewModel model);
        void RemoveRole(string roleId);
        IEnumerable<GroupRolesAccessViewModel> GetGroupRolesTree();

        void AssignGroupRoles(int menuId, string[] roles);

        GroupRolesAccessViewModel GetGroupRoles(int? roleGroupsId);
        List<GroupRoleViewModel> GetRoleGroupRoles(int? roleGroupsId);

        void AddNewGroupRole(RoleGroupsViewModel model);
        //IQueryable<GroupRolesAccessViewModel> GetGroupRole();
        void RemoveGroupRole(int roleGroupsId);

        void UpdateGroupRole(GroupRolesAccessViewModel model);

        bool CheckGroupRolesName(string roleName);

        bool CheckGroupRolesName2(string roleName, string roleId);

        void UpdateGroupForRole(RoleGroupViewModel model);

        IEnumerable<GroupRolesAccessViewModel> GetLeafGroupRolesTree();
    }

   
}
