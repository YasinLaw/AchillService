using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillService.Models
{
    public class CourseSubscriber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RecordId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string UserId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public Guid CourseId { get; set; }
    }
}
