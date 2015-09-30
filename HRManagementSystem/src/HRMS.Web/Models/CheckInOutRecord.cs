﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public class CheckInOutRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? WorkingReportId { get; set; }
        public DateTime CheckTime { get; set; }
        public UserInfo User { get; set; }
        public DailyWorkingReport WorkingReport { get; set; }
    }
}
