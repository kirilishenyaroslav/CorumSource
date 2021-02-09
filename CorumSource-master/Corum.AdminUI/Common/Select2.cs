using System.Collections.Generic;
using Corum.Models.ViewModels.Orders;

namespace CorumAdminUI.Common
{
    public class Select2PagedResult
    {
        public int Total { get; set; }
        public List<Select2Result> Results { get; set; }
    }

    public class Select2Result
    {
        public string id { get; set; }
        public string text { get; set; }
        public string element { get; set; }
    }

    public class Select2ResultGroup
    {        
        public string text { get; set; }
        public List<ChildrenRoles> children { get; set; }
    }

    public class Select2PagedResultGroup
    {
        public int Total { get; set; }
        public List<Select2ResultGroup> Results { get; set; }
    }

}