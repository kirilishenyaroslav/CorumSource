using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Barnivann.Models;

namespace Corum.Models.ViewModels.Admin
{
    public class FAQGroupesViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Название группы вопросов")]
        public string NameFAQGroup { get; set; }

    }
}
