using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Dashboard
{

    public class BPItemInfoViewModel
    {
        public int StatusId { get; set; }
        public string ItemName { get; set; }
        public int Percent { get; set; }
        public int OrderCount { get; set; }
        public string Color { get; set; }
    }

    public class DashboardViewModelItem
    {
        public int OrderType { get; set; }
        public string OrderTypeName { get; set; }
        public string OrderTypeShortName { get; set; }
        public bool IsTransportType { get; set; }
        public List<BPItemInfoViewModel> BPInfo { get; set; }
    }

    public class DashboardViewModel
    {
        public bool isFinishStatuses { get; set; }
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public List<DashboardViewModelItem> BPInfo { get; set; }
        public bool PermissionToCompetetiveList { get; set; }
        public Dictionary<string, int> shareTendersfromRegistyTenders { get; set; }
    }
}
