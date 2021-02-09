using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Corum.Common;
using Corum.Models.ViewModels.OrderConcurs;

namespace Corum.Models.ViewModels.Orders
{
    public class OrderUsedCarViewModel : OrderBaseViewModel //BaseViewModel
    {        
      //  public long Id { get; set; }        
        public long OrderId { get; set; }
        public int? ContractId { get; set; }
        public int? ContractExpBkId { get; set; }
        public string ContractInfo { get; set; }
        public string ContractExpBkInfo { get; set; }
        public string ExpeditorName { get; set; }
        public string CarOwnerInfo { get; set; }
        public string CarModelInfo { get; set; }
        public string CarRegNum { get; set; }
        public int? CarCapacity { get; set; }
        public string CarDriverInfo { get; set; }
        public string DriverContactInfo { get; set; }
        public string CarrierInfo { get; set; }
        public int? CarId { get; set; }
       // public decimal Summ { get; set; }
        public string DriverCardInfo { get; set; }
        public string Comments { get; set; }
        public int? ExpeditorId { get; set; }
        public int? CarrierId { get; set; }
        public string PlanDistance { get; set; }
        public int? PlanTimeWorkDay { get; set; }
        public int? PlanTimeHoliday { get; set; }
        public string BaseRate { get; set; }
        public string BaseRateWorkDay { get; set; }
        public string BaseRateHoliday { get; set; }
        public int? DelayDays { get; set; }
        [Display(Name = "Дата фактического отправления") ]         
        public DateTime? FactShipperDateTime { get; set; }
        [Display(Name = "Дата фактического прибытия")]
        public DateTime? FactConsigneeDateTime { get; set; }
        public string FactShipperDateTimeRaw { get; set; }
        public string FactConsigneeDateTimeRaw { get; set; }

        [Display(Name = "Дата фактического отправления") ]  
        public string FactShipperDate { get; set; }
        [Display(Name = "Дата фактического прибытия") ]
        public string FactConsigneeDate { get; set; }
        public string FactShipperDateRaw { get; set; }
        public string FactConsigneeDateRaw { get; set; }

        [Display(Name = "Время фактического отправления") ]  
        public string FactShipperTime { get; set; }
        [Display(Name = "Время фактического прибытия") ]
        public string FactConsigneeTime  { get; set; }
        public string FactShipperTimeRaw { get; set; }
        public string FactConsigneeTimeRaw { get; set; }
       // public string OrderTypeShortName { get; set; }

        [Display(Name = "Плановая дата отправления") ]  
        public string PlanShipperDate { get; set; }
        public string PlanShipperDateRaw { get; set; }
        [Display(Name = "Плановое время отправления") ]
        public string PlanShipperTime { get; set; }
        public string PlanShipperTimeRaw { get; set; }

        [Display(Name = "Плановая дата прибытия") ]
        public string PlanConsigneeDate { get; set; }
        public string PlanConsigneeDateRaw { get; set; }
        [Display(Name = "Плановое время прибытия") ]
        public string PlanConsigneeTime { get; set; }
        public string PlanConsigneeTimeRaw { get; set; }

        [Display(Name = "Дата фактической выгрузки") ]
        public string RealFactConsigneeDate { get; set; }
        public string RealFactConsigneeDateRaw { get; set; }
        [Display(Name = "Время фактической выгрузки") ]
        public string RealFactConsigneeTime { get; set; }
        public string RealFactConsigneeTimeRaw { get; set; }

        [Display(Name = "Дата фактической загрузки") ]
        public string RealFactShipperDate { get; set; }
        public string RealFactShipperDateRaw { get; set; }
         [Display(Name = "Время фактической загрузки") ]
        public string RealFactShipperTime { get; set; }
        public string RealFactShipperTimeRaw { get; set; }

        public CompetitiveListViewModel OrderListInfo { get; set; }
    }
}
