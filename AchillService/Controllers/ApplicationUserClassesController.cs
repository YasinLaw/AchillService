using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AchillService.Data;
using AchillService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AchillService.Controllers
{
    [Route("api/subscribe/class")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class ApplicationUserClassesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserClassesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取当前用户订阅的所有课程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetApplicationUsersClasses()
        {
            var user = await _userManager.GetUserAsync(User);
            var classes = await _context.ApplicationUsersClasses.AsNoTracking()
                .Where(x => x.ApplicationUserId == user.Id)
                .Include(x =>x.Class)
                .ToListAsync();
            return Ok(classes);
        }

        /// <summary>
        /// 通过班级Id获取当前用户订阅班级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetApplicationUserClass(string id)
        {
            var user = await _userManager.GetUserAsync(User);

            var record = await _context.ApplicationUsersClasses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id && x.ClassId == id);

            if (record == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FirstOrDefaultAsync(x => x.Id == record.ClassId);
            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }

        /// <summary>
        /// 订阅班级
        /// </summary>
        /// <param name="classSubscriber"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PostApplicationUserClass(ClassSubscriber classSubscriber)
        {
            var user = await _userManager.GetUserAsync(User);
            var @class = await _context.Courses.FirstOrDefaultAsync(x => x.Id == classSubscriber.ClassId);
            if (@class == null)
            {
                return NotFound();
            }
            if (@class.IsPublic == false)
            {
                if (@class.PrivateKey != classSubscriber.PrivateKey)
                {
                    return NotFound();
                }
            }

            var applicationUserClass = new ApplicationUserClass
            {
                ApplicationUserId = user.Id,
                ClassId = classSubscriber.ClassId
            };

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationUserClassExists(applicationUserClass.ApplicationUserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApplicationUserClass", new { id = applicationUserClass.ApplicationUserId }, applicationUserClass);
        }

        /// <summary>
        /// 删除订阅班级记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApplicationUserClass>> DeleteApplicationUserClass(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var applicationUserClass = await _context.ApplicationUsersClasses.FirstOrDefaultAsync(x => x.ClassId == id && x.ApplicationUserId == user.Id);
            if (applicationUserClass == null)
            {
                return NotFound();
            }
            _context.ApplicationUsersClasses.Remove(applicationUserClass);
            await _context.SaveChangesAsync();
            return Ok(applicationUserClass);
        }

        private bool ApplicationUserClassExists(string id)
        {
            return _context.ApplicationUsersClasses.Any(e => e.ApplicationUserId == id);
        }
    }
}
