using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate
{
    public class Sku : ValueObject
    {
        public string Value { get; private set; }
        
        public Sku()
        {
        }

        public Sku(string value)
        {
            Value = value;
        }

        public static Sku Add(string value)
        {
            return new Sku(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
