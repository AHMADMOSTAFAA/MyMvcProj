using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Repos.Departments;
using WebApplication2.Repos.Instructors;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class DepartmentController : Controller
    {
        IDeptRepo deptRepo;
        IInsRepo insRepo;
        public DepartmentController(IDeptRepo _deptRepo,IInsRepo _insRepo)
        {
            deptRepo=_deptRepo;
            insRepo = _insRepo;
        }
        public IActionResult Index()
        {
            var departments = deptRepo.LoadDeparmentsWithInstructors();
            return View(departments);
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Add()
        {
            DepartmentVM departmentVM = new DepartmentVM() { 
            AllInstructors = insRepo.LoadInstructors()
            };
            return View(departmentVM);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(DepartmentVM departmentVM)
        {
           if(ModelState.IsValid)
            {
                deptRepo.Add(departmentVM);
                return RedirectToAction("Index");
            }
            return View("Add",departmentVM);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
           var dept=deptRepo.FindDept(id);
            deptRepo.Delete(dept);
            return RedirectToAction("Index");
        }
    }
}
