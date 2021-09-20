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
    }
    public class ListWinnersInfoAfterChange : ListInfoAfterChange
    {

    }

    public class ListLosersInfoAfterChange: ListInfoAfterChange
    {
      
    }

    public class InfoToContragentsAfterChange
    {
        public List<ListWinnersInfoAfterChange> listWinnersInfoAfterChange { get; set; }
        public List<ListLosersInfoAfterChange> listLosersInfoAfterChange { get; set; }
    }
}
