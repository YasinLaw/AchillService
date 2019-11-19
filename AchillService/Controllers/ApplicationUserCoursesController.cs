using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AchillService.Data;
using AchillService.Models;
using Microsoft.AspNetCore.Authorization;

namespace AchillService.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/subscribe/course")]
    [ApiController]
    public class ApplicationUserCoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserCoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/subscribe/course
        [HttpGet]
        public async Task<IActionResult> GetApplicationUserCourses()
        {
            var userId = IdentityHelper.GetApplicationUserId(User.Identity);
            var courses = await _context.ApplicationUserCourses
                .AsNoTracking()
                .Where(x => x.ApplicationUserId == userId)
                .Include(x => x.Course)
                .ToListAsync();
            return Ok(courses);
        }

        // GET: api/ApplicationUserCourses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserCourse>> GetApplicationUserCourse(string id)
        {
            var applicationUserCourse = await _context.ApplicationUserCourses.FindAsync(id);

            if (applicationUserCourse == null)
            {
                return NotFound();
            }

            return applicationUserCourse;
        }

        // PUT: api/ApplicationUserCourses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUserCourse(string id, ApplicationUserCourse applicationUserCourse)
        {
            if (id != applicationUserCourse.ApplicationUserId)
            {
                return BadRequest();
            }

            _context.Entry(applicationUserCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserCourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApplicationUserCourses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ApplicationUserCourse>> PostApplicationUserCourse(ApplicationUserCourse applicationUserCourse)
        {
            applicationUserCourse.ApplicationUserId = IdentityHelper.GetApplicationUserId(User.Identity);
            _context.ApplicationUserCourses.Add(applicationUserCourse);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationUserCourseExists(applicationUserCourse.ApplicationUserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApplicationUserCourse", new { id = applicationUserCourse.ApplicationUserId }, applicationUserCourse);
        }

        // DELETE: api/ApplicationUserCourses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationUserCourse>> DeleteApplicationUserCourse(string id)
        {
            var applicationUserCourse = await _context.ApplicationUserCourses.FindAsync(id);
            if (applicationUserCourse == null)
            {
                return NotFound();
            }

            _context.ApplicationUserCourses.Remove(applicationUserCourse);
            await _context.SaveChangesAsync();

            return applicationUserCourse;
        }

        private bool ApplicationUserCourseExists(string id)
        {
            return _context.ApplicationUserCourses.Any(e => e.ApplicationUserId == id);
        }
    }
}
