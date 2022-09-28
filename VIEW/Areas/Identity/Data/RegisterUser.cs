using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USERVIEW.Areas.Identity.Data
{
    public class RegisterUser
    {
        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }
        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; }
        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string StaffNumber { get; set; }

        [Required]
        [PersonalData]
        [EmailAddress]
        [Display(Name = "Email")]
        public  string Email { get; set; }


        [Required]
        [PersonalData]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required]
        [PersonalData]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        
    }
}
