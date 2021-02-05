
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
    
public partial class LogisticSnapshots
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public LogisticSnapshots()
    {

        this.DocsSnapshot = new HashSet<DocsSnapshot>();

        this.RestsSnapshot = new HashSet<RestsSnapshot>();

        this.LogImportErrors = new HashSet<LogImportErrors>();

    }


    public int id_spanshot { get; set; }

    public System.DateTime shapshot_data { get; set; }

    public Nullable<bool> isRestsLoaded { get; set; }

    public Nullable<bool> isDocsLoaded { get; set; }

    public Nullable<System.DateTime> ActualDateBeg { get; set; }

    public Nullable<System.DateTime> ActualDateEnd { get; set; }

    public Nullable<int> isDefaultForReports { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<DocsSnapshot> DocsSnapshot { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<RestsSnapshot> RestsSnapshot { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<LogImportErrors> LogImportErrors { get; set; }

}

}
