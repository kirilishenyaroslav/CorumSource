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
    
    public partial class SpecificationNames
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SpecificationNames()
        {
            this.ContractSpecifications = new HashSet<ContractSpecifications>();
        }
    
        public int Id { get; set; }
        public int SpecCode { get; set; }
        public string SpecName { get; set; }
        public Nullable<int> nmcTestId { get; set; }
        public Nullable<int> nmcWorkId { get; set; }
        public Nullable<int> industryId { get; set; }
        public Nullable<int> industryId_Test { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractSpecifications> ContractSpecifications { get; set; }
    }
}
