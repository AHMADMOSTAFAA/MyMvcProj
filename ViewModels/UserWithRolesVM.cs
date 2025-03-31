using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class UserWithRolesVM
    {
        
        
            public ApplicationUser User { get; set; }
            public IList<string> Roles { get; set; }
        
    }
}
