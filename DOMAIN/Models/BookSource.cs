

using System.ComponentModel.DataAnnotations;

namespace DOMAIN.Models
{
    public class BookSource
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Book Source")]
        [Required]
        public string? Source { get; set; }

        
    }
}
