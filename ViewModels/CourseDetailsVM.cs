using System.ComponentModel.DataAnnotations;
using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.Validation;

namespace WebApplication2.ViewModels
{
    public class CourseDetailsVM
    {
        public int Id { get; set; }
        [MinLength(1, ErrorMessage = "Must be atleast 1 char")]
        [MaxLength(30, ErrorMessage = "The course name must not exceed 30 characters")]
        [Unique]
        public string Name { get; set; }
        [MinLength(2, ErrorMessage = "Must be atleast 2 char")]
        [MaxLength(30, ErrorMessage = "The Topic name must not exceed 30 characters")]
        public string Topic { get; set; }

        // Foreign Key
        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }  // Nullable to match FK

        public List<Course_Stds> Course_Stds { get; set; } = new List<Course_Stds>();
        public List<Instructor>?instructors { get; set; }
    }
}
