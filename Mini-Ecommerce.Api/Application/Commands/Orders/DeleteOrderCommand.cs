using MediatR;

namespace Mini_Ecommerce.Api.Application.Commands.Orders
{
    public class DeleteOrderCommand: IRequest<bool>
    {
        public Guid OrderId { get; set; }

        public DeleteOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
