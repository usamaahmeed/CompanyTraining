
namespace CompanyTraining.Repositories
{
    public class UserCourseRepository : Repository<UserCourse>, IUserCourseRepository
    {
        private readonly ApplicationDbContext _context;

        public UserCourseRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
