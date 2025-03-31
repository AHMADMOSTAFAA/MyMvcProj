using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        [Remote("UniqueName", "Instructor", AdditionalFields = "DepartmentId", ErrorMessage = "Firstname must be unique in the department")]
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public int Age { get; set; }
        public string? IMG { get; set; }

        // Foreign Key
        public int? DepartmentId { get; set; }  // Renamed to PascalCase
        public  Department? Department { get; set; }  // Nullable to match FK

        public virtual List<Course> Courses { get; set; } = new List<Course>();
    }
}
