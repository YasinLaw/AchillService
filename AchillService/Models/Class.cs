using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillService.Models
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ClassId { get; set; }

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
    }
}
