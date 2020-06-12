using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corum.Models;
using System.ComponentModel.DataAnnotations;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderObserverViewModel
    {
        public long Id { get; set; }
        public long OrderId { get; set; }

        [Required(ErrorMessage = "Выберите пользователя из списка")]
        [Display(Name = "Пользователь")]
        public string observerId { get; set; }

        public string observerName { get; set; }
        public string observerEmail { get; set; }

        public List<UserViewModel> AvailiableObserver { get; set; }

        public bool assigned { get; set; }
    }
}
