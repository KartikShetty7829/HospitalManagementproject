using Auth_Service.Data;

namespace Auth_Service.Interfaces
{
    public interface IGenericRepository<TContext, TEntity>
        where TContext : AuthDbContext
        where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
    }
}
