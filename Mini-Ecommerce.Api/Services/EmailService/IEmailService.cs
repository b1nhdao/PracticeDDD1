using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;

namespace Mini_Ecommerce.Api.Servuces.EmailService
{
    public interface IEmailService
    {
        void SendEmailOrderPurchased(Customer customer, CancellationToken cancellationToken = default);
        void SendEmail(Customer customer, CancellationToken cancellationToken = default);
    }
}
