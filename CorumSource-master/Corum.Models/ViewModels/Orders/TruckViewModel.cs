using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Orders
{

    public class TruckViewModel : OrderUsedCarViewModel //BaseViewModel, 
    {
      
         public String Id { get; set; }
        [Required(ErrorMessage = "Введите страну грузоотправителя")]
        [Display(Name = "Страна грузоотправителя")]
        public int ShipperCountryId { get; set; }
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Страна грузоотправителя' не больше 100 символов")]
        public string ShipperCountryName { get; set; }

        [Required(ErrorMessage = "Введите город грузоотправителя")]
        [Display(Name = "Город отгрузки")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Город отгрузки' не больше 255 символов")]
        public string ShipperCity { get; set; }

        [Required(ErrorMessage = "Введите адрес грузоотправителя")]
        [Display(Name = "Адрес грузоотправителя")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Адрес грузоотправителя' не больше 500 символов")]
        public string ShipperAdress { get; set; }
     
        [Required(ErrorMessage = "Введите организацию грузоотправителя")]
        [Display(Name = "Организация грузоотправитель")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Грузоотправитель' не больше 500 символов")]
        public string Shipper { get; set; }

        [Required(ErrorMessage = "Введите организацию грузополучателя")]
        [Display(Name = "Организация грузополучатель")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Грузополучатель' не больше 500 символов")]
        public string Consignee { get; set; }
  
        public long ShipperId { get; set; }

        public DateTime ReportsDate { get; set; }

        public bool IsSystemOrg { get; set; }

        public bool IsShipper { get; set; }

        public String IdParent { get; set; }

        public String Name { get; set; }


        public bool IsLeaf { get; set; }

        public int IdGroudId { get; set; }            

        public bool UseOrderDateFilter { get; set; }

        public string FilterOrderTypeId { get; set; }

        public bool UseOrderTypeFilter { get; set; }

        public string IdDetails { get; set; }

        public string ShipperName { get; set; }

        public string TruckDescription { get; set; }

        public bool isShipper { get; set; }

        public DateTime? PlanDateTime  { get; set; }

        public DateTime? FactDateTime  { get; set; }

        public string IsShipperString  { get; set; }

        public string PlanTime { get; set; }

        public string FactTime { get; set; }

        public string PlanDate { get; set; }

        public string FactDate { get; set; }

        public string Address { get; set; }

        public string BalanceKeeper { get; set; }

        public string CreatorByUserName { get; set; }
        
        public string DateFactConsignee { get; set; }

        public string TimeFactConsignee { get; set; }

        public String IdTree { get; set; }
       

        public TruckViewModel()
        {           
            ShipperCity = string.Empty;
        }

    }

}
