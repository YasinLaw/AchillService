using System;
using System.Threading.Tasks;
using AchillService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="model">用户注册模型</param>
        /// <response code="201">创建用户成功</response>
        /// <response code="400">模型验证失败</response>
        /// <response code="409">用户已注册</response>
        /// <returns>创建用户信息</returns>
        /// <exception cref="ArgumentNullException">注册模型为null</exception>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(System.Collections.Generic.IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null
                || await _userManager.FindByNameAsync(model.Username) != null)
            {
                return Conflict();
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
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns>当前用户信息</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        public async Task<IActionResult> GetApplicationUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(user);
        }
        
        /// <summary>
        /// 查找用户信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [Authorize(Roles = "Developer")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetApplicationUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(id);
            }
            return Ok(user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [Authorize(Roles = "Developer")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteApplicationUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userManager.DeleteAsync(user);

            return Ok();
        }
    }
}