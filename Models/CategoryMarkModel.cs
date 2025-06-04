using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class CategoryMark
    {
        public static readonly double? DefaultMark = null;
        [Key]
        public int Id { get; set; }
        public Category? Category { get; set; }
        public int? UserId { get; set; }
        [Display(Name = "Mark")]
        [DisplayFormat(NullDisplayText = "0")]
        public double? Mark { get; set; }
    }
}
