using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderAttachmentViewModel
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string DocDescription { get; set; }
        public string AddedByUser { get; set; }
        public string AddedByUserName { get; set; }
        public DateTime AddedDateTime { get; set; }
        public int DocType { get; set; }
        public string DocTypeName { get; set; }
        public byte[] DocBody { get; set; }
        public string RealFileName { get; set; }
    }
}
