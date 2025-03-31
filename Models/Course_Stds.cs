namespace WebApplication2.Models
{
    public class Course_Stds
    {
    

        // Foreign Keys
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int Degree { get; set; }

    }
}
