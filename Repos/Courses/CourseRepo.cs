using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using WebApplication2.Repos.Instructors;


namespace WebApplication2.Repos.Courses
{
    public class CourseRepo:ICourseRepo
    {
        ITIContext ITI;
        public CourseRepo(ITIContext _iTI)
        {
            ITI = _iTI;
        }   
        public List<Course> LoadCourses()
        {
            var Courses = ITI.Courses.ToList();
            return Courses;
        }
        public void AddCourse(CourseDetailsVM crsvm)
        {
            Course course = new Course()
            {
                Name = crsvm.Name,
                InstructorId = crsvm.InstructorId,
                Topic = crsvm.Topic
            };
            ITI.Courses.Add(course);
            ITI.SaveChanges();
        }
        public Course FindCourse(int id)
        {
            var course = ITI.Courses.Find(id);
            return course;
        }
        public void DeleteCourse(int id)
        {
            var course = FindCourse(id);
            ITI.Courses.Remove(course);
            ITI.SaveChanges();
        }
      
        public void EditCourse(CourseDetailsVM courseDetailsVM)
        {
            var course = FindCourse(courseDetailsVM.Id);
         
            if (course != null)
            {
                course.Name = courseDetailsVM.Name;
                course.Topic = courseDetailsVM.Topic;
                course.InstructorId = courseDetailsVM.InstructorId;
                ITI.SaveChanges();
            }
           

        }
        public List<Course> SelectedCrsesIDs(int[]courses)
        {
           var crsesids= ITI.Courses.Where(c => courses.Contains(c.Id)).ToList();
            return crsesids;
        }
       
    }
}
