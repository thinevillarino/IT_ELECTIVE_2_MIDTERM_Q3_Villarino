using System.ComponentModel.DataAnnotations;

namespace MVC.Auth.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(
            6,
            ErrorMessage = "New password must contain at least 6 characters."
        )]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
    }
}