using Newtonsoft.Json;

namespace AchillService.Models
{
    public class ApplicationUserCourse
    {
        public string ApplicationUserId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string CourseId { get; set; }

        [JsonIgnore]
        public virtual Course Course { get; set; }
    }
}
