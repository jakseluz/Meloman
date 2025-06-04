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
    public class CategoryController : Controller
    {
        private readonly MelomanContext _context;

        public CategoryController(MelomanContext context)
        {
            _context = context;
        }

        // GET: Category
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
            if (_context.Artist == null)
            {
                return StatusCode(403, "Access Denied.");
            }

            if (_context.Category == null)
            {
                return Problem("Entity set 'MelomanContext.Artist'  is null.");
            }

            int? id = HttpContext.Session.GetInt32(AccountController.UserIdKey);
            var categories = (await _context.Category.ToListAsync()).Where(cat =>
            {
                return cat.UserId == null || cat.UserId == id;
            });
            return View(categories.Select(category =>
            {
                category.Mark = GetCategoryMarkValue(category);
                return category;
            }));
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(
                u => u.Id == HttpContext.Session.GetInt32(AccountController.UserIdKey)
            );
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (_context.Artist == null)
            {
                return StatusCode(403, "Access Denied.");
            }

            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            if (category.UserId != null && category.UserId != currentUser.Id)
            {
                return StatusCode(403, "Access Denied.");

            }
            category.Mark = GetCategoryMarkValue(category);

            return View(category);
        }

        // GET: Category/Create
        public async Task<IActionResult> Create()
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(
                u => u.Id == HttpContext.Session.GetInt32(AccountController.UserIdKey)
            );
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (_context.Artist == null)
            {
                return StatusCode(403, "Access Denied.");
            }
            ViewBag.Role = currentUser.Role;
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Mark,IsGenre")] Category category)
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(
                u => u.Id == HttpContext.Session.GetInt32(AccountController.UserIdKey)
            );
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (_context.Artist == null)
            {
                return StatusCode(403, "Access Denied.");
            }

            if (ModelState.IsValid)
            {
                Console.WriteLine($"gatunek: {category.IsGenre}");
                category.UserId = category.IsGenre ? null : currentUser.Id;
                _context.Add(category);
                await _context.SaveChangesAsync();
                if (category.Mark != null)
                {
                    _context.Add(new CategoryMark
                    {
                        CategoryId = category.Id,
                        UserId = currentUser.Id,
                        Mark = category.Mark
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(
                u => u.Id == HttpContext.Session.GetInt32(AccountController.UserIdKey)
            );
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (_context.Artist == null)
            {
                return StatusCode(403, "Access Denied.");
            }

            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            if (category.UserId != currentUser.Id)
            {
                return StatusCode(403, "Access Denied.");
            }

            category.Mark = GetCategoryMarkValue(category);
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Mark")] Category category)
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(
                u => u.Id == HttpContext.Session.GetInt32(AccountController.UserIdKey)
            );
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (_context.Artist == null)
            {
                return StatusCode(403, "Access Denied.");
            }

            if (id != category.Id)
            {
                return NotFound();
            }

            // no need to check, if there is any result - category has appropriate Id
            var userId = _context.Category.Where(cat => cat.Id == category.Id).Select(cat => cat.UserId).First();
            if (userId != currentUser.Id)
            {
                return StatusCode(403, "Access Denied.");
            }
            category.UserId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    var mark = GetCategoryMark(category);

                    if (mark == null && category.Mark != null)
                    {
                        _context.Add(new CategoryMark { CategoryId = category.Id, UserId = currentUser.Id, Mark = category.Mark });
                    }
                    else if (mark != null)
                    {
                        mark.Mark = category.Mark;
                        _context.Update(mark);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(
                u => u.Id == HttpContext.Session.GetInt32(AccountController.UserIdKey)
            );
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (_context.Artist == null)
            {
                return StatusCode(403, "Access Denied.");
            }

            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            if (category.UserId != currentUser.Id)
            {
                return StatusCode(403, "Access Denied.");

            }
            category.Mark = GetCategoryMarkValue(category);

            return View(category);
        }

        // POST: Category/Delete/5
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
            if (_context.Artist == null)
            {
                return StatusCode(403, "Access Denied.");
            }

            if (_context.Category == null)
            {
                return Problem("Entity set 'MelomanContext.Category'  is null.");
            }
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                if (category.UserId != currentUser.Id)
                {
                    return StatusCode(403, "Access Denied.");

                }
                var mark = GetCategoryMark(category);
                if (mark != null)
                {
                    _context.CategoryMark.Remove(mark);
                }
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private CategoryMark? GetCategoryMark(Category category)
        {
            if (_context.CategoryMark == null)
            {
                return null;
            }

            var res = from mark in _context.CategoryMark
                      where mark.CategoryId == category.Id
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

        private double? GetCategoryMarkValue(Category category)
        {
            var mark = GetCategoryMark(category);
            if (mark == null)
            {
                return ArtistMark.defaultMark;
            }

            return mark.Mark;
        }
    }
}
