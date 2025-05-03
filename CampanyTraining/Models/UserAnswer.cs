namespace CompanyTraining.Models
{
    public class UserAnswer
    {

        public int UserQuizId { get; set; }
        public int QuizAttemptId { get; set; }
        public string SelectedAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public UserQuizAttempt UserQuiz { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }


    }
}
