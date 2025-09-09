using MediatR;
using Mini_Ecommerce.Api.Servuces.EmailService;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;
using Mini_Ecommerce.Domain.Events;

namespace Mini_Ecommerce.Api.Application.EventDomainHandler
{
    public class OrderStatusChangedToPaidDomainEventHandler : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
    {
        private readonly IProductRepository _productRepository;
        //private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderStatusChangedToPaidDomainEventHandler> _logger;
        private readonly IEmailService _emailService;

        public OrderStatusChangedToPaidDomainEventHandler(IProductRepository productRepository,IOrderRepository orderRepository,ILogger<OrderStatusChangedToPaidDomainEventHandler> logger,IEmailService emailService)
        {
            _productRepository = productRepository;
            //_customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _logger = logger;
            _emailService = emailService;
        }


        public async Task Handle(OrderStatusChangedToPaidDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Processing OrderStatusChangedToPaid event for Order ID: {notification.OrderId}");

            try
            {
                // 1. Get the order to access customer information
                var order = await _orderRepository.GetByIdAsync(notification.OrderId);
                if (order == null)
                {
                    _logger.LogError($"Order with ID {notification.OrderId} not found");
                    return;
                }

                // 2. Update product quantities (decrease stock)
                var stockUpdateTasks = new List<Task>();
                var stockUpdateResults = new List<(bool Success, string ProductId, string Message)>();

                foreach (var orderItem in notification.OrderItems)
                {
                    try
                    {
                        var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
                        if (product == null)
                        {
                            var errorMsg = $"Product with ID {orderItem.ProductId} not found";
                            _logger.LogError(errorMsg);
                            stockUpdateResults.Add((false, orderItem.ProductId.ToString(), errorMsg));
                            continue;
                        }

                        // Check if product has sufficient stock
                        if (product.Quantity < orderItem.Quantity)
                        {
                            var errorMsg = $"Insufficient stock for Product ID: {orderItem.ProductId}. " +
                                         $"Available: {product.Quantity}, Required: {orderItem.Quantity}";
                            _logger.LogWarning(errorMsg);
                            stockUpdateResults.Add((false, orderItem.ProductId.ToString(), errorMsg));
                            continue;
                        }

                        // Reduce stock - you'll need to add ReduceStock method to Product class
                        // For now, we'll directly modify the Quantity property
                        // In a real implementation, you should add a ReduceStock method to Product
                        var newQuantity = product.Quantity - orderItem.Quantity;

                        // Since Product doesn't have a setter for Quantity, you might need to use reflection
                        // or add a ReduceStock method. For this example, I'll assume you add the method:
                        // product.ReduceStock(orderItem.Quantity);

                        // Temporary workaround - you should implement ReduceStock method in Product class
                        var quantityProperty = typeof(Product).GetProperty("Quantity");
                        quantityProperty?.SetValue(product, newQuantity);

                        _productRepository.Update(product);

                        var successMsg = $"Reduced stock for Product ID: {orderItem.ProductId}, " +
                                       $"Quantity reduced by: {orderItem.Quantity}, " +
                                       $"Remaining stock: {newQuantity}";
                        _logger.LogInformation(successMsg);
                        stockUpdateResults.Add((true, orderItem.ProductId.ToString(), successMsg));
                    }
                    catch (Exception ex)
                    {
                        var errorMsg = $"Failed to update stock for Product ID: {orderItem.ProductId}. Error: {ex.Message}";
                        _logger.LogError(ex, errorMsg);
                        stockUpdateResults.Add((false, orderItem.ProductId.ToString(), errorMsg));
                    }
                }

                try
                {
                    await _productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Product stock updates saved successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to save product stock updates");
                    throw;
                }

                // 4. Get customer and send email notification
                try
                {
                    //var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
                    //if (customer == null)
                    //{
                    //    _logger.LogError($"Customer with ID {order.CustomerId} not found");
                    //    return;
                    //}

                    var customer = new Customer(Guid.Parse("e05955fd-570f-45c6-929a-f69d1860cb0d"), "Name 1", "Email 1", new Address("Street 1", "City 1", "State 1", "Country 1", "Zip code 1"));

                    // send order confirmation email
                    _emailService.SendEmailOrderPurchased(customer, cancellationToken);
                    _logger.LogInformation($"Order confirmation email sent to customer: {customer.Email}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send confirmation email to customer ID: {order.CustomerId}");
                }

                var successfulUpdates = stockUpdateResults.Count(r => r.Success);
                var failedUpdates = stockUpdateResults.Count(r => !r.Success);

                _logger.LogInformation($"Order paid event processing completed for Order ID: {notification.OrderId}. " +
                                     $"Stock updates - Successful: {successfulUpdates}, Failed: {failedUpdates}");

                if (failedUpdates > 0)
                {
                    _logger.LogWarning($"Some stock updates failed for Order ID: {notification.OrderId}. " +
                                     "Manual intervention may be required.");
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
