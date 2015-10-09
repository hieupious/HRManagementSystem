using System.Collections.Generic;
using Newtonsoft.Json;

namespace HRMS.Web.Models
{
    public class UserInfo
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string FingerPrintId { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public string WindowsAccount { get; set; }
        public Role Role { get; set; }
        public virtual UserInfo Manager { get; set; }
        public ICollection<UserInfo> Members { get; set; }
        public virtual Department Department { get; set; }
        public ICollection<DailyWorkingRecord> Approvals { get; set; }
        public ICollection<DailyWorkingRecord> DailyRecords { get; set; }
    }
}
