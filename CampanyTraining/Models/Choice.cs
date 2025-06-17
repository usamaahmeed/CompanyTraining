namespace CompanyTraining.Models
{
    public class Choice
    {
        public int Id { get; set; }
        public string ChoiceText { get; set; } = null!;
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public Question Question { get; set; } = null!;
    }
}
