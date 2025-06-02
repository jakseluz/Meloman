using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public required String Name { get; set; }
        [Display(Name = "Surname")]
        public required String Surname { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
    }
}
