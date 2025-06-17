using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Response
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public string QuestionHeader { get; set; } = string.Empty;

        public enQuestionLevel QuestionLevel { get; set; }
        public double Mark { get; set; }
    }
}
