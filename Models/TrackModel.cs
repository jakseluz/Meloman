using System.ComponentModel.DataAnnotations;
namespace Meloman.Models
{
    public class Track
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Title")]
        public required string Title { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Author")]
        public Artist? Author { get; set; }

    }
}
