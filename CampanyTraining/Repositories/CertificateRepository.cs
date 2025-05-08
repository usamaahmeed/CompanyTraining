
namespace CompanyTraining.Repositories
{
    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        private readonly ApplicationDbContext _context;

        public CertificateRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
