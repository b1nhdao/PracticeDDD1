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

        public WishlistProduct AddWishlishProduct(WishlistProduct product)
        {
            _context.WishlistProducts.Add(product);
            return product;
        }
    }
}
