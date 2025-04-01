using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address { get; set; } 
        public Student? Student { get; set; }//nav prop
        public Instructor? Instructor { get; set; }//nav prop
    }
}
