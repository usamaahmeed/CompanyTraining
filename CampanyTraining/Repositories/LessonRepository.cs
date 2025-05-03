
namespace CompanyTraining.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
