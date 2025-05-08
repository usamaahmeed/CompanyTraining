using Microsoft.EntityFrameworkCore;

namespace CompanyTraining.Models
{
    [PrimaryKey(nameof(QuestionId),nameof(UserQuizAttemptId))]
    public class UserAnswer
    {

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int UserQuizAttemptId { get; set; }
        public UserQuizAttempt UserQuizAttempt { get; set; } = null!;
        public bool SelectedAnswer { get; set; }
        public bool IsCorrect { get; set; }

    }
}
