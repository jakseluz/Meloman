using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Meloman.Data;
using Meloman.Models;
using Meloman.Filters;

namespace Meloman.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuthorize("user", "admin")]
    public class CategoryMarkApiController : ControllerBase
    {
        private readonly MelomanContext _context;

        public CategoryMarkApiController(MelomanContext context)
        {
            _context = context;
        }

        // GET: api/CategoryMarkApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryMark>>> GetCategoryMark()
        {
            if (_context.CategoryMark == null)
            {
                return NotFound();
            }
            return await _context.CategoryMark.ToListAsync();
        }

        // GET: api/CategoryMarkApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryMark>> GetCategoryMark(int id)
        {
            if (_context.CategoryMark == null)
            {
                return NotFound();
            }
            var categoryMark = await _context.CategoryMark.FindAsync(id);

            if (categoryMark == null)
            {
                return NotFound();
            }

            return categoryMark;
        }

        // PUT: api/CategoryMarkApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryMark(int id, CategoryMark categoryMark)
        {
            if (id != categoryMark.Id)
            {
                return BadRequest();
            }

            _context.Entry(categoryMark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryMarkExists(id))
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

        // POST: api/CategoryMarkApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryMark>> PostCategoryMark(CategoryMark categoryMark)
        {
            if (_context.CategoryMark == null)
            {
                return Problem("Entity set 'MelomanContext.CategoryMark'  is null.");
            }
            _context.CategoryMark.Add(categoryMark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryMark", new { id = categoryMark.Id }, categoryMark);
        }

        // DELETE: api/CategoryMarkApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryMark(int id)
        {
            if (_context.CategoryMark == null)
            {
                return NotFound();
            }
            var categoryMark = await _context.CategoryMark.FindAsync(id);
            if (categoryMark == null)
            {
                return NotFound();
            }

            _context.CategoryMark.Remove(categoryMark);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryMarkExists(int id)
        {
            return (_context.CategoryMark?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
