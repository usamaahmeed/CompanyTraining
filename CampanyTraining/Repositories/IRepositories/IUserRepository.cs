
namespace CompanyTraining.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        public IQueryable<ApplicationUser> GetCompaniesWithPackages();
    }
}
