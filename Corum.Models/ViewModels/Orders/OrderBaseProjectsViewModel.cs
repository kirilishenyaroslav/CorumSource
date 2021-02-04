using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderBaseProjectsViewModel : BaseViewModel
    {
        public bool PublicEntry { set; get; }

        [Display(Name = "Номер записи")]
        public long Id { set; get; }

        [Display(Name = "Номер проекта")]
        public int ProjectId { set; get; }

        [Display(Name = "Номер заявки")]
        public long OrderId { set; get; }

    }
}
