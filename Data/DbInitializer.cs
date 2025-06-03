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
            var user = context.User.FirstOrDefault(u => u.Role == "admin");
            if (user != null) return;

            var salt = PasswordHelper.GenerateSalt();
            var hash = PasswordHelper.HashPassword("admin", salt);
            var newUser = new User
            {
                Username = "admin",
                PasswordHash = hash,
                Salt = salt,
                Role = "admin"
            };
            context.User.Add(newUser);
            context.SaveChanges();
        }
    } 
}