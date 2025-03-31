using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Account
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //[NotMapped]
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
