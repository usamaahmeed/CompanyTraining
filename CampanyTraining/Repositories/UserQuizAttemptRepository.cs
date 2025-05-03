
namespace CompanyTraining.Repositories
{
    public class UserQuizAttemptRepository : Repository<UserQuizAttempt>, IUserQuizAttemptRepository
    {
        private readonly ApplicationDbContext _context;

        public UserQuizAttemptRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
