using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Repos.Instructors;
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
        IInsRepo _insRepo;
        IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser>userManager,SignInManager<ApplicationUser>signInManager,RoleManager<IdentityRole>roleManager,IStudentRepo studentRepo,IInsRepo insRepo,IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager=roleManager;
            _studentRepo=studentRepo;
            _insRepo=insRepo;
            _emailSender=emailSender;
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task <IActionResult> Register()
        {
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterVM registerVM,int?id)
        {
            if (ModelState.IsValid) { 
                ApplicationUser applicationUser=new ApplicationUser();
                applicationUser.UserName = registerVM.UserName;
                applicationUser.PasswordHash = registerVM.Password;
                if (id != null)
                {

                    var student = _studentRepo.Student(id.Value);
                    applicationUser.Email = student.Email;
                }
                applicationUser.Email = registerVM.Email;
               
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
                                         UserId = applicationUser.Id,
                                         
                                     };
                                   _studentRepo.Edit(StdVM);
                                }
                            }
                            await SendConfirmationEmail(applicationUser);
                            TempData["Success"] = "User registered and assigned to role! an Email Has Been Sent For Verfification";
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterIns()
        {
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterIns(RegisterVM registerVM, int?id)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = registerVM.UserName;
                applicationUser.PasswordHash = registerVM.Password;
                applicationUser.Email = registerVM.Email;
               
                applicationUser.Address = registerVM.Address;
                IdentityResult identityResult = await _userManager.CreateAsync(applicationUser, registerVM.Password);//
                if (identityResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(applicationUser, registerVM.Role);
                        if (roleResult.Succeeded)
                        {
                            if (id.HasValue)
                            {
                                var instructor = _insRepo.FindInstructor(id.Value);
                                if (instructor != null)
                                {
                                    instructor.UserId = applicationUser.Id;

                                 
                                    _insRepo.UpdateInstructor(instructor);

                                }
                            }
                            await SendConfirmationEmail(applicationUser);
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

            return View("Register", registerVM);
        }

        private async Task SendConfirmationEmail(ApplicationUser applicationUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var confirmationLink=Url.Action("ConfirmEmail","Account", 
                new { userId = applicationUser.Id, token = token },
            Request.Scheme);//start the confirm func below pleazzzzz
            //https is hidden in scheeeme
            await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm Your Email",
       $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest("Invalid email confirmation request.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["Success"] = "Email confirmed successfully!";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "Invalid or expired confirmation link.";
            return RedirectToAction("Error"); 
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
                
                ApplicationUser applicationUser = await _userManager.Users
                    .Include(u => u.Student).Include(u=>u.Instructor)
                    .FirstOrDefaultAsync(u => u.UserName == account.UserName);
 


                if (applicationUser != null)
                {
                    bool CheckPassword = await _userManager.CheckPasswordAsync(applicationUser, account.Password);
                    if (CheckPassword)
                    {
                        List<Claim> claims = new List<Claim>
                        {
                          new Claim("UserAddress", applicationUser.Address),
                        };
                        if (await _userManager.IsInRoleAsync(applicationUser,"Student")) {
                            claims.Add(new Claim("sid", applicationUser.Student.Id.ToString()));
                            claims.Add(new Claim("Img", applicationUser.Student.IMG??""));
                        }
                        else if (await _userManager.IsInRoleAsync(applicationUser, "Instructor"))
                        {
                            claims.Add(new Claim("sid", applicationUser.Instructor.Id.ToString()));
                            claims.Add(new Claim("Img", applicationUser.Instructor.IMG ?? ""));
                        }
                            await _signInManager.SignInWithClaimsAsync(applicationUser, account.RememberMe, claims);

                        if (await _userManager.IsInRoleAsync(applicationUser, "Admin"))
                            return RedirectToAction("Index", "Admin");
                        else if (await _userManager.IsInRoleAsync(applicationUser, "HR"))
                            return RedirectToAction("Index", "Instructor");
                        else if (await _userManager.IsInRoleAsync(applicationUser, "Instructor"))
                            return RedirectToAction("IDetailsVM", "Instructor", new { id = applicationUser.Instructor.Id });
                        else if (await _userManager.IsInRoleAsync(applicationUser, "Student"))
                            return RedirectToAction("DetailsVM", "Student", new { id = applicationUser.Student.Id });


                    }
                }

                ModelState.AddModelError("", "Username or password is invalid");
                TempData["ErrorMessage"] = "Login failed: Invalid username or password.";
            }

            return View("login", account);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                TempData["ErrorMessage"] = "Invalid email or unconfirmed account.";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account",
                new { token, email = user.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Reset Your Password",
                $"Click <a href='{resetLink}'>here</a> to reset your password.");

            TempData["Success"] = "Password reset link sent! Check your email.";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return BadRequest("Invalid password reset request.");
            }

            var model = new ResetPasswordVM { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Password reset successfully!";
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = string.Join(", ", result.Errors.Select(e => e.Description));
            return View();
        }


        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
