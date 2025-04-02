using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.ViewModels;
namespace WebApplication2.Repos.Students
{
    public class StudentRepo : IStudentRepo
    {
        ITIContext ITI;
        public StudentRepo(ITIContext _ITIContext) {
            ITI = _ITIContext;
        }
        public List<Student> LoadStdWithCrses()
        {
            var StudentsWithDeptAndCourses = ITI.Students.Include(s => s.Department).Include(s => s.Course_Stds).ThenInclude(c => c.Course).ToList();
            return StudentsWithDeptAndCourses;
        }
        public List<Student> LoadStdWithCrsesByDeptId(int id)
        {
            var StudentsWithCourses = ITI.Students.Where(s => s.DepartmentId == id).Include(s => s.Department).Include(s => s.Course_Stds).ThenInclude(c => c.Course).ToList();
            return StudentsWithCourses;
        }
        public Student LoadStdWithHisCourse(int id)
        {
            var StudentWithHisCourses = ITI.Students.Include(s => s.Course_Stds).ThenInclude(C => C.Course).FirstOrDefault(s => s.Id == id);
            return StudentWithHisCourses;
        }
       
        public Student Student(int id)
        {
            var student = ITI.Students.FirstOrDefault(s => s.Id == id);
            return student;
        }
        public void Add(StudentDetailsVM stdvm)
        {
            Student newStudent = new Student
            {
                Name = stdvm.Name,
                Age = stdvm.Age,
                Email = stdvm.Email,
                DepartmentId = stdvm.DepartmentId
            };

            if (stdvm.IMGFile != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(stdvm.IMGFile.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    stdvm.IMGFile.CopyTo(stream);
                }

                newStudent.IMG = "/Images/" + fileName;
            }

            ITI.Students.Add(newStudent);
            ITI.SaveChanges();
        }
        public void Edit(StudentDetailsVM stdvm)
        {
            var updatedstd= LoadStdWithHisCourse(stdvm.Id);
            if (updatedstd == null)
                return;

            updatedstd.Name = stdvm.Name;
            updatedstd.Age = stdvm.Age;
            updatedstd.DepartmentId = stdvm.DepartmentId;
            updatedstd.Email = stdvm.Email;
            updatedstd.UserId = stdvm.UserId;

            if (stdvm.IMGFile != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(stdvm.IMGFile.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    stdvm.IMGFile.CopyTo(stream);
                }

                updatedstd.IMG = "/Images/" + fileName;
            }

            if (stdvm.SelectedCrsIDs != null)
            {
                updatedstd.Course_Stds = stdvm.SelectedCrsIDs
                    .Select(courseId => new Course_Stds { StudentId = updatedstd.Id, CourseId = courseId })
                    .ToList();
            }

            ITI.SaveChanges();
        }
        public Student LastStudent()
        {
            var lastStudent = ITI.Students.OrderBy(s => s.Id).LastOrDefault();
            return lastStudent;
        }

        public void Delete(Student student)
        {
            ITI.Students.Remove(student);
            ITI.SaveChanges();
        }

    }
}
