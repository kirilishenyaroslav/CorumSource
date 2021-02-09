using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Corum.Models.ViewModels.Orders
{
    public class ProjectTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class OrderProjectViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Максимальная длина поля не больше 50 символов")]
        [Required(ErrorMessage = "Введите код проекта")]
        [Display(Name = "Код проекта")]
        public string Code { get; set; }

        [StringLength(500, ErrorMessage = "Максимальная длина поля не больше 500 символов")]
        [Required(ErrorMessage = "Введите наименование проекта")]
        [Display(Name = "Наименование проекта")]
        public string Description { get; set; }

        [StringLength(5000, ErrorMessage = "Максимальная длина поля не больше 5000 символов")]
        [Display(Name = "Комментарии")]
        public string Comments { get; set; }

        [Required(ErrorMessage = "Введите тип проекта")]
        [Display(Name = "Тип проекта")]
        public int ProjectTypeId { get; set; }

        public string ProjectTypeName { get; set; }

        [Required(ErrorMessage = "Введите ЦФО проекта")]
        [Display(Name = "ЦФО проекта")]
        public int ProjectCFOId { get; set; }

        public string ProjectCFOName { get; set; }

        [Required(ErrorMessage = "Введите заказчика проекта")]
        [Display(Name = "Заказчик проекта")]
        public string ProjectOrderer { get; set; }

        [Required(ErrorMessage = "Введите обозначение конструкции")]
        [Display(Name = "Обозначение конструкции")]
        public string ConstructionDesc { get; set; }

        [Display(Name = "План производства")]
        public int PlanCount { get; set; }

        [Display(Name = "Доступен для выбора в заявках")]
        public bool isActive { get; set; }

        [Required(ErrorMessage = "Введите название производственного предприятия")]
        [Display(Name = "Производственное предприятие ")]
        public string ManufacturingEnterprise { get; set; }

        //[Required(ErrorMessage = "Введите номер заказа в производство")]
        [Display(Name = "Номер заказа в производство")]
        public int NumOrder { get; set; }

        //[Required(ErrorMessage = "Введите дату открытия заказа в производство")]
        [Display(Name = "Дата открытия заказа в производство")]
        public string DateOpenOrder { get; set; }
        public string DateOpenOrderRaw { get; set; }

        [Display(Name = "Плановый срок обеспечения м.п.")]
        public string PlanPeriodForMP { get; set; }
        public string PlanPeriodForMPRaw { get; set; }

        [Display(Name = "Плановый срок обеспечения комплектующими")]
        public string PlanPeriodForComponents { get; set; }
        public string PlanPeriodForComponentsRaw { get; set; }

        [Display(Name = "План.срок на СГИ")]
        public string PlanPeriodForSGI { get; set; }
        public string PlanPeriodForSGIRaw { get; set; }

        [Display(Name = "План.срок подачи транспорта")]
        public string PlanPeriodForTransportation { get; set; }
        public string PlanPeriodForTransportationRaw { get; set; }

        [Display(Name = "Плановая дата доставки Грузополучателю")]
        public string PlanDeliveryToConsignee { get; set; }
        public string PlanDeliveryToConsigneeRaw { get; set; }

        [Display(Name = "Базис поставки правила ИНКОТРЕРМС")]
        public string DeliveryBasic { get; set; }

       // [Display(Name = "Базис поставки правила ИНКОТРЕРМС")]
        //public string DeliveryBasicName { get; set; }

        [Display(Name = "Грузоотправитель")]
        public long Shipper { get; set; }

        [Display(Name = "Грузоотправитель")]
        public string ShipperName { get; set; }

        [Display(Name = "Грузополучатель")]
        public long Consignee { get; set; }

        [Display(Name = "Грузополучатель")]
        public string ConsigneeName { get; set; }

        public bool CanShowManufacture { get; set; }

        public List<OrderClientCFOViewModel> AvailableCFOs { get; set; }
        public List<ProjectTypeViewModel> AvailableProjectTypes { get; set; }
    }
}
