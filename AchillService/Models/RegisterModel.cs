using System.ComponentModel.DataAnnotations;

namespace AchillService.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        public UserType Type { get; set; }

        [Required]
        public string RealName { get; set; }
    }
}
