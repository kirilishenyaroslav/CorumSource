using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.OrderConcurs
{
    public class ConcursDiscountRateModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Введите cтавку дисконтирования")]
        [Display(Name = "Ставка дисконтирования")]
        public string DiscountRate { get; set; }

        [Required(ErrorMessage = "Введите дату начала")]
        [Display(Name = "Дата начала")]
        public string DateBeg { get; set; }

        public string DateBegRaw { set; get; }

        [Required(ErrorMessage = "Введите дату конца")]
        [Display(Name = "Дата конца")]
        public string DateEnd { get; set; }

        public string DateEndRaw { set; get; }

        public ConcursDiscountRateModel()
        {
            DiscountRate = "0,00";
        }
    }
}
