using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Corum.Common;
using Foolproof;

namespace Corum.Models.ViewModels.Orders
{
    
    public class OrdersPassTransportViewModel: OrderBaseViewModel
    {
        public long PassTransportRowId { get; set; }

        public long OrderId { get; set; }

        [Required(ErrorMessage = "Введите дату отправления")]
        [Display(Name = "Дата отправления")]
        public string StartDateTimeOfTrip { get; set; }

        [Required(ErrorMessage = "Введите время отправления")]
        [Display(Name = "Время отправления")]
        public string StartDateTimeExOfTrip { get; set; }

        [Required(ErrorMessage = "Введите дату прибытия")]
        [Display(Name = "Дата прибытия")]
        public string FinishDateTimeOfTrip { get; set; }

        [Required(ErrorMessage = "Введите время возврата")]
        [Display(Name = "Время прибытия")]
        public string FinishDateTimeExOfTrip { get; set; }

        public string StartDateTimeOfTripRaw { get; set; }
        public string FinishDateTimeOfTripRaw { get; set; }

        public string StartDateTimeExOfTripRaw { get; set; }
        public string FinishDateTimeExOfTripRaw { get; set; }

        [Required(ErrorMessage = "Введите страну отправления")]
        [Display(Name = "Страна отправления")]        
        public int CountryFrom { get; set; }

        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Страна отправления' не больше 100 символов")]
        public string CountryFromName { get; set; }

        public int DefaultCountry { get; set; }
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Страна по умолчанию' не больше 100 символов")]
        public string DefaultCountryName { get; set; }

        [Required(ErrorMessage = "Введите город отправления")]
        [Display(Name = "Город отправления")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Город отправления' не больше 255 символов")]
        public string CityFrom { get; set; }

        [Required(ErrorMessage = "Введите адрес отправления")]
        [Display(Name = "Адрес отправления")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Адрес отправления' не больше 500 символов")]
        public string AdressFrom { get; set; }

        
        [Required(ErrorMessage = "Введите страну прибытия")]
        [Display(Name = "Страна прибытия")]
        public int CountryTo { get; set; }

        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Страна прибытия' не больше 100 символов")]
        public string CountryToName { get; set; }

        [Required(ErrorMessage = "Введите город прибытия")]
        [Display(Name = "Город прибытия")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Город прибытия' не больше 255 символов")]
        public string CityTo { get; set; }

        [Required(ErrorMessage = "Введите организацию отправителя")]
        [Display(Name = "Организация отправитель")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Организация отправитель' не больше 500 символов")]
        public string OrgFrom { get; set; }
        
        [Required(ErrorMessage = "Введите адрес прибытия")]
        [Display(Name = "Адрес прибытия")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Адрес прибытия' не больше 500 символов")]
        public string AdressTo { get; set; }

        [Required(ErrorMessage = "Введите организацию прибытия")]
        [Display(Name = "Организация прибытия")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Организация прибытия' не больше 500 символов")]
        public string OrgTo { get; set; }

        [Display(Name = "Бронирование автомобиля для обратного пути")]
        public bool NeedReturn { get; set; }

        [RequiredIf("NeedReturn", true, ErrorMessage = "Введите дату обратного отправления")]
        [Display(Name = "Дата обратного отправления")]
        public string ReturnStartDateTimeOfTrip { get; set; }

        [RequiredIf("NeedReturn", true, ErrorMessage = "Введите время обратного отправления")]
        [Display(Name = "Время обратного отправления")]
        public string ReturnStartDateTimeExOfTrip { get; set; }

        [RequiredIf("NeedReturn", true, ErrorMessage = "Введите дату окончания поездки")]
        [Display(Name = "Дата окончания поездки")]
        public string ReturnFinishDateTimeOfTrip { get; set; }

        [RequiredIf("NeedReturn", true, ErrorMessage = "Введите время окончания поездки")]
        [Display(Name = "Время окончания поездки")]
        public string ReturnFinishDateTimeExOfTrip { get; set; }

        public string ReturnStartDateTimeOfTripRaw { get; set; }
        public string ReturnFinishDateTimeOfTripRaw { get; set; }

        public string ReturnStartDateTimeExOfTripRaw { get; set; }
        public string ReturnFinishDateTimeExOfTripRaw { get; set; }

        [Required(ErrorMessage = "Введите цель поездки")]
        [Display(Name = "Цель поездки")]        
        public string TripDescription { get; set; }

        [Required(ErrorMessage = "Введите список пассажиров")]
        [Display(Name = "Список пассажиров")]
        public string PassInfo { get; set; }

        [Display(Name = "Номер и марка авто")]
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Номер и марка авто' не больше 100 символов")]
        public string CarDetailInfo { get; set; }

        [Display(Name = "ФИО водителя")]
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'ФИО водителя' не больше 100 символов")]
        public string CarDriverFio { get; set; }

        [Display(Name = "Контакты водителя")]        
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Контакты водителя' не больше 100 символов")]
        public string CarDriverContactInfo { get; set; }

        [Display(Name = "Время ожидания")]
        public string ReturnWaitingTime { get; set; }

        [Display(Name = "Тип поездки")]
        public int TripType { get; set; }

        [Display(Name = "Тип поездки")]
        public string NameRouteType { get; set; }

        public long OrgFromId { get; set; }

        public long OrgToId { get; set; }

        public decimal LatitudeOrgFrom { get; set; }

        public decimal LongitudeOrgFrom { get; set; }

        public decimal LatitudeOrgTo { get; set; }

        public decimal LongitudeOrgTo { get; set; }

        public OrdersPassTransportViewModel():base()
        {
            CityFrom = string.Empty;
            CityTo   = string.Empty;
            TripType = 0;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            base.Validate(validationContext);

            List<ValidationResult> errors = new List<ValidationResult>();

            try
            {

                


                var OrderDate = DateTimeConvertClass.getDateTime(this.OrderDateRaw).
                                                       AddHours(DateTimeConvertClass.getHours(this.OrderDateRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(this.OrderDateRaw));

                var StartDateTimeOfTrip = DateTimeConvertClass.getDateTime(this.StartDateTimeOfTripRaw).
                                                       AddHours(DateTimeConvertClass.getHours(this.StartDateTimeExOfTripRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(this.StartDateTimeExOfTripRaw));

                var FinishDateTimeOfTrip = DateTimeConvertClass.getDateTime(this.FinishDateTimeOfTripRaw).
                                                       AddHours(DateTimeConvertClass.getHours(this.FinishDateTimeExOfTripRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(this.FinishDateTimeExOfTripRaw));

                if ((StartDateTimeOfTrip != null) && (FinishDateTimeOfTrip != null))
                {
                    if (StartDateTimeOfTrip.Ticks > FinishDateTimeOfTrip.Ticks)
                        errors.Add(new ValidationResult("Дата выезда больше даты прибытия!"));

                    this.OrderServiceDatetime = StartDateTimeOfTrip;
                }


                var NeedReturn = this.NeedReturn;

                if (NeedReturn)
                {
                    var ReturnStartDateTimeOfTrip = DateTimeConvertClass.getDateTime(this.ReturnStartDateTimeOfTripRaw).
                                                           AddHours(DateTimeConvertClass.getHours(this.ReturnStartDateTimeExOfTripRaw)).
                                                           AddMinutes(DateTimeConvertClass.getMinutes(this.ReturnStartDateTimeExOfTripRaw));

                    var ReturnFinishDateTimeOfTrip = DateTimeConvertClass.getDateTime(this.ReturnFinishDateTimeOfTripRaw).
                                                           AddHours(DateTimeConvertClass.getHours(this.ReturnFinishDateTimeExOfTripRaw)).
                                                           AddMinutes(DateTimeConvertClass.getMinutes(this.ReturnFinishDateTimeExOfTripRaw));


                    if ((ReturnStartDateTimeOfTrip != null) && (ReturnFinishDateTimeOfTrip != null))
                    {
                        if (ReturnStartDateTimeOfTrip.Ticks > ReturnFinishDateTimeOfTrip.Ticks)
                            errors.Add(new ValidationResult("Дата обратного отправления больше даты окончания поездки!"));
                    }

                    if ((FinishDateTimeOfTrip != null) && (ReturnStartDateTimeOfTrip != null))
                    {
                        if (FinishDateTimeOfTrip.Ticks > ReturnStartDateTimeOfTrip.Ticks)
                            errors.Add(new ValidationResult("Дата обратного отправления меньше даты прибытия!"));
                    }

                }

                if (this.ClientId == 0)
                    errors.Add(new ValidationResult("Не выбрано функциональное подразделение!"));

                if (this.PayerId == 0)
                    errors.Add(new ValidationResult("Не выбран плательщик!"));
            }
            catch (Exception exc)
            {
                errors.Add(new ValidationResult(exc.Message));
            }

            //if ((this.PriorityType==0) && 
            //    ((StartDateTimeOfTrip-OrderDate).TotalDays<1))
            //{
            //    errors.Add(new ValidationResult("Не выдержаны сроки для плановой заявки!"));
            //}

            

            return errors;
        }
    }
}
