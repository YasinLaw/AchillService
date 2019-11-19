using Newtonsoft.Json;
using System;

namespace AchillService.Models
{
    public class ClassCourse
    {
        public string ClassId { get; set; }

        [JsonIgnore]
        public virtual Class Class { get; set; }

        public string CourseId { get; set; }

        [JsonIgnore]
        public virtual Course Course { get; set; }
    }
}