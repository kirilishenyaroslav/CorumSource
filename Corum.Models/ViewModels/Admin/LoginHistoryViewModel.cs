using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Admin
{
    public class LoginHistoryViewModel
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string userId { get; set; }
        public string Datetime { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
    }
}
