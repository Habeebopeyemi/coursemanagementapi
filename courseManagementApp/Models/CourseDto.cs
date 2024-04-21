using System.ComponentModel.DataAnnotations;

namespace courseManagementApi.Models
{
    public class CourseDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage ="Description cannot be empty.")]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage ="Kindly add instructor name.")]
        [MaxLength(50)]
        public string Instructor { get; set; }

        public DateOnly StartDate { get; set;}
    }
}
