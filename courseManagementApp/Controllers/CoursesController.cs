using courseManagementApi.Entities;
using courseManagementApi.Models;
using courseManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace courseManagementApi.Controllers
{
    [ApiController]
    [Route("api/courses")]
    [Authorize]
     public class CoursesController : ControllerBase
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly ICourseRepository _courseRepository;
        public CoursesController(ILogger<CoursesController> logger, ICourseRepository courseRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            try { 
                var courses = await _courseRepository.GetCoursesAsync();

                var results = new List<CourseDto>();

                foreach (var course in courses)
                {
                    results.Add(new CourseDto { 
                        Id = course.Id, 
                        Name = course.Name, 
                        Description = course.Description,
                        Instructor = course.Instructor,
                        StartDate = course.StartDate,
                    });
                }

                return Ok(results);
             }catch(Exception ex)
            {
                _logger.LogInformation($"Exception occur while creating a new course: {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            try {

                var course = await _courseRepository.GetCourseAsync(id);

                var result = new CourseDto()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    Instructor = course.Instructor,
                    StartDate = course.StartDate,
                };

                return Ok(result);

            }catch(Exception ex)
            {
                _logger.LogCritical($"Exception while getting a course with Id {id}", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateCourse(Course course)
        {
           if (!ModelState.IsValid){
                throw new Exception($"One or more validation failed, Kindly check the data provided");
            }
            try
            {
                bool IsCreated = await _courseRepository.CreateCourseAsync(course);

                if (IsCreated)
                {
                    _logger.LogInformation($"A new course was created at {DateTime.Now.ToString()}");
                }
                else
                {
                    _logger.LogInformation($"An error occured while creating a course at { DateTime.Now.ToString()}");
                }

            }catch(Exception ex )
            {
                _logger.LogInformation($"Exception occured while creating a new course on {DateTime.Now.ToString()}");
                return StatusCode(500, ex.Message);
            }

            return Ok("Course created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, Course courseUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                bool IsUpdated = await _courseRepository.UpdateCourseAsync(id, courseUpdate);

                if (!IsUpdated)
                {
                    return NotFound(id);
                }

                _logger.LogInformation($"An update was done on course with Id {id} at {DateTime.Now.ToString()}");
            }catch(Exception ex )
            {
                _logger.LogInformation($"Exception occured while updating the course with Id {id} on {DateTime.Now.ToString()}");
                return StatusCode(500, ex.Message);
            }

            return Ok("Course update successful.");

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            try
            {
                bool IsDeleted = await _courseRepository.DeleteCourseAsync(id);

                if(!IsDeleted)
                {
                    return NotFound();
                }

            }catch(Exception ex )
            {
                _logger.LogInformation($"Exception occured while deleting a course with Id {id} on {DateTime.Now.ToString()}");
                return StatusCode(500, ex.Message);
            }

            _logger.LogInformation($"A delete action was done on course with Id {id} at {DateTime.Now.ToString()}");
            return Ok("Course deleted successfully.");

        }
    }
}
