using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Orders
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if(order is null)
            {
                return false;
            }
            _orderRepository.Delete(order);
            await _orderRepository.UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
