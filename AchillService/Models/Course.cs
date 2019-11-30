using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AchillService.Models
{
    public class Course : DbBase
    {
        [Required]
        [ConcurrencyCheck]
        public string CourseName { get; set; }

        [ConcurrencyCheck]
        public int PublicKey { get; set; }

        [ConcurrencyCheck]
        public string PrivateKey { get; set; }

        [ConcurrencyCheck]
        public string FacultyName { get; set; }

        [ConcurrencyCheck]
        public string FacultyId { get; set; }

        [ConcurrencyCheck]
        public string Topic { get; set; }

        [Required]
        [ConcurrencyCheck]
        public bool IsPublic { get; set; }

        [JsonIgnore]
        public virtual ICollection<ApplicationUserCourse> ApplicationUserCourses { get; set; }
    }
}
