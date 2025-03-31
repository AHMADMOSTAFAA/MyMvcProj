using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Repos.Departments
{
    public interface IDeptRepo
    {
        public List<Department> LoadDeparments();
        public List<Department> LoadDeparmentsWithInstructors();
        public void Add(DepartmentVM departmentVM);
        public void Delete(Department department);
        public Department FindDept(int id);


    }
}
