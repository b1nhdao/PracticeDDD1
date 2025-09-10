using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Querries.Products
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public Guid Id { get; set; }

        public GetProductByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
