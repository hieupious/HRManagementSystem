using System;

namespace HRMS.Web.Models
{
    public class JobTracking
    {
        public int Id { get; set; }
        public DateTime RunTime { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public string Reason { get; set; }
        public int JobType { get; set; }
    }
}
