using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<(List<Product>, int TotalCount)> GetPagedAsync(int pageSize, int pageIndex);
    }
}
