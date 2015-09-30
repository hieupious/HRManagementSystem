using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public class DailyWorkingReport
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MonthlyReportId { get; set; }
        public DateTime WorkingDay { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public double MinuteLate { get; set; }
        public WorkingType WorkingType { get; set; }
        public UserInfo UserInfo { get; set; }
        public MonthlyReport MonthlyReport { get; set; }

    }
}
