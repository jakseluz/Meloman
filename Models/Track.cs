namespace Meloman.Models
{
    public class Track
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public String Name { get; set; }
    }
}
