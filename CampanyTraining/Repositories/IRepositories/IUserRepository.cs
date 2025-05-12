
namespace CompanyTraining.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        public IQueryable<ApplicationUser> GetCompaniesWithPackages();
        public void RemoveRange(IEnumerable<ApplicationUser> users);
    }
}
