namespace CompanyTraining.Models
{
    public class UserAnswer
    {

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int UserQuizAttemptId { get; set; }
        public UserQuizAttempt UserQuizAttempt { get; set; } = null!;
        public bool SelectedAnswer { get; set; }
        public bool IsCorrect { get; set; }

    }
}
