using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class LessonRequest
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]

        public IFormFile Video { get; set; } = null!;

    }
}
