
using Microsoft.EntityFrameworkCore;

namespace CompanyTraining.Repositories
{
    public class UserRepository : Repository<ApplicationCompany>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
        public IQueryable<ApplicationCompany> GetCompaniesWithPackages()
        {
            var companies = _dbContext.ApplicationCompanies.Include(e => e.Subscribes).ThenInclude(e=>e.Package);
            return companies;
        }
    }
}
