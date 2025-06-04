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
    public class ArtistMarkApiController : ControllerBase
    {
        private readonly MelomanContext _context;

        public ArtistMarkApiController(MelomanContext context)
        {
            _context = context;
        }

        // GET: api/ArtistMarkApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistMark>>> GetArtistMark()
        {
            if (_context.ArtistMark == null)
            {
                return NotFound();
            }
            return await _context.ArtistMark.ToListAsync();
        }

        // GET: api/ArtistMarkApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistMark>> GetArtistMark(int id)
        {
            if (_context.ArtistMark == null)
            {
                return NotFound();
            }
            var artistMark = await _context.ArtistMark.FindAsync(id);

            if (artistMark == null)
            {
                return NotFound();
            }

            return artistMark;
        }

        // PUT: api/ArtistMarkApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtistMark(int id, ArtistMark artistMark)
        {
            if (id != artistMark.Id)
            {
                return BadRequest();
            }

            _context.Entry(artistMark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistMarkExists(id))
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

        // POST: api/ArtistMarkApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArtistMark>> PostArtistMark(ArtistMark artistMark)
        {
            if (_context.ArtistMark == null)
            {
                return Problem("Entity set 'MelomanContext.ArtistMark'  is null.");
            }
            _context.ArtistMark.Add(artistMark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtistMark", new { id = artistMark.Id }, artistMark);
        }

        // DELETE: api/ArtistMarkApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtistMark(int id)
        {
            if (_context.ArtistMark == null)
            {
                return NotFound();
            }
            var artistMark = await _context.ArtistMark.FindAsync(id);
            if (artistMark == null)
            {
                return NotFound();
            }

            _context.ArtistMark.Remove(artistMark);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtistMarkExists(int id)
        {
            return (_context.ArtistMark?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
