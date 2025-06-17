namespace CompanyTraining.DTOs.Response
{
    public class GetQuestionResponseDto
    {
        public int Id { get; set; }
        public string QuestionHeader { get; set; } = string.Empty;
        public double Mark { get; set; }
        public ICollection<GetChoiceTextResponse> Choices { get; set; } = null!;
    }
}
