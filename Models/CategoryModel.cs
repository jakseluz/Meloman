using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public required String Name { get; set; }
        [Display(Name = "Added by User")]
        public User? User { get; set; } // null means it is a genre
    }
}
