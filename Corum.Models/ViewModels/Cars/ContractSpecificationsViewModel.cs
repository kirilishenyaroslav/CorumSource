using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Corum.Models.ViewModels.Orders;


namespace Corum.Models.ViewModels.Cars
{
    public class ContractSpecificationsViewModel
    {

        public int Id { get; set; }

        public int GroupeSpecId { get; set; }

        public long? RouteId { get; set; }

        public string CreatedByUser { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreateDate { get; set; }
        public string CreateDateRaw { get; set; }

        [Display(Name = "Дата начала")]
        public string DateBeg { get; set; }

        [Display(Name = "Дата конца")]
        public string DateEnd { get; set; }

        [Display(Name = "Дата начала")]
        public string DateBegRaw { get; set; }

        [Display(Name = "Дата конца")]
        public string DateEndRaw { get; set; }

        [Required(ErrorMessage = "Укажите грузоподъемность")]
        [Display(Name = "Грузоподъемность автомобиля")]
        public int? CarryCapacityId { get; set; }

        [Display(Name = "Грузоподъемность автомобиля")]
        public decimal? CarryCapacityVal { get; set; }

        [Display(Name = "Тип услуги")]
        public int TypeSpecId { get; set; }

        [Display(Name = "Тип услуги")]
        public string TypeSpecName { get; set; }

        [Display(Name = "Точка отправления")]
        public string DeparturePoint { get; set; }

        [Display(Name = "Точка прибытия")]
        public string ArrivalPoint { get; set; }

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
        public int? IntervalTypeId { get; set; }
    
        [Display(Name = "Тип интервала маршрута")]
        public string IntervalTypeName { get; set; }

        [Display(Name = "Тариф грн/км")]
        public string RateKm { get; set; }

        [Display(Name = "Тариф грн/час")]
        public string RateHour { get; set; }

        [Display(Name = "Тариф маш./час")]
        public string RateMachineHour { get; set; }

        [Display(Name = "Тариф за фрахт")]
        public string RateTotalFreight { get; set; }

        [Required(ErrorMessage = "Введите НДС")]
        [Display(Name = "НДС")]
        public string NDSTax { get; set; }
        public bool IsMainMenu { get; set;}

        [Display(Name = "Грузовые перевозки")]
        public bool IsTruck { get; set; }

        [Display(Name = "Договорная цена")]
        public bool IsPriceNegotiated { get; set; }

        [Display(Name = "Тип автомобиля")]
        public int TypeVehicleId { get; set; }

        [Display(Name = "Тип автомобиля")]
        public string TypeVehicleName { get; set; }

        [Required(ErrorMessage = "Введите название услуги")]
        [Display(Name = "Наименование услуги")]
        public int NameId { get; set; }

        [Required(ErrorMessage = "Введите название услуги")]
        [Display(Name = "Наименование услуги")]
        public string NameSpecification { get; set; }

        [Display(Name = "Название маршрута")]
        public string RouteName { get; set; }

        public int CountRows { get; set; }

        public int GenId { get; set; }

        public ContractsViewModel ContractInfo { set; get; }

        public int ContractId { get; set; }

        public GroupesSpecificationsViewModel GroupeSpecInfo { set; get; }

        public IList<CarryCapacitiesViewModel>  CarryCapacityInfo { set; get; }

        public IList<RouteTypesViewModel> RouteTypesInfo { set; get; }

        public IList<RouteIntervalTypesViewModel> IntervalTypesInfo { set; get; }

        public IList<SpecificationTypesViewModel> SpecTypeInfo { set; get; }

        public IList<VehicleViewModel> TypeVehicleInfo { set; get; }
    }

    public class GroupesSpecificationsViewModel : BaseViewModel
    {

        public int Id { get; set; }

        public int ContractId { get; set; }

        public string CreatedByUser { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreateDate { get; set; }
        public string CreateDateRaw { get; set; }

        [Display(Name = "Дата начала")]
        public string DateBeg { get; set; }

        [Display(Name = "Дата конца")]
        public string DateEnd { get; set; }

        [Display(Name = "Дата начала")]
        public string DateBegRaw { get; set; }

        [Display(Name = "Дата конца")]
        public string DateEndRaw { get; set; }

        [Required(ErrorMessage = "Введите название спецификации")]
        [Display(Name = "Название спецификации")]
        public string NameGroupeSpecification { get; set; }

        [Display(Name = "Отсрочка в днях")]
        public int? DaysDelay { get; set; }

        [Display(Name = "Активный")]
        public bool IsActive { get; set; }

        public bool IsMainMenu { get; set; }

        public int CountSpecifications { get; set; }

        public ContractsViewModel ContractInfo { set; get; }

        [Display(Name = "Стоимость топлива(ДТ/бензин)")]
        public string FuelPrice { get; set; }

        [Display(Name = "Курс грн/руб")]
        public string ExchangeRateUahRub { get; set; }

        [Required(ErrorMessage = "Введите НДС")]
        [Display(Name = "НДС")]
        public string NDSTax { get; set; }

        public string BackgroundColor { get; set; }
    }

    public class CarryCapacitiesViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Грузоподъемность")]
        public string CarryCapacity { get; set; }

        [Display(Name = "Максимальная грузоподъемность")]
        public string MaxCapacity { get; set; }

        [Display(Name = "Комментарий")]
        public string CommentCapacity { get; set; }
    }

    public class RouteTypesViewModel
    {

        public int Id { get; set; }

        public string RouteTypeName { get; set; }
    }

    public class RouteIntervalTypesViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Тип интервала маршрута")]
        public string IntervalTypeName { get; set; }

        [Display(Name = "Максимальный интервал маршрута")]
        public int? MaxInterval { get; set; }
    }

    public class SpecificationNamesViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Введите код услуги")]
        [Display(Name = "Код услуги")]
        public int SpecCode { get; set; }

        [Required(ErrorMessage = "Введите наименование услуги")]
        [Display(Name = "Наименование услуги")]
        public string SpecName { get; set; }
        public int? nmcTestId { get; set; }
        public int? nmcWorkId { get; set; }
        public int? industryId { get; set; }
    }

    public class SpecificationTypesViewModel
    {

        public int Id { get; set; }
        public string SpecificationType { get; set; }
        public bool Assigned { get; set; }

        public SpecificationTypesViewModel()
        {
            Assigned = false;
        }
    }


}
