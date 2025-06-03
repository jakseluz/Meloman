using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Meloman.Data;
using Meloman.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

namespace Meloman.Controllers
{
    public class ArtistController : Controller
    {
        private readonly MelomanContext _context;

        public ArtistController(MelomanContext context)
        {
            _context = context;
        }

        // GET: Artist
        public async Task<IActionResult> Index()
        {
            if (_context.Artist == null)
            {
                return Problem("Entity set 'MelomanContext.Artist'  is null.");
            }
            var artists = await _context.Artist.ToListAsync();
            return View(artists.Select(artist =>
            {
                artist.Mark = GetArtistMarkValue(artist);
                return artist;
            }));
        }

        // GET: Artist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .FirstOrDefaultAsync(m => m.Id == id);

            if (artist == null)
            {
                return NotFound();
            }

            artist.Mark = GetArtistMarkValue(artist);

            return View(artist);
        }

        // GET: Artist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,BirthDate,Mark")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artist);
                _context.Add(new ArtistMark { Id = 0, Artist = artist, User = null, Mark = artist.Mark });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            artist.Mark = GetArtistMarkValue(artist);
            return View(artist);
        }

        // POST: Artist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,BirthDate,Mark")] Artist artist)
        {
            if (id != artist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artist);
                    var mark = GetArtistMark(artist);
                    if (mark == null && artist.Mark != null)
                    {
                        _context.Add(new ArtistMark { Id = 0, Artist = artist, User = null, Mark = artist.Mark });
                    }
                    else if (mark != null)
                    {
                        mark.Mark = artist.Mark;
                        _context.Update(mark);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.Id))
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
            return View(artist);
        }

        // GET: Artist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }
            artist.Mark = GetArtistMarkValue(artist);

            return View(artist);
        }

        // POST: Artist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artist == null)
            {
                return Problem("Entity set 'MelomanContext.Artist'  is null.");
            }
            var artist = await _context.Artist.FindAsync(id);
            if (artist != null)
            {
                var mark = GetArtistMark(artist);
                if (mark != null)
                {
                    _context.ArtistMark.Remove(mark);
                }
                _context.Artist.Remove(artist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
            return (_context.Artist?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private ArtistMark? GetArtistMark(Artist artist)
        {
            if (_context.ArtistMark == null)
            {
                return null;
            }

            var res = from mark in _context.ArtistMark
                      where mark.Artist == artist
                      select mark;

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

        private double? GetArtistMarkValue(Artist artist)
        {
            var mark = GetArtistMark(artist);
            if (mark == null)
            {
                return ArtistMark.defaultMark;
            }

            return mark.Mark;
        }
    }
}
