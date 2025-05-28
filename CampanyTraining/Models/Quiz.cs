using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyTraining.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool isPass { get; set; }
        public bool IsGenerated { get; set; }
        public int NumberOfQuestions { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } =new List<Question>();
        public ICollection<UserQuizAttempt> UserQuizAttempts { get; set; } =null!;

    }
}
