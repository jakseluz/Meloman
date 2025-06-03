using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
namespace Meloman.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Username"), Required, MaxLength(100)]
        public required string Username { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required string Salt { get; set; }

        public string ?Role { get; set; }
    }
}
