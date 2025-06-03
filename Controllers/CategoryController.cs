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
    public class CategoryController : Controller
    {
        private readonly MelomanContext _context;

        public CategoryController(MelomanContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'MelomanContext.Artist'  is null.");
            }
            var categories = await _context.Category.ToListAsync();
            return View(categories.Select(category =>
            {
                category.Mark = GetCategoryMarkValue(category);
                return category;
            }));
            // return _context.Category != null ?
            //             View(await _context.Category.ToListAsync()) :
            //             Problem("Entity set 'MelomanContext.Category'  is null.");
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
            category.Mark = GetCategoryMarkValue(category);

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Mark")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                if (category.Mark != null)
                {
                    _context.Add(new CategoryMark
                    {
                        Id = 0,
                        Category = category,
                        User = null,
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
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
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
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    var mark = GetCategoryMark(category);

                    if (mark == null && category.Mark != null)
                    {
                        _context.Add(new CategoryMark { Id = 0, Category = category, User = null, Mark = category.Mark });
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
            category.Mark = GetCategoryMarkValue(category);

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'MelomanContext.Category'  is null.");
            }
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
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
                      where mark.Category == category
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
