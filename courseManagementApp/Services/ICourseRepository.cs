using courseManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace courseManagementApi.Services
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetCoursesAsync();

        Task<Course?> GetCourseAsync(int  courseId);

        Task<bool> CreateCourseAsync(Course course);

        Task<bool> UpdateCourseAsync(int courseId, Course course);

        Task<bool> DeleteCourseAsync(int courseId);
    }
}
