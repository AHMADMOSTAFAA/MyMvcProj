using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class RoleVM
    {
        [Display(Name="Role Name")]
        public string RoleName { get; set; }
    }
}
