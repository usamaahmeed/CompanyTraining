using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class QuestionRequest
    {
        [Required]
        public string QuestionHeader { get; set; } = string.Empty;
        [Required]
        public double Mark { get; set; }
        [Required]
        public bool CorrectAnswer { get; set; }
        [Required]
        public int QuizId { get; set; }

    }
}
