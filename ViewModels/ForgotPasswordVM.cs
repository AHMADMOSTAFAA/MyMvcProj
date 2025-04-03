using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
