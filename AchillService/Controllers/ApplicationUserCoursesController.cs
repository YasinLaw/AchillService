﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AchillService.Data;
using AchillService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Validation;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AchillService.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    [Route("api/subscribe/course")]
    [ApiController]
    public class ApplicationUserCoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserCoursesController(ApplicationDbContext context,
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
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetApplicationUserCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            var records = await _context.ApplicationUserCourses
                .AsNoTracking()
                .Where(x => x.ApplicationUserId == user.Id)
                .Include(x=>x.Course)
                .ToListAsync();

            return Ok(records);
        }

        /// <summary>
        /// 通过课程Id获取当前用户订阅课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetApplicationUserCourse(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var record = await _context.ApplicationUserCourses.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id && x.CourseId == id);
            if (record == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == record.CourseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        /// <summary>
        /// 订阅课程
        /// </summary>
        /// <param name="courseSubscriber"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PostApplicationUserCourse([FromBody] CourseSubscriber courseSubscriber)
        {
            var user = await _userManager.GetUserAsync(User);

            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseSubscriber.CourseId);
            if (course.IsPublic == false)
            {
                if (courseSubscriber.PrivateKey != course.PrivateKey)
                {
                    return NotFound();
                }
            }

            var applicationUserCourse = new ApplicationUserCourse
            {
                ApplicationUserId = user.Id,
                CourseId = course.Id
            };

            await _context.ApplicationUserCourses.AddAsync(applicationUserCourse);
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

        /// <summary>
        /// 删除订阅课程记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteApplicationUserCourse(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var applicationUserCourse = await _context.ApplicationUserCourses.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id && x.CourseId == id);
            if (applicationUserCourse == null)
            {
                return NotFound();
            }
            _context.ApplicationUserCourses.Remove(applicationUserCourse);
            await _context.SaveChangesAsync();
            return Ok(applicationUserCourse);
        }

        private bool ApplicationUserCourseExists(string id)
        {
            return _context.ApplicationUserCourses.Any(e => e.ApplicationUserId == id);
        }
    }
}
