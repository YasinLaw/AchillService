using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillService.Models
{
    public class ClassCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public Guid ClassId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public Guid CourseId { get; set; }
    }
}
