namespace CompanyTraining.Repositories.IRepositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        public  Task<IEnumerable<Question>> GetSimpleQuestions(int expectedSimpleQuestions);
        public Task<IEnumerable<Question>> GetMediumQuestions(int expectedMediumQuestions);
        public Task<IEnumerable<Question>> GetHardQuestions(int expectedHardQuestions);
    }
}
