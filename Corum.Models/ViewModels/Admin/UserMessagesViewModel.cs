using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Barnivann.Models;

namespace Corum.Models.ViewModels.Admin
{
     public class UserMessagesViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Текст сообщения")]
        public string MessageText { get; set; }

        [Display(Name = "Тема сообщения")]
        public string MessageSubject { get; set; }

        [Display(Name = "Отправитель")]
        public string CreatedFromUser { set; get; }

        [Display(Name = "Имя отправителя")]
        public string NameCreatedFromUser { set; get; }

        [Display(Name = "Получатель")]
        public string CreatedToUser { set; get; }

        [Display(Name = "Имя получателя")]
        public string NameCreatedToUser { set; get; }

        [Display(Name = "Дата и время")]
        public System.DateTime DateTimeCreate { set; get; }

        public System.DateTime? DateTimeOpen { set; get; }

        public bool IsOpened { set; get; }

        public bool IsMsgIn { get; set; }

        [Display(Name = "Номер заявки")]
        public long? OrderId { get; set; }

        public string MsgViewIdUser { set; get; }
        public string MsgViewNameUser { set; get; }
        public string MsgViewLabelUser { set; get; }
    }
}
