namespace CompanyTraining.Repositories
{
    public class UserRepository : Repository<ApplicationCompany>, IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
