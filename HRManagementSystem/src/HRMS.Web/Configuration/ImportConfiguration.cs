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
        public DateTime TimeToImport { get; set; }
        public string ApplicationBasePath { get; set; }
    }
}
