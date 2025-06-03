using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Meloman.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public required string Name { get; set; }
        [Display(Name = "Surname")]
        public required string Surname { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        [NotMapped]
        [Display(Name = "Mark")]
        [DisplayFormat(NullDisplayText = "0")]
        public double? Mark { get; set; }
    }
}
