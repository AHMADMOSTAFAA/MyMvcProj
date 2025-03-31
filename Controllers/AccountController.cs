using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Repos.Students;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        IStudentRepo _studentRepo;
        public AccountController(UserManager<ApplicationUser>userManager,SignInManager<ApplicationUser>signInManager,RoleManager<IdentityRole>roleManager,IStudentRepo studentRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager=roleManager;
            _studentRepo=studentRepo;
        }
        [HttpGet]
        public async Task <IActionResult> Register()
        {
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM,int?id)
        {
            if (ModelState.IsValid) { 
                ApplicationUser applicationUser=new ApplicationUser();
                applicationUser.UserName = registerVM.UserName;
                applicationUser.PasswordHash = registerVM.Password;
                applicationUser.Email = registerVM.Email;
                if (id!=null)
                {
                    
                    var student =_studentRepo.Student(id.Value);
                    applicationUser.Email = student.Email;
                }
                applicationUser.Address = registerVM.Address;
                IdentityResult identityResult=await _userManager.CreateAsync(applicationUser,registerVM.Password);//
                if (identityResult.Succeeded) 
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(applicationUser, registerVM.Role);
                        if (roleResult.Succeeded)
                        {
                            if (id.HasValue)
                            {
                                var student = _studentRepo.Student(id.Value);
                                if (student != null)
                                {
                                    student.UserId = applicationUser.Id;

                                     StudentDetailsVM StdVM = new StudentDetailsVM()
                                     {
                                         Id = student.Id,
                                         Name = student.Name,
                                         Email = student.Email,
                                         Age = student.Age,
                                         IMG=student.IMG,
                                         
                                     };
                                   _studentRepo.Edit(StdVM);
                                }
                            }
                            TempData["Success"] = "User registered and assigned to role!";
                            return RedirectToAction("Login");

                        }
                        else
                        {

                            await _userManager.DeleteAsync(applicationUser);
                            TempData["ErrorMessage"] = "User registration failed due to role assignment error: " +
                                string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        }

                    }
                    else
                    {
                        
                        await _userManager.DeleteAsync(applicationUser);
                        TempData["ErrorMessage"] = "User registration failed: No role selected.";
                    }
                }
                else
                {
                   
                    TempData["ErrorMessage"] = "User registration failed: " +
                        string.Join(", ", identityResult.Errors.Select(e => e.Description));
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid input. Please try again.";
            }
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View("Register",registerVM);
        }

        [HttpGet]
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(Account account)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser=await _userManager.FindByNameAsync(account.UserName);
                if (applicationUser != null) { 
                bool CheckPassword=await _userManager.CheckPasswordAsync(applicationUser, account.Password);
                    if (CheckPassword) { 
                    List<Claim>claims = new List<Claim>();
                        claims.Add(new Claim("UserAddress",applicationUser.Address));

                        await _signInManager.SignInWithClaimsAsync(applicationUser, account.RememberMe,claims);
                        TempData["Success"] = "Welcome back, " + applicationUser.UserName + "!";
                        return RedirectToAction("index", "Student");
                    }
                }
                ModelState.AddModelError("", "username or password Invalid");
                TempData["ErrorMessage"] = "Login failed: Invalid username or password.";
            }
            return View("login",account);
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return View("login");
        }
    }
}
