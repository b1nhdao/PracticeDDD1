using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Sku Sku { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public int Quantity { get; private set; }

        protected Product() { }

        public Product(Guid id, string name, string description, Sku sku, decimal price, int quantity) : this()
        {
            Id = id;
            Name = name;
            Description = description;
            Sku = sku;
            Price = price;
            IsActive = true;
            Quantity = quantity;
        }

        public void Update(string name, string description, decimal price, int quantity)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public static Product Add(string name, string description, Sku sku, decimal price, int quantity)
        {
            var product = new Product(Guid.NewGuid(), name, description, sku, price, quantity);
            return product;
        }

        public void DecreaseQuantity(int quantity)
        {
            if(quantity > Quantity)
            {
                throw new Exception("not enough supply");
            }

            if(quantity < 0)
            {
                throw new Exception("invalid quantity");
            }

            Quantity -= quantity;
        }

        public bool HasSufficientStock(int requestedQuantity)
        {
            return Quantity >= requestedQuantity;
        }

        public void ToggleProductActive()
        {
            IsActive = !IsActive;
        }
    }
}
