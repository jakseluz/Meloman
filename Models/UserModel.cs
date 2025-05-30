using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public String Name { get; set; }
        [Display(Name = "Surname")]
        public String Surname { get; set; }

    }
}
