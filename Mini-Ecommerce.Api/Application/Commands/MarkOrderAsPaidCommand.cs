using MediatR;

namespace Mini_Ecommerce.Api.Application.Commands
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
