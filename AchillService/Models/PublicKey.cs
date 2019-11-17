using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
