using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Corum.Common;
using Foolproof;

namespace Corum.Models.ViewModels.Orders
{
    
    public class OrdersTruckTransportViewModel : OrderBaseViewModel
    {
        public long TruckTransportRowId { get; set; }

        public long OrderId { get; set; }
        public int DefaultCountry { get; set; }

        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Страна по умолчанию' не больше 100 символов")]
        public string DefaultCountryName { get; set; }

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

        [Required(ErrorMessage = "Введите страну грузополучателя")]
        [Display(Name = "Страна грузополучателя")]
        public int ConsigneeCountryId { get; set; }
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Страна грузополучателя' не больше 100 символов")]
        public string ConsigneeCountryName { get; set; }

        [Required(ErrorMessage = "Введите город грузополучателя")]
        [Display(Name = "Город грузополучателя")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Город грузополучателя' не больше 255 символов")]
        public string ConsigneeCity { get; set; }

        [Required(ErrorMessage = "Введите адрес грузополучателя")]
        [Display(Name = "Адрес грузополучателя")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Адрес грузополучателя' не больше 500 символов")]
        public string ConsigneeAdress { get; set; }

        [Required(ErrorMessage = "Введите организацию грузоотправителя")]
        [Display(Name = "Грузоотправитель")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Грузоотправитель' не больше 500 символов")]
        public string Shipper { get; set; }
        
        [Required(ErrorMessage = "Введите организацию грузополучателя")]
        [Display(Name = "Грузополучатель")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Грузополучатель' не больше 500 символов")]
        public string Consignee { get; set; }

        [Required(ErrorMessage = "Введите дату подачи для загрузки")]
        [Display(Name = "Дата подачи для загрузки")]
        public string FromShipperDate { get; set; }
        public string FromShipperDateRaw { get; set; }

        [Required(ErrorMessage = "Введите время подачи для загрузки")]
        [Display(Name = "Время подачи для загрузки")]
        public string FromShipperTime { get; set; }
        public string FromShipperTimeRaw { get; set; }

        [Required(ErrorMessage = "Введите дату прибытия для разгрузки")]
        [Display(Name = "Дата прибытия для разгрузки")]
        public string ToConsigneeDate { get; set; }
        public string ToConsigneeDateRaw { get; set; }

        [Required(ErrorMessage = "Введите время прибытия для разгрузки")]
        [Display(Name = "Время прибытия для разгрузки")]
        public string ToConsigneeTime { get; set; }
        public string ToConsigneeTimeRaw { get; set; }

        [Required(ErrorMessage = "Введите вид упаковки")]
        [Display(Name = " Вид упаковки")]
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Вид упаковки' не больше 100 символов")]
        public string BoxingDescription { get; set; }

        [Required(ErrorMessage = "Введите информация о грузе")]
        [Display(Name = "Информация о грузе")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Информация о грузе' не больше 500 символов")]
        public string TruckDescription { get; set; }

        [Display(Name = "Тип поездки")]
        public int TripType { get; set; }

        [Required(ErrorMessage = "Введите вес")]
        [Display(Name = "Вес,т")]
        public /*double*/ string Weight { get; set; }

        [Required(ErrorMessage = "Введите объем")]
        [Display(Name = "Объем,м3")]
        public double Volume { get; set; }

        [Required(ErrorMessage = "Введите длина")]
        [Display(Name = "Длина,см")]
        public double DimenssionL { get; set; }

        [Required(ErrorMessage = "Введите ширину")]
        [Display(Name = "Ширина,см")]
        public double DimenssionW { get; set; }

        [Required(ErrorMessage = "Введите высоту")]
        [Display(Name = "Высота,см")]
        public double DimenssionH { get; set; }

        [Required(ErrorMessage = "Введите тип груза")]
        [Display(Name = "Тип груза")]
        public int TruckTypeId { get; set; }
        [StringLength(50, ErrorMessage = "Максимальная длина поля 'Тип груза' не больше 50 символов")]
        public string TruckTypeName { get; set; }

        [Required(ErrorMessage = "Введите тип автомобиля")]
        [Display(Name = "Информация о типе автомобиля")]
        public int VehicleTypeId { get; set; }
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Информация о типе автомобиля' не больше 100 символов")]
        public string VehicleTypeName { get; set; }

        [Required(ErrorMessage = "Введите тип загрузки")]
        [Display(Name = "Тип загрузки")]
        public int LoadingTypeId { get; set; }
        [StringLength(50, ErrorMessage = "Максимальная длина поля 'Тип загрузки' не больше 50 символов")]
        public string LoadingTypeName { get; set; }

        [Required(ErrorMessage = "Введите тип разгрузки")]
        [Display(Name = "Тип разгрузки")]
        public int UnloadingTypeId { get; set; }
        [StringLength(50, ErrorMessage = "Максимальная длина поля 'Тип разгрузки' не больше 50 символов")]
        public string UnloadingTypeName { get; set; }

        [Required(ErrorMessage = "Введите контактное лицо грузоотправителя")]
        [Display(Name = "Контактное лицо грузоотправителя")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Контактное лицо грузоотправителя' не больше 255 символов")]
        public string ShipperContactPerson { get; set; }

        [Required(ErrorMessage = "Введите телефон контактного лица грузоотправителя")]
        [Display(Name = "Телефон контактного лица грузоотправителя")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Телефон контактного лица грузоотправителя' не больше 255 символов")]
        public string ShipperContactPersonPhone { get; set; }

        [Required(ErrorMessage = "Введите контактное лицо грузополучателя")]
        [Display(Name = "Контактное лицо грузогрузополучателя")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Контактное лицо грузогрузополучателя' не больше 255 символов")]
        public string ConsigneeContactPerson { get; set; }

        [Required(ErrorMessage = "Введите телефон контактного лица грузополучателя")]
        [Display(Name = "Телефон контактного лица грузополучателя")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Телефон контактного лица грузополучателя' не больше 255 символов")]
        public string ConsigneeContactPersonPhone { get; set; }        

        [Display(Name = "Тип поездки")]
        public string NameRouteType { get; set; }

        public List<OrderAdditionalRoutePointModel> LoadPoints { get; set; }

        public List<OrderAdditionalRoutePointModel> UnLoadPoints { get; set; }

        public string OrganizationLoadPoints { get; set; }

        public string OrganizationUnLoadPoints { get; set; }

        public string AddressLoadPoints { get; set; }

        public string AddressUnLoadPoints { get; set; }

        public string ContactsLoadPoints { get; set; }

        public string ContactsUnLoadPoints { get; set; }

        public int CountLoadAndUnLoadPoints { get; set; }

        public int CountUnLoadPoints { get; set; }

        public long ShipperId { get; set; }

        public long ConsigneeId { get; set; }

        public decimal LatitudeShipper { get; set; }

        public decimal LatitudeConsignee { get; set; }

        public decimal LongitudeShipper { get; set; }

        public decimal LongitudeConsignee { get; set; }

        public OrdersTruckTransportViewModel():base()
        {
            ConsigneeCity = string.Empty;
            ShipperCity   = string.Empty;
            TripType = 0;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            base.Validate(validationContext);

            List<ValidationResult> errors = new List<ValidationResult>();

            try
            {

                var FromShipperDatetime = DateTimeConvertClass.getDateTime(this.FromShipperDateRaw).
                                                       AddHours(DateTimeConvertClass.getHours(this.FromShipperTimeRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(this.FromShipperTimeRaw));

                var ToConsigneeDatetime = DateTimeConvertClass.getDateTime(this.ToConsigneeDateRaw).
                                                       AddHours(DateTimeConvertClass.getHours(this.ToConsigneeTimeRaw)).
                                                       AddMinutes(DateTimeConvertClass.getMinutes(this.ToConsigneeTimeRaw));

                if (FromShipperDatetime.Ticks > ToConsigneeDatetime.Ticks)
                    errors.Add(new ValidationResult("Дата отгрузки больше даты разгрузки!"));

                this.OrderServiceDatetime = FromShipperDatetime;

                if (TruckTypeId == 0)
                    errors.Add(new ValidationResult("Выберите тип груза!"));

                if (VehicleTypeId == 0)
                    errors.Add(new ValidationResult("Выберите тип транспорта!"));

                if (LoadingTypeId == 0)
                    errors.Add(new ValidationResult("Выберите тип загрузки!"));

                if (UnloadingTypeId == 0)
                    errors.Add(new ValidationResult("Выберите тип разгрузки!"));

                if (this.ClientId == 0)
                    errors.Add(new ValidationResult("Не выбрано функциональное подразделение!"));

                if (this.PayerId == 0)
                    errors.Add(new ValidationResult("Не выбран плательщик!"));
            }
            catch (Exception exc)
            {
                errors.Add(new ValidationResult(exc.Message));
            }

            return errors;
        }
    }
}
