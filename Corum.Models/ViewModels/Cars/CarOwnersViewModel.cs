using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Corum.Models.ViewModels.Orders;

namespace Corum.Models.ViewModels.Cars
{
    public class ManageCarOwnersAccessViewModel
    {
        public string JSONData { get; set; }
    }

    public class CarOwnersAccessViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Введите перевозчика")]
        [Display(Name = "Название перевозчика")]
        public string CarrierName { get; set; }

        [Display(Name = "ЕДРПОУ")]
        [Required(ErrorMessage = "Введите код ЕДРПОУ")]
        [RegularExpression("^[0-9]{8}$|^[0-9]{10}$", ErrorMessage ="Неверное количество цифр")]
        public long? edrpou_aps { get; set; }

        //[Required(ErrorMessage = "Введите адрес")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Введите телефон")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Введите имейл")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string email_aps { get; set; }

        [Required(ErrorMessage = "Введите имейл")]
        [Display(Name = "Email-2")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string email_aps2 { get; set; }

        [Required(ErrorMessage = "Введите имейл")]
        [Display(Name = "Email-3")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string email_aps3 { get; set; }

        //[Required(ErrorMessage = "Введите контактное лицо")]
        [Display(Name = "Контактное лицо")]
        public string ContactPerson { get; set; }
        [Display(Name = "Корень")]
        public int? parentId { get; set; }
        public int IdForScript { get; set; }

        public bool is_Leaf { get; set; }

        public bool CanBeDelete { get; set; }

        [Display(Name = "Группа перевозчиков")]
        public bool isRoot { get; set; }

        [Display(Name = "Экспедитор")]
        public bool IsForwarder { get; set; }

        public List<string> emails { get; set; }

        public List<CarOwnersAccessViewModel> AvailableCarOwners { get; set; }

        public List<long?> edrpouListAllContragents { get; set; }
        public List<string> emailsListAllContragents { get; set; }
    }


    public class GroupCarsViewModel
    {
        public CarOwnersAccessViewModel GroupCarsInfo { get; set; }
        public IEnumerable<CarsViewModel> Cars { get; set; }
    }

    public class CarsGroupViewModel
    {
        public CarOwnersAccessViewModel GroupCarsInfo { get; set; }
        public CarsViewModel Cars { get; set; }
    }

    public class CarsViewModel
    {
        public int CarId { get; set; }       
        [Display(Name = "Марка")]
        public string CarModel { get; set; }

        [Required(ErrorMessage = "Введите госномер")]
        [Display(Name = "Госномер")]
        public string Number { get; set; }

        [Display(Name = "ФИО водителя")]
        public string Driver { get; set; }

        [Display(Name = "Серия прав")]
        public string DriverLicenseSeria { get; set; }

        [Display(Name = "Номер прав")]
        public int? DriverLicenseNumber { get; set; }

        [Display(Name = "Вид топлива")]        
        public List<CarsFuelTypeViewModel> FuelTypeList { get; set; }

        [Display(Name = "Расход на 100 км в режиме город, л")]
        public int? ConsumptionCity { get; set; }

        [Display(Name = "Расход на 100 км в режиме за город, л")]
        public int? ConsumptionHighway { get; set; }

        [Display(Name = "Количество пассажиров")]
        public int? PassNumber { get; set; }

        public int? CarOwnersId { get; set; }

        public int? FuelTypeId { get; set; }

        [Display(Name = "Вид топлива")]   
        public string FuelType { get; set; }

    }

    public class CarsFuelTypeViewModel
    {
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
    }

     public class ContractsViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int? CarOwnersId { get; set; }
        public string CarOwnersName { get; set; }

        [Display(Name = "Плательщик")]
        public int? BalanceKeeperId { get; set; }
        public string BalanceKeeperName { get; set; }

        public int? ExpeditorId { get; set; }
        [Display(Name = "Экспедитор")]
        public string ExpeditorName { get; set; }

        [Display(Name = "Номер договора")]
         public string ContractNumber { get; set; }
         [Display(Name = "Дата договора")]
         public string ContractDate { get; set; }

         public string ContractDateRaw { get; set; }

         public string DateBegRaw { get; set; }

         public string DateEndRaw { get; set; }
         
         [Display(Name = "Дата начала действия договора")]
         public string DateBeg { get; set; }
         [Display(Name = "Дата окончания действия договора")]
         public string DateEnd { get; set; }

        public bool IsForwarder { get; set; }

        [Display(Name = "Активный")]
        public bool IsActive { get; set; }

        [Display(Name = "Отсрочка в днях")]
        public int DaysDelay { get; set; }

        public bool IsMainMenu { get; set; }

        public int CountGroupeSpecifications { get; set; }

        [Required(ErrorMessage = "Введите дату")]
        [Display(Name = "Дата реального получения договора")]
        public string ReceiveDateReal { get; set; }

        public string ReceiveDateRealRaw { get; set; }

        [Display(Name = "Условия пересмотра договора")]
        public string ContractRevision { get; set; }

        [Display(Name = "Плательщик")]
        public List<OrderClientBalanceKeeperViewModel> AvailableKeepers { get; set; }

        public CarOwnersAccessViewModel GroupCarsInfo { get; set; }

        [Display(Name = "Экспедитор")]
        public List<CarOwnersAccessViewModel> AvailableCarOwners { get; set; }

        [Required(ErrorMessage = "Введите НДС")]
        [Display(Name = "НДС")]
        public string NDSTax { get; set; }

        public bool IsExpired { get; set; }

        public bool IsExpiredSoon { get; set; }

        public string BackgroundColor { get; set; }
    }
}
