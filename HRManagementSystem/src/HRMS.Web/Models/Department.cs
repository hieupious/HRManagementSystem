using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HRMS.Web.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Office { get; set; }
        [JsonIgnore]
        public List<UserInfo> UserInfoes { get; set; }
    }
}
