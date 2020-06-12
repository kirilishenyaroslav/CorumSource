using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Corum.Models.ViewModels.Customers
{
    public class RouteViewModel
    {

        public long Id { get; set; }

        public long OrgFromId { get; set; }
        [Display(Name = "Конечная точка маршрута")]
        [Required(ErrorMessage = "Введите конечную точку маршрута")]
        public long? OrgToId { get; set; }

        public int CountryFromId { get; set; }
        public int CountryToId { get; set; }

        [Display(Name = "Начальная точка маршрута")]
        public string OrgFromName { get; set; }

        public string OrgFromCity { get; set; }

        public string OrgFromAddress { get; set; }

        public string OrgFromCountry { get; set; }

        [Display(Name = "Конечная точка маршрута")]
        [Required(ErrorMessage = "Введите конечную точку маршрута")]
        public string OrgToName { get; set; }

        public string OrgToCity { get; set; }

        public string OrgToAddress { get; set; }

        public string OrgToCountry { get; set; }

        public bool OrgToIsTruck { get; set; }

        public bool OrgFromIsTruck { get; set; }

        [Required(ErrorMessage = "Введите время маршрута (дни:часы)")]
        [Display(Name = "Время маршрута (дни:часы)")]
        [DisplayFormat(DataFormatString = "{0:dd\\:hh}", ApplyFormatInEditMode = true)]
        public string RouteTime { get; set; }

        [Required(ErrorMessage = "Введите расстояние маршрута")]
        [Display(Name = "Расстояние маршрута")]
        public string RouteDistance { get; set; }

        [Display(Name = "Краткое название")]
        public string ShortName { get; set; }

        public List<OrganizationViewModel> orgInfo { get; set; }

        public IList<RoutePointsViewModel> RoutePointsLoadInfo { get; set; }
    }

    public class RouteOrgViewModel
    {
        public OrganizationViewModel orgInfo { get; set; }
        public IQueryable<RouteViewModel> routes { get; set; }
    }

}
