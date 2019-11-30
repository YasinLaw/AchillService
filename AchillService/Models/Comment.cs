using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace AchillService.Models
{
    public class Comment : DbBase
    {
        [Required]
        [ConcurrencyCheck]
        public string IssueId { get; set; }

        [ConcurrencyCheck]
        public string Username { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Content { get; set; }

        [ConcurrencyCheck]
        public DateTime CommentTime { get; set; } = DateTime.Now;

        [JsonIgnore]
        [ConcurrencyCheck]
        public Issue Issue { get; set; }
    }
}
