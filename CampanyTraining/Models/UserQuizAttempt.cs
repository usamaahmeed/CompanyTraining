namespace CompanyTraining.Models
{
    public class UserQuizAttempt
    {
        public int Id { get; set; }

        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        public int TotalQuestions { get; set; }

        public int Score { get; set; }
        public bool isPass { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; } = null!;
    }
}
