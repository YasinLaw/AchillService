﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AchillService.Data;
using AchillService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Validation;

namespace AchillService.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    [Route("api/course")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCourses()
        {
            return Ok(await _context.Courses.ToListAsync());
        }

        /// <summary>
        /// 通过课程Id获取课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCourse(string id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpGet("key/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCourseByPublicKey(int key)
        {
            var course = await _context.Courses.AsNoTracking()
                .FirstOrDefaultAsync(x => x.PublicKey == key);
            
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        /// <summary>
        /// 更新课程（未实现，禁用！）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutCourse(string id, Course course)
        {
            throw new NotImplementedException();
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        /// <summary>
        /// 添加课程
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PostCourse([FromBody] Course course)
        {
            var publicKey = new PublicKey { Type = PublicKeyType.Course };
            _context.PublicKeys.Add(publicKey);
            await _context.SaveChangesAsync();

            course.PublicKey = publicKey.Key;
            course.FacultyId = _userManager.GetUserId(User);
            course.FacultyName = User.Identity.Name;

            await _context.Courses.AddAsync(course);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseExists(course.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        /// <summary>
        /// 删除课程，并且级联删除选课记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var publicKey = await _context.PublicKeys.FirstOrDefaultAsync(x => x.Key == course.PublicKey);
            _context.PublicKeys.Remove(publicKey);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        private bool CourseExists(string id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
