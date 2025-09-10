using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer Add (Customer customer);
        Task<List<Customer>> GetAllAsync ();
        Task<Customer?> GetByIdAsync (Guid id);
        Task<List<Customer>?> GetCustomersWishlishedProductAsync(Guid productId);
        WishlistProduct AddWishlishProduct (WishlistProduct product);
    }
}
