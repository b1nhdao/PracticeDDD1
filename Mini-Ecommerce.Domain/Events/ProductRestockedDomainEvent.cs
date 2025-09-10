using MediatR;

namespace Mini_Ecommerce.Domain.Events
{
    public class ProductRestockedDomainEvent : INotification
    {
        public Guid ProductId { get; }
        public int Quantity { get; }

        public ProductRestockedDomainEvent(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
