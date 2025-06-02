using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public required string Name { get; set; }
        [Display(Name = "Surname")]
        public required string Surname { get; set; }
    }
}
