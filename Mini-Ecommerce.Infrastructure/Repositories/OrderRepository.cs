using Microsoft.EntityFrameworkCore;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository, IRepository<Order>
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public Order Add(Order order)
        {
            _context.Orders.Add(order);
            return order;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                await _context.Entry(order)
                    .Collection(i => i.OrderItems).LoadAsync();
            }

            return order;
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }
    }
}
