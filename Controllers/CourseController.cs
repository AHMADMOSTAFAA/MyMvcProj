using Microsoft.AspNetCore.Mvc;
using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.Repos.Courses;
using WebApplication2.Repos.Instructors;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class CourseController : Controller
    {
        ICourseRepo IcourseRepo;
        IInsRepo insRepo;
        public CourseController(ICourseRepo _IcourseRepo,IInsRepo _insRepo)
        {
            IcourseRepo = _IcourseRepo;
            insRepo = _insRepo;
        }

        public IActionResult index()
        {


            var cources = IcourseRepo.LoadCourses();

            return View(cources);
        }
        [HttpGet]
        public IActionResult Add()
        {
            CourseDetailsVM courseDetailsVM = new CourseDetailsVM();

            courseDetailsVM.instructors = insRepo.LoadInstructors();
            return View(courseDetailsVM);
        }
        [HttpPost]
        public IActionResult Add(CourseDetailsVM CrsVM)
        {
           
            if (ModelState.IsValid)
            {
               IcourseRepo.AddCourse(CrsVM);
                return RedirectToAction("Index");
            }
            CrsVM.instructors = insRepo.LoadInstructors();
            return View("add", CrsVM);
        }
        public IActionResult Delete(int id)
        {
            var selected_Course = IcourseRepo.FindCourse(id);
            if (selected_Course != null)
            {
                IcourseRepo.DeleteCourse(id);
                return RedirectToAction("index");
            }
            return Content("Couldnt delete");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var selected_Course = IcourseRepo.FindCourse(id);
            CourseDetailsVM courseDetailsVM = new CourseDetailsVM()
            {
                Id = id,
                Name = selected_Course.Name,
                Topic = selected_Course.Topic,
                InstructorId = selected_Course.InstructorId,
                instructors =insRepo.LoadInstructors(),
            };
            return View(courseDetailsVM);
        }
        [HttpPost]
        public IActionResult Edit(CourseDetailsVM courseDetailsVM)
        {

            if (ModelState.IsValid)
            {
                IcourseRepo.EditCourse(courseDetailsVM);
                return RedirectToAction("index");
            }
            courseDetailsVM.instructors = insRepo.LoadInstructors();
            return View("Edit", courseDetailsVM);
        }
    }
}
