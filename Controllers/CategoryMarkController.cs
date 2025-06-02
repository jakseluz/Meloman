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
    public class CategoryMarkController : Controller
    {
        private readonly MelomanContext _context;

        public CategoryMarkController(MelomanContext context)
        {
            _context = context;
        }

        // GET: CategoryMark
        public async Task<IActionResult> Index()
        {
              return _context.CategoryMark != null ? 
                          View(await _context.CategoryMark.ToListAsync()) :
                          Problem("Entity set 'MelomanContext.CategoryMark'  is null.");
        }

        // GET: CategoryMark/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryMark == null)
            {
                return NotFound();
            }

            var categoryMark = await _context.CategoryMark
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryMark == null)
            {
                return NotFound();
            }

            return View(categoryMark);
        }

        // GET: CategoryMark/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryMark/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] CategoryMark categoryMark)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryMark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryMark);
        }

        // GET: CategoryMark/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryMark == null)
            {
                return NotFound();
            }

            var categoryMark = await _context.CategoryMark.FindAsync(id);
            if (categoryMark == null)
            {
                return NotFound();
            }
            return View(categoryMark);
        }

        // POST: CategoryMark/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] CategoryMark categoryMark)
        {
            if (id != categoryMark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryMark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryMarkExists(categoryMark.Id))
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
            return View(categoryMark);
        }

        // GET: CategoryMark/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryMark == null)
            {
                return NotFound();
            }

            var categoryMark = await _context.CategoryMark
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryMark == null)
            {
                return NotFound();
            }

            return View(categoryMark);
        }

        // POST: CategoryMark/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryMark == null)
            {
                return Problem("Entity set 'MelomanContext.CategoryMark'  is null.");
            }
            var categoryMark = await _context.CategoryMark.FindAsync(id);
            if (categoryMark != null)
            {
                _context.CategoryMark.Remove(categoryMark);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryMarkExists(int id)
        {
          return (_context.CategoryMark?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
