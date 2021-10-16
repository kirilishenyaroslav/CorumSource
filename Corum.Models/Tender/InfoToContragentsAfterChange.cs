using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public abstract class ListInfoAfterChange
    {
        public bool isSelected { get; set; }
        public string expeditorName { get; set; }
        public int numberOfVehicles { get; set; }
        public int count { get; set; }
        public string senderEmail { get; set; }
        public string recipientEmail { get; set; }
        public string upperartOfTheMessage { get; set; }
        public string dataTable { get; set; }
        public string messageFooter { get; set; }
        public string subject { get; set; }
        public string bodyHTML { get; set; }
        public long orderId { get; set; }
        public Guid formUuid { get; set; }
        public int tenderNumber { get; set; }
        public DateTime dateCreate { get; set; }
        public DateTime dateUpdate { get; set; }
        public int industryId { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public Guid tenderItemUuid { get; set; }
        public bool flag { get; set; }
        public string industryName { get; set; }
        public string routeShort { get; set; }
        public string nameCargo { get; set; }
        public Nullable<double> weightCargo { get; set; }
        public DateTime dataDownload { get; set; }
        public DateTime dataUnload { get; set; }
        public Nullable<int> DelayPayment { get; set; }
        public bool flagCreate { get; set; }

    }
    public class ListWinnersInfoAfterChange : ListInfoAfterChange
    {
        public ListWinnersInfoAfterChange()
        {
            this.dateCreate = DateTime.Now;
        }
    }

    public class ListLosersInfoAfterChange : ListInfoAfterChange
    {

    }

    public class InfoToContragentsAfterChange
    {
        public List<ListWinnersInfoAfterChange> listWinnersInfoAfterChange { get; set; }
        public List<ListLosersInfoAfterChange> listLosersInfoAfterChange { get; set; }
    }

    public class BodyHtmlForm
    {
        public string body { get; set; }
        public string subject { get; set; }
    }
}
