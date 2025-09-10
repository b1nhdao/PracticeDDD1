using MediatR;
using Mini_Ecommerce.Api.DTOs;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Products
{
    public class UpdateProductCommand : IRequest<Product>
    {
        public Guid Id { get; set; }
        public UpdateProductDto ProductDto { get; set; }

        public UpdateProductCommand(Guid id, UpdateProductDto productDto)
        {
            Id = id;
            ProductDto = productDto;
        }
    }
}
