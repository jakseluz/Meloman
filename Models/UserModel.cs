using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
namespace Meloman.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Username"), Required, MaxLength(100)]
        public required string Username { get; set; }

        public string? PasswordHash { get; set; }

        public string? Salt { get; set; }

        public string? Role { get; set; }

        [MaxLength(64)]
        public string? ApiKey { get; set; }

        [NotMapped]
        public string? Password { get; set; }
    }
}