
namespace CompanyTraining.Repositories
{
    public class UserAnswerRepository : Repository<UserAnswer>, IUserAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public UserAnswerRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
