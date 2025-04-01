
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Context
{
    public class ITIContext : IdentityDbContext<ApplicationUser> // IdentityDbContext includes Identity tables
    {
        public ITIContext(DbContextOptions<ITIContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Course_Stds> Course_Stds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ✅ Fix: Configure Identity tables properly

            modelBuilder.Entity<Course_Stds>().HasKey(cs => new { cs.CourseId, cs.StudentId });

            modelBuilder.Entity<Course_Stds>()
                .HasOne(c => c.Course)
                .WithMany(cs => cs.Course_Stds)
                .HasForeignKey(c => c.CourseId);

            modelBuilder.Entity<Course_Stds>()
                .HasOne(s => s.Student)
                .WithMany(cs => cs.Course_Stds)
                .HasForeignKey(s => s.StudentId);

            modelBuilder.Entity<Course>()
                .HasOne(i => i.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Student>()
    .HasOne(s => s.User)
    .WithOne(u => u.Student)
    .OnDelete(DeleteBehavior.Cascade);  // Or Restrict

            modelBuilder.Entity<Instructor>()
    .HasOne(s => s.User)
    .WithOne(u => u.Instructor)
    .OnDelete(DeleteBehavior.Cascade);
        }

    }
}














//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using WebApplication2.Models;

//namespace WebApplication2.Context
//{
//    public class ITIContext:IdentityDbContext<ApplicationUser>//old was dbcontext only
//    {
//        public ITIContext(DbContextOptions options) : base(options)
//        {
//        }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseSqlServer("Server=localhost;Database=ITIProj;Trusted_Connection=True;Encrypt=False;");
//        }

//        public DbSet<Student> Students { get; set; }
//        public DbSet<Instructor> Instructors { get; set; }
//        public DbSet<Department> Departments { get; set; }
//        public DbSet<Course> Courses { get; set; }
//        public DbSet<Course_Stds> Course_Stds { get; set; }
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Course_Stds>().HasKey(cs => new { cs.CourseId, cs.StudentId });
//            modelBuilder.Entity<Course_Stds>().HasOne(c=>c.Course).WithMany(cs => cs.Course_Stds)
//                .HasForeignKey(c => c.CourseId);

//            modelBuilder.Entity<Course_Stds>().HasOne(s => s.Student).
//                WithMany(cs => cs.Course_Stds).HasForeignKey(s => s.StudentId);

//            modelBuilder.Entity<Course>().HasOne(i=>i.Instructor).WithMany(i=>i.Courses).HasForeignKey(c=>c.InstructorId)
//                .OnDelete(DeleteBehavior.SetNull);
//        }
//    }
//}
