namespace CompanyTraining.DTOs.Response
{
    public class SubmitExamResponse
    {
        public double TotalScore { get; set; }
        public double ObtainedScore { get; set; }
        public bool Passed { get; set; }
    }
}
