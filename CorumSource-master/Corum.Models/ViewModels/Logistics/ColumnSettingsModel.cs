using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Barnivann.Models;
// ReSharper disable All

namespace Corum.Models
{
    //Модель для передачи информации о настройках столбцов
    public class ColumnSettingsModel
    {        
        public string ColumnName { get; set; }
        public string SelectedColumnName { get; set; }
        [Display(Name = "Уникально")]
        public bool? isUnique { get; set; }

        public bool? isNotNull { get; set; }

        public bool? isLessActualDateBeg { get; set; }

        public bool? isBiggerActualDateEnd { get; set; }

        public bool? isSimilar { get; set; }

        public bool? isZeroDateReplace { get; set; }

        public bool? isNumeric { get; set; }

        public bool? isZeroNumericReplace { get; set; }

        public bool? isNotNullForRest { get; set; }

        public bool? isNotZeroForQPrihodNotZeroForDocs { get; set; }

        public bool? isNullForQPrihodZeroForDocs { get; set; }

        public bool? isNullForQPrihodNotZeroForDocs { get; set; }
        public bool? isNullForRest { get; set; }
        public int? id { get; set; }
    }

    public class ColumnNameModel
    {
        public string ColumnName { get; set; }        
    }

    public class ColumnSettingsImportModel
    {
        public string ColumnName { get; set; }
        public string ColumnImportName { get; set; }
        public bool? Value { get; set; }
    }


}
