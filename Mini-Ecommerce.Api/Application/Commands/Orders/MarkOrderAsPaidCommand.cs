using MediatR;

namespace Mini_Ecommerce.Api.Application.Commands.Orders
{
    public class MarkOrderAsPaidCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }

        public MarkOrderAsPaidCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
