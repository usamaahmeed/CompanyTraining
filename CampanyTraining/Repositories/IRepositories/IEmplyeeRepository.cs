namespace CompanyTraining.Repositories.IRepositories
{
    public interface IEmplyeeRepository : IRepository<ApplicationUser>
    {

        public Task<Quiz> GetQuizWithQuetionsWithChoices(int quizId,int courseId);
    }
}
