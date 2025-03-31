using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public int Age { get; set; }
        public string? IMG { get; set; }
      

        // Foreign Key
        public int? DepartmentId { get; set; }  // Renamed to PascalCase
        public Department? Department { get; set; }  // Nullable to match FK

        public List<Course_Stds> Course_Stds { get; set; } = new List<Course_Stds>();
    }
}
