using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productExisting = await _productRepository.GetByIdAsync(request.Id);

            productExisting.Update(request.ProductDto.Name, request.ProductDto.Description, request.ProductDto.Price, request.ProductDto.Quantity);

            _productRepository.Update(productExisting);
            await _productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return productExisting;
        }
    }
}
