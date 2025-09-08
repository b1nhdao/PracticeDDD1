using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Querries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
    }
}
