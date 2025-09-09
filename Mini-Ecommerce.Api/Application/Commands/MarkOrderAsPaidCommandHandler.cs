using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Commands
{
    public class MarkOrderAsPaidCommandHandler : IRequestHandler<MarkOrderAsPaidCommand, MarkOrderAsPaidResult>
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

        public async Task<MarkOrderAsPaidResult> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Processing MarkOrderAsPaid command for Order ID: {request.OrderId}");

            try
            {
                // 1. Validate input
                if (request.OrderId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid Order ID provided");
                    return MarkOrderAsPaidResult.CreateFailure("Invalid Order ID");
                }

                // 2. Get the order
                var order = await _orderRepository.GetByIdAsync(request.OrderId);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {request.OrderId} not found");
                    return MarkOrderAsPaidResult.CreateFailure($"Order with ID {request.OrderId} not found");
                }

                order.SetPaidStatus();

                _orderRepository.Update(order);

                var saveResult = await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                if (!saveResult)
                {
                    _logger.LogError($"Failed to save order {request.OrderId} as paid");
                    return MarkOrderAsPaidResult.CreateFailure("Failed to save order changes");
                }

                _logger.LogInformation($"Successfully marked order {request.OrderId} as paid");
                return MarkOrderAsPaidResult.CreateSuccess(request.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing MarkOrderAsPaid command for Order ID: {request.OrderId}");
                return MarkOrderAsPaidResult.CreateFailure($"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
