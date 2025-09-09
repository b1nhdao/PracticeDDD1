using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Orders
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _orderRepository.Update(request.Order);
            await _orderRepository.UnitOfWork.SaveChangesAsync();
            return order;
        }
    }
}
