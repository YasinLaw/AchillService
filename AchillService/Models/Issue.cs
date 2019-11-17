using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillService.Models
{
    public class Issue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid IssueId { get; set; }

        [ConcurrencyCheck]
        public Guid ClassId { get; set; }

        [ConcurrencyCheck]
        public Guid CourseId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Description { get; set; }

        [ConcurrencyCheck]
        public bool IsOpen { get; set; } = true;
    }
}
