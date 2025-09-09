using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mini_Ecommerce.Api.Application.Commands;
using Mini_Ecommerce.Api.Application.Commands.Orders;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;

namespace Mini_Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> DeleteOrder(DeleteOrderCommand request)
        {
            return await _mediator.Send(request) ? Ok() : BadRequest();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, UpdateOrderCommand request)
        {
            if(id == Guid.Empty)
            {
                return BadRequest(new {message = "invalid order Id"});
            }

            request.Id = id;

            return Ok(await _mediator.Send(request));
        }

        [HttpPut]
        [Route("{id}/mark-as-paid")]
        public async Task<IActionResult> MarkOrderAsPaid(Guid id, CancellationToken cancellationToken = default)
        {

            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid order ID" });
            }

            var command = new MarkOrderAsPaidCommand(id);
            return await _mediator.Send(command, cancellationToken) ? Ok() : BadRequest();
        }

    }
}
