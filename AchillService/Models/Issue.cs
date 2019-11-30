using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AchillService.Models
{
    public class Issue : DbBase
    {
        [Required]
        [ConcurrencyCheck]
        public IssueType IssueType { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string ParentId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Title { get; set; }

        [ConcurrencyCheck]
        public string Description { get; set; }

        [ConcurrencyCheck]
        public string Tags { get; set; }

        [ConcurrencyCheck]
        public bool IsOpen { get; set; } = true;

        [ConcurrencyCheck]
        public DateTime IssueTime { get; set; } = DateTime.Now;
        
        [ConcurrencyCheck]
        public List<Comment> Comments { get; }
    }

    public enum IssueType
    {
        Course,
        Class
    }
}
