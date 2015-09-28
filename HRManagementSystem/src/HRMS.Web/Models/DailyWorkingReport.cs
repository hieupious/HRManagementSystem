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
        public DateTime WorkingDay { get; set; }
        public DateTime ClockIn { get; set; }
        public DateTime ClockOut { get; set; }
        public string MinuteLate { get; set; }
        public WorkingType WorkingType { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}
