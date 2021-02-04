using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Corum.Models
{
    public class BaseViewModel: IValidatableObject
    {        
        [ScaffoldColumn(false)]
        public bool CanBeDelete { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string returnurl { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            return errors;
        }
    }
}
