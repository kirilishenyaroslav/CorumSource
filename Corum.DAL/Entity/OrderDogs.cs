
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
    
public partial class OrderDogs
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public OrderDogs()
    {

        this.OrdersBase = new HashSet<OrdersBase>();

    }


    public long Id { get; set; }

    public System.DateTime DogDateBeg { get; set; }

    public System.DateTime DogDateEnd { get; set; }

    public long ClientId { get; set; }



    public virtual OrderClients OrderClients { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<OrdersBase> OrdersBase { get; set; }

}

}
