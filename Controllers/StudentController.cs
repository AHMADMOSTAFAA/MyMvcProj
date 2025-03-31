using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using System.IO;
using WebApplication2.Repos.Students;
using WebApplication2.Repos.Departments;
using WebApplication2.Repos.Courses;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers
{
    public class StudentController : Controller
    {
        IStudentRepo IStudentRepo;
        IDeptRepo IDeptRepo;
        ICourseRepo ICourseRepo;
       public StudentController(IStudentRepo studentRepo,IDeptRepo deptRepo,ICourseRepo _ICourseRepo)
        {
            IStudentRepo = studentRepo;
            IDeptRepo = deptRepo;
            ICourseRepo = _ICourseRepo;
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            var students = IStudentRepo.LoadStdWithCrses();
            ViewBag.Departments = IDeptRepo.LoadDeparments() ?? new List<Department>(); ;
            return View(students);
        }

        public IActionResult FindStudentsInDepartment(int id)
        {
            var students = IStudentRepo.LoadStdWithCrsesByDeptId(id);
            return PartialView("_StudentsInDepartment", students);
        }

        public IActionResult DetailsVM(int id)
        {
            var student = IStudentRepo.LoadStdWithHisCourse(id);
            StudentDetailsVM studentDetailsVM = new StudentDetailsVM
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Age = student.Age,
                IMG = student.IMG,
                DepartmentId = student.DepartmentId,
                Course_Stds = student.Course_Stds,
                coursenames = student.Course_Stds.Select(crs => crs.Course.Name).ToList()
            };

            TempData["StudentName"] = studentDetailsVM.Name;
            return View(studentDetailsVM);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            StudentDetailsVM studentDetailsVM = new StudentDetailsVM
            {
                departments = IDeptRepo.LoadDeparments()
            };
            return View(studentDetailsVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(StudentDetailsVM stdvm)
        {
            if (ModelState.IsValid)
            {
               IStudentRepo.Add(stdvm);
                return RedirectToAction("Register", "Account", new {studentId=stdvm.Id});
            }

            stdvm.departments = IDeptRepo.LoadDeparments();
            return View(stdvm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student =IStudentRepo.Student(id);

            if (student == null)
                return NotFound();

            StudentDetailsVM studentDetailsVM = new StudentDetailsVM
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                IMG = student.IMG,
                Age = student.Age,
                DepartmentId = student.DepartmentId,
                departments = IDeptRepo.LoadDeparments(),
                courses =ICourseRepo.LoadCourses(),
                SelectedCrsIDs = student.Course_Stds.Select(cs => cs.CourseId).ToList()
            };

            return View(studentDetailsVM);
        }

        [HttpPost]
        public IActionResult Edit(StudentDetailsVM stdvm)
        {
            if (ModelState.IsValid)
            {
                IStudentRepo.Edit(stdvm);
                return RedirectToAction("Index");
            }

            stdvm.departments = IDeptRepo.LoadDeparments();
            stdvm.courses = ICourseRepo.LoadCourses();
            return View(stdvm);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var selectedstd = IStudentRepo.Student(id);
            IStudentRepo.Delete(selectedstd);

            return RedirectToAction("index");
        }
    }
}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication2.Context;
//using WebApplication2.Models;
//using WebApplication2.ViewModels;
//using System.IO;

//namespace WebApplication2.Controllers
//{
//    public class StudentController : Controller
//    {
//        ITIContext ITI = new ITIContext();

//        public IActionResult Index()
//        {
//            var students = ITI.Students.Include(s => s.Department).Include(s => s.Course_Stds).ThenInclude(c => c.Course).ToList();
//            ViewBag.Departments = ITI.Departments.ToList();
//            return View(students);
//        }

//        public IActionResult FindStudentsInDepartment(int id)
//        {
//            var students = ITI.Students.Where(s => s.DepartmentId == id).Include(s => s.Department).Include(s => s.Course_Stds).ThenInclude(c => c.Course).ToList();
//            return PartialView("_StudentsInDepartment", students);
//        }

//        public IActionResult DetailsVM(int id)
//        {
//            var student = ITI.Students.Include(s => s.Course_Stds).ThenInclude(C => C.Course).FirstOrDefault(s => s.Id == id);
//            StudentDetailsVM studentDetailsVM = new StudentDetailsVM
//            {
//                Id = student.Id,
//                Name = student.Name,
//                Email = student.Email,
//                Age = student.Age,
//                IMG = student.IMG,
//                DepartmentId = student.DepartmentId,
//                Course_Stds = student.Course_Stds,
//                coursenames = student.Course_Stds.Select(crs => crs.Course.Name).ToList()
//            };

//            TempData["StudentName"] = studentDetailsVM.Name;
//            return View(studentDetailsVM);
//        }

//        [HttpGet]
//        public IActionResult Add()
//        {
//            StudentDetailsVM studentDetailsVM = new StudentDetailsVM
//            {
//                departments = ITI.Departments.ToList()
//            };
//            return View(studentDetailsVM);
//        }

//        [HttpPost]
//        public IActionResult Add(StudentDetailsVM stdvm)
//        {
//            if (ModelState.IsValid)
//            {
//                Student newStudent = new Student
//                {
//                    Name = stdvm.Name,
//                    Age = stdvm.Age,
//                    Email = stdvm.Email,
//                    DepartmentId = stdvm.DepartmentId
//                };

//                if (stdvm.IMGFile != null)
//                {
//                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(stdvm.IMGFile.FileName)}";
//                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

//                    using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        stdvm.IMGFile.CopyTo(stream);
//                    }

//                    newStudent.IMG = "/Images/" + fileName;
//                }

//                ITI.Students.Add(newStudent);
//                ITI.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            stdvm.departments = ITI.Departments.ToList();
//            return View(stdvm);
//        }

//        [HttpGet]
//        public ActionResult Edit(int id)
//        {
//            var student = ITI.Students.FirstOrDefault(s => s.Id == id);

//            if (student == null)
//                return NotFound();

//            StudentDetailsVM studentDetailsVM = new StudentDetailsVM
//            {
//                Id = student.Id,
//                Name = student.Name,
//                Email = student.Email,
//                IMG = student.IMG,
//                Age = student.Age,
//                DepartmentId = student.DepartmentId,
//                departments = ITI.Departments.ToList(),
//                courses = ITI.Courses.ToList(),
//                SelectedCrsIDs = student.Course_Stds.Select(cs => cs.CourseId).ToList()
//            };

//            return View(studentDetailsVM);
//        }

//        [HttpPost]
//        public IActionResult Edit(StudentDetailsVM stdvm)
//        {
//            if (ModelState.IsValid)
//            {
//                var updatedstd = ITI.Students.Include(s => s.Course_Stds).FirstOrDefault(s => s.Id == stdvm.Id);

//                if (updatedstd == null)
//                    return NotFound();

//                updatedstd.Name = stdvm.Name;
//                updatedstd.Age = stdvm.Age;
//                updatedstd.DepartmentId = stdvm.DepartmentId;
//                updatedstd.Email = stdvm.Email;

//                if (stdvm.IMGFile != null)
//                {
//                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(stdvm.IMGFile.FileName)}";
//                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

//                    using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        stdvm.IMGFile.CopyTo(stream);
//                    }

//                    updatedstd.IMG = "/Images/" + fileName;
//                }

//                updatedstd.Course_Stds = stdvm.SelectedCrsIDs
//                    .Select(courseId => new Course_Stds { StudentId = updatedstd.Id, CourseId = courseId })
//                    .ToList();

//                ITI.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            stdvm.departments = ITI.Departments.ToList();
//            stdvm.courses = ITI.Courses.ToList();
//            return View(stdvm);
//        }

//        public IActionResult Delete(int id)
//        {
//            var selectedstd = ITI.Students.Find(id);
//            ITI.Students.Remove(selectedstd);
//            ITI.SaveChanges();

//            return RedirectToAction("index");
//        }
//    }
//}
