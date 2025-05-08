
namespace CompanyTraining.Repositories
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        private readonly ApplicationDbContext _context;

        public QuizRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
