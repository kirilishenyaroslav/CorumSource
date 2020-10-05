
namespace Corum.Models.ViewModels.Bucket
{
    public class BucketItem
    {
        public long Id { get; set; }
        public int OrderNum { get; set; }
        public string InnerPartyKey { get; set; }
        public string Storage { get; set; }
        public string Product { get; set; }
        public string Shifr { get; set; }
        public string Shifr_MDM { get; set; }
        public string BacodeConsignment { get; set; }
        public string BacodeProduct { get; set; }
        public string StorageCode { get; set; }
        public string Comments { get; set; }
        public string BalanceKeeper { get; set; }
        public decimal Weight { get; set; }
        public decimal TotalWeight { get; set; }
        public int Count { get; set; }
    }
}
