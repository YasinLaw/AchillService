using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AchillService.Data;
using AchillService.Models;
using Microsoft.AspNetCore.Authorization;

namespace AchillService.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Classes
        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            return Ok(await _context.Classes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClass(Guid id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            return Ok(@class);
        }

        // GET: api/Classes/publickey/5
        /// <summary>
        /// Get class by public key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        [Route("/publickey")]
        [HttpGet("{publicKey}")]
        public async Task<IActionResult> GetClassByPublicKey(int publicKey)
        {
            var @class = await _context.Classes
                .Where(x => x.PublicKey == publicKey)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }

        // PUT: api/Classes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(Guid id, [FromBody] Class @class)
        {
            if (id != @class.ClassId)
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

        // POST: api/Classes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<IActionResult> PostClass([FromBody] Class @class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Wait for a new generated public key
            var publicKey = new PublicKey { Type = PublicKeyType.Class };
            _context.PublicKeys.Add(publicKey);
            await _context.SaveChangesAsync();

            // Assign public key to class
            @class.PublicKey = publicKey.Key;

            await _context.Classes.AddAsync(@class);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetClassByPublicKey", new { id = @class.ClassId }, @class);
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Class>> DeleteClass(Guid id)
        {
            var @class = await _context.Classes.FindAsync(id);
            
            if (@class == null)
            {
                return NotFound();
            }
            // Delete publicKey for this class
            var key = await _context.PublicKeys
                .FirstOrDefaultAsync(x => x.Key == @class.PublicKey);

            _context.PublicKeys.Remove(key);
            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();

            return @class;
        }

        private bool ClassExists(Guid id)
        {
            return _context.Classes.Any(e => e.ClassId == id);
        }
    }
}
