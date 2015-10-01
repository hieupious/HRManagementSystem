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
        public int UserId { get; set; }
        public UserInfo UserInfo { get; set; }
        public List<DailyWorkingRecord> DailyRecords { get; set; }
        public double TotalLackTime { get; set; }
        public WorkingType Type { get; set; }
    }
}
