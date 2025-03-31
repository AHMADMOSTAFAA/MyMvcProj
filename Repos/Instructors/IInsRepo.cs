using WebApplication2.Models;

namespace WebApplication2.Repos.Instructors
{
    public interface IInsRepo
    {
        public List<Instructor> LoadInstructors();
        public bool InsNameInDept(string FName, int DepartmentId);
        public Instructor InsWithHisCourses(int id);
        public void Add(Instructor ins);
        public Instructor FindInstructor(int id);
        public void UpdateInstructor(Instructor instructor);
        public void Delete(int id);
    }
}
