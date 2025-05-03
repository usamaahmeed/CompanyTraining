namespace CompanyTraining.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionHeader { get; set; } = string.Empty;

        public double Mark { get; set; }
        public bool CorrectAnswer { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        public IEnumerable<UserAnswer> UserAnswers { get; set; } = null!;

    }
}
