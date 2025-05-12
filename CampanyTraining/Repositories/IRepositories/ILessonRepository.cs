namespace CompanyTraining.Repositories.IRepositories
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        public void RemoveRange(IEnumerable<Lesson> lessons);

    }
}
