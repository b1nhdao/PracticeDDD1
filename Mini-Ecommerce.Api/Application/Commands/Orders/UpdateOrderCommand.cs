using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Orders
{
    public class UpdateOrderCommand : IRequest<Order>
    {
        public Order Order { get; set; }
        public Guid Id { get; set; }

        public UpdateOrderCommand(Order order, Guid id)
        {
            Order = order;
            Id = id;
        }
    }

}
