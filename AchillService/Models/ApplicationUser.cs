using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AchillService.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public bool Gender { get; set; }

        [Required]
        public UserType Type { get; set; }

        [Required]
        public string RealName { get; set; }

        [JsonIgnore]
        public virtual ICollection<ApplicationUserClass> ApplicationUserClasses { get; set; }

        [JsonIgnore]
        public virtual ICollection<ApplicationUserCourse> ApplicationUserCourses { get; set; }
    }

    public enum UserType
    {
        Student,
        Faculty,
        Administrator,
        Developer
    }
}
