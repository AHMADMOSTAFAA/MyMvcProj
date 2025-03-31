using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class InstructorDetailsVM
    {
        public int Id { get; set; }
     public string FullName { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public int Age { get; set; }
        public string? IMG { get; set; }

        
        public int? DepartmentId { get; set; }  
       
        public List<string> Courses { get; set; } = new List<string>();
    }
}
