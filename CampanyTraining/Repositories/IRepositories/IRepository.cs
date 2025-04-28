namespace CampanyTraining.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        T? GetOne(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> EditAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> CommitAsync(CancellationToken cancellationToken = default);
    }
}
