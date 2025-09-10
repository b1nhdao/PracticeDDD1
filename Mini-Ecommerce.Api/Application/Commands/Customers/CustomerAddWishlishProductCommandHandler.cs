using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Customers
{
    public class CustomerAddWishlishProductCommandHandler : IRequestHandler<CustomerAddWishlishProductCommand, WishlistProduct>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public CustomerAddWishlishProductCommandHandler(ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<WishlistProduct> Handle(CustomerAddWishlishProductCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new Exception("product not found ");
            }

            var wishlishProduct = new WishlistProduct(Guid.NewGuid(), request.CustomerId, product.Id, product.Name, product.Price, DateTime.UtcNow);
    
            customer.AddProductToWishlish(wishlishProduct);

            _customerRepository.AddWishlishProduct(wishlishProduct);

            await _customerRepository.UnitOfWork.SaveEntitiesAsync();

            return wishlishProduct;
        }
    }
}
