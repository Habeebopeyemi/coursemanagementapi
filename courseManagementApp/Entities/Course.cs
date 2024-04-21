using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace courseManagementApi.Entities
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description cannot be empty.")]
        [MaxLength(200)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Kindly add instructor name.")]
        [MaxLength(50)]
        public string Instructor { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }
    }
}
