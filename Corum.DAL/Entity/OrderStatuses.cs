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
    
    public partial class OrderStatuses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderStatuses()
        {
            this.OrderPipelineSteps = new HashSet<OrderPipelineSteps>();
            this.OrderPipelineSteps1 = new HashSet<OrderPipelineSteps>();
            this.OrderStatusHistory = new HashSet<OrderStatusHistory>();
            this.OrderStatusHistory1 = new HashSet<OrderStatusHistory>();
            this.OrderFilterSettings = new HashSet<OrderFilterSettings>();
            this.OrderFilters = new HashSet<OrderFilters>();
            this.OrdersBase = new HashSet<OrdersBase>();
        }
    
        public int Id { get; set; }
        public string OrderStatusName { get; set; }
        public string Color { get; set; }
        public Nullable<bool> AllowEditRegData { get; set; }
        public Nullable<bool> AllowEditClientData { get; set; }
        public Nullable<bool> AllowEditExecuterData { get; set; }
        public string ActionName { get; set; }
        public string IconFile { get; set; }
        public string IconDescription { get; set; }
        public string ShortName { get; set; }
        public string FontColor { get; set; }
        public string BackgroundColor { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPipelineSteps> OrderPipelineSteps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPipelineSteps> OrderPipelineSteps1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderStatusHistory> OrderStatusHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderStatusHistory> OrderStatusHistory1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderFilterSettings> OrderFilterSettings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderFilters> OrderFilters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersBase> OrdersBase { get; set; }
    }
}
