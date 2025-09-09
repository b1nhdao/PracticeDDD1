using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Products
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productExisting = await _productRepository.GetByIdAsync(request.id);
            if (productExisting is null)
            {
                return false;
            }

            return _productRepository.Delete(productExisting);
        }
    }
}
