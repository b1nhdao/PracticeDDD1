using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<(List<Order>, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize);
        Task<List<Order>> GetOrdersByCustomerIdAsync(Guid customerId, int pageIndex, int pageSize);
        Task<Order> GetOrderByIdAsync(Guid id);
    }
}
