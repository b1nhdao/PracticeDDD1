using MediatR;
using Mini_Ecommerce.Api.Attributes.RedisCache;
using Mini_Ecommerce.Api.Models.Pagination;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Querries.Products
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PagedResponse<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _productRepository
                .GetPagedAsync(request.Parameters.PageIndex, request.Parameters.PageSize);

            return new PagedResponse<Product>(request.Parameters.PageIndex, request.Parameters.PageSize, totalCount, items);
        }
    }
}
