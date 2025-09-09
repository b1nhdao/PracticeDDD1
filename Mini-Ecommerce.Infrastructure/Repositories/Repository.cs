using Microsoft.EntityFrameworkCore;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public bool Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }
    }
}
