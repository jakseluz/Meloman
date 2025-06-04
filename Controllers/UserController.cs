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
using Meloman.Helpers;

namespace Meloman.Controllers
{
    public class UserController : Controller
    {
        private readonly MelomanContext _context;

        public UserController(MelomanContext context)
        {
            _context = context;
        }

        private void PrepareRoles()
        {
            ViewBag.RoleList = new SelectList(new List<string> { "admin", "user", "" });
        }

        // GET: User
        [ServiceFilter(typeof(AdminAllowedAttribute))]
        public async Task<IActionResult> Index()
        {
            return _context.User != null ?
                        View(await _context.User.ToListAsync()) :
                        Problem("Entity set 'MelomanContext.User'  is null.");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            PrepareRoles();
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                {
                    ViewBag.Error = "All fields must be filled.";
                    return View();
                }

                var existingUser = await _context.User
                    .FirstOrDefaultAsync(u => u.Username == user.Username);

                if (existingUser != null)
                {
                    ViewBag.Error = "User already registered!";
                    return View();
                }


                string salt = PasswordHelper.GenerateSalt();
                string hash = PasswordHelper.HashPassword(user.Password, salt);

                var newUser = new User
                {
                    Username = user.Username,
                    PasswordHash = hash,
                    Salt = salt,
                    ApiKey = Guid.NewGuid().ToString("N")
                };

                _context.User.Add(newUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            PrepareRoles();
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            PrepareRoles();
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,PasswordHash,Salt,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var foregoingUser = _context.User.FirstOrDefault(u => u.Id == user.Id);
                    if (foregoingUser!.PasswordHash == null || (user.Password != null && user.Password != ""))
                    {
                        foregoingUser.Salt = PasswordHelper.GenerateSalt();
                        foregoingUser.PasswordHash = PasswordHelper.HashPassword(user.Password ?? "", user.Salt);
                        foregoingUser.ApiKey = Guid.NewGuid().ToString("N");
                    }
                    foregoingUser.Role = user.Role;
                    _context.Update(foregoingUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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

            PrepareRoles();
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'MelomanContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}