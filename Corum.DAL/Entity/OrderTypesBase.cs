
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
    
public partial class OrderTypesBase
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public OrderTypesBase()
    {

        this.OrderFilterSettings = new HashSet<OrderFilterSettings>();

        this.OrderPipelineSteps = new HashSet<OrderPipelineSteps>();

        this.OrderFilters = new HashSet<OrderFilters>();

        this.OrdersBase = new HashSet<OrdersBase>();

    }


    public int Id { get; set; }

    public string TypeName { get; set; }

    public string UserRoleIdForClientData { get; set; }

    public string UserRoleIdForExecuterData { get; set; }

    public bool IsActive { get; set; }

    public string DefaultExecuterId { get; set; }

    public string ShortName { get; set; }

    public string UserForAnnonymousForm { get; set; }

    public string TypeAccessGroupId { get; set; }

    public Nullable<bool> IsTransportType { get; set; }

    public string UserRoleIdForCompetitiveList { get; set; }



    public virtual AspNetRoles AspNetRoles { get; set; }

    public virtual AspNetRoles AspNetRoles1 { get; set; }

    public virtual AspNetRoles AspNetRoles2 { get; set; }

    public virtual AspNetRoles AspNetRoles3 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<OrderFilterSettings> OrderFilterSettings { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<OrderPipelineSteps> OrderPipelineSteps { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<OrderFilters> OrderFilters { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<OrdersBase> OrdersBase { get; set; }

    public virtual AspNetUsers AspNetUsers { get; set; }

    public virtual AspNetUsers AspNetUsers1 { get; set; }

}

}
