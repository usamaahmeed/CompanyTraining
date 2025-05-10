using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class CourseRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public bool isActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public int CategoryId { get; set; }
        //[Required]
        //public int QuizId { get; set; }
        public ICollection<int> ModuleIds { get; set; } = new List<int>();
    }
}
