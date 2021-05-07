using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public class RegisterTenders
    {
        public int Id { get; set; }
        public long OrderId { get; set; }
        public System.Guid TenderUuid { get; set; }
    }
}
