using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace USERVIEW.Areas.Identity.Data;

// Add profile data for application users by adding properties to the USERVIEWUser class
public class USERVIEWUser : IdentityUser
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
    [Display(Name = "Register Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime RegisterDate { get; set; } = DateTime.Now;

    [Display(Name = "Registered By")]
    public string? RegisteredBy { get; set; } = "DEV";


}

