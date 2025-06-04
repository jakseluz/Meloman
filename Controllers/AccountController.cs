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
        public static readonly string UserIdKey = "UserId";
        public static readonly string UserNameKey = "Username";
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
                ViewBag.Error = "All fields must be filled.";
                return View();
            }

            var existingUser = await _context.User
                .FirstOrDefaultAsync(u => u.Username == username);

            if (existingUser != null)
            {
                ViewBag.Error = "User already registered!";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords cannot be different!";
                return View();
            }

            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                PasswordHash = hash,
                Salt = salt,
                ApiKey = Guid.NewGuid().ToString("N")
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
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
                ViewBag.Error = "Both fileds must be filled.";
                return View();
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                ViewBag.Error = "Incorrect login or password.";
                return View();
            }

            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                ViewBag.Error = "Incorrect login or password.";
                return View();
            }

            HttpContext.Session.SetInt32(UserIdKey, user.Id);
            HttpContext.Session.SetString(UserNameKey, user.Username);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(UserIdKey);
            HttpContext.Session.Remove(UserNameKey);
            return RedirectToAction("Index", "Home");
        }
    }
}