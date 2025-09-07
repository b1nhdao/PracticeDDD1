using MediatR;
using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;

namespace Mini_Ecommerce.Domain.Events
{
    public class OrderPlacedDomainEvent : INotification
    {
        // tbh, i dont know what for ...

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public string CustomerName { get; }
        public decimal TotalAmount { get; }
        public string Currency { get; }
        public Address ShippingAddress { get; }

        public OrderPlacedDomainEvent(Guid orderId, Guid customerId, string customerName, decimal totalAmount, string currency, Address shippingAddress)
        {
            OrderId = orderId;
            CustomerId = customerId;
            CustomerName = customerName;
            TotalAmount = totalAmount;
            Currency = currency;
            ShippingAddress = shippingAddress;
        }
    }
}
