using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AchillService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Faculty")]
        [HttpGet("hello")]
        public string GetHello()
        {
            string str = "";
            foreach (var claim in User.Claims)
            {
                str += claim;
                str += " ";
            }
            return str + User.Identity.Name + "hello";
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