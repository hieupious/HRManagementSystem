﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Web.Models;

namespace HRMS.Web.Services
{
    public interface IDailyWorkingProcessService
    {
        void ProcessDailyWorkingReport(DateTime day);
    }
}
