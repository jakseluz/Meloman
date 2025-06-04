using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class ArtistMark
    {
        public static readonly double? defaultMark = null;
        [Key]
        public int Id { get; set; }
        public int? ArtistId { get; set; }
        [Display(Name = "User")]
        public int? UserId { get; set; }
        [Display(Name = "Mark")]
        [DisplayFormat(NullDisplayText = "0")]
        public double? Mark { get; set; }
    }
}
