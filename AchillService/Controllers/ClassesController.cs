using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AchillService.Data;
using AchillService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AchillService.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取所有班级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            return Ok(await _context.Classes.ToListAsync());
        }

        /// <summary>
        /// 通过班级Id获取班级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetClass(string id)
        {
            var @class = await _context.Classes.FindAsync(id);

            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }

        /// <summary>
        /// 通过PublicKey获取班级
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("key/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetClassByPublicKey(int key)
        {
            var @class = await _context.Classes.FirstOrDefaultAsync(x => x.PublicKey == key);
            if (@class == null)
            {
                return NotFound();
            }
            return Ok(@class);
        }

        /// <summary>
        /// 更新班级信息（未实现，禁用！）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="class"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutClass(string id, Class @class)
        {
            throw new NotImplementedException();
            if (id != @class.Id)
            {
                return BadRequest();
            }

            _context.Entry(@class).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        /// <summary>
        /// 添加班级
        /// </summary>
        /// <param name="class"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Class>> PostClass(Class @class)
        {
            var publicKey = new PublicKey { Type = PublicKeyType.Class };
            await _context.PublicKeys.AddAsync(publicKey);
            await _context.SaveChangesAsync();

            @class.PublicKey = publicKey.Key;

            await _context.Classes.AddAsync(@class);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClassExists(@class.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClass", new { id = @class.Id }, @class);
        }

        /// <summary>
        /// 删除班级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Class>> DeleteClass(string id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            var publicKey = await _context.PublicKeys.FirstOrDefaultAsync(x => x.Key == @class.PublicKey);
            _context.PublicKeys.Remove(publicKey);
            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();

            return @class;
        }

        private bool ClassExists(string id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
    }
}
