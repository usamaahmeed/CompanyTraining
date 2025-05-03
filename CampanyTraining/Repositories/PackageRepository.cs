﻿
namespace CompanyTraining.Repositories
{
    public class PackageRepository : Repository<Package>, IPackageRepository
    {
        private readonly ApplicationDbContext _context;

        public PackageRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
