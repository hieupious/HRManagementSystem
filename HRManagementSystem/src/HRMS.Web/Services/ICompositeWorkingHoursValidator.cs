using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Services
{
    interface ICompositeWorkingHoursValidator : IWorkingHoursValidator
    {
        IReadOnlyCollection<IWorkingHoursValidator> Validators { get; }
    }
}
