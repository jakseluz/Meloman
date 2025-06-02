using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class CategoryMark
    {
        [Key]
        public int Id { get; set; }
        public Category? Category { get; set; }
        public User? User { get; set; }
        [Display(Name = "Mark")]
        public int Mark = 0;
    }
}
