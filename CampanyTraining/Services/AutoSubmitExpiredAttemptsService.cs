using Microsoft.EntityFrameworkCore;

namespace CompanyTraining.Services
{
    public class AutoSubmitExpiredAttemptsService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AutoSubmitExpiredAttemptsService(ApplicationDbContext applicationDbContext) 
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task  RunTask()
        {
            var now = DateTime.Now;

            var pendingAttempts = await _applicationDbContext.UserQuizAttempts.Include(e => e.UserAnswers)
                .ThenInclude(e => e.Question).Include(e => e.Quiz).Where(e => !e.IsSubmitted && e.EndDate <= e.StartDate).ToListAsync();

            foreach (var attempt in pendingAttempts)
            {
                double obtainedScore = 0;
                double totalScore = attempt.Quiz.Questions.Sum(q => q.Mark);

                foreach (var ua in attempt.UserAnswers)
                {
                    var selectedChoice = await _applicationDbContext.Choices.FindAsync(ua.SelectedChoiceId);
                    if (selectedChoice != null && selectedChoice.IsCorrect)
                        obtainedScore += ua.Question.Mark;
                }

                attempt.Score = (int)obtainedScore;
                attempt.isPass = obtainedScore >= totalScore * 0.5;
                attempt.IsSubmitted = true;

            }
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
