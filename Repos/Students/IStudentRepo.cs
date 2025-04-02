using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Repos.Students
{
    public interface IStudentRepo
    {


        public List<Student> LoadStdWithCrses();

        public List<Student> LoadStdWithCrsesByDeptId(int id);

        public Student LoadStdWithHisCourse(int id);



        public Student Student(int id);

        public void Add(StudentDetailsVM student);

        public void Edit(StudentDetailsVM student);

        public void Delete(Student student);

        public Student LastStudent();


}
}
