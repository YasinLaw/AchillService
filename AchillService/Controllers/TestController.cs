using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AchillService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("hello")]
        public string GetHello()
        {
            return IdentityHelper.GetApplicationUserId(User.Identity);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Developer")]
        [HttpGet("world")]
        public string GetWorld()
        {
            string str = "";
            foreach (var claim in User.Claims)
            {
                str += claim;
                str += " ";
            }
            return str + User.Identity.Name + "world";
        }
    }
}