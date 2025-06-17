
using Microsoft.EntityFrameworkCore;

namespace CompanyTraining.Repositories
{
    public class EmplyeeRepository : Repository<ApplicationUser>, IEmplyeeRepository
    {
        private readonly ApplicationDbContext context;

        public EmplyeeRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Quiz> GetQuizWithQuetionsWithChoices(int quizId,int courseId)
        {
            var quiz = await context.Quiz.Where(e=>e.Id==quizId && e.CourseId==courseId)
                .Include(e=>e.Questions).
                ThenInclude(e=>e.Choices).
                FirstOrDefaultAsync();
            return quiz;
        }
    }
}
