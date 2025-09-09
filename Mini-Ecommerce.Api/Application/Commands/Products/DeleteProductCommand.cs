using MediatR;

namespace Mini_Ecommerce.Api.Application.Commands.Products
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public Guid id { get; set; }

        public DeleteProductCommand(Guid id)
        {
            this.id = id;
        }
    }
}
