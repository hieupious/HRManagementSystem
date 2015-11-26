using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public class PublicHoliday
    {
        public int Id { get; set; }
        public int VietnamesePublicHolidayId { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public VietnamesePublicHoliday VNPublicHoliday { get; set; }
    }
}
