using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Commands.Orders
{
    public class AddOrderCommand : IRequest<Order>
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Price { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();

        public AddOrderCommand(Guid customerId, OrderStatus status, List<CreateOrderItemDto> orderItems)
        {
            CustomerId = customerId;
            OrderDate = DateTime.Now;
            Status = status;
            Price = 0;
            OrderItems = orderItems;
        }

        public class CreateOrderItemDto
        {
            public Guid ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}
