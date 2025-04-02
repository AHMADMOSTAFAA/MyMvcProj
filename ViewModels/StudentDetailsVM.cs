using WebApplication2.Context;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class StudentDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public int Age { get; set; }
        public string? IMG { get; set; }
        public List<string>? coursenames { get; set; }
        // Foreign Key
        public int? DepartmentId { get; set; }
        public string? UserId { get; set; } 
        public List<Course_Stds> Course_Stds { get; set; } = new List<Course_Stds>();
        public List<Department>? departments { get; set; }
        public List<int>? SelectedCrsIDs { get; set; }
        public List<Course>? courses { get; set; }
        
        public string color { get; set; } = "red";
        public IFormFile? IMGFile { get; set; } // Used for file upload
    }
}
