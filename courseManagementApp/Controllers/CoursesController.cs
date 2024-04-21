using courseManagementApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace courseManagementApi.Controllers
{
    [ApiController]
    [Route("api/courses")]
     public class CoursesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCourses()
        {
            var courses = CoursesData.CurrentCourses.Courses;
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public ActionResult<CourseDto> GetCourse(int id)
        {
            var course = CoursesData.CurrentCourses.Courses.FirstOrDefault(course => course.Id == id);

            if (course == null)
            {
                //without the arguments problem details service get returned
                return NotFound();

                /*
                 * argument added overrides the problem details service
                 * 
                 * return Ok($"Sorry the course with Id : {id}, does not exist");
                 */
            }

            return Ok(course);
        }

        [HttpPost("create")]
        public ActionResult CreateCourse(CourseDto course)
        {
           if (!ModelState.IsValid){
                return BadRequest();
            }
            var courseId = CoursesData.CurrentCourses.Courses.Max(course => course.Id);
            var currentCourse = new CourseDto()
            {
                Id = ++courseId,
                Name = course.Name,
                Description = course.Description,
                Instructor = course.Instructor,
                StartDate = course.StartDate,
            };

            CoursesData.CurrentCourses.Courses.Add(currentCourse);

            return Ok("Course created successfully.");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCourse(int id, CourseDto courseUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var course = CoursesData.CurrentCourses.Courses.FirstOrDefault(course => course.Id == id);

            if(course == null)
            {
                return NotFound(id);
            }

            course.Name = courseUpdate.Name;
            course.Description = courseUpdate.Description;
            course.Instructor = courseUpdate.Instructor;
            course.StartDate = courseUpdate.StartDate;

            return Ok("Course update successful.");

        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCourse(int id)
        {
            var courseToDelete = CoursesData.CurrentCourses.Courses.FirstOrDefault(course => course.Id == id);

            if(courseToDelete == null)
            {
                return NotFound();
            }

            CoursesData.CurrentCourses.Courses.Remove(courseToDelete);

            return Ok("Course deleted successfully.");

        }
    }
}
