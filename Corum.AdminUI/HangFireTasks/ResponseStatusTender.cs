using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorumAdminUI.HangFireTasks
{
    public class ResponseStatusTender
    {
        public bool success { get; set; }
        public Data data { get; set; }
    }
    public class Member
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
    }

    public class Data
    {
        public string tenderNumber { get; set; }
        public string tenderUuid { get; set; }
        public int apprPatternId { get; set; }
        public string apprPatternName { get; set; }
        public int stageNumber { get; set; }
        public int process { get; set; }
        public string tenderOwnerPath { get; set; }
        public List<Member> members { get; set; }
    }
}