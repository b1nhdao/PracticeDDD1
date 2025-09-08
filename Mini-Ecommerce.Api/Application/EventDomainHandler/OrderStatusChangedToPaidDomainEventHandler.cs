using MediatR;
using Mini_Ecommerce.Api.Servuces.EmailService;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;
using Mini_Ecommerce.Domain.Events;

namespace Mini_Ecommerce.Api.Application.EventDomainHandler
{
    public class OrderStatusChangedToPaidDomainEventHandler : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;

        public OrderStatusChangedToPaidDomainEventHandler(IProductRepository productRepository, ILogger logger, IEmailService emailService)
        {
            _productRepository = productRepository;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Handle(OrderStatusChangedToPaidDomainEvent notification, CancellationToken cancellationToken)
        {
            
        }
    }
}
