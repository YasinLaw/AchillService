using Microsoft.AspNetCore.Identity;

namespace AchillService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Gender { get; set; }

        public int Type { get; set; }
    }
}
