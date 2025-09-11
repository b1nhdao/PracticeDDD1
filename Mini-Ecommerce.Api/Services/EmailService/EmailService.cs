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

        public void SendEmail(Customer customer, CancellationToken cancellationToken = default)
        {
            _logger.LogError("EMAIL SERVICE: EMAIL SENT TO {c}", customer.Email);
        }

        public void SendEmailOrderPurchased(Customer customer, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("EMAIL SERVICE: EMAIL SENT TO {ce}. \nHELLO {cn}", customer.Email, customer.Name);
        }
    }
}
