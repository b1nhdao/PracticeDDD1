using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;
using Mini_Ecommerce.Infrastructure.Repositories;

namespace Mini_Ecommerce.Api.Application.Commands
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;

        public AddOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(
                            Guid.NewGuid(),
                            request.CustomerId,
                            request.CustomerName,
                            request.Status,
                            request.Address,
                            new List<OrderItem>()
                        );

            foreach (var i in request.OrderItems)
            {
                order.AddOrderItem(i.ProductId, i.ProductName, i.Price, i.Quantity);
            }

            _orderRepository.Add(order);

            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return order;

        }
    }
}
