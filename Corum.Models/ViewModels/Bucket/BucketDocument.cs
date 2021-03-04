using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Corum.Models.ViewModels.Bucket
{
    public class BucketDocument
    {
        [Display(Name = "Номер документа")]
        public long Id { get; set; }
        [Display(Name = "Дата создания документа")]
        public System.DateTime Date { get; set; }
        public string Number { set; get; }
        [Display(Name = "Автор документа")]
        public string CreatedBy { get; set; }
        public IQueryable<BucketItem> Items { get; set; }
    }
}
