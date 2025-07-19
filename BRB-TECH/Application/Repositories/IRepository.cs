namespace Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);        
        Task<T> GetAsync(long id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task RemoveAsync(T entity);
    }
}
