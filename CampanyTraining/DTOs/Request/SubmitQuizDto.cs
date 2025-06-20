namespace CompanyTraining.DTOs.Request
{
    public class SubmitQuizDto
    {
        public int QuizId { get; set; }
        public ICollection<AnswerQuestionDto> Answers { get; set; } = new List<AnswerQuestionDto>();
    }

}
