using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Meloman.Data;
using Meloman.Models;
using Meloman.Helpers;

namespace Meloman.Controllers
{
    public class AccountController : Controller
    {
        private readonly MelomanContext _context;

        public AccountController(MelomanContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Należy wypełnić wszystkie pola.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Hasła nie mogą się różnić!";
                return View();
            }

            var existingUser = await _context.User
                .FirstOrDefaultAsync(u => u.Username == username);

            if (existingUser != null)
            {
                ViewBag.Error = "Użytkownik jest już zarejestrowany!";
                return View();
            }

            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                PasswordHash = hash,
                Salt = salt
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            /* logowanie od razu 
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            */

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Należy wypełnić oba pola.";
                return View();
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            bool valid = false;

            if (user != null)
            {
                valid = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.Salt);
            }

            if (!valid)
            {
                ViewBag.Error = "Nieprawidłowy login lub hasło.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
    }
}