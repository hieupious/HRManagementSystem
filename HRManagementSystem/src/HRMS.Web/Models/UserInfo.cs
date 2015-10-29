using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace HRMS.Web.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int ExternalId { get; set; }
        public string EmployeeId { get; set; }
        [JsonIgnore]
        public string FingerPrintId { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string EnglishName { get; set; }
        public string WindowsAccount { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Office Office { get; set; }
        public int? WorkingPoliciesGroupId { get; set; }
        public DateTime? StartWorkingDay { get; set; }
        public virtual Department Department { get; set; }
        [JsonIgnore]
        public virtual WorkingPoliciesGroup WorkingPoliciesGroup { get; set; }
        [JsonIgnore]
        public ICollection<DailyWorkingRecord> Approvals { get; set; }
        [JsonIgnore]
        public ICollection<DailyWorkingRecord> DailyRecords { get; set; }
        [JsonIgnore]
        public ICollection<CheckInOutRecord> CheckInOutRecords { get; set; }
    }
}
