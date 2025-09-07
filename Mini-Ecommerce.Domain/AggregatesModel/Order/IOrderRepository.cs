using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.Order
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order Add(Order order);
        void Update(Order order);
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(Guid id);
    }
}
