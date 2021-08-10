using System;


namespace Corum.Models.ViewModels.OrderConcurs
{

    public class CompetitiveListStepViewModel : BaseViewModel
    {
        public long Id { set; get; }

        public string StepShortName { set; get; }

        public string StepFullName { set; get; }

    }


    public class CompetetiveListStepsInfoViewModel: BaseViewModel
    {
        public long Id { set; get; }

        public int StepId { set; get; }
        
        public string StepShortCode { set; get; }
        
        public string StepFullCode { set; get; }

        public int? PreviousStepId { set; get; }

        public string PreviousStepFullCode { set; get; }

        public long OrderId { set; get; }

        public string userId { set; get; }
        
        public string userName { set; get; }
        
        public DateTime? timestamp { set;  get;} 

        public string timestampRaw { set;  get;} 
        public Nullable<long> tenderNumber { get; set; }

    }


}
