

using System.ComponentModel.DataAnnotations;

namespace DOMAIN.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Student Name")]
        [Required]
        public string? Name { get; set; }

        [Display(Name = "Student Admin NO.")]
        [Required]
        public string? AdminNumber { get; set; }

        [Display(Name = "Active")]
        public string? Active { get; set; } = "YES";

        [Display(Name = "Registered By")]
        public string? RegisteredBy { get; set; } = "DEV";

        //Navigation
        [Display(Name = "Student Form")]
        public int FormID { get; set; }

        [Display(Name = "Student Form")]
        public Form? Form { get; set; }

        public ICollection<Borrowing>? Borrowings { get; set; }

    }
}
