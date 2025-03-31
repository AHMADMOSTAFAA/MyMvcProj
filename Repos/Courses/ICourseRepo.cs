using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Repos.Courses
{
    public interface ICourseRepo
    {
        public List<Course> LoadCourses();
        public void AddCourse(CourseDetailsVM crsvm);
        public Course FindCourse(int id);
        public void DeleteCourse(int id);
        public void EditCourse(CourseDetailsVM courseDetailsVM);
        public List<Course> SelectedCrsesIDs(int[] courses);

    }
}
