using System.ComponentModel.DataAnnotations;

namespace DOMAIN.Models
{
    public class Borrowing
    {
        [Key]
        public int Id { get; set; }
        public int StudentID { get; set; }
        public int BookID { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "Issued")]
        public string? Issued { get; set; } = "YES";

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Returned Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnedDate { get; set; }


        [Display(Name = "Issuing Officer")]
        public string? IssuedBy { get; set; } = "DEV";

        [Display(Name = "Returning Officer")]
        public string? ReturnedBy { get; set; } = "DEV";


        // Navigation Properties
        [Display(Name = "Student No.")]
        public Student CurrentStudent { get; set; }

        [Display(Name = "Book Serial No.")]
        public Book CurrentBook { get; set; }

    }
}
