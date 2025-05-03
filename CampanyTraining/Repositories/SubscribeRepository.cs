
namespace CompanyTraining.Repositories
{
    public class SubscribeRepository : Repository<Subscribe>, ISubscribeRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscribeRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
