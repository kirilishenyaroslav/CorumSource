//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Corum.DAL.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class RoleGroupsRole
    {
        public int RoleGroupsId { get; set; }
        public string RoleId { get; set; }
        public Nullable<System.DateTime> RoleGroupsDate { get; set; }
    
        public virtual AspNetRoles AspNetRoles { get; set; }
        public virtual RoleGroups RoleGroups { get; set; }
    }
}
