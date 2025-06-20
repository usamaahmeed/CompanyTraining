namespace CompanyTraining.Models
{
    public class UserQuizAttempt
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddHours(2);
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public int Score { get; set; }
        public bool isPass { get; set; }

        public bool IsSubmitted { get; set; } = false;
        public ICollection<UserAnswer> UserAnswers { get; set; } = null!;
    }
}
