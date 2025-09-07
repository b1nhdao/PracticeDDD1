using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.ValueObjects
{
    public class Price : ValueObject
    {
        public string Currency {  get; private set; }
        public decimal Amount { get; private set; }
        public Price() { }

        public Price(string currency, decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount must be greater than 0");
            }
            Currency = currency;
            Amount = amount;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency;
            yield return Amount;
        }
    }
}
