namespace Mini_Ecommerce.Domain.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        T Add(T entity);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        T Update(T entity);
        bool Delete(T entity);
    }
}
