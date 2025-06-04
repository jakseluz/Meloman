using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int? AuthorId { get; set; }
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        [NotMapped]
        [Display(Name = "Author name")]
        public string? AuthorName { get; set; }
        [NotMapped]
        [Display(Name = "Author surname")]
        public string? AuthorSurname { get; set; }
        [NotMapped]
        [Display(Name = "Category")]
        public string? Category { get; set; }

    }
}
