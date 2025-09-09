using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;

namespace Mini_Ecommerce.Api.Servuces.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public void SendEmailOrderPurchased(Customer customer, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"EMAIL SERVICE: EMAIL SENT TO {customer.Email}");
        }
    }
}
