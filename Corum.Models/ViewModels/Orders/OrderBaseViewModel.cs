using System;
using Corum.Models;
using System.Reflection;
using Corum.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Corum.Models.ViewModels.Cars;
using Corum.Models.ViewModels.Customers;

namespace Corum.Models.ViewModels.Orders
{
   
    public class OrderBaseViewModel:BaseViewModel
    {
        public bool PublicEntry { set; get; }

        [Display(Name = "Номер заявки")]
        public long Id { set; get; }

        [Required(ErrorMessage = "Введите дату заявки")]
        [Display(Name = "Дата заявки")]
        public string OrderDate { set; get; }
        public string OrderDateRaw { set; get; }

        [Display(Name = "Составитель заявки")]
        public string CreatedByUser { set; get; }

        [Display(Name = "Составитель заявки")]
        public string CreatedByUserName { set; get; }
        public System.DateTime CreateDatetime { set; get; }

        [Required(ErrorMessage = "Введите тип заявки")]
        [Display(Name = "Тип заявки")]
        public int OrderType { set; get; }

        public string OrderTypename { set; get; }
        public string OrderTypeShortName { set; get; }

        public bool AllowClientData { set; get; }
        public bool AllowExecuterData { set; get; }

        [Required(ErrorMessage = "Введите статус заявки")]
        [Display(Name = "Текущий сатус заявки")]
        public int CurrentOrderStatus { set; get; }

        public string CurrentOrderStatusColor { get; set; }
        public string CurrentStatusActionName { get; set; }

        public string CurrentOrderStatusName { get; set; }
        
        public string IconFile { get; set; }
        public string IconDescription { get; set; }

        [Display(Name = "Общее описание заявки")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля не больше 500 символов")]
        public string OrderDescription { set; get; }

        [Required(ErrorMessage = "Выберите информацию о заказчике")]
        [Display(Name = "ЦФО/Функциональное подразделение")]
        public long ClientId { set; get; }

        public string ClientName { set; get; }
        public string ClientCenterName { set; get; }

        public long ClientDogId { get; set; }
        public System.DateTime DogDateBeg { get; set; }
        public System.DateTime DogDateEnd { get; set; }
        
        [Required(ErrorMessage = "Введите информацию о сумме")]
        [Display(Name = "Сумма выполнения")]
        public decimal Summ { set; get; }

        [Display(Name = "Уведомлять при изменении")]
        public bool UseNotifications { set; get; }

        public string CreatorId{ set; get; }

        [Required(ErrorMessage = "Введите информацию о контактном лице")]
        [Display(Name = "Контактное лицо (Ф.И.О /должность)")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля не больше 255 символов")]
        public string CreatorPosition { set; get; }

        [Required(ErrorMessage = "Введите контактную информацию заказчика")]
        [Display(Name = "Контактный телефон по заявке", Prompt = "тел./e-mail/skype")]
        [StringLength(255, ErrorMessage = "Максимальная длина поля не больше 255 символов")]
        public string CreatorContact { set; get; }

        [Display(Name = "Тип приоритета заявки")]
        public int PriorityType { set; get; }

        [Display(Name = "Исполнитель заявки")]
        public string OrderExecuter { set; get; }

        [Display(Name = "Исполнитель заявки")]
        public string OrderExecuterName { set; get; }

        [Required(ErrorMessage = "Введите информацию о плательщике")]
        [Display(Name = "Плательщик")]
        public int PayerId { set; get; }

        [Display(Name = "Плательщик")]
        public string PayerName { set; get; }

        [Display(Name = "Проект")]
        public int ProjectId { set; get; }        

        [Display(Name = "Описание проекта")]
        [StringLength(500, ErrorMessage = "Максимальная длина поля не больше 500 символов")]
        public string ProjectDescription { set; get; }

        [Display(Name = "Необходимое количество автомобилей")]
        public int CarNumber { set; get; }

        [Display(Name = "Общее описание маршрута по заявке")]
        public string TotalDistanceDescription { set; get; }

        [Required(ErrorMessage = "Введите общую стоимость выполнения заявки")]
        //[Range(0.0, Double.MaxValue, ErrorMessage = "Введите корректное число")]
        [Display(Name = "Общая стоимость выполнения заявки")]
        public string TotalCost { set; get; }

        [Required(ErrorMessage = "Введите общее расстояние по заявке")]
        [Display(Name = "Общее расстояние по заявке")]
        public string TotalDistanceLenght { set; get; }

        [Display(Name = "Приватная заявка")]
        public bool IsPrivateOrder { set; get; }

        [Display(Name = "Заметки при выполнении")]
        public string ExecuterNotes { set; get; }

        public decimal Nds { set; get; }

        public IList<UserViewModel> AvaliableExecuters { set; get; }
        
        public IList<OrderStatusViewModel> nextAvialiableStatuses { get; set; }

        public IList<OrderStatusViewModel> previousStatuses { get; set; }

        public IList<OrderObserverViewModel> observers { get; set; }

        public IList<OrderAttachmentViewModel> attachments { get; set; }

        public string AcceptDate { get; set; }

        public string StartExecuteDate { get; set; }

        public System.DateTime OrderServiceDatetime { get; set; }

        public string StatusChangeComment { get; set; }        

        public string ObserversForNotification { get; set; }

        public string CurrentStatusShortName { get; set; }

        public string FontColor { get; set; }

        public string BackgroundColor { get; set; }

        public bool IsTransport { get; set; }

        public bool OpenedByCreator { get; set; }

        public bool OpenedByLPR { get; set; }

        public bool IsFinishOfTheProcess { get; set; }

        public string ReportStatusName { get; set; }
        public int ReportStatusId { get; set; }
        public string ReportColor { get; set; }

        public bool AllowData { get; set; }

        [Display(Name = "Тип услуги")]
        public int SpecificationTypeId { get; set; }

        [Display(Name = "Время нахождения на маршруте (дни:часы)")]
        [DisplayFormat(DataFormatString = "{0:dd\\:hh}", ApplyFormatInEditMode = true)]
        public string TimeRoute { get; set; }

        [Display(Name = "Время работы спецтранспорта (часы:минуты)")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public string TimeSpecialVehicles { get; set; }

        [Display(Name = "Тип услуги")]
        public string SpecificationType { get; set; }

        [Display(Name = "Проекты")]
        public string MultiProjectId { set; get; }

         [Display(Name = "Код проекта")]
        //[StringLength(50, ErrorMessage = "Максимальная длина поля не больше 50 символов")]
         public string ProjectNum { set; get; }

        [Display(Name = "Тип услуги")]
        public int TypeSpecId { get; set; }

        [Display(Name = "Дополнительные точки загрузки/выгрузки")]
        public bool IsAdditionalRoutePoints { get; set; }

        [Display(Name = "Маршрут")]
        public long RouteId { get; set; }

        public RouteViewModel RouteInfo { get; set; }

        public string RouteInfoStr { get; set; }

        public IList<OrderAdditionalRoutePointModel> RoutePointsLoadInfo { get; set; }

        public IList<OrderAdditionalRoutePointModel> RoutePointsUnloadInfo { get; set; }

        public IList<RoutePointsViewModel> RoutePointsInfo { get; set; }

        public IList<SpecificationTypesViewModel> SpecTypeInfo { set; get; }

        public IList<OrdersMapPointsViewModel> MapPoints { get; set; }

        public string[] handledItems { set; get; }

        public string CartUrl { set; get; }

        public bool IsLatLngAbsent { get; set; }

        public string GoogleMapApiKey { get; set; }

        public string ShortName { set; get; }
       
        public int ProjectsCnt  { set; get; }

        public OrderBaseViewModel()
        {
            CarNumber = 1;
            TotalCost = "0,00";
            TotalDistanceLenght = "0,00";
        }
    }

    
}
