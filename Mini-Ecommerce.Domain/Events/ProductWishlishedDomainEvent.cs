using MediatR;

namespace Mini_Ecommerce.Domain.Events
{
    public class ProductWishlishedDomainEvent : INotification
    {
        public Guid CustomerId { get; }
        public Guid ProductId { get; }
        public string Email { get; }
        public ProductWishlishedDomainEvent(Guid customerId, Guid productId, string email)
        {
            CustomerId = customerId;
            ProductId = productId;
            Email = email;
        }
    }
}
