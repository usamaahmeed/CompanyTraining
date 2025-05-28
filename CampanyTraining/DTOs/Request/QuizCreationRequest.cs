using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class QuizCreationRequest
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        public int CourseId { get; set; }

        public bool IsGenerated { get; set; } = false;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of questions must be at least 1.")]
        public int NumberOfQuestions { get; set; }

    }
}
