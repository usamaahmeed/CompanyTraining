
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CompanyTraining.Repositories.IRepositories
{
    public interface ICompanyRepository : IRepository<ApplicationUser>
    {

       public Task<ApplicationUser?> GetCompanyByUserId(string userId);

    }
}
