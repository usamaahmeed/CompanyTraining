using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyTraining.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool isPass { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public ICollection<Question> Questions { get; set; } = null!;
        public ICollection<UserQuizAttempt> UserQuizAttempts { get; set; } =null!;

    }
}
