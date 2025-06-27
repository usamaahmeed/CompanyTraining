
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CompanyTraining.Repositories
{
    public class CompanyRepository : Repository<ApplicationUser>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<ApplicationUser?> GetCompanyByUserId(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.Company;
        }
    }
}
