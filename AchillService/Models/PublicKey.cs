using System.ComponentModel.DataAnnotations.Schema;

namespace AchillService.Models
{
    public class PublicKey : DbBase
    {
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
