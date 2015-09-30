using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public class MonthlyReport
    {
        public int Id { get; set; }
        public int User { get; set; }
        public UserInfo UserInfo { get; set; }
        public List<DailyWorkingReport> DailyRecord { get; set; }
        public double TotalLackTime { get; set; }
    }
}
