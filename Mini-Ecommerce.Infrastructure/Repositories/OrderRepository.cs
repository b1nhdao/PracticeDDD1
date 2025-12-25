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

        public new Task<Order?> GetByIdAsync(Guid id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }

        public async Task<(List<Order>, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, bool isDescending)
        {
            var baseQuery = _context.Set<Order>()
                .AsNoTracking()
                .Include(o => o.OrderItems);

            var totalCount = await baseQuery.CountAsync();

            IQueryable<Order> query;
            if (isDescending)
            {
                query = baseQuery.OrderByDescending(o => o.OrderDate)
                                 .ThenByDescending(o => o.Id);
            }
            else
            {
                query = baseQuery.OrderBy(o => o.OrderDate)
                                 .ThenBy(o => o.Id);
            }

            var items = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public Order Update(Order order)
        {
            var existingOrderitems = _context.OrderItem.Where(oi => oi.OrderId == order.Id).ToList();

            var newOrderItemsAdded = order.OrderItems
                .Where(oi => !existingOrderitems.Any(ei => ei.ProductId == oi.ProductId))
                .Select(item => new OrderItem(
                    Guid.NewGuid(),
                    order.Id,
                    item.ProductId,
                    item.ProductName,
                    item.Price,
                    item.Quantity))
                .ToList();


            var addedEntities = _context.ChangeTracker.Entries();

            foreach (var item in addedEntities)
            {
                if (item.State == EntityState.Modified)
                {
                    item.State = EntityState.Added;
                }
            }

            _context.OrderItem.AddRange(newOrderItemsAdded);


            _context.Orders.Update(order);
            return order;
        }

        public Task<(List<Order>, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(Guid id)
        {
            return _context.OrderItem.Where(oi => oi.OrderId == id).ToListAsync();
        }
    }
}
