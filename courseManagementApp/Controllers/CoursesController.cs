using courseManagementApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace courseManagementApi.Controllers
{
    [ApiController]
    [Route("api/courses")]
     public class CoursesController : ControllerBase
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly CoursesData _coursesData;
        public CoursesController(ILogger<CoursesController> logger, CoursesData coursesData)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _coursesData = coursesData ?? throw new ArgumentNullException(nameof(coursesData));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCourses()
        {
            var courses = _coursesData.Courses;
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public ActionResult<CourseDto> GetCourse(int id)
        {
            try {

                var course = _coursesData.Courses.FirstOrDefault(course => course.Id == id);

                if (course == null)
                {
                    _logger.LogInformation($"Course with id {id} wasn't found when accessing the courses");
                    //without the arguments problem details service get returned
                    return NotFound();

                    /*
                     * argument added overrides the problem details service
                     * 
                     * return Ok($"Sorry the course with Id : {id}, does not exist");
                     */
                }
                return Ok(course);

            }catch(Exception ex)
            {
                _logger.LogCritical($"Exception while getting a course with Id {id}", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPost("create")]
        public ActionResult CreateCourse(CourseDto course)
        {
           if (!ModelState.IsValid){
                return BadRequest();
            }
            var courseId = _coursesData.Courses.Max(course => course.Id);
            var currentCourse = new CourseDto()
            {   
                Id = ++courseId,
                Name = course.Name,
                Description = course.Description,
                Instructor = course.Instructor,
                StartDate = course.StartDate,
            };

            _coursesData.Courses.Add(currentCourse);

            _logger.LogInformation($"A new course was created at { DateTime.Now.ToString()}");
            return Ok("Course created successfully.");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCourse(int id, CourseDto courseUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var course = _coursesData.Courses.FirstOrDefault(course => course.Id == id);

            if(course == null)
            {
                return NotFound(id);
            }

            course.Name = courseUpdate.Name;
            course.Description = courseUpdate.Description;
            course.Instructor = courseUpdate.Instructor;
            course.StartDate = courseUpdate.StartDate;

            _logger.LogInformation($"An update was done on course with Id {id} at {DateTime.Now.ToString()}");

            return Ok("Course update successful.");

        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCourse(int id)
        {
            var courseToDelete = _coursesData.Courses.FirstOrDefault(course => course.Id == id);

            if(courseToDelete == null)
            {
                return NotFound();
            }

            _coursesData.Courses.Remove(courseToDelete);

            _logger.LogInformation($"A delete action was done on course with Id {id} at {DateTime.Now.ToString()}");

            return Ok("Course deleted successfully.");

        }
    }
}
