using Auth_Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth_Service.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
         where TContext : DbContext
    {
        private readonly TContext _context;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        

        public async Task SaveChangesAsync()
        {
            dynamic dbcontext = _context;
            await dbcontext.SaveChangesAsync();
        }


        //public void Dispose()
        //{
        //    _context.Dispose();
        //}
    }
}
