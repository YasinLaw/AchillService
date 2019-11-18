using Microsoft.AspNetCore.Identity;

namespace AchillService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Gender { get; set; }

        public UserType Type { get; set; }
    }

    public enum UserType
    {
        Student,
        Faculty,
        Administrator,
        Developer
    }
}
