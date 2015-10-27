﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;

namespace HRMS.Web.Services
{
    public interface IDailyWorkingProcessService
    {
        DailyWorkingRecord ProcessDailyWorkingReport(int userId, DateTime day, ApplicationDbContext exDbContext = null);
    }
}
