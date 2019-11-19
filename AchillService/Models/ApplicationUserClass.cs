using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillService.Models
{
    public class ApplicationUserClass
    {
        public string ApplicationUserId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string ClassId { get; set; }

        [JsonIgnore]
        public virtual Class Class { get; set; }
    }
}
