using MediatR;
using Mini_Ecommerce.Api.DTOs;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Orders
{
    public class UpdateOrderCommand : IRequest<Order>
    {
        public UpdateOrderDto OrderDto { get; set; }
        public Guid Id { get; set; }

        public UpdateOrderCommand(UpdateOrderDto orderDto, Guid id)
        {
            OrderDto = orderDto;
            Id = id;
        }
    }
}
