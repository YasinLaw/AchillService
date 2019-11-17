using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillService.Models
{
    public class ClassSubscriber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RecordId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string StudentId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public Guid ClassId { get; set; }
    }
}
