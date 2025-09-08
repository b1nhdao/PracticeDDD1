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

        [HttpGet]
        public async Task<IActionResult> Test ()
        {
            Address address = new Address("street 1", "city 1", "state 1", "country 1", "zipcode 1");

            Guid orderId = Guid.NewGuid();

            Guid productId1 = Guid.Parse("9B2FF169-358F-42D6-9EED-B5944A670761");

            Guid productId2 = Guid.Parse("12345678-358F-42D6-9EED-B5944A670761");


            List<OrderItem> orderItems = new List<OrderItem>
            {
                new OrderItem(Guid.NewGuid(), orderId, productId1, "product 1", 200, 200),
                new OrderItem(Guid.NewGuid(), orderId,  productId2, "product 2", 300, 400),

            };

            Order order = new Order(orderId, Guid.Parse("9B2FF169-358F-42D6-9EED-B5944A670761"), "Binh", OrderStatus.Submitted, address, orderItems); ;
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody]AddOrderCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
