namespace CompanyTraining.DTOs.Response
{
    public class GetChoicesForQuestionResponse
    {
        public int Id { get; set; }
        public string ChoiceText { get; set; } = null!;
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
