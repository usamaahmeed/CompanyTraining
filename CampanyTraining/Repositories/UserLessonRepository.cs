
namespace CompanyTraining.Repositories
{
    public class UserLessonRepository : Repository<UserLesson>, IUserLessonRepository
    {
        private readonly ApplicationDbContext context;

        public UserLessonRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
