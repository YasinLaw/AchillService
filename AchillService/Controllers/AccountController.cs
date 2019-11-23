using System.Linq;
using System.Threading.Tasks;
using AchillService.Data;
using AchillService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation;

namespace AchillService.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    [Route("api/auth/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        /// <summary>
        /// Call /api/register
        /// </summary>
        /// <param name="email">string</param>
        /// <param name="username">string</param>
        /// <param name="password">string</param>
        /// <param name="gender">bool</param>
        /// <param name="type">int</param>
        /// <param name="realname">string</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(System.Collections.Generic.IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null
                || await _userManager.FindByNameAsync(model.Username) != null)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Username,
                Gender = model.Gender,
                Type = model.Type,
                RealName = model.RealName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                switch (user.Type)
                {
                    case UserType.Student:
                        if (!await _roleManager.RoleExistsAsync("Student"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Student"));
                        }
                        await _userManager.AddToRoleAsync(user, "Student");
                        break;
                    case UserType.Faculty:
                        if (!await _roleManager.RoleExistsAsync("Faculty"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Faculty"));
                        }
                        await _userManager.AddToRoleAsync(user, "Faculty");
                        break;
                    case UserType.Administrator:
                        if (!await _roleManager.RoleExistsAsync("Administrator"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Administrator"));
                        }
                        await _userManager.AddToRoleAsync(user, "Administrator");
                        break;
                    case UserType.Developer:
                        if (!await _roleManager.RoleExistsAsync("Developer"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Developer"));
                        }
                        await _userManager.AddToRoleAsync(user, "Developer");
                        break;
                    default:
                        break;
                }
                return CreatedAtAction("GetApplicationUser", user);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetApplicationUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(user);
        }
        
        [Authorize(Roles = "Developer")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(id);
            }
            return Ok(user);
        }

        // [Authorize(Roles = "Developer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var classes = await _context.ApplicationUsersClasses.Where(x => x.ApplicationUser == user).ToListAsync();
            var courses = await _context.ApplicationUserCourses.Where(x => x.ApplicationUser == user).ToListAsync();

            _context.ApplicationUsersClasses.RemoveRange(classes);
            _context.ApplicationUserCourses.RemoveRange(courses);

            await _context.SaveChangesAsync();
            await _userManager.DeleteAsync(user);

            return Ok();
        }
    }
}