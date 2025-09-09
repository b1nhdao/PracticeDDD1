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
    }
}
