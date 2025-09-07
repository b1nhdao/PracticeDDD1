using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate
{
    public interface IProductRepository : IRepository<Product>
    {
        Product Add(Product product);
        void Update(Product product);
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
    }
}
