using MediatR;
using Mini_Ecommerce.Api.Servuces.EmailService;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;
using Mini_Ecommerce.Domain.Events;

namespace Mini_Ecommerce.Api.Application.EventDomainHandler
{
    public class ProductRestockedDomainEventHandler : INotificationHandler<ProductRestockedDomainEvent>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;

        public ProductRestockedDomainEventHandler(ICustomerRepository customerRepository, IEmailService emailService)
        {
            _customerRepository = customerRepository;
            _emailService = emailService;
        }

        public async Task Handle(ProductRestockedDomainEvent notification, CancellationToken cancellationToken)
        {
            var customerWishlishedProduct = await _customerRepository.GetCustomersWishlishedProductAsync(notification.ProductId);
            
            if(customerWishlishedProduct == null)
                { return; }
            
            foreach(var customer in customerWishlishedProduct)
            {
                _emailService.SendEmail(customer, cancellationToken);
            }
        }
    }
}
