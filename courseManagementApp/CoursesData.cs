using courseManagementApi.Models;

namespace courseManagementApi
{
    public class CoursesData
    {
        public List<CourseDto> Courses { get; set; }

        public static CoursesData CurrentCourses { get; } = new CoursesData();
        public CoursesData()
        {
            Courses = new List<CourseDto>()
            {
                new CourseDto() {Id = 1, Name = "Applied mathematics", Description ="This is Applied mathematics", Instructor = "Adams Smith", StartDate = DateOnly.Parse("2024-04-21") },
                new CourseDto() {Id = 2, Name="Statistics", Description = "This is Statistics", Instructor = "Willsmith", StartDate = DateOnly.Parse("12-04-2024"),}
            };
        }
    }
}
