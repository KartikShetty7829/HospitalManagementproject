using Auth_Service.Data;
using Auth_Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth_Service.Repositories
{
    public class GenericRepository<TContext, TEntity> : IGenericRepository<TContext, TEntity>
        where TContext : AuthDbContext
        where TEntity : class
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);   // ✅ using _dbSet directly
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);           // ✅ using _dbSet directly
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);       // ✅ using _dbSet directly
                await _context.SaveChangesAsync();
            }
        }
    }

}
