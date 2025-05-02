namespace CompanyTraining.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public double Mark { get; set; }
        public string QuestionHeader { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

    }
}
