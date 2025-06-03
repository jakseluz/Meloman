using System.Linq;
using Meloman.Models;
using Meloman.Helpers;
using System.Security.Policy;

namespace Meloman.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MelomanContext context)
        {
            if (context.User.Any(u => u.Username == "admin")) return;

            var salt = PasswordHelper.GenerateSalt();
            var hash = PasswordHelper.HashPassword("admin", salt);
            var user = new User
            {
                Username = "admin",
                PasswordHash = hash,
                Salt = salt,
                Role = "admin"
            };
            context.User.Add(user);
            context.SaveChanges();
        }
    } 
}