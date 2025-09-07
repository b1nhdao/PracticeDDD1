using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Sku Sku { get; private set; }
        public Price Price { get; private set; }
        public bool IsActive { get; private set; }

        protected Product() { }

        public Product(Guid id, string name, string description, Sku sku, Price price) : this()
        {
            Id = id;
            Name = name;
            Description = description;
            Sku = sku;
            Price = price;
            IsActive = true;
        }

        public static Product Add(string name, string description, Sku sku, Price price)
        {
            var product = new Product(Guid.NewGuid(), name, description, sku, price);
            return product;
        }

        public void ToggleProductActive(Product product)
        {
            if (product.IsActive)
            {
                product.IsActive = false;
            }
            else
            {
                product.IsActive = true;
            }
        }
    }
}
