namespace CompanyTraining.Models
{
    public enum enQuestionLevel
    {
        Simple =1,
        Medium=2,
        Hard =3
    }
    public class Question
    {
        public int Id { get; set; }
        public string QuestionHeader { get; set; } = string.Empty;
        public double Mark { get; set; }
        public enQuestionLevel QuestionLevel { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser Company { get; set; } = null!;

        public int? QuizId { get; set; }
        public Quiz? Quiz { get; set; } 
        public ICollection<Choice> Choices { get; set; } = null!;
        public IEnumerable<UserAnswer> UserAnswers { get; set; } = null!;

    }
}
