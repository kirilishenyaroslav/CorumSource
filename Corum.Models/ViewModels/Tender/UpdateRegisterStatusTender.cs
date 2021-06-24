using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Tender
{
    public class UpdateRegisterStatusTender
    {
        public DateTime dateEnd { get; set; }
        public DateTime dateStart { get; set; }
        public string processValue { get; set; }
        public string remainingTime { get; set; }
        public string stageNumber { get; set; }
        public DateTime dateUpdateStatus { get; set; }
        public string lotState { get; set; }
        public string process { get; set; }
    }
}
