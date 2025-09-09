using MediatR;
using Mini_Ecommerce.Api.Servuces.EmailService;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;
using Mini_Ecommerce.Domain.Events;

namespace Mini_Ecommerce.Api.Application.EventDomainHandler
{
    public class OrderStatusChangedToPaidDomainEventHandler : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderStatusChangedToPaidDomainEventHandler> _logger;
        private readonly IEmailService _emailService;

        public OrderStatusChangedToPaidDomainEventHandler(IProductRepository productRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository,ILogger<OrderStatusChangedToPaidDomainEventHandler> logger,IEmailService emailService)
        {
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _logger = logger;
            _emailService = emailService;
        }


        public async Task Handle(OrderStatusChangedToPaidDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Processing OrderStatusChangedToPaid event for Order ID: {notification.OrderId}");

            try
            {
                var order = await _orderRepository.GetByIdAsync(notification.OrderId);
                if (order == null)
                {
                    _logger.LogError($"Order with ID {notification.OrderId} not found");
                    return;
                }

                foreach (var orderItem in notification.OrderItems)
                {
                    try
                    {
                        var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
                        if (product == null)
                        {
                            var errorMsg = $"Product with ID {orderItem.ProductId} not found";
                            _logger.LogError(errorMsg);
                            continue;
                        }

                        product.DecreaseQuantity(orderItem.Quantity);

                        _productRepository.Update(product);
                    }
                    catch (Exception ex)
                    {
                        var errorMsg = $"Failed to update stock for Product ID: {orderItem.ProductId}. Error: {ex.Message}";
                        _logger.LogError(ex, errorMsg);
                    }
                }

                try
                {
                    await _productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("saved successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "failed to save to db");
                    throw;
                }

                try
                {
                    //temp
                    //var customer = new Customer(Guid.Parse("1DC22C3B-A944-4166-9FE9-142730CA4632"), "Name 1", "Email 1", new Address("Street 1", "City 1", "State 1", "Country 1", "Zip code 1"));

                    var customer = await _customerRepository.GetByIdAsync(Guid.Parse("1DC22C3B-A944-4166-9FE9-142730CA4632"));

                    _emailService.SendEmailOrderPurchased(customer, cancellationToken);
                    _logger.LogInformation($"Order confirmation email sent to customer: {customer.Email}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send confirmation email to customer ID: {order.CustomerId}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Critical error processing OrderStatusChangedToPaid event for Order ID: {notification.OrderId}");
                throw;
            }
        }
    }
}
