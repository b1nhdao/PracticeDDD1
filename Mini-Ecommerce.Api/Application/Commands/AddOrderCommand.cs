using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;

namespace Mini_Ecommerce.Api.Application.Commands
{
    public class AddOrderCommand : IRequest<Order>
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public Address Address { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();

        public AddOrderCommand(Guid customerId, string customerName, DateTime orderDate, OrderStatus status, Address address, string currency, decimal price, List<CreateOrderItemDto> orderItems)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            OrderDate = orderDate;
            Status = status;
            Address = address;
            Currency = currency;
            Price = price;
            OrderItems = orderItems;
        }

        public class CreateOrderItemDto
        {
            public Guid ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public string Currency { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}
