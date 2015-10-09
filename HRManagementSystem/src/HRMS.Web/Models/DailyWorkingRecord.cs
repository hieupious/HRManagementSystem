using System;
using Newtonsoft.Json;

namespace HRMS.Web.Models
{
    public class DailyWorkingRecord
    {
        public int Id { get; set; }
        public int? UserInfoId { get; set; }
        public int? MonthlyRecordId { get; set; }
        public DateTime WorkingDay { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public double MinuteLate { get; set; }
        public WorkingType WorkingType { get; set; }
        public string GetApprovedReason { get; set; }
        public int? ApproverId { get; set; }
        public string ApproverComment { get; set; }
        public ApprovedStatus ApprovedStatus { get; set; }
        [JsonIgnore]
        public virtual UserInfo UserInfo { get; set; }
        public virtual UserInfo Approver { get; set; }
        [JsonIgnore]
        public virtual MonthlyRecord MonthlyRecord { get; set; }

    }
}
