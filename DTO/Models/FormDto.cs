

using System.ComponentModel.DataAnnotations;

namespace DTO.Models
{
    public class FormDto
    {
        public int Id { get; set; }

        [Display(Name = "Book Class")]
        [Required]
        public string? Name { get; set; }

        [Display(Name = "Registered By")]
        public string? RegisteredBy { get; set; } = "DEV";
    }
}
