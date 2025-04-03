using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.Repos.Courses;
using WebApplication2.Repos.Departments;
using WebApplication2.Repos.Instructors;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class InstructorController : Controller
    {
        IInsRepo insRepo;
        ICourseRepo ICourseRepo;
        IDeptRepo deptRepo;
        public InstructorController(IInsRepo _insRepo,ICourseRepo courseRepo,IDeptRepo deptRepo) { 
        insRepo= _insRepo;
            ICourseRepo = courseRepo;
            this.deptRepo = deptRepo;
        }
        public IActionResult Index()
        {
            var instructors =insRepo.LoadInstructors(); 
            return View(instructors);
        }
        public IActionResult UniqueName(string FName, int DepartmentId)
        {
            var instructorname =insRepo.InsNameInDept(FName, DepartmentId);
            if (instructorname)
            {
                return Json(false);
            }
            return Json(true);
        }

       
        private static bool _isLoaded = false; // Flag to track execution
        [Authorize(Roles = "Instructor")]
        public IActionResult IDetailsVM(int id)
        {
            if (_isLoaded)
                return View(); // Prevents multiple executions

            _isLoaded = true; // Set flag to true to prevent re-execution

            var instructor = insRepo.InsWithHisCourses(id);
            if (instructor == null)
            {
                _isLoaded = false; // Reset flag if instructor not found
                return NotFound();
            }

            InstructorDetailsVM instructorVM = new InstructorDetailsVM()
            {
                Id = instructor.Id,
                FullName = instructor.FName + " " + instructor.LName,
                HireDate = instructor.HireDate,
                Salary = instructor.Salary,
                Age = instructor.Age,
                IMG = instructor.IMG,
                DepartmentId = instructor.DepartmentId,
                Courses = instructor.Courses.Select(c => c.Name).ToList(),
            };

            _isLoaded = false; // Reset flag after successful execution

            return View(instructorVM);
        }


        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Add()
        {
            var Courses = ICourseRepo.LoadCourses();
            ViewBag.courses = Courses;
            var departments = deptRepo.LoadDeparments();
            ViewBag.departments = departments;

            var instructor = new Instructor();

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(Instructor ins, int[] courses)
        {
            if (ModelState.IsValid)
            {
                var selectedCourses = ICourseRepo.SelectedCrsesIDs(courses);
                ins.Courses = selectedCourses;
                insRepo.Add(ins);
                return RedirectToAction("RegisterIns", "Account", new { id = ins.Id });
            }

            ViewBag.courses = ICourseRepo.LoadCourses();
            ViewBag.departments =deptRepo.LoadDeparments();

            return View("Add", ins);
        }
        [HttpGet]
        [Authorize(Roles ="HR")]
        public IActionResult Edit(int id)
        {
            var ins = insRepo.FindInstructor(id);
            ViewBag.courses = ICourseRepo.LoadCourses();
            ViewBag.departments = deptRepo.LoadDeparments();
            ViewBag.InsCources = ins.Courses.Select(c => c.Id).ToList();
            return View(ins);
        }
        [HttpPost]
        [Authorize(Roles = "HR")]
        public IActionResult Edit(Instructor i, int[] courses)
        {
            if (ModelState.IsValid)
            {
                // Fetch the instructor with their existing courses
                var instructor = insRepo.InsWithHisCourses(i.Id);

                if (instructor == null)
                    return NotFound();

                // Update scalar properties
                instructor.FName = i.FName;
                instructor.LName = i.LName;
                instructor.HireDate = i.HireDate;
                instructor.Age = i.Age;
                instructor.Salary = i.Salary;
                instructor.IMG = i.IMG;
                instructor.DepartmentId = i.DepartmentId;

                // Fetch the selected courses
                var selectedCourses = ICourseRepo.SelectedCrsesIDs(courses);

                // Replace the existing courses with the new list
                instructor.Courses = selectedCourses;
                insRepo.UpdateInstructor(instructor);
               

                return RedirectToAction("Index");
            }

            // If validation fails, return to the edit view with the model
            ViewBag.courses = ICourseRepo.LoadCourses();
            ViewBag.departments = deptRepo.LoadDeparments();
            ViewBag.InsCources = i.Courses.Select(c => c.Id).ToList();
            return View("Edit", i);
        }
        [Authorize(Roles = "HR")]
        public IActionResult delete(int id)
        {
            insRepo.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
