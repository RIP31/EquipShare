using System.ComponentModel.DataAnnotations;

namespace EquipShare.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}