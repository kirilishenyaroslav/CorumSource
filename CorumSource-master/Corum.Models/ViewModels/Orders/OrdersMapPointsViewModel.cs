using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Orders
{
    public class OrdersMapPointsViewModel
    {
        public OrdersMapPointsViewModel(string NamePoint, decimal Latitude, decimal Longitude, int NumberPoint)
        {
            this.NamePoint = NamePoint;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.NumberPoint = NumberPoint;
        }

        public string NamePoint { set; get; }

        [Display(Name = "Широта")]
        public decimal Latitude { get; set; }

        [Display(Name = "Долгота")]
        public decimal Longitude { get; set; }

        [Display(Name = "номер точки")]
        public int NumberPoint { get; set; }
    }
}
