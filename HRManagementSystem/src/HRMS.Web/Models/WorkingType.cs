using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public enum WorkingType
    {
        FullWorkingDay = 1,
        HalfDayMorning = 2,
        HalfDayAfternoon = 3,
        Absence = 4,
        LackCheckOut = 5,
        LackTime = 6
    }
}
