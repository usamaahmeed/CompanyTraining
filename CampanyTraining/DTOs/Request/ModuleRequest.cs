using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class ModuleRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int CourseId { get; set; }
    }
}
