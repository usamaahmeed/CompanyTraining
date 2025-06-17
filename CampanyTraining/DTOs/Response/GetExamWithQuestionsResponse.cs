namespace CompanyTraining.DTOs.Response
{
    public class GetExamWithQuestionsResponse
    {
        public string Title { get; set; } = string.Empty;
        public int NumberOfQuestions { get; set; }
        public ICollection<GetQuestionResponseDto> Questions { get; set; }
    }
}
