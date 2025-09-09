using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Orders
{
    public class MarkOrderAsPaidCommandHandler : IRequestHandler<MarkOrderAsPaidCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<MarkOrderAsPaidCommandHandler> _logger;

        public MarkOrderAsPaidCommandHandler(
            IOrderRepository orderRepository,
            ILogger<MarkOrderAsPaidCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Processing MarkOrderAsPaid command for Order ID: {request.OrderId}");


            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
            {
                return false;
            }

            order.SetPaidStatus();

            _orderRepository.Update(order);

            var saveResult = await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return true;
        }
    }
}

