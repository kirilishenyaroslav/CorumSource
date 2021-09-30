using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public class RegisterMessageToContragents
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegisterMessageToContragents()
        {
            this.RegisterFormFromContragents = new HashSet<RegisterFormFromContragents>();
        }

        public int Id { get; set; }
        public long orderId { get; set; }
        public long formId { get; set; }
        public int tenderNumber { get; set; }
        public string contragentName { get; set; }
        public string emailOperacionist { get; set; }
        public string emailContragent { get; set; }
        public System.DateTime dateCreate { get; set; }
        public System.DateTime dateUpdate { get; set; }
        public string industryName { get; set; }
        public string descriptionTender { get; set; }
        public Nullable<int> acceptedTransportUnits { get; set; }
        public Nullable<double> cost { get; set; }
        public System.Guid tenderItemUuid { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisterFormFromContragents> RegisterFormFromContragents { get; set; }
    }
}
