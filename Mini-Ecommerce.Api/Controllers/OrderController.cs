using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mini_Ecommerce.Api.Application.Commands;
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
        public async Task<IActionResult> AddOrder([FromBody]AddOrderCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{orderId}/mark-as-paid")]
        public async Task<IActionResult> MarkOrderAsPaid(Guid orderId, CancellationToken cancellationToken = default)
        {

            try
            {
                // Validate the order ID
                if (orderId == Guid.Empty)
                {
                    return BadRequest(new { message = "Invalid order ID" });
                }

                // Create and send the command
                var command = new MarkOrderAsPaidCommand(orderId);
                var result = await _mediator.Send(command, cancellationToken);

                // Return appropriate response based on result
                if (result.Success)
                {
                    return Ok(new
                    {
                        success = result.Success,
                        message = result.Message,
                        orderId = result.OrderId
                    });
                }
                else
                {

                    // Return appropriate HTTP status based on the error message
                    if (result.Message.Contains("not found"))
                    {
                        return NotFound(new
                        {
                            success = result.Success,
                            message = result.Message
                        });
                    }
                    else if (result.Message.Contains("cancelled") || result.Message.Contains("already"))
                    {
                        return BadRequest(new
                        {
                            success = result.Success,
                            message = result.Message
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {
                            success = result.Success,
                            message = result.Message
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred while processing the request"
                });
            }
        }
    }
}
