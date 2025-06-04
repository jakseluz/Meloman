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
    public class TrackApiController : ControllerBase
    {
        private readonly MelomanContext _context;

        public TrackApiController(MelomanContext context)
        {
            _context = context;
        }

        // GET: api/TrackApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> GetTrack()
        {
            if (_context.Track == null)
            {
                return NotFound();
            }
            return await _context.Track.ToListAsync();
        }

        // GET: api/TrackApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrack(int id)
        {
            if (_context.Track == null)
            {
                return NotFound();
            }
            var track = await _context.Track.FindAsync(id);

            if (track == null)
            {
                return NotFound();
            }

            return track;
        }

        // PUT: api/TrackApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrack(int id, Track track)
        {
            if (id != track.Id)
            {
                return BadRequest();
            }

            _context.Entry(track).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackExists(id))
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

        // POST: api/TrackApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Track>> PostTrack(Track track)
        {
            if (_context.Track == null)
            {
                return Problem("Entity set 'MelomanContext.Track'  is null.");
            }
            _context.Track.Add(track);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrack", new { id = track.Id }, track);
        }

        // DELETE: api/TrackApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            if (_context.Track == null)
            {
                return NotFound();
            }
            var track = await _context.Track.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            _context.Track.Remove(track);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrackExists(int id)
        {
            return (_context.Track?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
