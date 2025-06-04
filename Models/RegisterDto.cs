namespace Meloman.Models
{
    public class RegisterDto
    {
        // Nazwa użytkownika
        public string Username { get; set; } = null!;

        // Hasło w postaci jawnej (plaintext) – będzie zahashowane po stronie serwera
        public string Password { get; set; } = null!;

        // Rola (opcjonalnie); domyślnie „user”
        public string Role { get; set; } = "user";
    }
}
