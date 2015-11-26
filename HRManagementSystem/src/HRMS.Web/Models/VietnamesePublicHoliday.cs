using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HRMS.Web.Models
{
    public class VietnamesePublicHoliday
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsFixed { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<PublicHoliday> PublicHolidays { get; set; }

    }
}
