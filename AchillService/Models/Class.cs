using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillService.Models
{
    public class Class : DbBase
    {
        [Required]
        [ConcurrencyCheck]
        public string ClassName { get; set; }

        [ConcurrencyCheck]
        public int PublicKey { get; set; }

        [MaxLength(20)]
        [MinLength(6)]
        [Required]
        [ConcurrencyCheck]
        public string PrivateKey { get; set; }

        [JsonIgnore]
        public virtual ICollection<ApplicationUserClass> ApplicationUserClasses { get; set; }

        [JsonIgnore]
        public virtual ICollection<ClassCourse> ClassCourses { get; set; }
    }
}
