using System.ComponentModel.DataAnnotations;

namespace MVC.Auth.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}