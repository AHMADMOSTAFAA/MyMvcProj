using System.ComponentModel.DataAnnotations;
using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Validation
{
    public class UniqueAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string name=value as string;
            var iticontext = validationContext.GetRequiredService<ITIContext>();
            CourseDetailsVM course=validationContext.ObjectInstance as CourseDetailsVM;
           
            
                var crs = iticontext.Courses.FirstOrDefault(c => c.Id != course.Id && c.Name == course.Name);
                if (crs == null)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("Name already exists");
            
        }
    }
}
