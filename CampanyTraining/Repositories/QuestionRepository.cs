


using Microsoft.EntityFrameworkCore;

namespace CompanyTraining.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Question>> GetHardQuestions(int expectedHardQuestions)
        {
            var hardQuestions = await _context.Question.Where(e=>e.QuestionLevel == enQuestionLevel.Simple && e.QuizId ==null).OrderBy(e=>Guid.NewGuid()).Take(expectedHardQuestions).ToListAsync();
            return hardQuestions;
        }

        public async Task<IEnumerable<Question>> GetMediumQuestions(int expectedMediumQuestions)
        {
            var mediumQuestions = await _context.Question.Where(e => e.QuestionLevel == enQuestionLevel.Medium && e.QuizId == null).OrderBy(e => Guid.NewGuid()).Take(expectedMediumQuestions).ToListAsync();
            return mediumQuestions;
        }

        public async Task<IEnumerable<Question>> GetSimpleQuestions(int expectedSimpleQuestions)
        {
            var simpleQuestions = await _context.Question.Where(e => e.QuestionLevel == enQuestionLevel.Simple && e.QuizId == null).OrderBy(e => Guid.NewGuid()).Take(expectedSimpleQuestions).ToListAsync();
            return simpleQuestions;
        }
    }
}
