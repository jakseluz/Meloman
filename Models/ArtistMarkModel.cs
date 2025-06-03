using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class ArtistMark
    {
        public static readonly double? defaultMark = null;
        [Key]
        public int Id { get; set; }
        public Artist? Artist { get; set; }
        [Display(Name = "User")]
        public User? User { get; set; }
        [Display(Name = "Mark")]
        [DisplayFormat(NullDisplayText = "0")]
        public double? Mark { get; set; }
    }
}
