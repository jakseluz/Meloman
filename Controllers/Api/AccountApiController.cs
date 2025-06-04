using Microsoft.AspNetCore.Mvc;
using Meloman.Data;
using Meloman.Models;
using Meloman.Helpers; // np. PasswordHelper
using System.Security.Cryptography;

namespace Meloman.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly MelomanContext _context;

        public AccountApiController(MelomanContext context)
        {
            _context = context;
        }

        // POST /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Username i password są wymagane." });

            // 1. Sprawdź, czy już istnieje user o tej samej nazwie
            if (_context.User.Any(u => u.Username == dto.Username))
                return Conflict(new { message = "Ten użytkownik już istnieje." });

            // 2. Generowanie soli i hashowanie hasła
            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(dto.Password, salt);

            // 3. Generowanie ApiKey (GUID bez myślników)
            string apiKey = Guid.NewGuid().ToString("N");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hash,
                Salt = salt,
                Role = dto.Role,
                ApiKey = apiKey
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            // 4. Zwróć minimalne info (np. username + apiKey)
            return Ok(new
            {
                username = user.Username,
                apiKey = user.ApiKey
            });
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Username i password są wymagane." });

            var user = _context.User
                        .FirstOrDefault(u => u.Username == dto.Username);
            if (user == null)
                return Unauthorized(new { message = "Nieprawidłowy login lub hasło." });

            bool isValid = PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash, user.Salt);
            if (!isValid)
                return Unauthorized(new { message = "Nieprawidłowy login lub hasło." });

            // Jeśli chcesz odświeżyć ApiKey przy każdym logowaniu, zrób tu:
            // user.ApiKey = Guid.NewGuid().ToString("N");
            // await _context.SaveChangesAsync();

            return Ok(new
            {
                username = user.Username,
                apiKey = user.ApiKey,
                role = user.Role
            });
        }
    }
}
