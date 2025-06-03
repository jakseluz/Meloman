using System.ComponentModel.DataAnnotations;
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

        // Dodatkowe pola (opcjonalnie): Email, DataRejestracji, Rola itp.
        // [Required, EmailAddress]
        // public string Email { get; set; }
    }
}
