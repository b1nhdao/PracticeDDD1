using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;
using Mini_Ecommerce.Domain.Events;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate
{
    public class Customer : Entity, IAggregateRoot
    {
        private readonly List<WishlistProducts> _wishlishProducts = [];
        public string Name { get; private set; }
        public string Email { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<WishlistProducts> WishlistProducts => _wishlishProducts;

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

        public void AddWishlishProducts(Product product)
        {
            if (_wishlishProducts.Any(p => p.ProductId == product.Id))
            {
                throw new Exception("Product already in wishlist");
            }
            var wishlistProduct = new WishlistProducts(Guid.NewGuid(), Id, product.Id, product.Name, product.Price, DateTime.UtcNow);
            _wishlishProducts.Add(wishlistProduct);
        
            AddDomainEvent(new ProductWishlishedDomainEvent(Id, product.Id, Email));
        }
    }
}
