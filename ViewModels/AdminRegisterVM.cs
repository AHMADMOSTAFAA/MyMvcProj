using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class AdminRegisterVM
    {
        public string Password { get; set; }
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
