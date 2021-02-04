using System.Collections.Generic;
using System.Linq;
using Corum.DAL.Mappings;
using Corum.Models;
using Corum.Models.ViewModels;
using System;
using Corum.DAL.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using Corum.DAL.Entity;

namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {
        public IQueryable<RoleViewModel> getRolesInPipeline(string userId)
        {
            var query = (from ro in db.AspNetRoles 
                         where (ro.AspNetUsers.Where(u => u.Id == userId).Count() > 0)
                         select ro);

            return query.Select(Mapper.Map).OrderBy(o => o.roleId).AsQueryable();
        }


        public IQueryable<RoleViewModel> getRoles(string Search)
        {
            var search = (Search != null) ? Search : string.Empty;

            return db.AspNetRoles
                      .AsNoTracking()
                       .Where(u => (((search == "") || ((search != "") && ((u.Name.ToUpper().StartsWith(search.ToUpper()))
                                                                                                  || (u.Name.ToUpper().Contains(search.ToUpper()))
                                                                                                  || (u.Name.ToUpper().EndsWith(search.ToUpper())))))
                               || ((search == "") || ((search != "") && ((u.RoleDescription.ToUpper().StartsWith(search.ToUpper()))
                                                                                                  || (u.RoleDescription.ToUpper().Contains(search.ToUpper()))
                                                                                                  || (u.RoleDescription.ToUpper().EndsWith(search.ToUpper())))))
                               ))
                        .Select(Mapper.Map)
                         .AsQueryable();
        }
        public RoleViewModel getRole(string roleId)
        {
            return Mapper.Map(db.AspNetRoles.AsNoTracking().FirstOrDefault(u => u.Id == roleId));
        }

        public void UpdateRole(RoleViewModel model)
        {
            var dbInfo = db.AspNetRoles.FirstOrDefault(u => u.Id == model.roleId);
            if (dbInfo == null) return;

            dbInfo.Name = model.roleName;
            dbInfo.RoleDescription = model.roleDescription;

            db.SaveChanges();
        }

        public void AddNewRole(RoleViewModel model)
        {
            Guid g;
            g = Guid.NewGuid();

            var roleInfo = new AspNetRoles()
            {
                Id = g.ToString(),
                Name = model.roleName,
                RoleDescription = model.roleDescription
            };
            
            db.AspNetRoles.Add(roleInfo);
            db.SaveChanges();

            AddGroupRole(model.roleGroupsId, g.ToString());
            
        }
      
        public void RemoveRole(string roleId)
        {
            var dbInfo = db.AspNetRoles.FirstOrDefault(u => u.Id == roleId);
            if (dbInfo == null) return;
  
            db.AspNetRoles.Remove(dbInfo);
            db.SaveChanges();
        }
    
        //есть ли листья у элемента?
        public bool RoleGroupsHasChild(int roleGroupsId)
        {            
            var childRoleGroupsItems = db.RoleGroups.Where(x => x.parentId == roleGroupsId);                

            if (childRoleGroupsItems.Count() > 0)
                return true;
            else return false;
           
        }       

        public void AssignGroupRoles(int groupRoleId, string[] roles)
        {           
            var allRoles = db.AspNetRoles.ToList();

            foreach (var role in allRoles)
            {
                RemoveGroupRole(groupRoleId, role.Id);
            }

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    AddGroupRole(groupRoleId, role);
                }
            }
        }

        public void AddGroupRole(int groupRoleId, string roleId)
        {
           /*var role = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId);
            var groupRoleInfo = db.RoleGroups.FirstOrDefault(u => u.Id == groupRoleId);
            if ((role == null) && (groupRoleInfo == null)) return;
            {
                if (GroupRoleHasRole(groupRoleId, roleId)) return;
                groupRoleInfo.AspNetRoles.Add(role);*/

            var groupRole = db.RoleGroupsRole.FirstOrDefault(r => r.RoleGroupsId == groupRoleId && r.RoleId == roleId);
            
            //если уже было добавлено, то не добавлять
            if (groupRole != null) return;

            var groupRoleInfo = new RoleGroupsRole()
            {
                RoleGroupsId = groupRoleId,
                RoleId = roleId,
                RoleGroupsDate = DateTime.Now
            };
            
                db.RoleGroupsRole.Add(groupRoleInfo);
                db.SaveChanges();            
        }

        public void RemoveGroupRole(int groupRoleId, string roleId)
        {
            /*var role = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId);
            var groupRoleInfo = db.RoleGroups.FirstOrDefault(u => u.Id == groupRoleId);
            if ((role == null) || (groupRoleInfo == null)) return;
            {
                if ((!MenuHasRole(groupRoleId, roleId))) return;*/
            var groupRole = db.RoleGroupsRole.FirstOrDefault(r => r.RoleGroupsId == groupRoleId && r.RoleId == roleId);            
            
            if (groupRole == null) return;          

                db.RoleGroupsRole.Remove(groupRole);
                db.SaveChanges();            
        }

        public bool GroupRoleHasRole(int groupRoleId, string roleId)
        {
            return true;
            /*
            var menuInfo = db.RoleGroups
                   .Include("AspNetRoles")
                   .Where(x => x.Id == groupRoleId);

            var roleInfo = menuInfo.Where(x => x.AspNetRoles.Any(r => r.Id == roleId)).ToList();

            var result = roleInfo.Count();
            return (result > 0);*/
        }

        public GroupRolesAccessViewModel GetGroupRoles(int? groupRoleId)
        {
            if (groupRoleId != null)
            {
                //return Mapper.Map(db.RoleGroups.AsNoTracking().FirstOrDefault(u => u.Id == groupRoleId));
                var groupRoleItem = db.RoleGroups.FirstOrDefault(u => u.Id == groupRoleId);

                var roleGroupsRoleItem = db.RoleGroupsRole.Where(u => u.RoleGroupsId == groupRoleId);

                GroupRolesAccessViewModel groupRoleInfoItem = new GroupRolesAccessViewModel();
                groupRoleInfoItem.Id = groupRoleItem.Id;
                groupRoleInfoItem.GroupRolesName = groupRoleItem.Name;
                groupRoleInfoItem.parentId = groupRoleItem.parentId;
                groupRoleInfoItem.IdForScript = groupRoleItem.Id;
                
                //если  нет children - элементов, то это лист
                groupRoleInfoItem.is_Leaf = !(RoleGroupsHasChild(groupRoleItem.Id));
                
                //можно удалять только если нет ролей и если это корень, то нет детей от него
              //  if ((groupRoleItem.AspNetRoles.Count() > 0) || (RoleGroupsHasChild(groupRoleItem.Id)))
                if ((roleGroupsRoleItem.Count() > 0) || (RoleGroupsHasChild(groupRoleItem.Id)))
                    groupRoleInfoItem.CanBeDelete = false;
                else groupRoleInfoItem.CanBeDelete = true;
                groupRoleInfoItem.isRoot = (groupRoleItem.parentId == null) ? true : false;
                return groupRoleInfoItem;
            }          
            else return null;
        }

       /* public IQueryable<GroupRolesAccessViewModel> GetGroupRole()
        {
            return db.RoleGroups.AsNoTracking().Select(Mapper.Map).AsQueryable();
        }*/
        
        public IEnumerable<GroupRolesAccessViewModel> GetGroupRolesTree()
        {
            List<GroupRolesAccessViewModel> groupRoleInfo = new List<GroupRolesAccessViewModel>();
            var groupRoleItems = db.RoleGroups.ToList();            

            foreach (var groupRoleItem in groupRoleItems)
            {
                GroupRolesAccessViewModel groupRoleInfoItem = new GroupRolesAccessViewModel();
                groupRoleInfoItem.Id = groupRoleItem.Id;
                groupRoleInfoItem.GroupRolesName = groupRoleItem.Name;
                groupRoleInfoItem.parentId = groupRoleItem.parentId;
                groupRoleInfoItem.IdForScript = groupRoleItem.Id;

                //если  нет children - элементов, то это лист
                groupRoleInfoItem.is_Leaf = !(RoleGroupsHasChild(groupRoleItem.Id));

                var roleGroupsRoleItem = db.RoleGroupsRole.Where(u => u.RoleGroupsId == groupRoleInfoItem.Id);
                //можно удалять только если нет ролей и если это корень, то нет детей от него
                if ((roleGroupsRoleItem.Count() > 0) || (RoleGroupsHasChild(groupRoleItem.Id)))
                    groupRoleInfoItem.CanBeDelete = false;
                else groupRoleInfoItem.CanBeDelete = true;
                groupRoleInfoItem.isRoot = (groupRoleItem.parentId == null) ? true : false;
                groupRoleInfo.Add(groupRoleInfoItem);

            }
            return groupRoleInfo;
        }
        

        public List<GroupRoleViewModel> GetRoleGroupRoles(int? roleGroupsId)
        {
            List<GroupRoleViewModel> roleGroupRoles = new List<GroupRoleViewModel>();

            roleGroupRoles = (from R in db.AspNetRoles
                              join RGR in db.RoleGroupsRole on R.Id equals RGR.RoleId                            
                    where //(R.RoleGroups.Any(m => m.Id == roleGroupsId))    
                              RGR.RoleGroupsId == roleGroupsId
                    select new GroupRoleViewModel()
                    {
                        RoleId = R.Id,
                        RoleName = R.Name,
                        RoleDescription = R.RoleDescription,
                        CanBeDelete = !((R.Id != null) && (R.AspNetUsers.Count() > 0)||(R.Id=="1000")),
                        RoleGroupsDate = RGR.RoleGroupsDate,
                        Assigned = true

                    }).ToList();
            return roleGroupRoles.OrderByDescending(x => x.RoleGroupsDate).ToList();
        }

        public void AddNewGroupRole(RoleGroupsViewModel model)
        {
            var roleGroupsInfo = new RoleGroups();
              
            if (roleGroupsInfo != null)                
            {
                roleGroupsInfo.Name = model.Name;
                roleGroupsInfo.parentId = model.isRoot ? null : model.parentId;

                db.RoleGroups.Add(roleGroupsInfo);
                db.SaveChanges();
            };           
        }

        public void UpdateGroupRole(GroupRolesAccessViewModel model)
        {
            var dbInfo = db.RoleGroups.FirstOrDefault(u => u.Id == model.Id);
            if (dbInfo == null) return;

            dbInfo.Name = model.GroupRolesName;
            dbInfo.parentId = model.isRoot ? null : model.parentId;            
            db.SaveChanges();
        }

        public void RemoveGroupRole(int roleGroupsId)
        {
            var dbInfo = db.RoleGroups.FirstOrDefault(u => u.Id == roleGroupsId);
            if (dbInfo == null) return;

            db.RoleGroups.Remove(dbInfo);
            db.SaveChanges();
        }


        public bool CheckGroupRolesName(string roleName)
        {

            return  db.AspNetRoles.Any(u => u.Name == roleName);         
        }

        public bool CheckGroupRolesName2(string roleName, string roleId)
        {
            
            return db.AspNetRoles.Any(u => u.Name == roleName && u.Id != roleId);         
        }

        public void UpdateGroupForRole(RoleGroupViewModel model)
        {

            RemoveGroupRole(model.GroupRolesInfo.Id, model.Roles.roleId);

            AddGroupRole((int)model.GroupRolesInfo.parentId, model.Roles.roleId);

        }


        public IEnumerable<GroupRolesAccessViewModel> GetLeafGroupRolesTree()
        {
            List<GroupRolesAccessViewModel> groupRoleInfo = new List<GroupRolesAccessViewModel>();
            var groupRoleItems = db.RoleGroups.ToList();

            foreach (var groupRoleItem in groupRoleItems)
            {
                if (!(RoleGroupsHasChild(groupRoleItem.Id)))
                {
                    GroupRolesAccessViewModel groupRoleInfoItem = new GroupRolesAccessViewModel();
                    groupRoleInfoItem.Id = groupRoleItem.Id;
                    groupRoleInfoItem.GroupRolesName = groupRoleItem.Name;
                    groupRoleInfoItem.parentId = groupRoleItem.parentId;
                    groupRoleInfoItem.IdForScript = groupRoleItem.Id;

                    //если  нет children - элементов, то это лист
                    groupRoleInfoItem.is_Leaf = !(RoleGroupsHasChild(groupRoleItem.Id));

                    var roleGroupsRoleItem = db.RoleGroupsRole.Where(u => u.RoleGroupsId == groupRoleInfoItem.Id);
                    //можно удалять только если нет ролей и если это корень, то нет детей от него
                    if ((roleGroupsRoleItem.Count() > 0) || (RoleGroupsHasChild(groupRoleItem.Id)))
                        groupRoleInfoItem.CanBeDelete = false;
                    else groupRoleInfoItem.CanBeDelete = true;
                    groupRoleInfoItem.isRoot = (groupRoleItem.parentId == null) ? true : false;
                    groupRoleInfo.Add(groupRoleInfoItem);
                }
            }
            return groupRoleInfo;
        }
    }
}


   
