using System;

namespace HRMS.Web.Models
{
    public class DailyJobTracking
    {
        public int Id { get; set; }
        public DateTime RunningTime { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public string Reason { get; set; }
        public int JobType { get; set; }
    }
}
