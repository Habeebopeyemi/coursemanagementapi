using courseManagementApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace courseManagementApi.DBContexts
{
    public class CourseContext : DbContext
    {
        public CourseContext(DbContextOptions<CourseContext> options) : base(options)
        {
            
        }
        public DbSet<Course> Courses { get; set; }

        //database seeding
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
                new Course() { 
                    Id = 1, 
                    Name = "Applied mathematics", 
                    Description = "This is Applied mathematics", 
                    Instructor = "Adams Smith", 
                    StartDate = DateOnly.Parse("2024-04-21") },
                new Course() { 
                    Id = 2, 
                    Name = "Statistics", 
                    Description = "This is Statistics", 
                    Instructor = "Willsmith", 
                    StartDate = DateOnly.Parse("12-04-2024"), }


);
            base.OnModelCreating(modelBuilder);
        }

    }
}
