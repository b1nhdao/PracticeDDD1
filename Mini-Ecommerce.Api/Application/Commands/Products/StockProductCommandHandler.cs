using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Products
{
    public class StockProductCommandHandler : IRequestHandler<StockProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;

        public StockProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(StockProductCommand request, CancellationToken cancellationToken)
        {
            var productExisting = await _productRepository.GetByIdAsync(request.Id);
            if (productExisting == null)
            {
                throw new Exception("no product found ");
            }

            productExisting.Restock(request.Quantity);
            var result = _productRepository.Update(productExisting);
            await _productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
