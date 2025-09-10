using MediatR;
using Mini_Ecommerce.Api.Models.Pagination;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Querries.Products
{
    public class GetAllProductsQuery : IRequest<PagedResponse<Product>>
    {
        public PagedRequested Parameters { get; set; }

        public GetAllProductsQuery(PagedRequested parameters)
        {
            Parameters = parameters;
        }
    }
}
