using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public class DailyWorkingRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MonthlyRecordId { get; set; }
        public DateTime WorkingDay { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public double MinuteLate { get; set; }
        public WorkingType WorkingType { get; set; }
        public UserInfo UserInfo { get; set; }
        public MonthlyRecord MonthlyRecord { get; set; }

    }
}
