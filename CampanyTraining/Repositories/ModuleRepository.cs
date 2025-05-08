
namespace CompanyTraining.Repositories
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        private readonly ApplicationDbContext _context;

        public ModuleRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
