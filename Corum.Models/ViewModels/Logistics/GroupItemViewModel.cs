namespace Corum.Models.ViewModels
{
    public class GroupItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class GroupItemFilters
    {
        public string FilterStorageId { get; set; }
        public string FilterCenterId { get; set; }
        public string FilterRecieverPlanId { get; set; }
        public string FilterRecieverFactId { get; set; }
        public string FilterKeeperId { get; set; }
        public string FilterProducerId { get; set; }
        public string FilterOrderProjectId { get; set; }

        public bool UseStorageFilter { get; set; }
        public bool UseCenterFilter { get; set; }
        public bool UseRecieverPlanFilter { get; set; }
        public bool UseRecieverFactFilter { get; set; }
        public bool UseKeeperFilter { get; set; }
        public bool UseProducerFilter { get; set; }
        public bool UseOrderProjectFilter { get; set; }

        public int IsPrihodDocs { get; set; }

    }
}
