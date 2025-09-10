using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Customers
{
    public class CustomerAddWishlishProductCommand : IRequest<WishlistProduct>
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }

        public CustomerAddWishlishProductCommand(Guid customerId, Guid productId)
        {
            CustomerId = customerId;
            ProductId = productId;
        }
    }
}
