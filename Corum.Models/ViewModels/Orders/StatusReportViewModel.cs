using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{

   
    public class StatusReportViewModel : BaseViewModel    
    {
        [Display(Name = "Вид перевозки")]
        public int TruckTypeId { get; set; }
        public string TruckTypeName { get; set; }

        [Display(Name = "Плательщик")]
        public string PayerName { set; get; }

        public int PriotityType { get; set; }

        public int CntAll { get; set; }

        public int CntZero { get; set; }
        public double CntZeroPercent { get; set; }
        public string CntZeroPercentRaw { get; set; }

        public int CntOne { get; set; }
        public double CntOnePercent { get; set; }
        public string CntOnePercentRaw { get; set; }
    }
}
