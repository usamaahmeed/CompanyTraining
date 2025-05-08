namespace CompanyTraining.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
