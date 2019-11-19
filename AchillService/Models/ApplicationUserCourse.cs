using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillService.Models
{
    public class ApplicationUserCourse
    {
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}
