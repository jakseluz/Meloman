using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Meloman.Data;
using Meloman.Models;
using Meloman.Filters;

namespace Meloman.Controllers
{
    public class TrackController : Controller
    {
        private readonly MelomanContext _context;

        public TrackController(MelomanContext context)
        {
            _context = context;
        }

        // GET: Track
        [ServiceFilter(typeof(VerifiedUserAllowed))]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(
                u => u.Id == HttpContext.Session.GetInt32(AccountController.UserIdKey)
            );
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (currentUser.Role == null)
            {
                return StatusCode(403, "Access Denied.");
            }

            if (_context.Track == null)
            {
                return Problem("Entity set 'MelomanContext.Track'  is null.");
            }
            var tracks = await _context.Track.ToListAsync();
            return View(tracks);
        }

        // GET: Track/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(
                u => u.Id == HttpContext.Session.GetInt32(AccountController.UserIdKey)
            );
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (currentUser.Role == null)
            {
                return StatusCode(403, "Access Denied.");
            }

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

            var category = GetTrackCategory(currentUser.Id, track);
            if (category != null)
            {
                track.Category = category.Name;
            }
            var author = GetTrackAuthor(track);
            if (author != null)
            {
                track.AuthorName = author.Name;
                track.AuthorSurname = author.Surname;
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
        public async Task<IActionResult> Create([Bind("Id,Title,CreationDate")] Track track)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreationDate")] Track track)
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
                return Problem("Entity set 'MelomanContext.Track'  is null.");
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

        private Artist? GetTrackAuthor(Track track)
        {
            if (_context.Artist == null)
            {
                return null;
            }

            var res = from author in _context.Artist
                      where author.Id == track.AuthorId
                      select author;

            if (res == null)
            {
                return null;
            }

            var enumRes = res.AsEnumerable();

            if (!enumRes.Any())
            {
                return null;
            }

            return enumRes.AsEnumerable().ElementAt(0);

        }

        private Category? GetTrackCategory(int userId, Track track)
        {
            if (_context.Category == null)
            {
                return null;
            }

            var res = from category in _context.Category
                      where category.UserId == userId
                      where category.Id == track.CategoryId
                      select category;

            if (res == null)
            {
                return null;
            }

            var enumRes = res.AsEnumerable();

            if (!enumRes.Any())
            {
                return null;
            }

            return enumRes.AsEnumerable().ElementAt(0);

        }
    }
}
