using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.OrderConcurs
{

    public class CompetitiveListViewModel : BaseViewModel
    {
        public bool PublicEntry { set; get; }

        public CompetetiveListStepsInfoViewModel currentStep { get; set; }

        [Display(Name = "Номер заявки")]
        public long Id { set; get; }

        [Display(Name = "Тип маршрута")]
        public string tripTypeName { set; get; }

        [Required(ErrorMessage = "Введите дату заявки")]
        [Display(Name = "Дата заявки")]
        public string OrderDate { set; get; }
        public string OrderDateRaw { set; get; }

        [Display(Name = "Составитель заявки")]
        [StringLength(128, ErrorMessage = "Максимальная длина поля 'Составитель заявки' не больше 128 символов")]
        public string CreatedByUser { set; get; }

        [Display(Name = "Составитель заявки")]
        [StringLength(128, ErrorMessage = "Максимальная длина поля 'Составитель заявки' не больше 128 символов")]
        public string CreatedByUserName { set; get; }
        public System.DateTime CreateDatetime { set; get; }

        [Required(ErrorMessage = "Введите тип заявки")]
        [Display(Name = "Тип заявки")]
        public int OrderType { set; get; }

        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Название типа заявки' не больше 100 символов")]
        public string OrderTypename { set; get; }
        [StringLength(128, ErrorMessage = "Максимальная длина поля 'Сокращенное наименование типа' не больше 128 символов")]
        public string OrderTypeShortName { set; get; }

        public bool AllowClientData { set; get; }
        public bool AllowExecuterData { set; get; }

        [Required(ErrorMessage = "Введите статус заявки")]
        [Display(Name = "Текущий сатус заявки")]
        public int CurrentOrderStatus { set; get; }

        [StringLength(20, ErrorMessage = "Максимальная длина поля 'Название цвета' не больше 20 символов")]
        public string CurrentOrderStatusColor { get; set; }
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Название действия' не больше 255 символов")]
        public string CurrentStatusActionName { get; set; }
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Название статуса заявки' не больше 100 символов")]
        public string CurrentOrderStatusName { get; set; }

        [StringLength(25, ErrorMessage = "Максимальная длина поля 'Иконка' не больше 25 символов")]
        public string IconFile { get; set; }
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Описание статуса для подсказки' не больше 500 символов")]
        public string IconDescription { get; set; }

        [Display(Name = "Общее описание заявки")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Общее описание заявки' не больше 500 символов")]
        public string OrderDescription { set; get; }

        [Required(ErrorMessage = "Выберите информацию о заказчике")]
        [Display(Name = "ЦФО/Функциональное подразделение")]
        public long ClientId { set; get; }

        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Подразделение заказчика' не больше 500 символов")]
        public string ClientName { set; get; }
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'ЦФО заказчика' не больше 500 символов")]
        public string ClientCenterName { set; get; }

        public long ClientDogId { get; set; }
        public System.DateTime DogDateBeg { get; set; }
        public System.DateTime DogDateEnd { get; set; }

        [Required(ErrorMessage = "Введите информацию о сумме")]
        [Display(Name = "Сумма выполнения")]
        public decimal Summ { set; get; }

        [Display(Name = "Уведомлять при изменении")]
        public bool UseNotifications { set; get; }

        [Required(ErrorMessage = "Введите информацию о контактном лице")]
        [Display(Name = "Контактное лицо (Ф.И.О /должность)")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Контактное лицо' не больше 255 символов")]
        public string CreatorPosition { set; get; }

        [Required(ErrorMessage = "Введите контактную информацию заказчика")]
        [Display(Name = "Контактный телефон по заявке", Prompt = "тел./e-mail/skype")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля 'Контактная информация заказчика' не больше 255 символов")]
        public string CreatorContact { set; get; }

        [Display(Name = "Тип приоритета заявки")]
        public int PriorityType { set; get; }

        [Display(Name = "Исполнитель заявки")]
        [StringLength(128, ErrorMessage = "Максимальная длина поля 'Исполнитель заявки' не больше 128 символов")]
        public string OrderExecuter { set; get; }

        [Display(Name = "Исполнитель заявки")]
        [StringLength(256, ErrorMessage = "Максимальная длина поля 'Исполнитель заявки' не больше 256 символов")]
        public string OrderExecuterName { set; get; }

        [Required(ErrorMessage = "Введите информацию о плательщике")]
        [Display(Name = "Плательщик")]
        public int PayerId { set; get; }

        [Display(Name = "Плательщик")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Плательщик' не больше 500 символов")]
        public string PayerName { set; get; }

        [Display(Name = "Номер проекта")]
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Номер проекта' не больше 100 символов")]
        public string ProjectNum { set; get; }

        [Display(Name = "Необходимое количество автомобилей")]
        public int CarNumber { set; get; }

        [Display(Name = "Общее описание маршрута по заявке")]
        public string TotalDistanceDescription { set; get; }

        [Required(ErrorMessage = "Введите общую стоимость выполнения заявки")]
        //[Range(0.0, Double.MaxValue, ErrorMessage = "Введите корректное число в поле 'Общая стоимость выполнения заявки'")]
        [Display(Name = "Общая стоимость выполнения заявки")]
        public decimal TotalCost { set; get; }

        [Required(ErrorMessage = "Введите общее расстояние по заявке")]
        //[Range(0, int.MaxValue, ErrorMessage = "Введите корректное число в поле 'Общее расстояние по заявке'")]
        [Display(Name = "Общее расстояние по заявке")]
        public decimal TotalDistanceLenght { set; get; }

        [Display(Name = "Приватная заявка")]
        public bool IsPrivateOrder { set; get; }

        [Display(Name = "Заметки при выполнении")]
        public string ExecuterNotes { set; get; }

        public IList<UserViewModel> AvaliableExecuters { set; get; }

        public string AcceptDate { get; set; }

        public string StartExecuteDate { get; set; }

        public System.DateTime OrderServiceDatetime { get; set; }

        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Комментарий при смене статуса' не больше 500 символов")]
        public string StatusChangeComment { get; set; }

        public string ObserversForNotification { get; set; }

        [StringLength(25, ErrorMessage = "Максимальная длина поля 'Сокращенное наименование' не больше 25 символов")]
        public string CurrentStatusShortName { get; set; }

        [StringLength(25, ErrorMessage = "Максимальная длина поля 'Цвет шрифта' не больше 25 символов")]
        public string FontColor { get; set; }
        [StringLength(25, ErrorMessage = "Максимальная длина поля 'Цвет фона' не больше 25 символов")]
        public string BackgroundColor { get; set; }

        public bool IsTransport { get; set; }

        public bool OpenedByCreator { get; set; }

        public bool OpenedByLPR { get; set; }

        public bool IsFinishOfTheProcess { get; set; }

        public string ReportStatusName { get; set; }
        public int ReportStatusId { get; set; }
        public string ReportColor { get; set; }

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
        [Display(Name = "Организация грузоотправитель")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля 'Грузоотправитель' не больше 500 символов")]
        public string Shipper { get; set; }

        [Required(ErrorMessage = "Введите организацию грузополучателя")]
        [Display(Name = "Организация грузополучатель")]
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

        [Display(Name = "Информация о грузе")]
        public string TruckDescription_Cut { get; set; }

        [Display(Name = "Тип поездки")]
        public int TripType { get; set; }

        [Required(ErrorMessage = "Введите вес")]
        [Display(Name = "Вес,т")]
        public string Weight { get; set; }

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

        public string TruckTypeName_Cut { get; set; }

        public string AvtoPlanFact { get; set; }

        [Required(ErrorMessage = "Введите тип автомобиля")]
        [Display(Name = "Тип автомобиля")]
        public int VehicleTypeId { get; set; }
        
        [StringLength(100, ErrorMessage = "Максимальная длина поля 'Информация о типе автомобиля' не больше 100 символов")]
        [Display(Name = "Тип автомобиля")]
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

        [Display(Name = "Пункт отправления")]
        public string FromInfo { get; set; }

        [Display(Name = "Пункт прибытия")]
        public string ToInfo { get; set; }


        [Display(Name = "Автор заявки")]
        public string OrdersAuthor { get; set; }

        public List<string> FinalStatuses { get; set; }

        [Display(Name = "Тип услуги")]
        public string SpecificationType { get; set; }

        public string FinalComment { get; set; }

        public int CurrentOrderStatusCnt { get; set; }

        public int FactCarNumber { set; get; }

        public string FromDate { get; set; }
        public string FromDateRaw { get; set; }

        public string ToDate { get; set; }
        public string ToDateRaw { get; set; }

        public string Route { get; set; }

        public string Dimenssion { get; set; }

        public string CityFrom { get; set; }

        public string CityTo { get; set; }

        public bool IsTruck { get; set; }

        public bool UseTripTypeFilter { get; set; }

       public string FilterTripTypeId { get; set; }

       public bool UseVehicleTypeFilter { get; set; }

       public bool UsePayerFilter { get; set; }

       public string FilterVehicleTypeId { get; set; }

       public string FilterPayerId { get; set; }

       public bool UseSpecificationTypeFilter { get; set; }

       public string FilterSpecificationTypeId { get; set; }

        public bool UseRouteFilter { get; set; }

        [Display(Name = "Время нахождения на маршруте")]
        [DisplayFormat(DataFormatString = "{0:dd\\:hh}", ApplyFormatInEditMode = true)]
        public string TimeRoute { get; set; }

        [Display(Name = "Время работы спецтранспорта")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public string TimeSpecialVehicles { get; set; }        

        public CompetitiveListViewModel()
        {
            CarNumber = 1;
            TotalCost = 0;
            TotalDistanceLenght = 0;
            ConsigneeCity = string.Empty;
            ShipperCity = string.Empty;
            TripType = 0;
            IsTruck = false;
        }

    }

}
