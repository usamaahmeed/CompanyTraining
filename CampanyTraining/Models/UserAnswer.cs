using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CompanyTraining.Models
{
    [PrimaryKey(nameof(QuestionId),nameof(UserQuizAttemptId))]
    public class UserAnswer
    {

        public int QuestionId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public Question Question { get; set; } = null!;
        public int UserQuizAttemptId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public UserQuizAttempt UserQuizAttempt { get; set; } = null!;
        public int SelectedChoiceId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public Choice SelectedChoice { get; set; } = null!;
        public bool IsCorrect { get; set; }

    }
}
