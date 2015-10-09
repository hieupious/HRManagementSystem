using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public class MonthlyRecord
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int? UserInfoId { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public ICollection<DailyWorkingRecord> DailyRecords { get; set; }
        public double TotalLackTime { get; set; }        
    }
}
