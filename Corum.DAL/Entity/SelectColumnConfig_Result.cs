//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Corum.DAL.Entity
{
    using System;
    
    public partial class SelectColumnConfig_Result
    {
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public Nullable<bool> isNotNull { get; set; }
        public Nullable<bool> isNumeric { get; set; }
        public Nullable<bool> isZeroDateReplace { get; set; }
        public Nullable<bool> isZeroNumericReplace { get; set; }
        public Nullable<bool> isNotNullForRest { get; set; }
        public Nullable<bool> isNullForRest { get; set; }
        public Nullable<bool> isBiggerActualDateEnd { get; set; }
        public Nullable<bool> isLessActualDateBeg { get; set; }
        public Nullable<bool> isSimilar { get; set; }
        public Nullable<bool> isUnique { get; set; }
        public Nullable<bool> isNotZeroForQPrihodNotZeroForDocs { get; set; }
        public Nullable<bool> isNullForQPrihodNotZeroForDocs { get; set; }
        public Nullable<bool> isNullForQPrihodZeroForDocs { get; set; }
        public Nullable<int> id { get; set; }
    }
}
