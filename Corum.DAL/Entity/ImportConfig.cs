
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
    
public partial class ImportConfig
{

    public int id { get; set; }

    public string Column_Name { get; set; }

    public string ColumnType { get; set; }

    public Nullable<bool> isUnique { get; set; }

    public Nullable<bool> isNotNull { get; set; }

    public Nullable<bool> isLessActualDateBeg { get; set; }

    public Nullable<bool> isBiggerActualDateEnd { get; set; }

    public Nullable<bool> isSimilar { get; set; }

    public Nullable<bool> isZeroDateReplace { get; set; }

    public Nullable<bool> isNumeric { get; set; }

    public Nullable<bool> isZeroNumericReplace { get; set; }

    public Nullable<bool> isNotNullForRest { get; set; }

    public Nullable<bool> isNotZeroForQPrihodNotZeroForDocs { get; set; }

    public Nullable<bool> isNullForQPrihodZeroForDocs { get; set; }

    public Nullable<bool> isNullForQPrihodNotZeroForDocs { get; set; }

    public Nullable<bool> isNullForRest { get; set; }

}

}
