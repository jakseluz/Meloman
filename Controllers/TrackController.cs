using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Meloman.Data;
using Meloman.Models;

namespace Meloman.Controllers
{
    public class TrackController : Controller
    {
        private readonly MvcTrackContext _context;

        public TrackController(MvcTrackContext context)
        {
            _context = context;
        }

        // GET: Track
        public async Task<IActionResult> Index()
        {
              return _context.Track != null ? 
                          View(await _context.Track.ToListAsync()) :
                          Problem("Entity set 'MvcTrackContext.Track'  is null.");
        }

        // GET: Track/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Track == null)
            {
                return NotFound();
            }

            var track = await _context.Track
                .FirstOrDefaultAsync(m => m.Id == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // GET: Track/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Track/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Track track)
        {
            if (ModelState.IsValid)
            {
                _context.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(track);
        }

        // GET: Track/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Track == null)
            {
                return NotFound();
            }

            var track = await _context.Track.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }
            return View(track);
        }

        // POST: Track/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Track track)
        {
            if (id != track.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(track);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackExists(track.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(track);
        }

        // GET: Track/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Track == null)
            {
                return NotFound();
            }

            var track = await _context.Track
                .FirstOrDefaultAsync(m => m.Id == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // POST: Track/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Track == null)
            {
                return Problem("Entity set 'MvcTrackContext.Track'  is null.");
            }
            var track = await _context.Track.FindAsync(id);
            if (track != null)
            {
                _context.Track.Remove(track);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackExists(int id)
        {
          return (_context.Track?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
