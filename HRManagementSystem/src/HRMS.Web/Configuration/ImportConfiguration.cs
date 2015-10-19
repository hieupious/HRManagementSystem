using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Configuration
{
    public class ImportConfiguration
    {
        public string ImportedDBPath { get; set; }
        public string FileToCopyPath { get; set; }
        public TimeSpan TimeToImportAtNight { get; set; }
        public TimeSpan TimeToImportAtNoon { get; set; }
        public string ApplicationBasePath { get; set; }
    }
}
