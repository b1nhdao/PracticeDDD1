using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Products
{
    public class StockProductCommand : IRequest<Product>
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public StockProductCommand(Guid id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }
    }
}
