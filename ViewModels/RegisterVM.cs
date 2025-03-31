using System.ComponentModel.DataAnnotations;
using WebApplication2.Context;

namespace WebApplication2.ViewModels
{
    public class RegisterVM
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email {  get; set; }
        public string Address { get; set; }
        [Required]
        public string Role { get; set; }

       
       

    }
}
