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

    }
}
