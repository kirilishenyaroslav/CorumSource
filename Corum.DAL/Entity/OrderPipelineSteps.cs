
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
    
public partial class OrderPipelineSteps
{

    public int Id { get; set; }

    public int FromStatus { get; set; }

    public int ToStatus { get; set; }

    public string AccessRoleId { get; set; }

    public Nullable<bool> StartDateForClient { get; set; }

    public Nullable<bool> StartDateForExecuter { get; set; }

    public Nullable<int> OrderTypeId { get; set; }

    public Nullable<bool> FinishOfTheProcess { get; set; }



    public virtual OrderStatuses OrderStatuses { get; set; }

    public virtual OrderStatuses OrderStatuses1 { get; set; }

    public virtual AspNetRoles AspNetRoles { get; set; }

    public virtual OrderTypesBase OrderTypesBase { get; set; }

}

}
