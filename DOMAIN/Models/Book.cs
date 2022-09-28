

using System.ComponentModel.DataAnnotations;

namespace DOMAIN.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Book Title")]
        [Required]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Register Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "Book Subject")]
        [Required]
        public string? Subject { get; set; }

        [Display(Name = "Book Serial NO.")]
        [Required]
        public string? SerialNumber { get; set; }

        [Display(Name = "Registered By")]
        public string? RegisteredBy { get; set; }

        [Display(Name = "Issued")]
        public string? Issued { get; set; } = "NO";

        // Navigation Properties
        [Required]
        [Display(Name = "Book Form")]
        public int FormID { get; set; }

        
        [Display(Name = "Book Form")]
        public Form? Form { get; set; }

        [Required]
        [Display(Name = "Book Source")]
         public int BookSourceID { get; set; }

        
        [Display(Name = "Book Source")]
         public BookSource? BookSource { get; set; }

        public ICollection<Borrowing>? Borrowings { get; set; }


    }
}
