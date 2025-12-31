using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mini_Ecommerce.Api.Application.Commands.Products;
using Mini_Ecommerce.Api.Application.Querries.Products;
using Mini_Ecommerce.Api.Attributes.RedisCache;
using Mini_Ecommerce.Api.DTOs;
using Mini_Ecommerce.Api.Models.Pagination;

namespace Mini_Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CacheRewriteActionFilter("Products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [RedisCache(1, KeyPrefix = "product_list")]
        public async Task<IActionResult> GetAllProductsPaged([FromQuery] PagedRequest request)
        {
            var query = new GetAllProductsQuery(request);
            return Ok(await _mediator.Send(query));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDto updateProductDto)
        {
            var command = new UpdateProductCommand(id, updateProductDto);
            return Ok(await _mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var command = new DeleteProductCommand(id);
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        [RedisCache()]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            GetProductByIdQuery query = new GetProductByIdQuery(id);
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("{id}/Stock")]
        public async Task<IActionResult> Stock(Guid id, int quantity)
        {
            StockProductCommand command = new StockProductCommand(id, quantity);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
