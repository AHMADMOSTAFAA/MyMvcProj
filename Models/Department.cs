namespace WebApplication2.Models
{
    public class Department
    {
        public int Id { get; set; }  // Changed to PascalCase for consistency
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }

        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
        public List<Student> Students { get; set; } = new List<Student>();


    }
}
