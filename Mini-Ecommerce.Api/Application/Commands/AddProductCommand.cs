using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Commands
{
    public class AddProductCommand : IRequest<Product>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Sku { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public int Quantity { get; private set; }

        public AddProductCommand(string name, string description, string sku, decimal price, bool isActive, int quantity)
        {
            Name = name;
            Description = description;
            Sku = sku;
            Price = price;
            IsActive = isActive;
            Quantity = quantity;
        }
    }
}
