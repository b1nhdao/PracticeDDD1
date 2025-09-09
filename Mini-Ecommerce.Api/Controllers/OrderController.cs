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
        public async Task<IActionResult> UpdateOrder(UpdateOrderCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPut("{orderId}/mark-as-paid")]
        public async Task<IActionResult> MarkOrderAsPaid(Guid orderId, CancellationToken cancellationToken = default)
        {

            if (orderId == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid order ID" });
            }

            var command = new MarkOrderAsPaidCommand(orderId);
            return await _mediator.Send(command, cancellationToken) ? Ok() : BadRequest();
        }

    }
}
