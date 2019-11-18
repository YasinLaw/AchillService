using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillService.Models
{
    public class PublicKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid KeyId { get; set; }

        public PublicKeyType Type { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Key { get; set; }
    }
    
    public enum PublicKeyType
    {
        Class,
        Course
    }
}
