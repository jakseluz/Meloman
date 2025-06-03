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
    public class ArtistMarkController : Controller
    {
        private readonly MelomanContext _context;

        public ArtistMarkController(MelomanContext context)
        {
            _context = context;
        }

        // GET: ArtistMark
        [ServiceFilter(typeof(VerifiedUserAllowed))]
        public async Task<IActionResult> Index()
        {
            return _context.ArtistMark != null ?
                        View(await _context.ArtistMark.ToListAsync()) :
                        Problem("Entity set 'MelomanContext.ArtistMark'  is null.");
        }

        // GET: ArtistMark/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ArtistMark == null)
            {
                return NotFound();
            }

            var artistMark = await _context.ArtistMark
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistMark == null)
            {
                return NotFound();
            }

            return View(artistMark);
        }

        // GET: ArtistMark/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArtistMark/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mark")] ArtistMark artistMark)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistMark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artistMark);
        }

        // GET: ArtistMark/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ArtistMark == null)
            {
                return NotFound();
            }

            var artistMark = await _context.ArtistMark.FindAsync(id);
            if (artistMark == null)
            {
                return NotFound();
            }
            return View(artistMark);
        }

        // POST: ArtistMark/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mark")] ArtistMark artistMark)
        {
            if (id != artistMark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artistMark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistMarkExists(artistMark.Id))
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
            return View(artistMark);
        }

        // GET: ArtistMark/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ArtistMark == null)
            {
                return NotFound();
            }

            var artistMark = await _context.ArtistMark
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistMark == null)
            {
                return NotFound();
            }

            return View(artistMark);
        }

        // POST: ArtistMark/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ArtistMark == null)
            {
                return Problem("Entity set 'MelomanContext.ArtistMark'  is null.");
            }
            var artistMark = await _context.ArtistMark.FindAsync(id);
            if (artistMark != null)
            {
                _context.ArtistMark.Remove(artistMark);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistMarkExists(int id)
        {
          return (_context.ArtistMark?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
