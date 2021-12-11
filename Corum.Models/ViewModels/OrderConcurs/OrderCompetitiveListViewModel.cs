using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.OrderConcurs
{
    public class OrderCompetitiveListViewModel
    {
        public long Id { get; set; }

        public long OrderId { get; set; }

        public IEnumerable<CompetitiveListStepViewModel> avialiableSteps { set; get; }

        public CompetetiveListStepsInfoViewModel currentStep { get; set; }

        public int GenId { get; set; }

        [Display(Name = "Плательщик")]
        public int PayerId { set; get; }

        [Display(Name = "Плательщик")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Плательщик' не больше 500 символов")]
        public string PayerName { set; get; }        

        [Display(Name = "Грузоподъемность автомобиля, тонн")]
        public string CarryCapacity { get; set; }

        [Display(Name = "Предложено транспортных единиц, шт.")]
        public int CarsOffered { get; set; }

        [Display(Name = "Акцептовано транспортных единиц, шт.")]
        public int CarsAccepted { get; set; }

        [Display(Name = "НДС")]
        public string NDS { get; set; }

         [Display(Name = "Стоимость одного автомобиля, грн. без НДС (согласно договору)")] //6
        //[Display(Name = "Стоимость 1-го автомобиля, грн. без НДС (кол. 6)")] //6
        public string CarCostDog { get; set; }

        [Display(Name = "Стоимость одного автомобиля, грн. без НДС (согласно КП-1)")]
       // [Display(Name = "Стоимость 1-го автомобиля, грн. без  НДС (кол. 7)")]
        public string CarCost7 { get; set; }

        [Display(Name = "Стоимость одного автомобиля, грн. без НДС (согласно КП-2)")]
        //[Display(Name = "Стоимость 1-го автомобиля, грн. без  НДС (кол. 8)")]
        public string CarCost { get; set; }

        [Display(Name = "Договорная отсрочка платежей, дней")]
        public int? DaysDelay { get; set; }

        [Display(Name = "Эффект от отсрочки")]
        public string DelayEffect { get; set; }

        [Display(Name = "Предоплата дней (12 колонка)")]
        public int? Prepayment { get; set; }
        
        [Display(Name = "Эффект от предоплаты (14 колонка)")]
        public string PrepaymentEffect { get; set; }

        [Display(Name = "Сумма предоплаты (13 колонка)")]
        public string Prepayment2 { get; set; }

        [Display(Name = "Эффект от предоплаты (15 колонка)")]
        public string PrepaymentEffect2 { get; set; }

        [Display(Name = "Стоим. 1-го авто грн. с учетом стоим. денег")]
        public string CarCostWithMoneyCost { get; set; }

        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Примечание' не больше 255 символов")]
        [Display(Name = "Примечание")]
        public string Comments { get; set; }

        [Display(Name = "Примечание")]
        public string Comments_Cut { get; set; }

        [Display(Name = "Средняя цена за грн./км.")]
        public string AverageCost { get; set; }

        [Display(Name = "Перевозчик/Экспедитор")]                        
        public string ExpeditorName { get; set; }

        [Display(Name = "Выбрано")]
        public bool IsSelectedId { get; set; }

        [Display(Name = "Фрахт?")]
        public bool IsFreight { get; set; }

        [Display(Name = "Стоимость за 1 км?")]
        public bool IsRateKm { get; set; }

        [Display(Name = "Длина маршрута")]
        public string RouteLength { get; set; }

        [Display(Name = "Стоимость за 1 км")]
        public string RateKm { get; set; }

        [Display(Name = "Стоимость за 1 час")]
        public string RateHour { get; set; }

        [Display(Name = "Стоимость за 1 машино/час")]
        public string RateMachineHour { get; set; }

        [Display(Name = "Идентификатор спецификации")]
        public int SpecificationId { get; set; }

        public bool IsChange { get; set; }

        [Display(Name = "Тип автомобиля")]
        public string VehicleTypeName { get; set; }

        [Display(Name = "Тип автомобиля")]
        public int TypeVehicleId { get; set; }

        public bool isTruck { get; set; }

        [Display(Name = "Ставка дисконтирования ")]
        public double? DiscountRate { get; set; }

        public string SelectedItem { get; set; }

        public string NameSpecification { get; set; }

        public string NameCarCostDog { get; set; }

        [Display(Name = "КП-1) отсрочка платежей, дней")]
        public int DaysDelayStep1 { get; set; }

        [Display(Name = "КП-2) отсрочка платежей, дней")]
        public int DaysDelayStep2 { get; set; }

        [Display(Name = "Акцептованная цена")]
        public decimal AcceptPrice { get; set; }

        [Display(Name = "Акцептованная отсрочка")]
        public int AcceptDaysDelay { get; set; }

        public DateTime OrderDate { get; set; }

        public bool IsZeroTarif { get; set; }

        public string DaysDelaySteps { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string Route { get; set; }

        public decimal TotalDistanceLenght { set; get; }

        public int CntNameSpecification { set; get; }

        public long HistoryOrderId { get; set; }

        public CompetitiveListViewModel CompetitiveListInfo { get; set; }
        public Nullable<int> tenderNumber { get; set; }
        public string itemDescription { get; set; }
        public string cargoWeight { get; set; }
        public string emailContragent { get; set; }
        public string[] emailsContragent { get; set; }
        public Nullable<System.Guid> formUuid { get; set; }
        public Nullable<int> tenderTureNumber { get; set; }
    }

}

