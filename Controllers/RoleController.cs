using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _RoleManager;
        public RoleController(RoleManager<IdentityRole>Role)
        {
            _RoleManager = Role;
        }
        [HttpGet]
        public IActionResult AddRole()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole();
                identityRole.Name = roleVM.RoleName;
                IdentityResult identityResult = await _RoleManager.CreateAsync(identityRole);
                if (identityResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role Added Successfully";
                    return View("AddRole");
                }
                else
                {
                    TempData["ErrorMssg"] = "Failed to add role. " + string.Join(", ", identityResult.Errors.Select(e => e.Description));
                }
            }
            else
            {
                TempData["ErrorMssg"] = "Invalid input. Please try again.";
            }
            return View("AddRole",roleVM);
        }
    }
}
