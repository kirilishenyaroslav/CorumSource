using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Tender
{
    public class RemainingTimeUpdate
    {
        public Dictionary<string, Time> TimeList { get; set; }
    }

    public class Time
    {
        public string TenderUuid { get; set; }
        public string remainingTime { get; set; }
    }
}
