using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillService.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CourseId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string CourseName { get; set; }

        [ConcurrencyCheck]
        public int PublicKey { get; set; }

        [ConcurrencyCheck]
        public string PrivateKey { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string FacultyName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string FacultyId { get; set; }

        [ConcurrencyCheck]
        public string Topic { get; set; }

        [Required]
        [ConcurrencyCheck]
        public bool IsPublic { get; set; }
    }
}
