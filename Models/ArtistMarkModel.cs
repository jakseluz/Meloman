using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class ArtistMark
    {
        [Key]
        public int Id { get; set; }
        public Artist? Artist { get; set; }
        public User? User { get; set; }
        [Display(Name = "Mark")]
        public int Mark = 0;
    }
}
