using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public class TenderParams
    {
        public string[] typeTures, typePublications, regums, nmcs, unitNames;
        DateTime date;

        public string dateStart, tenderName, authorId, companyId, subCompanyId,
        services, depId, budget, ture, typePublication, dateCreate, dateClose, regume,
        lotName, nmcName, unitName, qty;

        public void InitializeParams()
        {
            typeTures = new string[] { "Тендер RFx", "Аукцион/Редукцион" };

            typePublications = new string[] { "Открытый", "Закрытый" };

            regums = new string[] { "Облегченный" };

            nmcs = new string[] { "Номенклатура 1", "Номенклатура 2", "Номенклатура 3", "Номенклатура 4", "Номенклатура 5" };

            unitNames = new string[] { "шт.", "послуга" };

            date = DateTime.Now;
            dateStart = date.ToString("yyyy-MM-dd'T'HH:mm");
        }
    }
}
