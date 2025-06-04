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
            var allCategories = _context.CategoryMark.ToList();
            var allArtists = _context.ArtistMark.ToList();
            var scoredTracks = _context.Track.ToList().Select(t =>
            {
                var categoryScore = allCategories.Where(
                    mark => mark?.Category?.Id == t.CategoryId && mark?.UserId == currentUser.Id
                    ).FirstOrDefault()?.Mark;

                var artistScore = allArtists.Where(
                    mark => mark?.ArtistId == t.AuthorId && mark?.UserId == currentUser.Id
                    ).FirstOrDefault()?.Mark;

                double sum = 0;
                if (categoryScore != null)
                {
                    sum += (double)categoryScore;
                }
                if (artistScore != null)
                {
                    sum += (double)artistScore;
                }
                t.Mark = sum;
                return t;
            });
            var sorted = scoredTracks.OrderByDescending(o => o.Mark);

            return View(sorted);
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
            else
            {
                Console.WriteLine("category to null");

            }
            var author = GetTrackAuthor(track);
            if (author != null)
            {
                track.AuthorName = author.Name;
                track.AuthorSurname = author.Surname;
            }
            else
            {
                Console.WriteLine("author to null");

            }

            return View(track);
        }

        // GET: Track/Create
        public async Task<IActionResult> Create()
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
            PopulateAuthorsDropDownList();
            PopulateCategoriesDropDownList(currentUser.Id);
            return View();
        }

        // POST: Track/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreationDate")] Track track, IFormCollection form)
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

            string authorValue = form["AuthorId"].ToString();
            string categoryValue = form["CategoryId"].ToString();
            if (ModelState.IsValid)
            {
                track.AuthorId = null;
                track.CategoryId = null;
                if (authorValue != "-1")
                {
                    track.AuthorId = int.Parse(authorValue);
                }
                if (categoryValue != "-1")
                {
                    track.CategoryId = int.Parse(categoryValue);
                }
                _context.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(track);
        }

        // GET: Track/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var track = await _context.Track.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            PopulateAuthorsDropDownList(track.AuthorId);
            PopulateCategoriesDropDownList(currentUser.Id, track.CategoryId);
            return View(track);
        }

        // POST: Track/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreationDate")] Track track, IFormCollection form)
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

            if (id != track.Id)
            {
                return NotFound();
            }

            string authorValue = form["AuthorId"].ToString();
            string categoryValue = form["CategoryId"].ToString();
            if (ModelState.IsValid)
            {
                try
                {
                    track.AuthorId = null;
                    track.CategoryId = null;
                    if (authorValue != "-1")
                    {
                        track.AuthorId = int.Parse(authorValue);
                    }
                    if (categoryValue != "-1")
                    {
                        track.CategoryId = int.Parse(categoryValue);
                    }
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

            return View(track);
        }

        // POST: Track/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

        private void PopulateAuthorsDropDownList(object? selectedAuthor = null)
        {
            var chosenAuthors = from e in _context.Artist
                                orderby e.Surname
                                select new { Id = e.Id, Name = $"{e.Surname} {e.Name}" };
            var res = chosenAuthors.AsNoTracking();
            ViewBag.AuthorIds = new SelectList(res, "Id", "Name", selectedAuthor);
        }

        private void PopulateCategoriesDropDownList(int userId, object? selectedCategory = null)
        {
            var chosenCategories = from e in _context.Category
                                   where e.UserId == null || e.UserId == userId
                                   orderby e.Name
                                   select e;
            var res = chosenCategories.AsNoTracking();
            ViewBag.CategoryIds = new SelectList(res, "Id", "Name", selectedCategory);
        }
    }
}
