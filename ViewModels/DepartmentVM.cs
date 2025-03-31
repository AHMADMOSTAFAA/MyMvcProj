using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }  // Changed to PascalCase for consistency
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }

        public List<Instructor> AllInstructors;

        public List<int> InstructorsIDs { get; set; } = new List<int>();

        public int[] StudentsIDs;
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
