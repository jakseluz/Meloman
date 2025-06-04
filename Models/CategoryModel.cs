using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Meloman.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public required string Name { get; set; }
        [Display(Name = "Added by User")]
        public int? UserId { get; set; } // null means it is a genre
        [NotMapped]
        [Display(Name = "Mark")]
        [DisplayFormat(NullDisplayText = "0")]
        public double? Mark { get; set; }
        [NotMapped]
        [Display(Name = "Genre")]
        public bool IsGenre { get; set; }
    }
}
