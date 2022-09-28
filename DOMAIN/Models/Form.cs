using System.ComponentModel.DataAnnotations;

namespace DOMAIN.Models
{
    public class Form
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
                
        public string? RegisteredBy { get; set; } = "DEV";


    }
}
