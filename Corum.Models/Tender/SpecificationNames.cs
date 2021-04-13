using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    // Создание модели услуг
    public class SpecificationNames
    {
        public int Id { get; set; }
        public int SpecCode { get; set; }
        public string SpecName { get; set; }

        public int? nmcTestId { get; set; }
        public int? nmcWorkId { get; set; }
        public int? industryId { get; set; }
    }
}
