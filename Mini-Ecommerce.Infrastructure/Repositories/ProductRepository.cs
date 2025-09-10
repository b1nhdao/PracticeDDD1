using Microsoft.EntityFrameworkCore;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository, IRepository<Product>
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<(List<Product>, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize)
        {
            var query = _context.Set<Product>().AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .AsNoTracking()
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
