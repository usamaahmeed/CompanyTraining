
namespace CompanyTraining.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<ApplicationCompany>
    {
        public IQueryable<ApplicationCompany> GetCompaniesWithPackages();
    }
}
