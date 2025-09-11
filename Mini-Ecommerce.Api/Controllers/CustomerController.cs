using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mini_Ecommerce.Api.Application.Commands.Customers;
using Mini_Ecommerce.Api.Application.Querries.Customers;
using Mini_Ecommerce.Api.Models.Pagination;

namespace Mini_Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("productId")]
        public async Task<IActionResult> AddWishlishItem(Guid customerId, Guid ProductId)
        {
            var command = new CustomerAddWishlishProductCommand(customerId, ProductId);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProducts([FromQuery]PagedRequest request)
        {
            var query = new GetCustomersQuery(request);
            return Ok(await _mediator.Send(query));
        }
    }
}
