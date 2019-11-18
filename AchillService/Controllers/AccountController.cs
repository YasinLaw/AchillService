using System.Threading.Tasks;
using AchillService.Data;
using AchillService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AchillService.Controllers
{
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext applicationDbContext;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.applicationDbContext = applicationDbContext;
        }
        /// <summary>
        /// Call /api/register
        /// </summary>
        /// <param name="email">string</param>
        /// <param name="username">string</param>
        /// <param name="password">string</param>
        /// <param name="gender">bool</param>
        /// <param name="type">int</param>
        /// <returns></returns>
        [HttpPost("~/api/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            if (await userManager.FindByEmailAsync(model.Email) != null
                && await userManager.FindByNameAsync(model.Username) != null)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Username,
                Gender = model.Gender,
                Type = model.Type
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                switch (user.Type)
                {
                    case UserType.Student:
                        if (!await roleManager.RoleExistsAsync("Student"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Student"));
                        }
                        await userManager.AddToRoleAsync(user, "Student");
                        break;
                    case UserType.Faculty:
                        if (!await roleManager.RoleExistsAsync("Faculty"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Faculty"));
                        }
                        await userManager.AddToRoleAsync(user, "Faculty");
                        break;
                    case UserType.Administrator:
                        if (!await roleManager.RoleExistsAsync("Administrator"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Administrator"));
                        }
                        await userManager.AddToRoleAsync(user, "Administrator");
                        break;
                    case UserType.Developer:
                        if (!await roleManager.RoleExistsAsync("Developer"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Developer"));
                        }
                        await userManager.AddToRoleAsync(user, "Developer");
                        break;
                    default:
                        break;
                }
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}