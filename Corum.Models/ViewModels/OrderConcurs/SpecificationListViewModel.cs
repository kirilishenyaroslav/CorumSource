using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.OrderConcurs
{

    public class SpecificationListViewModel : BaseViewModel
    {

        public int Id { get; set; }

        public int GenId { get; set; }        

        public int OrderId { get; set; }

        public string ExpeditorName { get; set; }

        public bool IsForwarder { get; set; }

        public string tripTypeName { get; set; }
        public Nullable<long> edrpou_aps { get; set; }
        public string email_aps { get; set; }

        [Required(ErrorMessage = "Введите название спецификации")]
        [Display(Name = "Название спецификации")]
        public string NameGroupeSpecification { get; set; }

        [Display(Name = "Отсрочка в днях")]
        public int? DaysDelay { get; set; }

        public int GroupeSpecId { get; set; }
       
        [Required(ErrorMessage = "Введите название услуги")]
        [Display(Name = "Название услуги")]
        public string NameSpecification { get; set; }

        public string FreightName { get; set; }

        public decimal? CarryCapacity { get; set; }

        public string NameIntervalType { get; set; }

        [Required(ErrorMessage = "Укажите грузоподъемность")]
        [Display(Name = "Грузоподъемноость автомобиля")]
        public int? CarryCapacityId { get; set; }

        [Display(Name = "Грузоподъемноость автомобиля")]
        public decimal? CarryCapacityVal { get; set; }

        [Display(Name = "Тип услуги")]
        public bool IsFreight { get; set; }
        
        [Display(Name = "Длина маршрута")]
        public string RouteLength { get; set; }

        [Display(Name = "Тип передвижения")]
        public int? MovingType { get; set; }

        [Display(Name = "Тип передвижения")]
        public string MovingTypeName { get; set; }

        [Display(Name = "Тип маршрута")]
        public int RouteTypeId { get; set; }

        [Display(Name = "Тип маршрута")]
        public string RouteTypeName { get; set; }

        [Display(Name = "Тип интервала маршрута")]
        public int IntervalTypeId { get; set; }

        [Display(Name = "Тип интервала маршрута")]
        public string IntervalTypeName { get; set; }

        [Display(Name = "Тариф грн/км")]
        public string RateKm { get; set; }

        [Display(Name = "Тариф грн/час")]
        public string RateHour { get; set; }

        [Display(Name = "Тариф машино/час")]
        public string RateMachineHour { get; set; }

        [Display(Name = "Тариф фрахта")]
        public string RateTotalFreight { get; set; }

        [Required(ErrorMessage = "Введите НДС")]
        [Display(Name = "НДС")]
        public string NDSTax { get; set; }

        public string UsedRateName { get; set; }

        public int UsedRateId { get; set; }

        public string RateValue { get; set; }

        [Display(Name = "Точка отправления")]
        public string DeparturePoint { get; set; }

        [Display(Name = "Точка прибытия")]
        public string ArrivalPoint { get; set; }

        [Display(Name = "Название маршрута")]
        public string RouteName { get; set; }

        [Display(Name = "Тип автомобиля")]
        public string VehicleTypeName { get; set; }

        public bool isTruck { get; set; }    
        
        public bool UseTripTypeFilter { get; set; }

        public string FilterTripTypeId { get; set; }

        public bool UseSpecificationTypeFilter { get; set; }

        public string FilterSpecificationTypeId { get; set; }

        public bool UseVehicleTypeFilter { get; set; }

        public string FilterVehicleTypeId { get; set; }

        public bool UsePayerFilter { get; set; }

        public bool UseRouteFilter { get; set; }

        public string FilterPayerId { get; set; }
        public int? tenderNumber { get; set; }
        public long itemExternalNumber { get; set; }
        public string ContragentName { get; set; }
        public int ContragentIdAps { get; set; }
        public System.DateTime DateUpdateInfo { get; set; }
        public bool IsWinner { get; set; }
        public long EDRPOUContragent { get; set; }
        public string emailContragent { get; set; }
        public int transportUnitsProposed { get; set; }
        public Nullable<int> acceptedTransportUnits { get; set; }
        public double costOfCarWithoutNDS { get; set; }
        public int PaymentDelay { get; set; }
        public System.Guid tenderItemUuid { get; set; }
        public string nmcName { get; set; }
        public double costOfCarWithoutNDSToNull { get; set; }
        public string note { get; set; }

    }

}
