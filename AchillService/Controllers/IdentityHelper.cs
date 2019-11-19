using System.Security.Claims;
using System.Security.Principal;

namespace AchillService.Controllers
{
    internal static class IdentityHelper
    {
        public static string GetApplicationUserId(IIdentity identity)
        {
            var claimIdentity = identity as ClaimsIdentity;
            return claimIdentity.FindFirst("sub").Value;
        }
    }
}
