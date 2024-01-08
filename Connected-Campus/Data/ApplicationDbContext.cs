using Connected_Campus.Models;
using Microsoft.EntityFrameworkCore;

namespace Connected_Campus.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<RegisteredCourse> RegisteredCourses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisteredCourse>()
                .HasKey(rc => rc.StudentId); // Define StudentId as the primary key

            // Other configurations...

            base.OnModelCreating(modelBuilder);
        }
    }
}
