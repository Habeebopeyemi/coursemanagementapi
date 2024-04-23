using courseManagementApi.DBContexts;
using courseManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace courseManagementApi.Services
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseContext _context;
        public CourseRepository(CourseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> CreateCourseAsync(Course course)
        {
            _context.Courses.Add(course);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            bool IsCourseExisting = await IsExist(courseId);

            if (!IsCourseExisting)
            {
                throw new KeyNotFoundException($"Error occurs while deleting because course with Id {courseId} does not exist");
            }

            var currentCourse = await _context.Courses.FirstOrDefaultAsync(course => course.Id == courseId);

            _context.Courses.Remove(currentCourse);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Course?> GetCourseAsync(int courseId)
        {
            bool IsCourseExisting = await IsExist(courseId);

            if (!IsCourseExisting)
            {
                throw new KeyNotFoundException($"The course with Id {courseId} does not exist {DateTime.Now.ToString()}");
            }

            return await _context.Courses.Where(course => course.Id == courseId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
           return await _context.Courses.OrderBy(course => course.Name).ToListAsync();
        }

        public async Task<bool> UpdateCourseAsync(int courseId, Course course)
        {
            bool IsCourseExisting = await IsExist(courseId);

            if (!IsCourseExisting)
            {
                throw new KeyNotFoundException($"Exception occurs while updating course with Id {courseId} at {DateTime.Now.ToString()}");
            }

            var currentCourse = await _context.Courses.FirstOrDefaultAsync(course => course.Id == courseId);


            currentCourse.Name = course.Name;
            currentCourse.Description = course.Description;
            currentCourse.Instructor = course.Instructor;
            currentCourse.StartDate = course.StartDate;

            await _context.SaveChangesAsync();

            return true;

        }

        private async Task<bool> IsExist(int courseId)
        {
            var currentCourse = await _context.Courses.FirstOrDefaultAsync(course => course.Id == courseId);

            if (currentCourse == null)
            {
                return false;
            }
            return true;
        }
    }
}
