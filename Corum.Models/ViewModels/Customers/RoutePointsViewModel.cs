using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Customers
{
    public class RoutePointsViewModel
    {
        public int Id { get; set; }

        public long RoutePointId { get; set; }

        public long OrganizationId { get; set; }

        public int? RoutePointTypeId { get; set; }

        public string FullNamePointType { get; set; }

        public string ShortNamePointType { get; set; }

        [Display(Name = "Контактное лицо")]
        public string ContactPerson { set; get; }

        [Display(Name = "Контактный телефон")]
        public string ContactPersonPhone { set; get; }

        [Display(Name = "Номер по порядку доп точки")]
        public int NumberPoint { get; set; }
        public string NamePoint { set; get; }

        public string CityPoint { set; get; }

        public string AddressPoint { set; get; }

        public string CountryPoint { set; get; }

        public string Contacts { set; get; }

        public string CityAddress { set; get; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public long IdTemp { get; set; }

        public bool IsSaved { get; set; }
        public bool IsSavedToDelete { get; set; }



    }
}

