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
    public class FAQAnswersViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Вопрос")]
        public string Question { get; set; }

        [Display(Name = "Ответ")]
        [AllowHtml]
        public string Answer { get; set; }

        [Required(ErrorMessage = "Выберите группу вопросов")]
        [Display(Name = "Название группы вопросов")]
        public int GroupeId { get; set; }

        public string NameFAQGroup { get; set; }
    }
}
