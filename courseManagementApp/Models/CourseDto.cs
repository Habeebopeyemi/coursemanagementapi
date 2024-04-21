using System.ComponentModel.DataAnnotations;

namespace courseManagementApi.Models
{
    public class CourseDto
    {

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Instructor { get; set; } = string.Empty;

        public DateOnly StartDate { get; set;}
    }
}
