using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate
{
    public class Customer : Entity, IAggregateRoot
    {
        private readonly List<WishlistProduct> _wishlishProducts = [];
        public string Name { get; private set; }
        public string Email { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<WishlistProduct> WishlistProducts => _wishlishProducts;

        public Customer(Guid id, string name, string email, Address address) : this()
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
        }

        protected Customer() { }

        public static Customer Add(string name, string email, Address address)
        {
            var customer = new Customer(Guid.NewGuid(), name, email, address);
            return customer;
        }

        public void AddProductToWishlish(WishlistProduct product)
        {
            _wishlishProducts.Add(product);
        }
    }
}
