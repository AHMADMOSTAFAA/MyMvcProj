using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Repos.Departments
{
    public class DeptRepo:IDeptRepo
    {
        ITIContext ITI;
        public DeptRepo(ITIContext _iTI) { 
            ITI= _iTI;  
        }

        public List<Department> LoadDeparments()
        {
            var Departments = ITI.Departments.ToList();
            return Departments;
        }
        public List<Department> LoadDeparmentsWithInstructors()
        {
            var Departments = ITI.Departments.Include(d=>d.Instructors).ToList();
            return Departments;
        }
        public void Add(DepartmentVM departmentVM)
        {
            Department department = new Department() { 
            Name = departmentVM.Name,
            Location = departmentVM.Location,
            Description = departmentVM.Description,
            Instructors = ITI.Instructors.Where(i => departmentVM.InstructorsIDs.Contains(i.Id)).ToList(),
              
            };
            ITI.Departments.Add(department);
            ITI.SaveChanges();
        }
        public void Delete(Department department)
        {
          
            ITI.Departments.Remove(department);
            ITI.SaveChanges();
        }

        public Department FindDept(int id)
        {
            var dept= ITI.Departments.FirstOrDefault(i => i.Id == id);

            return dept;
        }
    }
}
