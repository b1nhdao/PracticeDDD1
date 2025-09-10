using Microsoft.EntityFrameworkCore;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository, IRepository<Order>
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }

        public Task<List<Order>> GetOrdersByCustomerIdAsync(Guid customerId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<Order>, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize)
        {
            var query = _context.Set<Order>()
            .Include(o => o.OrderItems)
            .AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
