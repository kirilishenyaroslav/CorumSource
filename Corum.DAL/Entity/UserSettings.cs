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
    
    public partial class UserSettings
    {
        public string userId { get; set; }
        public int PageSize { get; set; }
        public int Id { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
