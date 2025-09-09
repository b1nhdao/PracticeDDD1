using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Orders
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderExisting = await _orderRepository.GetByIdAsync(request.Id);

            if(orderExisting is null)
            {
                throw new Exception("order not found");
            }

            orderExisting.Update(request.Order);

            var order = _orderRepository.Update(orderExisting);
            await _orderRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogError("done update ");

            return order;
        }
    }
}
