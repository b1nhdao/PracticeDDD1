using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.DTOs
{
    public class UpdateOrderDto
    {
        public Guid CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
