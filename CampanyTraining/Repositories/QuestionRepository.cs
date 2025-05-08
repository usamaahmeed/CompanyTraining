
namespace CompanyTraining.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
