using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository, IRepository<Customer>
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<List<Customer>?> GetCustomersWishlishedProductAsync(Guid productId)
        {
            return await _context.Customers
                .Where(c => c.WishlistProducts
                .Any(w => w.ProductId == productId))
                .ToListAsync();
        }

        public async Task<(List<Customer>, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, bool isDescending)
        {
            var query = _context.Customers.AsNoTracking().Include(c => c.WishlistProducts).AsQueryable();

            var totalCount = await query.CountAsync();

            if (isDescending)
            {
                query = query.OrderByDescending(c => c.Name);
            }
            else
            {
                query = query.OrderBy(c => c.Name);
            }

            var items = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public WishlistProduct AddWishlishProduct(WishlistProduct product)
        {
            _context.WishlistProducts.Add(product);
            return product;
        }
    }
}
